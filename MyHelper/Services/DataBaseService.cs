﻿using Microsoft.EntityFrameworkCore;
using MyHelper.Data;
using MyHelper.Dto;
using MyHelper.Enums;
using MyHelper.Extensions;
using MyHelper.Models.Dto;
using MyHelper.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyHelper.Services
{
    /// <summary>
    /// Сервис для работы с БД.
    /// </summary>
    public class DataBaseService
    {
        /// <summary>
        /// Контекст данных.
        /// </summary>
        private readonly ProgramContext _context;

        public DataBaseService(ProgramContext context)
        {
            this._context = context;
        }

        /// <summary>
        /// Получение таблиц.
        /// </summary>
        /// <param name="tableId">Таблица.</param>
        public List<TableDto> GetTables(int tableId = 0)
        {
            var tablesDto = this._context.Tables
                .Where(x => tableId > 0 ? x.Id == tableId : !x.IsDeleted)
                .ToArray()
                .Select(entity => new TableDto().MapTableEntityToDto(entity))
                .ToList();

            var tableIds = tablesDto.Select(x => x.Id);
            var colomnsDto = this._context.Colomns
                .Where(x => tableIds.Contains(x.Table.Id) && !x.IsDeleted)
                .ToArray()
                .Select(entity => new ColomnDto().MapColomnEntityToDto(entity))
                .ToArray();

            foreach (var table in tablesDto)
            {
                table.Colomns = colomnsDto.Where(x => x.TableId == table.Id).ToList();
            }

            return tablesDto.OrderByDescending(x => x.Sort).ToList();
        }

        /// <summary>
        /// Получить наименование связанной таблицы.
        /// </summary>
        /// <param name="mainTableName">Наименование основной таблицы.</param>
        /// <param name="colomnName">Наименование колонки.</param>
        /// <returns>Наименование связанной таблицы.</returns>
        public (string, string, string) GetDirectoryTableName(string mainTableName, string colomnName)
        {
            if (mainTableName.IsNullOrDefault() || colomnName.IsNullOrDefault())
            {
                return (string.Empty, string.Empty, string.Empty);
            }

            var result = this._context.TableDirectories
                .Where(x => x.rf_Colomn.ToLower() == colomnName.ToLower())
                .OrderByDescending(x => x.rf_Table.ToLower() == mainTableName.ToLower())
                .Select(x => new { x.Table, x.TabkeKey, x.TableNickname })
                .FirstOrDefault();

            if (result != null && !result.Table.IsNullOrDefault())
            {
                return (result.Table, result.TabkeKey, result.TableNickname);
            }

            var key = colomnName.StartsWith("rf_") ? colomnName.Substring(3) : colomnName;
            var table = "oms_" + (key.EndsWith("ID", StringComparison.OrdinalIgnoreCase) ? key.Substring(0, key.Length - 2) : key);

            return (table, key, string.Empty);
        }

        /// <summary>
        /// Сохранение новой таблицы.
        /// </summary>
        /// <param name="dto">Таблица.</param>
        public TableDto AddTable(TableDto dto)
        {
            var entityTable = new TableEntity().MapTableDtoToEntity(dto);

            var entityColomns = new List<ColomnEntity>();
            foreach (var dtoColomn in dto.Colomns)
            {
                var entityColomn = new ColomnEntity().MapColomnDtoToEntity(dtoColomn);
                entityColomn.Table = entityTable;
                entityColomns.Add(entityColomn);
            }

            this._context.Colomns.AddRange(entityColomns);
            this._context.SaveChanges();
            dto.Id = entityTable.Id;
            dto.Colomns.ForEach(colomn =>
            {
                colomn.Id = entityTable.Colomns
                    .Where(x => x.Name == colomn.Name)
                    .Select(x => x.Id)
                    .FirstOrDefault();
            });

            return dto;
        }

        /// <summary>
        /// Изменение списка таблиц.
        /// </summary>
        /// <param name="tables">Таблицы.</param>
        public void UpdateTables(params TableDto[] tables)
        {
            if (tables == null || !tables.Any())
            {
                return;
            }

            var tableIds = tables.Select(x => x.Id);
            var entities = this._context.Tables
                .Where(x => tableIds.Contains(x.Id))
                .ToList();

            foreach (var entity in entities)
            {
                var dto = tables.First(x => x.Id == entity.Id);
                entity.MapTableDtoToEntity(dto);
            }

            this._context.SaveChanges();
        }

        /// <summary>
        /// Проставления признака удаленной записи.
        /// </summary>
        /// <param name="tableDto">Таблица</param>
        public void RemoveTable(TableDto tableDto)
        {
            var entity = this._context.Tables.FirstOrDefault(x => x.Id == tableDto.Id);
            entity.IsDeleted = true;
            this._context.SaveChanges();
        }

        /// <summary>
        /// Изменение списка колонок.
        /// </summary>
        /// <param name="colomns">Колонки.</param>
        public void UpdateColomns(string tableName, params ColomnDto[] colomns)
        {
            if (colomns == null || !colomns.Any())
            {
                return;
            }

            var colomnIds = colomns.Select(x => x.Id);
            var colomnEntities = this._context.Colomns
                .Where(x => colomnIds.Contains(x.Id))
                .ToList();

            foreach (var entity in colomnEntities)
            {
                var dto = colomns.First(x => x.Id == entity.Id);
                entity.MapColomnDtoToEntity(dto);
            }

            var colomnsWithIsExistDirectory = colomns.Where(x => x.IsExistDirectory);
            if (colomnsWithIsExistDirectory.Any())
            {
                var colomnNames = colomns.Where(x => x.IsExistDirectory).Select(x => x.Name);
                var tableDirectories = this._context.TableDirectories.Where(x => x.rf_Table.ToLower() == tableName.ToLower() && colomnNames.Contains(x.rf_Colomn)).ToList();

                foreach (var colomn in colomnsWithIsExistDirectory)
                {
                    var tableDirectory = tableDirectories.FirstOrDefault(x => x.rf_Colomn == colomn.Name);
                    if (tableDirectory == null)
                    {
                        tableDirectory = new TableDirectoryEntity()
                        {
                            rf_Table = tableName,
                            rf_Colomn = colomn.Name,
                        };
                        this._context.TableDirectories.Add(tableDirectory);
                    }
                    tableDirectory.Table = colomn.DirectoryTableName;
                    tableDirectory.TabkeKey = colomn.DirectoryTableKey;
                    tableDirectory.TableNickname = colomn.DirectoryTableNickname;
                }
            }
            this._context.SaveChanges();
        }

        /// <summary>
        /// Проставления признака удаленной записи.
        /// </summary>
        /// <param name="colomnDto">Колонка</param>
        public void RemoveColomn(ColomnDto colomnDto)
        {
            var entity = this._context.Colomns.FirstOrDefault(x => x.Id == colomnDto.Id);
            entity.IsDeleted = true;
            this._context.SaveChanges();
        }

        /// <summary>
        /// Получить модель настроек для сохранения скрипта.
        /// </summary>
        /// <returns></returns>
        public SaveScriptModelDto GetSaveScriptModel()
        {
            var settingCodes = new List<string>()
            {
                SettingEnums.Path,
                SettingEnums.Task,
                SettingEnums.Sprint,
                SettingEnums.Project,
                SettingEnums.IsOpenFile,
                SettingEnums.IsCreateSubFolder,
            };

            var settings = this._context.Settings
                .Where(x => settingCodes.Contains(x.Code))
                .ToArray();

            return new SaveScriptModelDto()
            {
                Path = settings.FirstOrDefault(x => x.Code == SettingEnums.Path)?.Value ?? string.Empty,
                Task = settings.FirstOrDefault(x => x.Code == SettingEnums.Task)?.Value ?? string.Empty,
                Sprint = settings.FirstOrDefault(x => x.Code == SettingEnums.Sprint)?.Value ?? string.Empty,
                Project = settings.FirstOrDefault(x => x.Code == SettingEnums.Project)?.Value ?? string.Empty,
                IsOpenFile = settings.FirstOrDefault(x => x.Code == SettingEnums.IsOpenFile)?.Value ?? "1",
                IsCreateSubFolder = settings.FirstOrDefault(x => x.Code == SettingEnums.IsCreateSubFolder)?.Value ?? "1",
            };
        }

        /// <summary>
        /// Сохранить настройки выгрузки скрипта.
        /// </summary>
        /// <param name="saveScriptModelDto"></param>
        public void SaveSettingScript(SaveScriptModelDto saveScriptModelDto)
        {
            this.SaveValueSettingScript(SettingEnums.Path, saveScriptModelDto.Path);
            this.SaveValueSettingScript(SettingEnums.Sprint, saveScriptModelDto.Sprint);
            this.SaveValueSettingScript(SettingEnums.Project, saveScriptModelDto.Project);
            this.SaveValueSettingScript(SettingEnums.IsOpenFile, saveScriptModelDto.IsOpenFile);
            this.SaveValueSettingScript(SettingEnums.IsCreateSubFolder, saveScriptModelDto.IsCreateSubFolder);

            // Из Disp-xxxx получить Disp-
            var task = saveScriptModelDto.Task;
            this.SaveValueSettingScript(SettingEnums.Task, task.Substring(0, task.IndexOf('-') + 1));
        }

        /// <summary>
        /// Сохранение в найстроки БД.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="value"></param>
        private void SaveValueSettingScript(string code, string value)
        {
            var setting = this._context.Settings
                .Where(x => x.Code == code)
                .FirstOrDefault() ?? new SettingEntity() {Code = code };

            setting.Value = value;

            if (setting.Id == 0)
            {
                this._context.Add(setting);
            }

            this._context.SaveChanges();
        }
    }
}
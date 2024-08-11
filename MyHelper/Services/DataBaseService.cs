using Microsoft.EntityFrameworkCore;
using MyHelper.Data;
using MyHelper.Dto;
using MyHelper.Extensions;
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
            var tableQuery = this._context.Tables.AsQueryable();
            if (tableId > 0)
            {
                tableQuery = tableQuery.Where(x => x.Id == tableId);
            }
            else
            {
                tableQuery = tableQuery.Where(x => !x.IsDeleted);
            }

            var tableEntities = tableQuery.ToList();
            var tablesDto = new List<TableDto>();
            foreach (var table in tableEntities)
            {
                var tableDto = new TableDto().MapTableEntityToDto(table);
                tablesDto.Add(tableDto);
            }

            var tableIds = tablesDto.Select(x => x.Id).ToArray();
            var colomnEntities = this._context.Colomns
                .Where(x => tableIds.Contains(x.Table.Id) && !x.IsDeleted)
                .ToArray();

            var colomnsDto = new List<ColomnDto>();
            foreach (var entity in colomnEntities)
            {
                var colomnDto = new ColomnDto().MapColomnEntityToDto(entity);
                colomnsDto.Add(colomnDto);
            }

            foreach (var table in tablesDto)
            {
                table.Colomns = colomnsDto.Where(x => x.TableId == table.Id).ToList();
            }

            return tablesDto.OrderByDescending(x => x.Sort).ToList();
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
            var entity = _context.Tables.FirstOrDefault(x => x.Id == tableDto.Id);
            entity.IsDeleted = true;
            this._context.SaveChanges();
        }

        /// <summary>
        /// Изменение списка колонок.
        /// </summary>
        /// <param name="colomns">Колонки.</param>
        public void UpdateColomns(params ColomnDto[] colomns)
        {
            if (colomns == null || !colomns.Any())
            {
                return;
            }

            var colomnIds = colomns.Select(x => x.Id);
            var entities = this._context.Colomns
                .Where(x => colomnIds.Contains(x.Id))
                .ToList();

            foreach (var entity in entities)
            {
                var dto = colomns.First(x => x.Id == entity.Id);
                entity.MapColomnDtoToEntity(dto);
            }

            this._context.SaveChanges();
        }

        /// <summary>
        /// Проставления признака удаленной записи.
        /// </summary>
        /// <param name="colomnDto">Колонка</param>
        public void RemoveColomn(ColomnDto colomnDto)
        {
            var entity = _context.Colomns.FirstOrDefault(x => x.Id == colomnDto.Id);
            entity.IsDeleted = true;
            this._context.SaveChanges();
        }
    }
}
﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyHelper.Models.Entity
{
    /// <summary>
    /// Entity-модель таблицы (с модельюсохранения)
    /// </summary>
    public class TableEntity : BaseModelEntity
    {
        /// <summary>
        /// Признак добавления шаблона.
        /// </summary>
        public bool IsTemplateScript { get; set; }

        /// <summary>
        /// Колонки.
        /// </summary>
        public List<ColomnEntity> Colomns { get; set; } = new List<ColomnEntity>();

        /// <summary>
        /// Путь.
        /// </summary>
        public string Path { get; set; } = string.Empty;

        /// <summary>
        /// Уникальный идентификатор.
        /// </summary>
        public string Guid { get; set; }

        /// <summary>
        /// Спинт.
        /// </summary>
        public string Sprint { get; set; }

        /// <summary>
        /// Задача.
        /// </summary>
        public string Task { get; set; }

        /// <summary>
        /// Проект.
        /// </summary>
        public string Project { get; set; }

        /// <summary>
        /// Номер скрипта.
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Описание скрипта.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Признак открытия папки после сохранения.
        /// </summary>
        public string IsOpenFile { get; set; }

        /// <summary>
        /// Признак создания подпапки.
        /// </summary>
        public string IsCreateSubFolder { get; set; }
    }
}

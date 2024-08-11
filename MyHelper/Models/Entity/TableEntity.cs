using System.Collections.Generic;
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
    }
}

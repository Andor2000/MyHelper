using System.ComponentModel.DataAnnotations;

namespace MyHelper.Models.Entity
{
    public class ColomnEntity
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Наименование.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Место в списке.
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// Признак сравнения по записи.
        /// </summary>
        public bool IsStar{ get; set; }

        /// <summary>
        /// Записи.
        /// </summary>
        public string Records { get; set; } = string.Empty;

        /// <summary>
        /// Идентификатор таблицы.
        /// </summary>
        public int TableId { get; set; }

        /// <summary>
        /// Таблица.
        /// </summary>
        public TableEntity TableEntity { get; set; }
    }
}

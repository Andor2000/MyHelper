using System.ComponentModel.DataAnnotations;

namespace MyHelper.Models.Entity
{
    /// <summary>
    /// Entity-модель колонки.
    /// </summary>
    public class ColomnEntity : BaseModelEntity
    {
        /// <summary>
        /// Записи.
        /// </summary>
        public string Records { get; set; } = string.Empty;

        /// <summary>
        /// Признак сравнения по записи.
        /// </summary>
        public bool IsEqualsRecordStar { get; set; }

        /// <summary>
        /// Признак добавления кавычек.
        /// </summary>
        public bool IsQuotes { get; set; }

        /// <summary>
        /// Ключ ссылочной таблицы.
        /// </summary>
        [MaxLength(70)]
        public string DirectoryTableKey { get; set; } = string.Empty;

        /// <summary>
        /// Наименование ссылочной таблицы.
        /// </summary>
        [MaxLength(70)]
        public string DirectoryTableName { get; set; } = string.Empty;

        /// <summary>
        /// Наименование ссылочной колонки.
        /// </summary>
        [MaxLength(70)]
        public string DirectoryColomnName { get; set; } = string.Empty;

        /// <summary>
        /// Никнейм ссылочной таблицы.
        /// </summary>
        [MaxLength(70)]
        public string DirectoryTableNickname { get; set; } = string.Empty;

        /// <summary>
        /// Идентификатор таблицы.
        /// </summary>
        public int TableId { get; set; }

        /// <summary>
        /// Таблица.
        /// </summary>
        public virtual TableEntity Table { get; set; }
    }
}

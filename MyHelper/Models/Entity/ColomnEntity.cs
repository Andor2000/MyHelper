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
        /// Наименование ссылочной колонки.
        /// </summary>
        public string DirectoryColomnName { get; set; } = string.Empty;

        /// <summary>
        /// Наименование ссылочной таблицы (если колонка - ссылка).
        /// </summary>
        public string DirectoryTableName { get; set; } = string.Empty;

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

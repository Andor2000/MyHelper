using System.ComponentModel.DataAnnotations;

namespace MyHelper.Models.Entity
{
    /// <summary>
    /// Entity-модель ссылочного поля и таблицы.
    /// </summary>
    public class TableDirectoryEntity
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Таблица.
        /// </summary>
        public string rf_Table { get; set; }

        /// <summary>
        /// Наименование.
        /// </summary>
        public string rf_Colomn { get; set; }

        /// <summary>
        /// Ссылочная таблица.
        /// </summary>
        public string Table { get; set; }

        /// <summary>
        /// Ключ ссылочной таблицы.
        /// </summary>
        public string TabkeKey { get; set; }
    }
}

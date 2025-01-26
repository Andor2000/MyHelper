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
        /// Наименование.
        /// </summary>
        public string ColomnName { get; set; }

        /// <summary>
        /// Таблица.
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// Ссылочная таблица.
        /// </summary>
        public string ReferenceTable { get; set; }
    }
}

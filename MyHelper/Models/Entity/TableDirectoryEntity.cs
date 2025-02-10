using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [MaxLength(70)]
        public string rf_Table { get; set; }

        /// <summary>
        /// Наименование.
        /// </summary>
        [MaxLength(70)]
        public string rf_Colomn { get; set; }

        /// <summary>
        /// Ссылочная таблица.
        /// </summary>
        [MaxLength(70)]
        public string Table { get; set; }

        /// <summary>
        /// Ключ ссылочной таблицы.
        /// </summary>
        [MaxLength(70)]
        public string TabkeKey { get; set; }

        /// <summary>
        /// Никнейм ссылочной таблицы.
        /// </summary>
        [MaxLength(70)]
        public string TableNickname { get; set; } = string.Empty;
    }
}

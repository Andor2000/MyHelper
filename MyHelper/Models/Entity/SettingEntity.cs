using System.ComponentModel.DataAnnotations;

namespace MyHelper.Models.Entity
{
    /// <summary>
    /// Entity-модель настроек.
    /// </summary>
    public class SettingEntity
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Код.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Наименование.
        /// </summary>
        public string Value { get; set; }
    }
}

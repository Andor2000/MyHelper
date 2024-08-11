using System.ComponentModel.DataAnnotations;

namespace MyHelper.Models.Entity
{
    public class BaseModelEntity
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
        /// Признак удаленной записи.
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyHelper.Models.Entity
{
    public class TableEntity
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
        /// Колонки.
        /// </summary>
        public List<ColomnEntity> Colomns { get; set; } =new List<ColomnEntity>();
    }
}

using System.Collections.Generic;

namespace MyHelper.Dto
{
    public class TableDto : BaseModelDto
    {
        public TableDto()
        {
            Context.Items.Add(ContextDeleted);
        }

        /// <summary>
        /// Окончательный скрипт.
        /// </summary>
        //public string EndScript { get; set; } = string.Empty;

        /// <summary>
        /// Список колонок.
        /// </summary>
        public List<ColomnDto> Colomns { get; set; } = new List<ColomnDto>();
    }
}

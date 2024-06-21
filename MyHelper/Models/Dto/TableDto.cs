using System;
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
        /// Признак добавления шаблона.
        /// </summary>
        public bool IsTemplateScript { get; set; }

        /// <summary>
        /// Уникальный идентификатор шаблона.
        /// </summary>
        public Guid GuidTemplate { get; set; } = Guid.Empty;

        /// <summary>
        /// Список колонок.
        /// </summary>
        public List<ColomnDto> Colomns { get; set; } = new List<ColomnDto>();

        /// <summary>
        /// Окончательный скрипт.
        /// </summary>
        //public string EndScript { get; set; } = string.Empty;
    }
}

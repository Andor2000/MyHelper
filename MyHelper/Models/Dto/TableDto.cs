using MyHelper.Enums;
using MyHelper.Models.Dto;
using System.Collections.Generic;

namespace MyHelper.Dto
{
    public class TableDto : BaseModelDto
    {
        public TableDto()
        {
            Icon.Image = IconEnums.Quest;
        }

        /// <summary>
        /// Признак добавления шаблона.
        /// </summary>
        public bool IsTemplateScript { get; set; }

        /// <summary>
        /// Модель сохранения скрипта.
        /// </summary>
        public SaveScriptModelDto SaveScriptModel { get; set; } = new SaveScriptModelDto();

        /// <summary>
        /// Список колонок.
        /// </summary>
        public List<ColomnDto> Colomns { get; set; } = new List<ColomnDto>();
    }
}

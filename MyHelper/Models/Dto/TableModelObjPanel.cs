using System.Collections.Generic;

namespace MyHelper.Dto
{
    public class TableModelObjPanel : BaseModelObjPanel
    {
        /// <summary>
        /// Окончательный скрипт.
        /// </summary>
        //public string EndScript { get; set; } = string.Empty;

        /// <summary>
        /// Список колонок.
        /// </summary>
        public List<ColomnModelObjPanel> Colomns { get; set; } = new List<ColomnModelObjPanel>();
    }
}

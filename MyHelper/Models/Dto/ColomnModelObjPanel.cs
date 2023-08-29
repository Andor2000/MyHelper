using MyHelper.Enums;
using System.Drawing;
using System.Windows.Forms;

namespace MyHelper.Dto
{
    public class ColomnModelObjPanel : BaseModelObjPanel
    {

        public ColomnModelObjPanel()
        {
            TextBoxCount.Margin = new Padding(0, 7, 0, 0);
            TextBoxCount.ForeColor = Color.White;            // цвет текста
            TextBoxCount.BackColor = Colors.PanelFon;           //Colors.PanelFon; ; // цвет фона 
            TextBoxCount.Width = 40;
            TextBoxCount.Font = new Font("Segoe UI", 10);
            TextBoxCount.ShortcutsEnabled = false;           // убрать контекстное меню
            TextBoxCount.BorderStyle = BorderStyle.None;
            TextBoxCount.ReadOnly = true;
            TextBoxCount.Cursor = Cursors.Arrow;             // вид курсора всегда одинаковый
            TextBoxCount.Text = "1";
            TextBoxCount.TextAlign = HorizontalAlignment.Right;

            Panel.Controls.Add(TextBoxCount);
        }

        /// <summary>
        /// Записи.
        /// </summary>
        public string Records { get; set; } = string.Empty;

        /// <summary>
        /// Количество записей.
        /// </summary>
        public int CountRecords { get; set; } = 1;

        /// <summary>
        /// Текстовое поле для количества записей.
        /// </summary>
        public TextBox TextBoxCount { get; set; } = new TextBox();
    }
}

using MyHelper.Enums;
using System.Drawing;
using System.Windows.Forms;

namespace MyHelper.Dto
{
    /// <summary>
    /// Объект задачи.
    /// </summary>
    public class BaseModelObjPanel
    {
        public BaseModelObjPanel()
        {
            Panel.BackColor = Colors.PanelFon ;// цвет фона 
            Panel.Size = new Size(200, SizeEnums.HeightPanel); // размер
            Panel.Padding = new Padding(0, 0, 0, 0);

            Icon.SizeMode = PictureBoxSizeMode.Zoom;    // картинка во весь pictureBox
            Icon.Size = new Size(31, 31);
            Icon.Margin = new Padding(0, 2, 0, 0);

            TextBox.Margin = new Padding(0, 7, 0, 0);
            TextBox.ForeColor = Color.White;            // цвет текста
            TextBox.BackColor = Colors.PanelFon;              //Colors.PanelFon; ; // цвет фона 
            TextBox.Font = new Font("Segoe UI", 10);
            TextBox.Width = 115;                        // ширина текстбокса
            TextBox.ShortcutsEnabled = false;           // убрать контекстное меню
            TextBox.BorderStyle = BorderStyle.None;
            TextBox.ReadOnly = true;
            TextBox.Cursor = Cursors.Arrow;             // вид курсора всегда одинаковый

            TextBox.ContextMenuStrip = Context;
            Panel.Controls.Add(Icon);
            Panel.Controls.Add(TextBox);
        }

        /// <summary>
        /// Сортировка
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// Поле.
        /// </summary>
        public FlowLayoutPanel Panel { get; set; } = new FlowLayoutPanel();

        /// <summary>
        /// Иконка.
        /// </summary>
        public PictureBox Icon { get; set; } = new PictureBox();

        /// <summary>
        /// Текст.
        /// </summary>
        public TextBox TextBox { get; set; } = new TextBox();

        /// <summary>
        /// Контект данных.
        /// </summary>
        public ContextMenuStrip Context { get; set; } = new ContextMenuStrip();
    }
}
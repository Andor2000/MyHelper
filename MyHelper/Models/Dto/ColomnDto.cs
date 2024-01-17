using MyHelper.Enums;
using System.Drawing;
using System.Windows.Forms;

namespace MyHelper.Dto
{
    public class ColomnDto : BaseModelDto
    {

        public ColomnDto()
        {
            TextBoxCount.Margin = new Padding(0, 7, 0, 0);
            TextBoxCount.ForeColor = Color.White;            // цвет текста
            TextBoxCount.BackColor = Colors.PanelFon;           //Colors.PanelFon; ; // цвет фона 
            TextBoxCount.Width = 35;
            TextBoxCount.Font = new Font("Segoe UI", 10);
            TextBoxCount.ShortcutsEnabled = false;           // убрать контекстное меню
            TextBoxCount.BorderStyle = BorderStyle.None;
            TextBoxCount.ReadOnly = true;
            TextBoxCount.Cursor = Cursors.Arrow;             // вид курсора всегда одинаковый
            TextBoxCount.Text = "1";
            TextBoxCount.TextAlign = HorizontalAlignment.Right;

            Icon.Image = IconEnums.Pencil;

            IconStar.SizeMode = PictureBoxSizeMode.Zoom;    // картинка во весь pictureBox
            IconStar.Size = new Size(10, 10);
            IconStar.Margin = new Padding(0, 3, 0, 0);
            IconStar.Visible = false;
            IconStar.Image = IconEnums.Star;

            Context.Items.Add(ContextStar);
            Context.Items.Add(ContextDeleted);

            Panel.Controls.Add(TextBoxCount);
            Panel.Controls.Add(IconStar);
        }

        /// <summary>
        /// Признак сравнения по записи.
        /// </summary>
        public bool IsEqualsRecordStar { get; set; }

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

        /// <summary>
        /// Иконка звезды.
        /// </summary>
        public PictureBox IconStar { get; set; } = new PictureBox();

        /// <summary>
        /// Пункт в контекстном меню звезды.
        /// </summary>
        public ToolStripMenuItem ContextStar { get; set; } = new ToolStripMenuItem("Добавить в сравнение");
    }
}

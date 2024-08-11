using System.Windows.Forms;

namespace MyHelper.NewPanelComponent
{
    public class TextBoxEx : TextBox
    {
        /// <summary>
        /// Подсказка.
        /// </summary>
        public string Placeholder { get; set; }

        /// <summary>
        /// Признак пустой строки.
        /// </summary>
        public bool IsEmpty { get; set; }

        /// <summary>
        /// Признак валидности текстового поля.
        /// </summary>
        public bool IsValid { get; set; }
    }
}

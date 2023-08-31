using System.Drawing;

namespace MyHelper.Enums
{
    /// <summary>
    /// Список цветов.
    /// </summary>
    public class Colors
    {
        /// <summary>
        /// Цвет фона.
        /// </summary>
        public static readonly Color PanelFon = Color.FromArgb(51, 51, 76);

        /// <summary>
        /// Цвет наведения на объект.
        /// </summary>
        public static readonly Color PanelMouseMoveObject = ColorTranslator.FromHtml("#3d3d5a");

        /// <summary>
        /// Цвет активного объекта.
        /// </summary>
        public static readonly Color PanelActiveObject = ColorTranslator.FromHtml("#6B90B8");

        /// <summary>
        /// Цвет текста у активного объекта. (красноватый)
        /// </summary>
        public static readonly Color PanelActiveObjectFore = ColorTranslator.FromHtml("#86144B");
    }
}

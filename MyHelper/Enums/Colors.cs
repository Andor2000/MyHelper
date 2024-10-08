﻿using System.Drawing;

namespace MyHelper.Enums
{
    /// <summary>
    /// Список цветов.
    /// </summary>
    public class Colors
    {
        /// <summary>
        /// Цвет фона (верх).
        /// </summary>
        public static readonly Color PanelUpFon = Color.FromArgb(41, 41, 56);

        /// <summary>
        /// Цвет фона.
        /// </summary>
        public static readonly Color PanelFon = Color.FromArgb(51, 51, 76);

        /// <summary>
        /// Цвет наведения на объект.
        /// </summary>
        public static readonly Color PanelMouseMoveObject = ColorTranslator.FromHtml("#3d3d5a");

        /// <summary>
        /// Цвет наведения на объект.
        /// </summary>
        public static readonly Color PanelMouseMoveObject2 = ColorTranslator.FromHtml("#bdd1e6");

        /// <summary>
        /// Цвет активного объекта.
        /// </summary>
        public static readonly Color PanelActiveObject = ColorTranslator.FromHtml("#6B90B8");

        /// <summary>
        /// Цвет текста у активного объекта. (красноватый)
        /// </summary>
        public static readonly Color PanelActiveObjectFore = ColorTranslator.FromHtml("#86144B");

        /// <summary>
        /// Серый цвет для placeholder.
        /// </summary>
        public static readonly Color FontPlaceholderGrey = ColorTranslator.FromHtml("#b3b1b1");

        /// <summary>
        /// Черный цвет для placeholder.
        /// </summary>
        public static readonly Color FontPlaceholderBlack = Color.Black;

        /// <summary>
        /// Цвет блока при неправильном заполнении.
        /// </summary>
        public static readonly Color BackColorNotCorrectText = Color.FromArgb(255, 192, 192);

        /// <summary>
        /// Цвет блока при обычном заполнении.
        /// </summary>
        public static readonly Color BackColorCorrectText = SystemColors.Window;
    }
}

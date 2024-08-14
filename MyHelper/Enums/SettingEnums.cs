namespace MyHelper.Enums
{
    /// <summary>
    /// Список настроек.
    /// </summary>
    public static class SettingEnums
    {
        /// <summary>
        /// Путь.
        /// </summary>
        public static string Path { get; set; } = "Path";

        /// <summary>
        /// Спринт.
        /// </summary>
        public static string Sprint { get; set; } = "Sprint";

        /// <summary>
        /// Задача.
        /// </summary>
        public static string Task { get; set; } = "Task";

        /// <summary>
        /// Проект.
        /// </summary>
        public static string Project { get; set; } = "Project";

        /// <summary>
        /// Признак открытия папки.
        /// </summary>
        public static string IsOpenFile { get; set; } = "IsOpenFile";

        /// <summary>
        /// Признак создания подпапки.
        /// </summary>
        public static string IsCreateSubFolder { get; set; } = "IsCreateSubFolder";
    }
}
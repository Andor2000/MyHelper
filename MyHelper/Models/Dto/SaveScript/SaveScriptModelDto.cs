namespace MyHelper.Models.Dto
{
    /// <summary>
    /// Dto-модель информации для сохранения скрипта.
    /// </summary>
    public class SaveScriptModelDto
    {
        /// <summary>
        /// Итоговый скрипт.
        /// </summary>
        public string Script { get; set; } = string.Empty;

        /// <summary>
        /// Путь.
        /// </summary>
        public string Path { get; set; } = string.Empty;

        /// <summary>
        /// Уникальный идентификатор.
        /// </summary>
        public string Guid { get; set; } = System.Guid.NewGuid().ToString().ToUpper();

        /// <summary>
        /// Спинт.
        /// </summary>
        public string Sprint { get; set; } = string.Empty;

        /// <summary>
        /// Задача.
        /// </summary>
        public string Task { get; set; } = string.Empty;

        /// <summary>
        /// Проект.
        /// </summary>
        public string Project { get; set; } = string.Empty;

        /// <summary>
        /// Номер скрипта.
        /// </summary>
        public string Number { get; set; } = string.Empty;

        /// <summary>
        /// Описание скрипта.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Признак открытия папки после сохранения.
        /// </summary>
        public string IsOpenFile { get; set; }
    }
}

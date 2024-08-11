using MyHelper.Models.Dto.SaveScript;

namespace MyHelper.Models.Dto
{
    /// <summary>
    /// Dto-модель информации для сохранения скрипта.
    /// </summary>
    public class SaveScriptModelDto
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public SaveScriptModelDto()
        {
            Guid.Text = System.Guid.NewGuid().ToString().ToUpper();
            IsOpenFile = true;
        }

        /// <summary>
        /// Итоговый скрипт.
        /// </summary>
        public SaveScriptField Script { get; set; } = new SaveScriptField();

        /// <summary>
        /// Путь.
        /// </summary>
        public SaveScriptField Path { get; set; } = new SaveScriptField();

        /// <summary>
        /// Уникальный идентификатор.
        /// </summary>
        public SaveScriptField Guid { get; set; } = new SaveScriptField();

        /// <summary>
        /// Спинт.
        /// </summary>
        public SaveScriptField Sprint { get; set; } = new SaveScriptField();

        /// <summary>
        /// Задача.
        /// </summary>
        public SaveScriptField Task { get; set; } = new SaveScriptField();

        /// <summary>
        /// Проект.
        /// </summary>
        public SaveScriptField Project { get; set; } = new SaveScriptField();

        /// <summary>
        /// Номер скрипта.
        /// </summary>
        public SaveScriptField Number { get; set; } = new SaveScriptField();

        /// <summary>
        /// Описание скрипта.
        /// </summary>
        public SaveScriptField Description { get; set; } = new SaveScriptField();

        /// <summary>
        /// Признак открытия папки после сохранения.
        /// </summary>
        public bool IsOpenFile { get; set; }
    }
}

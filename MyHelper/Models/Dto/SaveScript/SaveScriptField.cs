namespace MyHelper.Models.Dto.SaveScript
{
    /// <summary>
    /// Модель "поля" для сохранения скрипта.
    /// </summary>
    public class SaveScriptField
    {
        /// <summary>
        /// Текст поля.
        /// </summary>
        public string Text{ get; set; } = string.Empty;

        /// <summary>
        /// Поле не заполнено.
        /// </summary>
        public bool IsEmpty { get; set; }
    }
}

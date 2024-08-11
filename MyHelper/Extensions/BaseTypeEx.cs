namespace MyHelper.Extensions
{
    /// <summary>
    /// Расширения базовых типов.
    /// </summary>
    public static class BaseTypeEx
    {
        /// <summary>
        /// Парсинг строки в число.
        /// </summary>
        /// <param name="str">Строка.</param>
        /// <returns>Конвертированное число.</returns>
        public static int ToInt(this string str)
            => int.TryParse(str, out var result) ? result : 0;       
    }
}

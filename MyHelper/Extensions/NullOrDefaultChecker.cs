using System.Collections.Generic;
using System;
using System.Linq;

namespace MyHelper.Extensions
{
    /// <summary>
    /// Классы расширений для проверки объекта на дефолтное значение.
    /// </summary>
    public static class NullOrDefaultChecker
    {
        /// <summary>
        /// Получение признака пустой даты.
        /// </summary>
        /// <param name="date">Дата.</param>
        /// <returns>Признак пустой даты.</returns>
        public static bool IsNullOrDefault(this DateTime date)
        {
            return date.Year <= 1900 || date.Year >= 2222;
        }

        /// <summary>
        /// Получение признака пустой даты или null.
        /// </summary>
        /// <param name="date">Дата.</param>
        /// <returns>Признак пустой даты.</returns>
        public static bool IsNullOrDefault(this DateTime? date)
        {
            return date == null || date.Value.IsNullOrDefault();
        }

        /// <summary>
        /// Получение признака пустой строки.
        /// </summary>
        /// <param name="str">Строка.</param>
        /// <returns>Признак пустой строки.</returns>
        public static bool IsNullOrDefault(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// Проверка объекта на null или дефолтное значение для структур.
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="obj">Объект для проверки.</param>
        /// <returns>Признак дефолтного объекта.</returns>
        public static bool IsNullOrDefault<T>(this T obj)
            where T : struct
        {
            return obj.Equals(default(T));
        }

        /// <summary>
        /// Проверка объекта на null или дефолтное значение для объектов класса.
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="obj">Объект для проверки.</param>
        /// <returns>Признак дефолтного объекта.</returns>
        public static bool IsNullOrDefault<T>(this T? obj)
            where T : struct
        {
            return obj == null || obj.Value.IsNullOrDefault();
        }

        /// <summary>
        /// Проверка списка на null или пустое значение.
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="obj">Объект для проверки.</param>
        /// <returns>Признак дефолтного объекта.</returns>
        public static bool IsNullOrDefault<T>(this ICollection<T> obj)
        {
            return obj == null || obj.Count == 0;
        }

        /// <summary>
        /// Проверка списка на null или пустое значение.
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="obj">Объект для проверки.</param>
        /// <returns>Признак дефолтного объекта.</returns>
        public static bool IsNullOrDefault<T>(this IEnumerable<T> obj)
        {
            return obj == null || !obj.Any();
        }
    }
}

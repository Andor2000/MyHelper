using MyHelper.Models.Dto;
using System.Diagnostics;
using System.IO;

namespace MyHelper.Services
{
    /// <summary>
    /// Сервис для сохранения скрипта в файл.
    /// </summary>
    public static class SaveScriptService
    {
        /// <summary>
        /// Сохранить скрипт.
        /// </summary>
        /// <param name="dto">Модель сохранения скрипта.</param>
        public static void SaveScript(SaveScriptModelDto dto)
        {
            var path = dto.Path.Text + @"\" + dto.Sprint.Text + @"\" +
                (dto.Task.Text.Contains("-")
                    ? dto.Task.Text.Substring(0, dto.Task.Text.IndexOf('-'))
                    : dto.Task.Text);

            Directory.CreateDirectory(path);

            var pathFile = path + @"\" + GetFileName(dto);
            File.WriteAllText(pathFile, GetEndScriptWithTemplate(dto));

            if (dto.IsOpenFile)
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "explorer",
                    Arguments = $"/n, /select, {pathFile}"
                });
            }
        }

        /// <summary>
        /// Получить итоговое наименование файла.
        /// </summary>
        /// <param name="dto">Модель сохранения скрипта.</param>
        /// <returns></returns>
        public static string GetFileName(SaveScriptModelDto dto)
        {
            return dto.Sprint.Text.Replace(".", string.Empty).Replace(" ", string.Empty).PadRight(4, '0') +
                 $"{dto.Number.Text.PadLeft(3, '0')}. " +
                 $"{dto.Project.Text}. " +
                 $"({dto.Task.Text}). " +
                 $"{dto.Description.Text}.sql";
        }

        /// <summary>
        /// Получение конечного скрипта с шаблоном.
        /// </summary>
        /// <param name="dto">Модель сохранения скрипта.</param>
        /// <returns>Скрипт с шаблоном.</returns>
        public static string GetEndScriptWithTemplate(SaveScriptModelDto dto)
            => string.Format(
                BuildingScript.TemplateScriptSoftrust,
                dto.Guid,
                dto.Task,
                dto.Description,
                dto.Script);
    }
}

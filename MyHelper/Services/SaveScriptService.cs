using MyHelper.Models.Dto;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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
            try
            {


                var pathBuilder = new StringBuilder()
                    .Append(dto.Path + @"\" + dto.Sprint);

                if (dto.IsCreateSubFolder == "1")
                {
                    pathBuilder = pathBuilder
                        .Append(@"\")
                        .Append(dto.Task.Contains("-")
                            ? dto.Task.Substring(0, dto.Task.IndexOf('-'))
                            : dto.Task);
                }

                var path = pathBuilder.ToString();
                Directory.CreateDirectory(path);

                var pathFile = path + @"\" + GetFileName(dto);
                File.WriteAllText(pathFile, GetEndScriptWithTemplate(dto), Encoding.GetEncoding("windows-1251"));

                if (dto.IsOpenFile == "1")
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "explorer",
                        Arguments = $"/n, /select, {pathFile}"
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка сохранения скрипта");
            }
        }

        /// <summary>
        /// Получить итоговое наименование файла.
        /// </summary>
        /// <param name="dto">Модель сохранения скрипта.</param>
        /// <returns></returns>
        public static string GetFileName(SaveScriptModelDto dto)
        {
            return dto.Sprint.Replace(".", string.Empty).Replace(" ", string.Empty).PadRight(4, '0') +
                 $"{dto.Number.PadLeft(3, '0')}. " +
                 $"{dto.Project}. " +
                 $"({dto.Task}). " +
                 $"{dto.Description}.sql";
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

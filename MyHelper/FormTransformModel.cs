using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MyHelper
{
    public partial class FormTransformModel : Form
    {
        public FormTransformModel()
        {
            InitializeComponent();
        }

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            richTextBox2.Text = this.ConvertModel(richTextBox1.Text);
        }

        // Метод преобразования C# модели в TypeScript-модель
        private string ConvertModel(string input)
        {
            // Извлекаем комментарий класса (если есть)
            string classComment = "";
            var classCommentMatch = Regex.Match(input, @"(?<classComment>(\s*///.*\n)+)\s*public\s+class");
            if (classCommentMatch.Success)
            {
                classComment = TransformComment(classCommentMatch.Groups["classComment"].Value);
            }

            // Попытка извлечь объявление класса
            var classDeclMatch = Regex.Match(input, @"public\s+class\s+(?<className>\w+)\s*(?::\s*(?<baseClasses>[^{]+))?\s*{");
            string header = "";
            string footer = "";
            if (classDeclMatch.Success)
            {
                string backendClassName = classDeclMatch.Groups["className"].Value;
                string tsClassName = TransformClassName(backendClassName);

                // Обрабатываем базовые классы – оставляем только те, что не начинаются на "I"
                string baseClassesStr = classDeclMatch.Groups["baseClasses"].Value;
                List<string> baseClasses = new List<string>();
                if (!string.IsNullOrWhiteSpace(baseClassesStr))
                {
                    var parts = baseClassesStr.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var part in parts)
                    {
                        string trimmed = part.Trim();
                        if (trimmed.StartsWith("I"))
                            continue;
                        baseClasses.Add(TransformBaseClass(trimmed));
                    }
                }
                string extendsStr = "";
                if (baseClasses.Count > 0)
                {
                    extendsStr = " extends " + string.Join(", ", baseClasses);
                }
                header = $"export class {tsClassName}{extendsStr} {{";
                footer = "}";
            }

            // Поиск всех свойств
            var propertyPattern = @"(?<propComment>(\s*///.*\n)+)?\s*public\s+(?<type>[\w<>\.,\s]+)\s+(?<propName>\w+)\s*(?:{[^}]*}|=>\s*[^;]+;)";
            var propMatches = Regex.Matches(input, propertyPattern);
            List<Property> properties = new List<Property>();

            foreach (Match m in propMatches)
            {
                string rawComment = m.Groups["propComment"].Value;
                string comment = TransformComment(rawComment);
                string type = m.Groups["type"].Value.Trim();
                string originalPropName = m.Groups["propName"].Value.Trim();
                string tsPropName = TransformPropertyName(originalPropName);
                string tsType = TransformType(type, originalPropName);
                string defaultValue = GetDefaultValue(tsType);

                properties.Add(new Property
                {
                    Comment = comment,
                    OriginalName = originalPropName,
                    Name = tsPropName,
                    Type = tsType,
                    DefaultValue = defaultValue
                });
            }

            // Формирование результирующего кода
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(header))
            {
                sb.AppendLine(header);
                sb.AppendLine();
            }
            foreach (var prop in properties)
            {
                if (!string.IsNullOrEmpty(prop.Comment))
                {
                    sb.Append(prop.Comment.TrimEnd());
                    sb.AppendLine();
                }
                // Если тип не примитивный – добавляем декоратор @Type
                if (!IsPrimitiveType(prop.Type))
                {
                    sb.AppendLine($"  @Type(() => {GetBaseTypeName(prop.Type)})");
                }
                sb.AppendLine($"  public {prop.Name}: {prop.Type} = {prop.DefaultValue};");
                sb.AppendLine();
            }
            if (!string.IsNullOrEmpty(footer))
            {
                sb.AppendLine(footer);
            }
            return sb.ToString();
        }

        // Преобразует XML-комментарии (/// <summary>...</summary>) в формат JSDoc (/** ... */)
        private string TransformComment(string rawComment)
        {
            if (string.IsNullOrWhiteSpace(rawComment))
                return "";

            string[] lines = rawComment.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("  /**");
            foreach (var line in lines)
            {
                string trimmed = line.Trim();
                trimmed = Regex.Replace(trimmed, @"^///\s*", "");
                trimmed = trimmed.Replace("<summary>", "").Replace("</summary>", "").Trim();
                if (!string.IsNullOrEmpty(trimmed))
                    sb.AppendLine("   * " + trimmed);
            }
            sb.AppendLine("   */");
            return sb.ToString();
        }

        // Если имя класса заканчивается на Dto, заменяем окончание на Model
        private string TransformClassName(string name)
        {
            if (name.EndsWith("Dto"))
                return name.Substring(0, name.Length - 3) + "Model";
            return name;
        }

        // Обработка базового класса: если имя начинается с "Base" – удаляем префикс; если оканчивается на "Model" – прибавляем "Base"
        private string TransformBaseClass(string name)
        {
            string result = name;
            if (result.StartsWith("Base"))
                result = result.Substring(4);
            if (result.EndsWith("Model"))
                result = result + "Base";
            return result;
        }

        // Преобразуем имя свойства: первый символ в нижний регистр и замена "Favourite" на "favorite"
        private string TransformPropertyName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return name;
            string result = char.ToLower(name[0]) + name.Substring(1);
            result = Regex.Replace(result, "favourite", "favorite", RegexOptions.IgnoreCase);
            return result;
        }

        // Преобразуем тип свойства: заменяем окончание Dto на Model, маппинг обобщённых типов и т.п.
        private string TransformType(string type, string propName)
        {
            type = type.Trim();

            // Обработка обобщённых типов, например: ReferenceCodeValue<string>
            var genericMatch = Regex.Match(type, @"^(?<gen>\w+)\s*<\s*(?<arg>.+?)\s*>$");
            if (genericMatch.Success)
            {
                string genericName = genericMatch.Groups["gen"].Value;
                string arg = genericMatch.Groups["arg"].Value.Trim();
                if (genericName.Equals("ReferenceCodeValue"))
                {
                    return $"ValueDictionary<{arg}>";
                }
                else
                {
                    string transformedArg = TransformType(arg, propName);
                    if (genericName.EndsWith("Dto"))
                        genericName = genericName.Substring(0, genericName.Length - 3) + "Model";
                    return $"{genericName}<{transformedArg}>";
                }
            }

            // Примитивные типы
            switch (type)
            {
                case "string":
                    return "string";
                case "bool":
                    return "boolean";
                case "int":
                    return "number";
            }

            // Если тип заканчивается на Dto, заменяем окончание на Model
            if (type.EndsWith("Dto"))
                type = type.Substring(0, type.Length - 3) + "Model";

            return type;
        }

        // Возвращает значение по умолчанию для TypeScript-типа
        private string GetDefaultValue(string tsType)
        {
            switch (tsType)
            {
                case "string":
                    return "''";
                case "boolean":
                    return "false";
                case "number":
                    return "0";
                default:
                    return $"new {tsType}()";
            }
        }

        // Проверяет, является ли TS-тип примитивным
        private bool IsPrimitiveType(string tsType)
        {
            string baseType = GetBaseTypeName(tsType);
            return baseType == "string" || baseType == "boolean" || baseType == "number";
        }

        // Возвращает базовое имя типа (без обобщённых параметров)
        private string GetBaseTypeName(string tsType)
        {
            int index = tsType.IndexOf('<');
            if (index >= 0)
                return tsType.Substring(0, index);
            return tsType;
        }

        // Вспомогательный класс для хранения информации о свойстве
        private class Property
        {
            public string Comment { get; set; }
            public string OriginalName { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
            public string DefaultValue { get; set; }
        }

        private void pictureBoxOpenFormQuotes_Click(object sender, EventArgs e)
        {
            var from = new FormQuotes();
            from.Show();
            this.Close();
        }
    }
}

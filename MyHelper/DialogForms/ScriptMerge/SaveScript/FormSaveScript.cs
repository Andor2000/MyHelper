using MyHelper.DialogForms.ScriptMerge.SaveScript;
using MyHelper.Enums;
using MyHelper.Models.Dto;
using MyHelper.NewPanelComponent;
using MyHelper.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MyHelper.DialogForms.ScriptMerge
{
    public partial class FormSaveScript : Form
    {
        /// <summary>
        /// Модель для сохранения скрипта.
        /// </summary>
        private SaveScriptModelDto _saveScriptModelDto;

        /// <summary>
        /// Список текстовых полей.
        /// </summary>
        private List<TextBoxEx> _textBoxList = new List<TextBoxEx>();

        /// <summary>
        /// Серивс для работы с базой данных.
        /// </summary>
        private DataBaseService _dataBaseService { get; set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="saveScriptModelDto">Модель для сохранения скрипта.</param>
        /// <param name="dataBaseService">Серивс для работы с базой данных.</param>
        public FormSaveScript(
            SaveScriptModelDto saveScriptModelDto,
            DataBaseService dataBaseService)
        {
            _saveScriptModelDto = saveScriptModelDto;
            _dataBaseService = dataBaseService;
            InitializeComponent();

            this.InitTextBox();
            textBoxFileName.Text = SaveScriptService.GetFileName(_saveScriptModelDto);
            richTextBox1.Text = _saveScriptModelDto.Script;
        }

        /// <summary>
        /// Сгенерировать гуид.
        /// </summary>
        private void buttonUpdateGuid_Click(object sender, EventArgs e)
        {
            textBoxGuid.Text = Guid.NewGuid().ToString().ToUpper();
            _saveScriptModelDto.Guid = textBoxGuid.Text;

            textBoxGuid.BackColor = Colors.BackColorCorrectText;
            textBoxGuid.ForeColor = Colors.FontPlaceholderBlack;
            textBoxGuid.IsValid = true;
        }

        /// <summary>
        /// Обзор папок.
        /// </summary>
        private void buttonReview_Click(object sender, EventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            dialog.SelectedPath = textBoxPath.Text;

            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            _saveScriptModelDto.Path = dialog.SelectedPath;
            textBoxPath.Text = _saveScriptModelDto.Path;
            textBoxPath.BackColor = Colors.BackColorCorrectText;
            textBoxPath.ForeColor = Colors.FontPlaceholderBlack;
            textBoxPath.IsValid = true;
        }

        /// <summary>
        /// Сохранение скрипта.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSaveScript_Click(object sender, EventArgs e)
        {
            if (this.ValidateFormSaveScript())
            {
                if (checkBox3.Checked)
                {
                    var formDialog = new FormCreateCommit(
                        _saveScriptModelDto.Path,
                        _saveScriptModelDto.Task,
                        _saveScriptModelDto.Description);

                    formDialog.ShowDialog();
                }

                _saveScriptModelDto.Script = richTextBox1.Text;
                SaveScriptService.SaveScript(_saveScriptModelDto);
                _dataBaseService.SaveSettingScript(_saveScriptModelDto);
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Нажал на текстовое поле.
        /// </summary>
        private void GotFocusRemoveText_TextBox(object sender, EventArgs e)
        {
            var tb = (TextBoxEx)sender;
            tb.BackColor = Colors.BackColorCorrectText;

            if (tb.IsEmpty)
            {
                tb.Text = string.Empty;
                tb.ForeColor = Colors.FontPlaceholderBlack;
            }
        }

        /// <summary>
        /// Убрал фокус с текстового поля.
        /// </summary>
        private void LostFocusAddText_TextBox(object sender, EventArgs e)
        {
            this.CheckValid((TextBoxEx)sender);
        }

        /// <summary>
        /// Проверка на валидность.
        /// </summary>
        private void CheckValid(TextBoxEx tb)
        {
            tb.Text = tb.Text.Trim();
            if (tb.Text == string.Empty)
            {
                tb.IsEmpty = true;
                tb.IsValid = false;
                tb.Text = tb.Placeholder;
                tb.ForeColor = Colors.FontPlaceholderGrey;
            }
            else
            {
                tb.IsEmpty = false;
                tb.IsValid = this.CheckIsValid(tb);
            }
        }

        /// <summary>
        /// Валидация формы. Подсветка полей.
        /// </summary>
        /// <returns></returns>
        private bool ValidateFormSaveScript()
        {
            this._textBoxList.Where(x => !x.IsValid).ToList()
                .ForEach(x => x.BackColor = Colors.BackColorNotCorrectText);

            return _textBoxList.All(x => x.IsValid);
        }

        private void textBoxSprint_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Разрешаем только цифры и управляющие символы (например, Backspace)
            e.Handled = !char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar);

            if (char.IsControl(e.KeyChar) && (textBoxSprint.Text.Length == 2 || textBoxSprint.Text.Length == 5))
            {
                textBoxSprint.Text = textBoxSprint.Text.Substring(0, textBoxSprint.Text.Length - 1);
            }
        }

        /// <summary>
        /// Маска для номера спринта.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxSprint_TextChanged(object sender, EventArgs e)
        {
            var rawText = textBoxSprint.Text.Replace(".", "");
            // Форматируем текст по шаблону "x.xx.x"
            if (rawText.Length > 1)
            {
                rawText = rawText.Insert(1, ".");
            }
            if (rawText.Length > 4)
            {
                rawText = rawText.Insert(4, ".");
            }

            // Обновляем текстовое поле без изменения позиции курсора
            textBoxSprint.TextChanged -= textBoxSprint_TextChanged;
            textBoxSprint.Text = rawText;
            textBoxSprint.SelectionStart = rawText.Length;
            textBoxSprint.TextChanged += textBoxSprint_TextChanged;

        }

        /// <summary>
        /// Инициализация текстовых полей.
        /// </summary>
        private void InitTextBox()
        {
            textBoxPath.Text = _saveScriptModelDto.Path;
            textBoxGuid.Text = _saveScriptModelDto.Guid;
            textBoxSprint.Text = _saveScriptModelDto.Sprint;
            textBoxTask.Text = _saveScriptModelDto.Task;
            textBoxProject.Text = _saveScriptModelDto.Project;
            textBoxNumber.Text = _saveScriptModelDto.Number;
            textBoxDescription.Text = _saveScriptModelDto.Description;
            checkBox2.Checked = _saveScriptModelDto.IsOpenFile == "1";
            checkBox1.Checked = _saveScriptModelDto.IsCreateSubFolder == "1";

            this._textBoxList.AddRange(new List<TextBoxEx>
            {
                textBoxPath,
                textBoxGuid,
                textBoxSprint,
                textBoxTask,
                textBoxProject,
                textBoxNumber,
                textBoxDescription
            });

            this._textBoxList.ForEach(x =>
            {
                x.GotFocus += GotFocusRemoveText_TextBox;
                x.LostFocus += LostFocusAddText_TextBox;
                this.CheckValid(x);
            });
        }

        /// <summary>
        /// Номер скрипта.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxNumber_KeyPress(object sender, KeyPressEventArgs e)
            => e.Handled = !char.IsDigit(e.KeyChar) && e.KeyChar != 8;

        /// <summary>
        /// Ввод текста в поля =
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateNameFileKeyDownAndUp(object sender, KeyEventArgs e)
        {
            switch ((TextBoxEx)sender)
            {
                case var x when x == textBoxPath:
                    _saveScriptModelDto.Path = x.Text;
                    break;
                case var x when x == textBoxGuid:
                    _saveScriptModelDto.Guid = x.Text;
                    break;
                case var x when x == textBoxSprint:
                    _saveScriptModelDto.Sprint = x.Text;
                    break;
                case var x when x == textBoxTask:
                    _saveScriptModelDto.Task = x.Text;
                    break;
                case var x when x == textBoxProject:
                    _saveScriptModelDto.Project = x.Text;
                    break;
                case var x when x == textBoxNumber:
                    _saveScriptModelDto.Number = x.Text;
                    break;
                case var x when x == textBoxDescription:
                    _saveScriptModelDto.Description = x.Text;
                    break;
                default:
                    throw new Exception("Добавь сюда новый компонент.");
            }

            textBoxFileName.Text = SaveScriptService.GetFileName(_saveScriptModelDto);
        }

        /// <summary>
        /// Проверка валидности текстового поля.
        /// </summary>
        /// <param name="tb">Текстовое поле.</param>
        /// <returns>Признак валидности текстового поля.</returns>
        private bool CheckIsValid(TextBoxEx tb)
        {
            switch (tb)
            {
                case var x when x == textBoxPath:
                    return !string.IsNullOrWhiteSpace(tb.Text) && Directory.Exists(tb.Text);
                case var x when x == textBoxGuid:
                    return Guid.TryParse(tb.Text, out _);
                case var x when x == textBoxSprint:
                    return tb.Text.Count(t => t != '.' && t != ' ') == 4;
                case var x when x == textBoxTask:
                    return Regex.IsMatch(tb.Text, @"^[^\s\d-]+-\d+$");
                default:
                    return !string.IsNullOrWhiteSpace(tb.Text);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            _saveScriptModelDto.IsCreateSubFolder = checkBox1.Checked ? "1" : "0";
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            _saveScriptModelDto.IsOpenFile = checkBox2.Checked ? "1" : "0";
        }
    }
}

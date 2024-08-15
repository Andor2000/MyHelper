using MyHelper.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace MyHelper.DialogForms.ScriptMerge.SaveScript
{
    public partial class FormCreateCommit : Form
    {
        /// <summary>
        /// Задача.
        /// </summary>
        public string _path { get; set; }

        /// <summary>
        /// Задача.
        /// </summary>
        public string _task { get; set; }

        /// <summary>
        /// Сообщение коммита.
        /// </summary>
        public string _description { get; set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="task">Задача.</param>
        /// <param name="description">Сообщение коммита.</param>
        public FormCreateCommit(
            string path,
            string task,
            string description)
        {
            InitializeComponent();
            this._path = path;
            this._task = task;
            this._description = description;

            richTextBox1.Text = $"{_task}. {_description}";
            richTextBox1.GotFocus += richTextBox1_GotFocus;
        }

        /// <summary>
        /// Сделать коммит.
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            if (!richTextBox1.Text.Contains(_task))
            {
                richTextBox1.BackColor = Colors.BackColorNotCorrectText;
                return;
            }
            // git branch -a --list "dev*"
            // Путь к вашему репозиторию Git
            string repoPath = @"C:\Path\To\Your\Repo";

            // Название ветки, на которую нужно переключиться и создать новую ветку
            string baseBranch = "main";  // Основная ветка
            string newBranch = "feature/new-feature";  // Новая ветка

            /// Выполняем команды последовательно
            // Переключение на ветку:
            ExecuteGitCommand(repoPath, $"git checkout {baseBranch}");
            // Обновление до актуальной версии:
            ExecuteGitCommand(repoPath, $"git pull origin {baseBranch}");
            // Создание новой ветки:
            ExecuteGitCommand(repoPath, $"git checkout -b {newBranch}");
            // Добавление изменений и создание коммита:
            ExecuteGitCommand(repoPath, "git add .");
            ExecuteGitCommand(repoPath, $"git commit -m {richTextBox1.Text}");
            // Отправка изменений в удаленный репозиторий:
            ExecuteGitCommand(repoPath, $"git push origin {newBranch}");
        }

        /// <summary>
        /// Отмена.
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void richTextBox1_GotFocus(object sender, EventArgs e)
        {
            richTextBox1.BackColor = Colors.BackColorCorrectText;
        }

        private void ExecuteGitCommand(string repoPath, string command)
        {
            if(this.DialogResult == DialogResult.Cancel)
            {
                return;
            }

            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                WorkingDirectory = repoPath,
                FileName = "cmd.exe",
                Arguments = $"/C {command}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = new Process { StartInfo = processStartInfo })
            {
                process.Start();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (!string.IsNullOrEmpty(error))
                {
                    this.DialogResult = DialogResult.Cancel;
                    MessageBox.Show(error);
                }
            }
        }
    }
}

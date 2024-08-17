using MyHelper.Enums;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace MyHelper.DialogForms.ScriptMerge.SaveScript
{
    public partial class FormCreateCommit : Form
    {
        /// <summary>
        /// Задача.
        /// </summary>
        private string _path { get; set; }

        /// <summary>
        /// Задача.
        /// </summary>
        private string _task { get; set; }

        /// <summary>
        /// Сообщение коммита.
        /// </summary>
        private string _description { get; set; }

        /// <summary>
        /// Имя файла.
        /// </summary>
        private string _fileName { get; set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="path">Путь к папке.</param>
        /// <param name="task">Задача.</param>
        /// <param name="description">Сообщение коммита.</param>
        /// <param name="fileName">Имя файла.</param>
        public FormCreateCommit(
            string path,
            string task,
            string description,
            string fileName)
        {
            InitializeComponent();
            this._path = path;
            this._task = task;
            this._description = description;
            this._fileName = fileName;

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
            string baseBranch = "dev";  // Основная ветка
            string newBranch = "DISP-9210";  // Новая ветка

            /// Выполняем команды последовательно
            // Переключение на ветку:
            ExecuteGitCommand($"git checkout {baseBranch}");
            // Обновление до актуальной версии:
            ExecuteGitCommand($"git pull origin {baseBranch}");
            // Создание новой ветки:
            ExecuteGitCommand($"git checkout -b {newBranch}");
            // Добавление изменений и создание коммита:
            ExecuteGitCommand($"git add {_fileName}");
            ExecuteGitCommand($"git commit -m {richTextBox1.Text}");
            // Отправка изменений в удаленный репозиторий:
            ExecuteGitCommand($"git push origin {newBranch}");
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

        private string ExecuteGitCommand(string command)
        {
            if (this.DialogResult == DialogResult.Cancel)
            {
                return string.Empty;
            }

            var processStartInfo = new ProcessStartInfo
            {
                WorkingDirectory = _path,
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
                string result = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    this.DialogResult = DialogResult.Cancel;
                    MessageBox.Show(error);
                }

                return result;
            }
        }
    }
}

using System;
using System.Windows.Forms;

namespace MyHelper.DialogForms
{
    public partial class FormAddDataBase : Form
    {
        /// <summary>
        /// Название таблицы.
        /// </summary>
        public string Server { get; private set; } = string.Empty;

        /// <summary>
        /// База.
        /// </summary>
        public string DataBase { get; private set; } = string.Empty;

        /// <summary>
        /// Логин.
        /// </summary>
        public string Login { get; private set; } = string.Empty;

        /// <summary>
        /// Пароль.
        /// </summary>
        public string Password { get; private set; } = string.Empty;

        public FormAddDataBase(string server, string dataBase, string login, string password)
        {
            InitializeComponent();

            textBox1.Text = this.Server = server;
            textBox2.Text = this.DataBase = dataBase;
            textBox3.Text = this.Login = login;
            textBox4.Text = this.Password = password;

            label2.Visible = false;
            label3.Visible = false;
            label5.Visible = false;
            label7.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //label2.Visible = string.IsNullOrWhiteSpace(textBoxPath.Text);
            //label3.Visible = string.IsNullOrWhiteSpace(textBoxGuid.Text);
            //label5.Visible = string.IsNullOrWhiteSpace(textBoxTask.Text);
            //label7.Visible = string.IsNullOrWhiteSpace(textBoxProject.Text);

            this.DialogResult = DialogResult.OK;
            Server = textBox1.Text.Trim();
            DataBase = textBox2.Text.Trim();
            Login = textBox3.Text.Trim();
            Password = textBox4.Text.Trim();
            this.Close();

        }
    
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
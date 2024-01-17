using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyHelper.DialogForms
{
    public partial class FormAddDataBase : Form
    {
        /// <summary>
        /// Название таблицы.
        /// </summary>
        public string ServerName { get; private set; } = string.Empty;

        /// <summary>
        /// Логин.
        /// </summary>
        public string Login{ get; private set; } = string.Empty;

        /// <summary>
        /// Пароль.
        /// </summary>
        public string Password { get; private set; } = string.Empty;

        /// <summary>
        /// Список колонкок.
        /// </summary>
        public string TableNames { get; private set; } = string.Empty;

        public FormAddDataBase()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}

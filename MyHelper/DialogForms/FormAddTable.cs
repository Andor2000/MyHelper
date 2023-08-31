using System;
using System.Windows.Forms;

namespace MyHelper
{
    public partial class FormAddTable : Form
    {
        /// <summary>
        /// Название таблицы.
        /// </summary>
        public string TableName { get; private set; } = string.Empty;

        /// <summary>
        /// Список колонкок.
        /// </summary>
        public string ColomnNames { get; private set; } = string.Empty;

        public FormAddTable()
        {
            InitializeComponent();
            label2.Visible = false;
            label4.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text.Trim();
            richTextBox1.Text = richTextBox1.Text.Trim();

            if (textBox1.Text == string.Empty)
            {
                label2.Visible = true;
            }
            if (richTextBox1.Text == string.Empty)
            {
                label4.Visible = true;
            }

            if (textBox1.Text != string.Empty && richTextBox1.Text != string.Empty)
            {
                TableName = textBox1.Text;
                ColomnNames = richTextBox1.Text;
                this.Close();
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                richTextBox1.Focus();
            }
        }
    }
}
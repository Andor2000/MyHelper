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
            label2.Visible = string.IsNullOrWhiteSpace(textBox1.Text);
            label4.Visible = string.IsNullOrWhiteSpace(richTextBox1.Text);

            if (!string.IsNullOrWhiteSpace(textBox1.Text) && !string.IsNullOrWhiteSpace(richTextBox1.Text))
            {
                TableName = textBox1.Text.Trim();
                ColomnNames = richTextBox1.Text.Trim();
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
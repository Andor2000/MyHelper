using System;
using System.Windows.Forms;

namespace MyHelper
{
    public partial class FormAddColomns : Form
    {
        /// <summary>
        /// Список колонкок.
        /// </summary>
        public string ColomnNames { get; private set; } = string.Empty;

        public FormAddColomns(FormScriptMerge formScriptMerge)
        {
            InitializeComponent();
            label2.Visible = false;
            richTextBox1.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = richTextBox1.Text.Trim();
            if (richTextBox1.Text == string.Empty)
            {
                label2.Visible = true;
            }

            if (richTextBox1.Text != string.Empty)
            {
                ColomnNames = richTextBox1.Text;
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

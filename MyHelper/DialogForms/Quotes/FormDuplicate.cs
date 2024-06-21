using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MyHelper.DialogForms.Quotes
{
    public partial class FormDuplicate : Form
    {
        public FormDuplicate(IEnumerable<string> duplicates)
        {
            InitializeComponent();
            this.richTextBox1.Text = string.Join("\n", duplicates);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

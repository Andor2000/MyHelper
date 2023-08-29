using System;
using System.Windows.Forms;

namespace MyHelper
{
    public partial class FormAddColomns : Form
    {
        private FormScriptMerge formScriptMerge;
        public FormAddColomns(FormScriptMerge formScriptMerge)
        {
            this.formScriptMerge = formScriptMerge;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //formScriptMerge.richTextBox1.Text = richTextBox1.Text;
            this.Close();
        }
    }
}

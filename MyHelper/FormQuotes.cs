using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace MyHelper
{
    public partial class FormQuotes : Form
    {
        public FormQuotes()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox2.Text = string.Empty;
            richTextBox3.Text = string.Empty;
            richTextBox4.Text = string.Empty;
            richTextBox5.Text = string.Empty;
            richTextBox6.Text = string.Empty;
            richTextBox7.Text = string.Empty;

            var texts = FormatingString(
                str: richTextBox1.Text,
                splitSlovo: radioButton2.Checked,
                splitZapiataya: checkBox1.Checked,
                deleteEmpty: checkBox2.Checked,
                deleteDistinct: checkBox3.Checked);

            richTextBox2.Text = string.Join(", ", texts.Select(x => "'" + x + "'"));
            richTextBox4.Text = string.Join(", ", texts.Select(x => "\"" + x + "\""));
            richTextBox6.Text = string.Join(", ", texts);
            richTextBox3.Text = string.Join(",\n", texts.Select(x => "'" + x + "'"));
            richTextBox5.Text = string.Join(",\n", texts.Select(x => "\"" + x + "\""));
            richTextBox7.Text = string.Join(",\n", texts);

            label6.Text = texts.Count().ToString();
        }

        public IEnumerable<string> FormatingString(
            string str,
            bool splitSlovo,
            bool splitZapiataya,
            bool deleteEmpty,
            bool deleteDistinct)
        {
            var splitSeparators = new List<char> { '\t', '\n' };
            if (splitSlovo)
            {
                splitSeparators.Add(' ');
                if (splitZapiataya)
                    splitSeparators.Add(',');
            }

            var texts = str.Split(splitSeparators.ToArray()).Select(x => x.Trim());

            if (deleteEmpty)
                texts = texts.Where(x => x != string.Empty);

            if (deleteDistinct)
                texts = texts.Distinct();

            return texts;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            checkBox1.Enabled = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            checkBox1.Enabled = true;
        }
    }
}
using System;
using System.Windows.Forms;

namespace MyHelper
{
    public partial class FormMain : Form
    {
        /// <summary>
        /// Форма для формирорвания скрипта с merge.
        /// </summary>
        private FormScriptMerge formScript = new FormScriptMerge();

        /// <summary>
        /// Форма для кавычек.
        /// </summary>
        private FormQuotes quotes = new FormQuotes();

        public FormMain()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Кавычки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            quotes.Show();
        }

        /// <summary>
        /// Merge.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            formScript.Show();
        }
    }
}
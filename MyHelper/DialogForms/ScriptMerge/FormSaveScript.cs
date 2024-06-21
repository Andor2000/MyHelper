using System;
using System.Windows.Forms;

namespace MyHelper.DialogForms.ScriptMerge
{
    public partial class FormSaveScript : Form
    {
        public static string _path { get; private set; }
        public static Guid _guid { get; private set; } = new Guid();
        public static string _sprint { get; private set; } = "4600";
        public static string _task { get; private set; } = "DISP-";
        public static string _project { get; private set; } = "Диспансеризация";
        public static int _number { get; private set; } = 1;
        public static string _description { get; private set; } = "Название";
        public static bool _openFile { get; private set; } = true;

        public FormSaveScript()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            _guid = Guid.NewGuid();
            textBox2.Text = _guid.ToString().ToUpper();
        }
    }
}

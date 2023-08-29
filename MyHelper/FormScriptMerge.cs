using MyHelper.Dto;
using MyHelper.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MyHelper
{
    public partial class FormScriptMerge : Form
    {
        /// <summary>
        /// Активная таблица.
        /// </summary>
        private TableModelObjPanel _mainTable { get; set; } = new TableModelObjPanel();

        /// <summary>
        /// Активный столбец
        /// </summary>
        private ColomnModelObjPanel _mainColomn { get; set; } = new ColomnModelObjPanel();

        /// <summary>
        /// Левая колонка с задачами.
        /// </summary>
        private List<TableModelObjPanel> _panelTableLeft { get; set; } = new List<TableModelObjPanel>();

        private string EndScriptTable { get; set; } = string.Empty;
        private string EndScriptRecord { get; set; } = string.Empty;
        private string EndScriptColomn { get; set; } = string.Empty;

        /// <summary>
        /// Сервис кавычек.
        /// </summary>
        private FormQuotes _formQuotesService { get; set; } = new FormQuotes();

        public FormScriptMerge()
        {
            InitializeComponent();
            lineNumberRTB1.RichTextBox.BackColor = Colors.PanelFon;
            lineNumberRTB1.RichTextBox.ForeColor = Color.White;
            lineNumberRTB1.RichTextBox.WordWrap = false;
            lineNumberRTB1.RichTextBox.KeyDown += lineNumberRTB1_KeyDown;
            lineNumberRTB1.RichTextBox.KeyUp += lineNumberRTB1_KeyUp;

            panel3.HorizontalScroll.Maximum = 0;
            panel3.AutoScroll = false;
            panel3.VerticalScroll.Visible = false;
            panel3.AutoScroll = true;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            panel5.BackColor = panel5.Parent.BackColor;
            var formAddTableName = new FormAddTable();
            formAddTableName.ShowDialog();

            if (formAddTableName.TableName == string.Empty || formAddTableName.ColomnNames == string.Empty)
            {
                return;
            }

            var table = new TableModelObjPanel();
            _panelTableLeft.Add(table);
            table.Sort = _panelTableLeft.Count();
            table.Icon.Image = Image.FromFile(@"images\icons\quest.png");
            table.TextBox.Text = formAddTableName.TableName;

            table.TextBox.MouseMove += MouseMove;      // Курсор на объекте, навелся, перетаскивание.
            table.TextBox.MouseDown += MouseDown;      // Нажал левую кнопку мыши, не отпустил \ перетаскивание
            table.TextBox.MouseUp += MouseUp;          // Клинкул по объекту (отпустил мышь)
            table.TextBox.MouseLeave += MouseLeave;    // Отвел курсор с объекта.

            // получение названий колонок.
            var colomNames = _formQuotesService.FormatingString(formAddTableName.ColomnNames, true, true, true, true);
            foreach (var colom in colomNames)
            {
                var colomn = new ColomnModelObjPanel();
                colomn.Sort = colomNames.Count() - table.Colomns.Count();
                colomn.Icon.Image = Image.FromFile(@"images\icons\pencil.png");
                colomn.TextBox.Text = colom;
                table.Colomns.Add(colomn);

                colomn.TextBox.MouseMove += MouseMove;      // Курсор на объекте, навелся, перетаскивание.
                colomn.TextBox.MouseDown += MouseDown;      // Нажал левую кнопку мыши, не отпустил \ перетаскивание
                colomn.TextBox.MouseUp += MouseUp;          // Клинкул по объекту (отпустил мышь)
                colomn.TextBox.MouseLeave += MouseLeave;    // Отвел курсор с объекта.
            }

            this.SetMainTable(table);
            this.UpdatePanelTable();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            var formDialog = new FormAddColomns(this);
            formDialog.ShowDialog();
        }

        /// <summary>
        /// Курсор на объекте, навелся, перетаскивание.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseMove(object sender, EventArgs e)
        {
            if ((TextBox)sender == _mainTable.TextBox || (TextBox)sender == _mainColomn.TextBox)
            {
                return;
            }

            ((TextBox)sender).BackColor = Colors.PanelMouseMoveObject;
            ((TextBox)sender).Parent.BackColor = Colors.PanelMouseMoveObject;
        }

        /// <summary>
        /// Клинкул по объекту (отпустил мышь)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseUp(object sender, EventArgs e)
        {
            if ((TextBox)sender == _mainTable.TextBox || (TextBox)sender == _mainColomn.TextBox)
            {
                return;
            }

            _mainTable.EndScript = richTextBox3.Text;
            _mainColomn.Records = lineNumberRTB1.RichTextBox.Text;

            var colomn = _mainTable.Colomns.FirstOrDefault(x => x.TextBox == (TextBox)sender);
            if (colomn != null)
            {
                this.SetMainColomn(colomn);
            }
            else
            {
                var table = _panelTableLeft.FirstOrDefault(x => x.TextBox == (TextBox)sender);
                colomn = table.Colomns.FirstOrDefault(x => x.Sort == table.Colomns.Count());
                this.SetMainTable(table);
                this.UpdatePanelColomn();
            }

        }

        /// <summary>
        /// Нажал левую кнопку мыши, не отпустил \ перетаскивание
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ((TextBox)sender).Parent.Focus(); // убираем каретку
            }
        }

        /// <summary>
        /// Отвел курсор с объекта.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseLeave(object sender, EventArgs e)
        {
            if ((TextBox)sender == _mainTable.TextBox || (TextBox)sender == _mainColomn.TextBox)
            {
                return;
            }

            ((TextBox)sender).BackColor = Colors.PanelFon;
            ((TextBox)sender).Parent.BackColor = Colors.PanelFon;
        }


        private void navel_na_ikonky_MouseMove(object sender, MouseEventArgs e)     // навел на иконку              
        {
            ((PictureBox)sender).Parent.BackColor = Colors.PanelMouseMoveObject;
        }
        private void navel_na_ikonky_MouseLeave(object sender, EventArgs e)         // убрал мышку с иконки         
        {
            ((PictureBox)sender).Parent.BackColor = ((PictureBox)sender).Parent.Parent.BackColor;
        }
        private void navel_na_ikonky_MouseDown(object sender, MouseEventArgs e)     // нажал на мышь (не отпустил)  
        {
            ((PictureBox)sender).Parent.BackColor = Colors.PanelActiveObject;
        }
        private void navel_na_ikonky_MouseUp(object sender, MouseEventArgs e)       // отпустил мышь                
        {
            ((PictureBox)sender).Parent.BackColor = Colors.PanelFon;
        }

        private void SetMainTable(TableModelObjPanel newMainTable)
        {
            _mainTable.Panel.BackColor = Colors.PanelFon;
            _mainTable.TextBox.BackColor = Colors.PanelFon;
            _mainTable = newMainTable;
            _mainTable.Panel.BackColor = Colors.PanelActiveObject;
            _mainTable.TextBox.BackColor = Colors.PanelActiveObject;

            this.UpdateEndScriptTable();
            this.UpdateEndScriptColomn();
            this.UpdateEndScriptRecord();

            var newMainColomn = _mainTable.Colomns.OrderByDescending(x => x.Sort).FirstOrDefault();
            this.SetMainColomn(newMainColomn);
        }

        private void SetMainColomn(ColomnModelObjPanel newMainColomn)
        {
            _mainColomn.Panel.BackColor = Colors.PanelFon;
            _mainColomn.TextBox.BackColor = Colors.PanelFon;
            _mainColomn.TextBoxCount.BackColor = Colors.PanelFon;
            _mainColomn = newMainColomn;
            _mainColomn.Panel.BackColor = Colors.PanelActiveObject;
            _mainColomn.TextBox.BackColor = Colors.PanelActiveObject;
            _mainColomn.TextBoxCount.BackColor = Colors.PanelActiveObject;

            lineNumberRTB1.RichTextBox.Text = _mainColomn.Records;
        }

        private void UpdatePanelTable()
        {
            panel3.Controls.Clear();
            var tables = _panelTableLeft.OrderByDescending(x => x.Sort);

            foreach (var table in tables)
            {
                table.Panel.Location = new Point(0, (_panelTableLeft.Count() - table.Sort) * SizeEnums.HeightPanel);
                panel3.Controls.Add(table.Panel);
            }

            this.UpdatePanelColomn();
        }

        private void UpdatePanelColomn()
        {
            panel2.Controls.Clear();
            var colomns = _mainTable.Colomns.OrderByDescending(x => x.Sort);

            foreach (var colomn in colomns)
            {
                colomn.Panel.Location = new Point(0, (_mainTable.Colomns.Count() - colomn.Sort) * SizeEnums.HeightPanel);
                panel2.Controls.Add(colomn.Panel);
            }
        }

        private void lineNumberRTB1_KeyDown(object sender, KeyEventArgs e)
        {
            RewriteCountRecord(e);
        }

        private void lineNumberRTB1_KeyUp(object sender, KeyEventArgs e)
        {
            RewriteCountRecord(e);
            this.UpdateEndScriptRecord();
        }

        /// <summary>
        /// Пересчитать количество записей в таблице.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private void RewriteCountRecord(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter ||
                e.KeyCode == Keys.Delete ||
                e.KeyCode == Keys.Back ||
                e.Control && e.KeyCode == Keys.Z ||
                e.Control && e.KeyCode == Keys.V ||
                e.Control && e.KeyCode == Keys.A && e.KeyCode == Keys.Delete ||
                e.Control && e.KeyCode == Keys.A && e.KeyCode == Keys.Back)
            {
                _mainColomn.CountRecords = lineNumberRTB1.RichTextBox.Lines.Length;
                _mainColomn.TextBoxCount.Text = _mainColomn.CountRecords.ToString();
                if (_mainColomn.TextBoxCount.Text == "0")
                {
                    _mainColomn.TextBoxCount.Text = "1";
                }
            }
        }


        private void UpdateEndScriptTable()
        {
            EndScriptTable = string.Format(BuildingScript.Table, _mainTable.TextBox.Text);
        }

        private void UpdateEndScriptColomn()
        {
            EndScriptColomn = string.Format(
                BuildingScript.Colomns,
                string.Join(", ", _mainTable.Colomns.Select(x => x.TextBox.Text)),
                string.Join(",\n", _mainTable.Colomns.Select(x => string.Format(BuildingScript.Assign, x.TextBox.Text))),
                string.Join(", ", _mainTable.Colomns.Select(x => "source." + x.TextBox.Text)));
        }

        private void UpdateEndScriptRecord()
        {
            _mainColomn.Records = lineNumberRTB1.RichTextBox.Text;
            var maxCount = _mainTable.Colomns.Max(x => x.CountRecords);
            var helper = _mainTable.Colomns.Select(x => x.Records.Split('\n'));

            List<string> recs = new List<string>();
            for (int i = 0; i < maxCount; i++)
            {
                List<string> rec = new List<string>();
                foreach (var help in helper)
                {
                    if (i < help.Count())
                    {
                        rec.Add("'" + help[i] + "'");
                    }
                    else
                    {
                        rec.Add("''");
                    }
                }
                recs.Add("(" + string.Join(", ", rec) + ")");
            }
            EndScriptRecord = string.Join("\n       ,", recs);
            OutputEndScript();
        }

        private void OutputEndScript()
        {
            richTextBox3.Text = EndScriptTable + EndScriptRecord + EndScriptColomn;
        }
    }
}
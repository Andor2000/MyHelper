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

            panel2.HorizontalScroll.Maximum = 0;
            panel2.AutoScroll = false;
            panel2.VerticalScroll.Visible = false;
            panel2.AutoScroll = true;
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
            table.TextBox.Text = formAddTableName.TableName;

            table.TextBox.MouseMove += MouseMove;      // Курсор на объекте, навелся, перетаскивание.
            table.TextBox.MouseDown += MouseDown;      // Нажал левую кнопку мыши, не отпустил \ перетаскивание
            table.TextBox.MouseUp += MouseUp;          // Клинкул по объекту (отпустил мышь)
            table.TextBox.MouseLeave += MouseLeave;    // Отвел курсор с объекта

            CreateColomns(table, formAddTableName.ColomnNames);

            var firstColomn = table.Colomns.OrderBy(x => x.Sort).FirstOrDefault();
            firstColomn.EqualsRecordStar = true;
            firstColomn.IconStar.Visible = true;

            this.SetMainTable(table);
            this.UpdatePanelTable();
            this.OutputEndScript();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            var formDialog = new FormAddColomns(this);
            formDialog.ShowDialog();

            if (formDialog.ColomnNames == string.Empty)
            {
                return;
            }

            this.CreateColomns(_mainTable, formDialog.ColomnNames);
            this.UpdatePanelColomn();

            this.UpdateEndScriptColomn();
            this.UpdateEndScriptRecord();
            this.OutputEndScript();
        }

        private void DeleteColomn(object sender, EventArgs e)
        {
            var deletedColomn = _mainTable.Colomns.First(x => x.Context == ((ToolStripMenuItem)sender).Owner);

            if (_mainColomn == deletedColomn)
            {
                var newMainColomn = _mainTable.Colomns.FirstOrDefault(x => x.Sort == deletedColomn.Sort + 1)
                    ?? _mainTable.Colomns.FirstOrDefault(x => x.Sort == deletedColomn.Sort - 1)
                    ?? new ColomnModelObjPanel();

                this.SetMainColomn(newMainColomn);
            }

            if (deletedColomn.Sort < _mainTable.Colomns.Count())
            {
                _mainTable.Colomns
                    .Where(x => x.Sort > deletedColomn.Sort)
                    .ToList()
                    .ForEach(x => x.Sort--);
            }

            _mainTable.Colomns.Remove(deletedColomn);
            deletedColomn = null;

            this.UpdatePanelColomn();
            this.UpdateEndScriptRecord();
            this.UpdateEndScriptColomn();
            this.OutputEndScript();

            // Вроде как чистика памяти.
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        /// <summary>
        /// Добавить сравнение.
        /// </summary>
        private void AddStarColomn(object sender, EventArgs e)
        {
            var colomn = _mainTable.Colomns.First(x => x.Context == ((ToolStripMenuItem)sender).Owner);
            colomn.EqualsRecordStar = true;
            colomn.IconStar.Visible = true;

            this.UpdateEndScriptColomn();
            this.OutputEndScript();
        }

        /// <summary>
        /// Добавить сравнение.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (_mainColomn.EqualsRecordStar)
            {
                pictureBox3.Image = Image.FromFile(@"images\icons\star.png");
            }
            else
            {
                pictureBox3.Image = Image.FromFile(@"images\icons\star_active.png");
            }

            _mainColomn.EqualsRecordStar = !_mainColomn.EqualsRecordStar;
            _mainColomn.IconStar.Visible = !_mainColomn.IconStar.Visible;
            this.UpdateEndScriptColomn();
            this.OutputEndScript();
        }

        private void CreateColomns(TableModelObjPanel table, string colomnNames)
        {
            var colomnNamesList = _formQuotesService.FormatingString(colomnNames, true, true, true, true);
            colomnNamesList = colomnNamesList.Where(x => !_mainTable.Colomns.Select(y => y.TextBox.Text).Contains(x));

            foreach (var colomnName in colomnNamesList)
            {
                var colomn = new ColomnModelObjPanel();
                colomn.Sort = table.Colomns.Count();
                colomn.TextBox.Text = colomnName;
                table.Colomns.Add(colomn);

                colomn.TextBox.MouseMove += MouseMove;      // Курсор на объекте, навелся, перетаскивание.
                colomn.TextBox.MouseDown += MouseDown;      // Нажал левую кнопку мыши, не отпустил \ перетаскивание
                colomn.TextBox.MouseUp += MouseUp;          // Клинкул по объекту (отпустил мышь)
                colomn.TextBox.MouseLeave += MouseLeave;    // Отвел курсор с объекта.

                ToolStripMenuItem deleteMenuItem = new ToolStripMenuItem("Удалить");
                ToolStripMenuItem AddEqualsRecordStar = new ToolStripMenuItem("Добавить в сравнение");
                deleteMenuItem.Click += DeleteColomn;
                AddEqualsRecordStar.Click += AddStarColomn;

                colomn.Context.Items.AddRange(new[]
                {
                    AddEqualsRecordStar,
                    deleteMenuItem
                });
            }
        }

        /// <summary>
        /// Курсор на объекте, навелся, перетаскивание.
        /// </summary>
        private void MouseMove(object sender, EventArgs e)
        {
            if ((TextBox)sender == _mainTable.TextBox || (TextBox)sender == _mainColomn.TextBox)
            {
                return;
            }

            if (this.CheckIsColomn(sender))
            {
                var colomn = _mainTable.Colomns.First(x => x.TextBox == (TextBox)sender);
                colomn.TextBoxCount.BackColor = Colors.PanelMouseMoveObject;
            }

            ((TextBox)sender).BackColor = Colors.PanelMouseMoveObject;
            ((TextBox)sender).Parent.BackColor = Colors.PanelMouseMoveObject;
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

            if (this.CheckIsColomn(sender))
            {
                var colomn = _mainTable.Colomns.First(x => x.TextBox == (TextBox)sender);
                colomn.TextBoxCount.BackColor = Colors.PanelFon;
            }

            ((TextBox)sender).BackColor = Colors.PanelFon;
            ((TextBox)sender).Parent.BackColor = Colors.PanelFon;
        }

        /// <summary>
        /// Клинкул по объекту (отпустил мышь)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                return;
            }

            if ((TextBox)sender == _mainTable.TextBox || (TextBox)sender == _mainColomn.TextBox)
            {
                return;
            }

            _mainTable.EndScript = richTextBox3.Text;
            _mainColomn.Records = lineNumberRTB1.RichTextBox.Text;

            if (this.CheckIsColomn(sender))
            {
                var colomn = _mainTable.Colomns.FirstOrDefault(x => x.TextBox == (TextBox)sender);
                this.SetMainColomn(colomn);
            }
            else
            {
                var table = _panelTableLeft.FirstOrDefault(x => x.TextBox == (TextBox)sender);
                this.SetMainTable(table);
                this.UpdatePanelColomn();
            }
        }

        /// <summary>
        /// Установить активную таблицу.
        /// </summary>
        /// <param name="newMainTable"></param>
        private void SetMainTable(TableModelObjPanel newMainTable)
        {
            _mainTable.Panel.BackColor = Colors.PanelFon;
            _mainTable.TextBox.BackColor = Colors.PanelFon;
            _mainTable.TextBox.ForeColor = Color.White;
            _mainTable.Icon.Image = Image.FromFile(@"images\icons\quest.png");
            _mainTable = newMainTable;
            _mainTable.Panel.BackColor = Colors.PanelActiveObject;
            _mainTable.TextBox.BackColor = Colors.PanelActiveObject;
            _mainTable.TextBox.ForeColor = Colors.PanelActiveObjectFore;
            _mainTable.Icon.Image = Image.FromFile(@"images\icons\quest_active.png");

            textBox1.Text = _mainTable.TextBox.Text;

            this.UpdateEndScriptTable();
            this.UpdateEndScriptRecord();
            this.UpdateEndScriptColomn();

            var newMainColomn = _mainTable.Colomns.OrderBy(x => x.Sort).FirstOrDefault();
            this.SetMainColomn(newMainColomn);
        }

        private void SetMainColomn(ColomnModelObjPanel newMainColomn)
        {
            _mainColomn.Panel.BackColor = Colors.PanelFon;
            _mainColomn.TextBox.BackColor = Colors.PanelFon;
            _mainColomn.TextBoxCount.BackColor = Colors.PanelFon;
            _mainColomn.TextBox.ForeColor = Color.White;
            _mainColomn.TextBoxCount.ForeColor = Color.White;
            _mainColomn.Icon.Image = Image.FromFile(@"images\icons\pencil.png");
            _mainColomn.IconStar.Image = Image.FromFile(@"images\icons\star.png");

            _mainColomn = newMainColomn;

            _mainColomn.Panel.BackColor = Colors.PanelActiveObject;
            _mainColomn.TextBox.BackColor = Colors.PanelActiveObject;
            _mainColomn.TextBoxCount.BackColor = Colors.PanelActiveObject;
            _mainColomn.TextBox.ForeColor = Colors.PanelActiveObjectFore;
            _mainColomn.TextBoxCount.ForeColor = Colors.PanelActiveObjectFore;
            _mainColomn.Icon.Image = Image.FromFile(@"images\icons\pencil_active.png");
            _mainColomn.IconStar.Image = Image.FromFile(@"images\icons\star_active.png");

            pictureBox3.Image = _mainColomn.EqualsRecordStar
                ? Image.FromFile(@"images\icons\star_active.png")
                : Image.FromFile(@"images\icons\star.png");

            lineNumberRTB1.RichTextBox.Text = _mainColomn.Records;
            textBox2.Text = _mainColomn.TextBox.Text;
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
            var colomns = _mainTable.Colomns.OrderBy(x => x.Sort);

            foreach (var colomn in colomns)
            {
                colomn.Panel.Location = new Point(0, colomn.Sort * SizeEnums.HeightPanel);
                panel2.Controls.Add(colomn.Panel);
            }
        }

        private void lineNumberRTB1_KeyDown(object sender, KeyEventArgs e)
        {
            this.RewriteCountRecord(e);
        }

        private void lineNumberRTB1_KeyUp(object sender, KeyEventArgs e)
        {
            //this.RewriteCountRecord(e);
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

        /// <summary>
        /// Изменение начало скрипта (название таблицы)
        /// </summary>
        private void UpdateEndScriptTable()
        {
            EndScriptTable = string.Format(BuildingScript.Table, _mainTable.TextBox.Text);
        }

        /// <summary>
        /// Изменение середины скрипта (записи).
        /// </summary>
        private void UpdateEndScriptRecord()
        {
            if (!_mainTable.Colomns.Any())
            {
                return;
            }
            _mainColomn.Records = lineNumberRTB1.RichTextBox.Text;
            var maxCount = _mainTable.Colomns.Max(x => x.CountRecords);
            var helper = _mainTable.Colomns.OrderBy(x => x.Sort).Select(x => x.Records.Split('\n'));

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
            this.OutputEndScript();
        }

        /// <summary>
        /// Изменение конца скрипта (название колонок).
        /// </summary>
        private void UpdateEndScriptColomn()
        {
            var sortColomn = _mainTable.Colomns.OrderBy(x => x.Sort);
            EndScriptColomn = string.Format(
                BuildingScript.Colomns,
                string.Join(", ", sortColomn.Select(x => x.TextBox.Text)),
                string.Join(" and ", sortColomn.Where(x => x.EqualsRecordStar).Select(x => string.Format(BuildingScript.Assign, x.TextBox.Text))),
                string.Join(",\n\t\t", sortColomn.Select(x => string.Format(BuildingScript.Assign, x.TextBox.Text))),
                string.Join(", ", sortColomn.Select(x => "source." + x.TextBox.Text)));
        }

        /// <summary>
        /// Вывод скрипта.
        /// </summary>
        private void OutputEndScript()
        {
            richTextBox3.Text = EndScriptTable + EndScriptRecord + EndScriptColomn;
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

        /// <summary>
        /// Проверка, является ли textBox таблицей (или это колонка).
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        private bool CheckIsColomn(object sender)
        {
            return _mainTable.Colomns.Any(x => x.TextBox == (TextBox)sender);
        }

        /// <summary>
        /// Изменение название таблицы.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            _mainTable.TextBox.Text = textBox1.Text;
            this.UpdateEndScriptTable();
            this.OutputEndScript();
        }

        /// <summary>
        /// Изменение название колонки.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            _mainColomn.TextBox.Text = textBox2.Text;
            this.UpdateEndScriptColomn();
            this.OutputEndScript();
        }
    }
}
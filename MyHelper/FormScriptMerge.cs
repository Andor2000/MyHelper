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
        private string EndScriptTable { get; set; } = string.Empty;
        private string EndScriptRecord { get; set; } = string.Empty;
        private string EndScriptColomn { get; set; } = string.Empty;

        /// <summary>
        /// Признак зажатия клавиши мыши.
        /// </summary>
        private bool _isClickMouse { get; set; }

        /// <summary>
        /// Признак перетаскивания.
        /// </summary>
        private bool _isDragAndDrop { get; set; }

        /// <summary>
        /// Перетаскиваемая таблица.
        /// </summary>
        private TableDto _DragAndDropTable { get; set; }

        /// <summary>
        /// Перетаскиваемая колонка.
        /// </summary>
        private ColomnDto _DragAndDropColomn { get; set; }

        /// <summary>
        /// Координата у для перетаскивания (мышь).
        /// </summary>
        private int y_mouse { get; set; }

        /// <summary>
        /// Координата у для перетаскивания (панель).
        /// </summary>
        private int y_panel { get; set; }

        /// <summary>
        /// Активная таблица.
        /// </summary>
        private TableDto _mainTable { get; set; } = new TableDto();

        /// <summary>
        /// Активный столбец
        /// </summary>
        private ColomnDto _mainColomn { get; set; } = new ColomnDto();

        /// <summary>
        /// Левая колонка с задачами.
        /// </summary>
        private List<TableDto> _panelTableLeft { get; set; } = new List<TableDto>();

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

        /// <summary>
        /// Добавление новой таблицы.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            panel5.BackColor = panel5.Parent.BackColor;
            var formAddTableName = new FormAddTable();
            formAddTableName.ShowDialog();

            if (formAddTableName.TableName == string.Empty || formAddTableName.ColomnNames == string.Empty)
            {
                return;
            }

            var table = new TableDto();
            table.Sort = _panelTableLeft.Count();
            table.TextBox.Text = formAddTableName.TableName;
            _panelTableLeft.Add(table);

            table.TextBox.MouseDown += MouseDownObject;     // Нажал левую кнопку мыши, не отпустил \ перетаскивание
            table.TextBox.MouseMove += MouseMoveObject;     // Курсор на объекте, навелся, перетаскивание.
            table.TextBox.MouseUp += MouseUpObject;         // Клинкул по объекту (отпустил мышь)
            table.TextBox.MouseLeave += MouseLeaveObject;   // Отвел курсор с объекта
            table.ContextDeleted.Click += DeleteTable;             // 

            CreateColomns(table, formAddTableName.ColomnNames);

            var firstColomn = table.Colomns.OrderBy(x => x.Sort).FirstOrDefault();
            firstColomn.EqualsRecordStar = true;
            firstColomn.IconStar.Visible = true;
            firstColomn.ContextStar.Text = "Убрать из сравнения";

            this.SetMainTable(table);
            this.UpdatePanelTable();
            this.UpdatePanelColomn();
            this.OutputEndScript();
        }

        /// <summary>
        /// Добавление новых колонок.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Удаление колонки.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteTable(object sender, EventArgs e)
        {
            if(_panelTableLeft.Count() == 1)
            {
                return;
            }

            var deletedTable = _panelTableLeft.First(x => x.ContextDeleted == (ToolStripMenuItem)sender);

            if (_mainTable == deletedTable)
            {
                var newMainTable = _panelTableLeft.FirstOrDefault(x => x.Sort == deletedTable.Sort - 1)
                    ?? _panelTableLeft.FirstOrDefault(x => x.Sort == deletedTable.Sort + 1);

                this.SetMainTable(newMainTable);
            }

            _panelTableLeft
                 .Where(x => x.Sort > deletedTable.Sort)
                .ToList()
                .ForEach(x => x.Sort--);            

            _panelTableLeft.Remove(deletedTable);
            /// не получилось удалить колонки
            deletedTable = null;

            panel3.Controls.Clear();
            this.UpdatePanelTable();
            this.UpdatePanelColomn();

            // Вроде как чистика памяти.
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        /// <summary>
        /// Удаление колонки.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteColomn(object sender, EventArgs e)
        {
            if (_mainTable.Colomns.Count() == 1)
            {
                return;
            }

            var deletedColomn = _mainTable.Colomns.First(x => x.ContextDeleted == (ToolStripMenuItem)sender);

            if (_mainColomn == deletedColomn)
            {
                var newMainColomn = _mainTable.Colomns.FirstOrDefault(x => x.Sort == deletedColomn.Sort + 1)
                    ?? _mainTable.Colomns.FirstOrDefault(x => x.Sort == deletedColomn.Sort - 1)
                    ?? new ColomnDto();

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
            this.UpdateEndScriptColomn();
            this.UpdateEndScriptRecord();

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
            if (colomn.EqualsRecordStar)
            {
                colomn.EqualsRecordStar = false;
                colomn.IconStar.Visible = false;
                colomn.ContextStar.Text = "Добавить в сравнение";
            }
            else
            {
                colomn.EqualsRecordStar = true;
                colomn.IconStar.Visible = true;
                colomn.ContextStar.Text = "Убрать из сравнения";
            }

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
                pictureBox3.Image = IconEnums.Star;
            }
            else
            {
                pictureBox3.Image = IconEnums.StarActive;
            }

            _mainColomn.EqualsRecordStar = !_mainColomn.EqualsRecordStar;
            _mainColomn.IconStar.Visible = !_mainColomn.IconStar.Visible;
            this.UpdateEndScriptColomn();
            this.OutputEndScript();
        }

        private void CreateColomns(TableDto table, string colomnNames)
        {
            var colomnNamesList = _formQuotesService.FormatingString(colomnNames, true, true, true, true);
            colomnNamesList = colomnNamesList.Where(x => !table.Colomns.Select(y => y.TextBox.Text).Contains(x));

            foreach (var colomnName in colomnNamesList)
            {
                var colomn = new ColomnDto();
                colomn.Sort = table.Colomns.Count();
                colomn.TextBox.Text = colomnName;
                table.Colomns.Add(colomn);

                colomn.TextBox.MouseDown += MouseDownObject;      // Нажал левую кнопку мыши, не отпустил \ перетаскивание
                colomn.TextBox.MouseMove += MouseMoveObject;      // Курсор на объекте, навелся, перетаскивание.
                colomn.TextBox.MouseUp += MouseUpObject;          // Клинкул по объекту (отпустил мышь)
                colomn.TextBox.MouseLeave += MouseLeaveObject;    // Отвел курсор с объекта.

                colomn.ContextDeleted.Click += DeleteColomn;
                colomn.ContextStar.Click += AddStarColomn;
            }
        }

        /// <summary>
        /// Курсор на объекте, навелся, перетаскивание.
        /// </summary>
        private void MouseMoveObject(object sender, EventArgs e)
        {
            if (!_isClickMouse)
            {
                this.MouseMoveObject(sender);
                return;
            }

            int y = Cursor.Position.Y - y_mouse;
            if (!_isDragAndDrop && Math.Abs(y) > 1)
            {
                _isDragAndDrop = true;
            }

            if (_isDragAndDrop)
            {
                if (this.CheckIsColomn(sender))
                {
                    this.ColomnMouseMove(y);
                }
                else
                {
                    this.TableMouseMove(y);
                }
            }
        }

        private void ColomnMouseMove(int y)
        {
            _DragAndDropColomn.Panel.Location = new Point(0, y_panel + y);
            var count = y / SizeEnums.HeightPanel;
            if (y > 0)
            {
                count += y % SizeEnums.HeightPanel > 10 ? 1 : 0;
                var colomnDown = _mainTable.Colomns.Where(x => x.Sort > _DragAndDropColomn.Sort);
                foreach (var h in colomnDown.Where(x => x.Sort <= _DragAndDropColomn.Sort + count))
                {
                    h.Panel.Location = new Point(0, (h.Sort - 1) * SizeEnums.HeightPanel);
                }
                foreach (var h in colomnDown.Where(x => x.Sort > _DragAndDropColomn.Sort + count))
                {
                    h.Panel.Location = new Point(0, h.Sort * SizeEnums.HeightPanel);
                }
            }
            else
            {
                count -= y % SizeEnums.HeightPanel < -10 ? 1 : 0;
                count = Math.Abs(count);

                var colomnUp = _mainTable.Colomns.Where(x => x.Sort < _DragAndDropColomn.Sort);
                foreach (var h in colomnUp.Where(x => x.Sort >= _DragAndDropColomn.Sort - count))
                {
                    h.Panel.Location = new Point(0, (h.Sort + 1) * SizeEnums.HeightPanel);
                }
                foreach (var h in colomnUp.Where(x => x.Sort < _DragAndDropColomn.Sort - count))
                {
                    h.Panel.Location = new Point(0, h.Sort * SizeEnums.HeightPanel);
                }
            }
        }

        private void TableMouseMove(int y)
        {
            _DragAndDropTable.Panel.Location = new Point(0, y_panel + y);
            var count = y / SizeEnums.HeightPanel;
            if (y > 0)
            {
                count += y % SizeEnums.HeightPanel > 10 ? 1 : 0;
                var colomnDown = _panelTableLeft.Where(x => x.Sort < _DragAndDropTable.Sort);
                foreach (var h in colomnDown.Where(x => x.Sort >= _DragAndDropTable.Sort - count))
                {
                    h.Panel.Location = new Point(0, (_panelTableLeft.Count - h.Sort - 2) * SizeEnums.HeightPanel);
                }
                foreach (var h in colomnDown.Where(x => x.Sort < _DragAndDropTable.Sort - count))
                {
                    h.Panel.Location = new Point(0, (_panelTableLeft.Count - h.Sort - 1) * SizeEnums.HeightPanel);
                }
            }
            else
            {
                count -= y % SizeEnums.HeightPanel < -10 ? 1 : 0;
                count = Math.Abs(count);
                var colomnUp = _panelTableLeft.Where(x => x.Sort > _DragAndDropTable.Sort);
                foreach (var h in colomnUp.Where(x => x.Sort <= _DragAndDropTable.Sort + count))
                {
                    h.Panel.Location = new Point(0, ((_panelTableLeft.Count - h.Sort)) * SizeEnums.HeightPanel);
                }
                foreach (var h in colomnUp.Where(x => x.Sort > _DragAndDropTable.Sort + count))
                {
                    h.Panel.Location = new Point(0, (_panelTableLeft.Count - h.Sort - 1) * SizeEnums.HeightPanel);
                }
            }
        }

        private void MouseMoveObject(object sender)
        {
            if ((TextBox)sender == _mainTable.TextBox || (TextBox)sender == _mainColomn.TextBox)
                return;

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
        private void MouseDownObject(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                return;
            }
            ((TextBox)sender).Parent.Focus(); // убираем каретку

            if (this.CheckIsColomn(sender))
            {
                _DragAndDropColomn = _mainTable.Colomns.First(x => x.TextBox == (TextBox)sender);
                _DragAndDropColomn.Panel.BringToFront();
                y_panel = _DragAndDropColomn.Panel.Location.Y;
            }
            else
            {
                _DragAndDropTable = _panelTableLeft.First(x => x.TextBox == (TextBox)sender);
                _DragAndDropTable.Panel.BringToFront();
                y_panel = _DragAndDropTable.Panel.Location.Y;
            }
            y_mouse = Cursor.Position.Y;
            _isClickMouse = true;
        }

        /// <summary>
        /// Отвел курсор с объекта.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseLeaveObject(object sender, EventArgs e)
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
        private void MouseUpObject(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                return;

            _isClickMouse = false;
            if (_isDragAndDrop)
            {
                _isDragAndDrop = false;
                if (this.CheckIsColomn(sender))
                {
                    this.ColomnMouseUp();
                }
                else
                {
                    this.TableMouseUp();
                }
                return;
            }

            if ((TextBox)sender == _mainTable.TextBox || (TextBox)sender == _mainColomn.TextBox)
                return;

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

        private void ColomnMouseUp()
        {
            int y = Cursor.Position.Y - y_mouse;
            var count = y / SizeEnums.HeightPanel;

            if (y > 0)
            {
                count += y % SizeEnums.HeightPanel > 10 ? 1 : 0;
                var colomnDown = _mainTable.Colomns.Where(x => x.Sort > _DragAndDropColomn.Sort);
                foreach (var h in colomnDown.Where(x => x.Sort <= _DragAndDropColomn.Sort + count))
                {
                    h.Sort--;
                    h.Panel.Location = new Point(0, h.Sort * SizeEnums.HeightPanel);
                }
            }
            else
            {
                count -= y % SizeEnums.HeightPanel < -10 ? 1 : 0;
                var colomnUp = _mainTable.Colomns.Where(x => x.Sort < _DragAndDropColomn.Sort);
                foreach (var h in colomnUp.Where(x => x.Sort >= _DragAndDropColomn.Sort - Math.Abs(count)))
                {
                    h.Sort++;
                    h.Panel.Location = new Point(0, h.Sort * SizeEnums.HeightPanel);
                }
            }

            _DragAndDropColomn.Sort = _DragAndDropColomn.Sort + count > _mainTable.Colomns.Count
                ? _mainTable.Colomns.Count - 1
                : _DragAndDropColomn.Sort + count < 1
                    ? 0
                    : _DragAndDropColomn.Sort + count;

            _DragAndDropColomn.Panel.Location = new Point(0, _DragAndDropColomn.Sort * SizeEnums.HeightPanel);

            this.UpdateEndScriptRecord();
            this.UpdateEndScriptColomn();
            this.OutputEndScript();
        }

        private void TableMouseUp()
        {
            int y = Cursor.Position.Y - y_mouse;
            var count = y / SizeEnums.HeightPanel;

            if (y > 0)
            {
                count += y % SizeEnums.HeightPanel > 10 ? 1 : 0;
                var colomnDown = _panelTableLeft.Where(x => x.Sort < _DragAndDropTable.Sort);
                foreach (var h in colomnDown.Where(x => x.Sort >= _DragAndDropTable.Sort - count))
                {
                    h.Sort += 1;
                }
            }
            else
            {
                count -= y % SizeEnums.HeightPanel < -10 ? 1 : 0;
                var colomnUp = _panelTableLeft.Where(x => x.Sort > _DragAndDropTable.Sort);
                foreach (var h in colomnUp.Where(x => x.Sort <= _DragAndDropTable.Sort + Math.Abs(count)))
                {
                    h.Sort--;
                }
            }

            _DragAndDropTable.Sort = _DragAndDropTable.Sort - count >= _panelTableLeft.Count
                ? _panelTableLeft.Count - 1
                : _DragAndDropTable.Sort - count <= 0
                    ? 0
                    : _DragAndDropTable.Sort - count;

            this.UpdatePanelTable();
        }

        /// <summary>
        /// Установить основную таблицу.
        /// </summary>
        /// <param name="newMainTable"></param>
        private void SetMainTable(TableDto newMainTable)
        {
            _mainTable.Panel.BackColor = Colors.PanelFon;
            _mainTable.TextBox.BackColor = Colors.PanelFon;
            _mainTable.TextBox.ForeColor = Color.White;
            _mainTable.Icon.Image = IconEnums.Quest;
            _mainTable = newMainTable;
            _mainTable.Panel.BackColor = Colors.PanelActiveObject;
            _mainTable.TextBox.BackColor = Colors.PanelActiveObject;
            _mainTable.TextBox.ForeColor = Colors.PanelActiveObjectFore;
            _mainTable.Icon.Image = IconEnums.QuestActive;

            textBox1.Text = _mainTable.TextBox.Text;

            this.UpdateEndScriptTable();
            this.UpdateEndScriptColomn();
            this.UpdateEndScriptRecord(); // тут вывод скрипта

            var newMainColomn = _mainTable.Colomns.OrderBy(x => x.Sort).FirstOrDefault();
            this.SetMainColomn(newMainColomn);
        }

        /// <summary>
        /// Установить основну колонку.
        /// </summary>
        /// <param name="newMainColomn"></param>
        private void SetMainColomn(ColomnDto newMainColomn)
        {
            _mainColomn.Panel.BackColor = Colors.PanelFon;
            _mainColomn.TextBox.BackColor = Colors.PanelFon;
            _mainColomn.TextBoxCount.BackColor = Colors.PanelFon;
            _mainColomn.TextBox.ForeColor = Color.White;
            _mainColomn.TextBoxCount.ForeColor = Color.White;
            _mainColomn.Icon.Image = IconEnums.Pencil;
            _mainColomn.IconStar.Image = IconEnums.Star;
            _mainColomn.Records = lineNumberRTB1.RichTextBox.Text;

            _mainColomn = newMainColomn;

            _mainColomn.Panel.BackColor = Colors.PanelActiveObject;
            _mainColomn.TextBox.BackColor = Colors.PanelActiveObject;
            _mainColomn.TextBoxCount.BackColor = Colors.PanelActiveObject;
            _mainColomn.TextBox.ForeColor = Colors.PanelActiveObjectFore;
            _mainColomn.TextBoxCount.ForeColor = Colors.PanelActiveObjectFore;
            _mainColomn.Icon.Image = IconEnums.PencilActive;
            _mainColomn.IconStar.Image = IconEnums.StarActive;
            lineNumberRTB1.RichTextBox.Text = _mainColomn.Records;

            pictureBox3.Image = _mainColomn.EqualsRecordStar
                ? IconEnums.StarActive
                : IconEnums.Star;

            textBox2.Text = _mainColomn.TextBox.Text;
        }

        /// <summary>
        /// Изменение отображение таблиц.
        /// </summary>
        private void UpdatePanelTable()
        {
            var tables = _panelTableLeft.OrderByDescending(x => x.Sort);

            foreach (var table in tables)
            {
                table.Panel.Location = new Point(0, (_panelTableLeft.Count() - table.Sort - 1) * SizeEnums.HeightPanel);
                panel3.Controls.Add(table.Panel);
            }
        }

        /// <summary>
        /// Изменение отображения колонок.
        /// </summary>
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
            this.RewriteCountRecord(e);
            this.UpdateEndScriptRecord();
        }

        /// <summary>
        /// Пересчитать количество записей в таблице.
        /// </summary>
        /// <param name="e"></param>
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
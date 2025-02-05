using MyHelper.Data;
using MyHelper.DialogForms;
using MyHelper.DialogForms.ScriptMerge;
using MyHelper.Dto;
using MyHelper.Enums;
using MyHelper.Extensions;
using MyHelper.Services;
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
        private string Server { get; set; } = string.Empty;
        private string DataBase { get; set; } = string.Empty;
        private string Login { get; set; } = string.Empty;
        private string Password { get; set; } = string.Empty;

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

        /// <summary>
        /// Серивс для работы с базой данных.
        /// </summary>
        private DataBaseService _dataBaseService { get; set; }

        public FormScriptMerge(ProgramContext context)
        {
            this._dataBaseService = new DataBaseService(context);
            this.InitializeComponent();
            this.InitForm();
            this.GetTablesDB();
        }

        /// <summary>
        /// Инициализация формы.
        /// </summary>
        private void InitForm()
        {
            lineNumberRTB1.RichTextBox.BackColor = Colors.PanelFon;
            lineNumberRTB1.RichTextBox.ForeColor = Color.White;
            lineNumberRTB1.RichTextBox.WordWrap = false;
            lineNumberRTB1.RichTextBox.KeyDown += lineNumberRTB1_KeyDown;
            lineNumberRTB1.RichTextBox.KeyUp += lineNumberRTB1_KeyUp;

            /*Не знаю как работает, но так не появляется скрол*/
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
        /// Получение таблиц из базы данных.
        /// </summary>
        private void GetTablesDB()
        {
            this._panelTableLeft = this._dataBaseService.GetTables();

            if (this._panelTableLeft != null && this._panelTableLeft.Any())
            {
                this._panelTableLeft.ForEach(x => this.AddTableEvents(x));
                this._panelTableLeft.SelectMany(x => x.Colomns).ToList()
                    .ForEach(x =>
                    {
                        this.SetCountRecordColomn(x);
                        this.AddColomnEvents(x);

                        x.IconStar.Visible = x.IsEqualsRecordStar;
                        pictureBoxAddStar.Image = x.IsEqualsRecordStar
                            ? IconEnums.StarActive
                            : IconEnums.Star;

                    });

                this.SetMainTable(_panelTableLeft[0]);
                this.UpdatePanelTable();
                this.UpdatePanelColomn();
            }
        }

        /// <summary>
        /// Добавление новой таблицы.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBoxAddTable_Click(object sender, EventArgs e)
        {
            this._dataBaseService.UpdateTables(this._mainTable);
            this._dataBaseService.UpdateColomns(this._mainColomn);

            this.panel5.BackColor = this.panel5.Parent.BackColor;
            var formAddTableName = new FormAddTable();
            formAddTableName.ShowDialog();

            if (formAddTableName.TableName.IsNullOrDefault() || formAddTableName.ColomnNames.IsNullOrDefault())
            {
                return;
            }

            var table = new TableDto();
            table.Sort = this._panelTableLeft.Count;
            table.TextBox.Text = formAddTableName.TableName;

            this.CreateColomns(table, formAddTableName.ColomnNames);
            formAddTableName.Close();

            var firstColomn = table.Colomns.OrderBy(x => x.Sort).FirstOrDefault();
            firstColomn.IsEqualsRecordStar = true;
            firstColomn.IconStar.Visible = true;
            firstColomn.ContextStar.Text = "Убрать из сравнения";

            table.SaveScriptModel = this._dataBaseService.GetSaveScriptModel();
            table = this._dataBaseService.AddTable(table);
            this.AddTableEvents(table);
            this._panelTableLeft.Add(table);

            this.SetMainTable(table);
            this.UpdatePanelTable();
            this.UpdatePanelColomn();
            this.OutputEndScript();

            this.pictureBoxAddTemplate.Image = IconEnums.TemplateScript;
        }

        /// <summary>
        /// Добавление таблице событий.
        /// </summary>
        /// <param name="table">Таблица.</param>
        private void AddTableEvents(TableDto table)
        {
            table.TextBox.MouseDown += MouseDownObject;     // Нажал левую кнопку мыши, не отпустил \ перетаскивание
            table.TextBox.MouseMove += MouseMoveObject;     // Курсор на объекте, навелся, перетаскивание.
            table.TextBox.MouseUp += MouseUpObject;         // Клинкул по объекту (отпустил мышь)
            table.TextBox.MouseLeave += MouseLeaveObject;   // Отвел курсор с объекта
            table.ContextDeleted.Click += DeleteTable;      // Удаление колонки.
        }

        /// <summary>
        /// Добавление новых колонок.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBoxAddColomns_Click(object sender, EventArgs e)
        {
            _dataBaseService.UpdateTables(_mainTable);
            _dataBaseService.UpdateColomns(_mainColomn);
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
        /// Добавить сравнение.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBoxAddStar_Click(object sender, EventArgs e)
        {
            this._mainColomn.IsEqualsRecordStar = !_mainColomn.IsEqualsRecordStar;
            this._mainColomn.IconStar.Visible = !_mainColomn.IconStar.Visible; // в левой панели
            pictureBoxAddStar.Image = this._mainColomn.IsEqualsRecordStar
                ? IconEnums.StarActive
                : IconEnums.Star;

            _dataBaseService.UpdateTables(_mainTable);
            _dataBaseService.UpdateColomns(_mainColomn);

            this.UpdateEndScriptColomn();
            this.OutputEndScript();
        }

        /// <summary>
        /// Добавить кавычки в скрипте
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBoxAddQuotes_Click(object sender, EventArgs e)
        {
            this._mainColomn.IsQuotes = !_mainColomn.IsQuotes;
            pictureBoxAddQuotes.Image = this._mainColomn.IsQuotes
                ? IconEnums.QuotesActive2
                : IconEnums.Quotes2;

            _dataBaseService.UpdateTables(_mainTable);
            _dataBaseService.UpdateColomns(_mainColomn);

            this.UpdateEndScriptRecord();
            this.OutputEndScript();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            var formDialog = new FormAddDataBase(this.Server, this.DataBase, this.Login, this.Password);
            formDialog.ShowDialog();
            if (formDialog.DialogResult != DialogResult.OK)
            {
                return;
            }

            this.Server = formDialog.Server;
            this.DataBase = formDialog.DataBase;
            this.Login = formDialog.Login;
            this.Password = formDialog.Password;

            pictureBox4.Image =
                string.IsNullOrWhiteSpace(this.Server) ||
                string.IsNullOrWhiteSpace(this.DataBase) ||
                string.IsNullOrWhiteSpace(this.Login) ||
                string.IsNullOrWhiteSpace(this.Password)
                ? IconEnums.DataBase
                : IconEnums.DataBaseActive;
        }

        private void pictureBoxOpenFormQuotes_Click(object sender, EventArgs e)
        {
            _dataBaseService.UpdateTables(_mainTable);
            _dataBaseService.UpdateColomns(_mainColomn);
            var formDialog = new FormQuotes();
            formDialog.ShowDialog();
        }

        /// <summary>
        /// Добавить шаблон в скрипт (для отображения в интерфейсе).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBoxAddTemplate_Click(object sender, EventArgs e)
        {
            this._mainTable.IsTemplateScript = !_mainTable.IsTemplateScript;
            pictureBoxAddTemplate.Image = this._mainTable.IsTemplateScript
                ? IconEnums.TemplateScriptActive
                : IconEnums.TemplateScript;

            _dataBaseService.UpdateTables(_mainTable);
            _dataBaseService.UpdateColomns(_mainColomn);

            this.OutputEndScript();
        }

        /// <summary>
        /// Удаление колонки.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteTable(object sender, EventArgs e)
        {
            if (_panelTableLeft.Count() == 1)
            {
                return;
            }

            var deletedTable = this._panelTableLeft.First(x => x.ContextDeleted == (ToolStripMenuItem)sender);

            if (_mainTable == deletedTable)
            {
                var newMainTable = this._panelTableLeft.FirstOrDefault(x => x.Sort == deletedTable.Sort - 1)
                    ?? this._panelTableLeft.FirstOrDefault(x => x.Sort == deletedTable.Sort + 1)
                    ?? this._panelTableLeft.FirstOrDefault();

                this.SetMainTable(newMainTable);
            }

            this._panelTableLeft
                .Where(x => x.Sort > deletedTable.Sort).ToList()
                .ForEach(x => x.Sort--);

            _dataBaseService.RemoveTable(deletedTable);
            this._panelTableLeft.Remove(deletedTable);
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

            var deletedColomn = this._mainTable.Colomns.First(x => x.ContextDeleted == (ToolStripMenuItem)sender);

            if (_mainColomn == deletedColomn)
            {
                var newMainColomn = this._mainTable.Colomns.FirstOrDefault(x => x.Sort == deletedColomn.Sort + 1)
                    ?? this._mainTable.Colomns.FirstOrDefault(x => x.Sort == deletedColomn.Sort - 1)
                    ?? this._mainTable.Colomns.FirstOrDefault();

                this.SetMainColomn(newMainColomn);
            }

            if (deletedColomn.Sort < this._mainTable.Colomns.Count())
            {
                this._mainTable.Colomns
                    .Where(x => x.Sort > deletedColomn.Sort)
                    .ToList()
                    .ForEach(x => x.Sort--);
            }

            _dataBaseService.RemoveColomn(deletedColomn);
            this._mainTable.Colomns.Remove(deletedColomn);
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
            var colomn = this._mainTable.Colomns.First(x => x.Context == ((ToolStripMenuItem)sender).Owner);
            if (colomn.IsEqualsRecordStar)
            {
                colomn.IsEqualsRecordStar = false;
                colomn.IconStar.Visible = false;
                colomn.ContextStar.Text = "Добавить в сравнение";
            }
            else
            {
                colomn.IsEqualsRecordStar = true;
                colomn.IconStar.Visible = true;
                colomn.ContextStar.Text = "Убрать из сравнения";
            }

            this.UpdateEndScriptColomn();
            this.OutputEndScript();
        }

        /// <summary>
        /// Создание колонок.
        /// </summary>
        /// <param name="table">Dto-модель таблицы.</param>
        /// <param name="colomnNames">Имена колонок.</param>
        private void CreateColomns(TableDto table, string colomnNames)
        {
            var existsColomnNames = table.Colomns.Select(y => y.TextBox.Text);

            var colomnNamesList = this._formQuotesService
                .FormatingString(colomnNames, true, true, true, true)
                .Where(x => !existsColomnNames.Contains(x));

            foreach (var colomnName in colomnNamesList)
            {
                var colomn = new ColomnDto();
                colomn.Sort = table.Colomns.Count();
                this.SetDirectoryName(table.TextBox.Text, colomn, colomnName);
                table.Colomns.Add(colomn);
                this.AddColomnEvents(colomn);
            }
        }

        /// <summary>
        /// Добавление колонке события.
        /// </summary>
        /// <param name="colomn">Колонка</param>
        private void AddColomnEvents(ColomnDto colomn)
        {
            colomn.TextBox.MouseDown += MouseDownObject;      // Нажал левую кнопку мыши, не отпустил \ перетаскивание
            colomn.TextBox.MouseMove += MouseMoveObject;      // Курсор на объекте, навелся, перетаскивание.
            colomn.TextBox.MouseUp += MouseUpObject;          // Клинкул по объекту (отпустил мышь)
            colomn.TextBox.MouseLeave += MouseLeaveObject;    // Отвел курсор с объекта.

            colomn.ContextDeleted.Click += DeleteColomn;
            colomn.ContextStar.Click += AddStarColomn;
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
                var colomnDown = this._mainTable.Colomns.Where(x => x.Sort > _DragAndDropColomn.Sort);
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

                var colomnUp = this._mainTable.Colomns.Where(x => x.Sort < _DragAndDropColomn.Sort);
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
                var colomnDown = this._panelTableLeft.Where(x => x.Sort < _DragAndDropTable.Sort);
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
                var colomnUp = this._panelTableLeft.Where(x => x.Sort > _DragAndDropTable.Sort);
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
            if ((TextBox)sender == this._mainTable.TextBox || (TextBox)sender == this._mainColomn.TextBox)
                return;

            if (this.CheckIsColomn(sender))
            {
                var colomn = this._mainTable.Colomns.First(x => x.TextBox == (TextBox)sender);
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
                _DragAndDropColomn = this._mainTable.Colomns.First(x => x.TextBox == (TextBox)sender);
                _DragAndDropColomn.Panel.BringToFront();
                y_panel = _DragAndDropColomn.Panel.Location.Y;
            }
            else
            {
                _DragAndDropTable = this._panelTableLeft.First(x => x.TextBox == (TextBox)sender);
                _DragAndDropTable.Panel.BringToFront();
                y_panel = _DragAndDropTable.Panel.Location.Y;
            }
            this.y_mouse = Cursor.Position.Y;
            this._isClickMouse = true;
        }

        /// <summary>
        /// Отвел курсор с объекта.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseLeaveObject(object sender, EventArgs e)
        {
            if ((TextBox)sender == this._mainTable.TextBox || (TextBox)sender == this._mainColomn.TextBox)
            {
                return;
            }

            if (this.CheckIsColomn(sender))
            {
                var colomn = this._mainTable.Colomns.First(x => x.TextBox == (TextBox)sender);
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

                    this.UpdateEndScriptRecord();
                    this.UpdateEndScriptColomn();
                    this.OutputEndScript();
                    _dataBaseService.UpdateColomns(_mainTable.Colomns.ToArray());
                }
                else
                {
                    this.TableMouseUp();
                    this.UpdatePanelTable();
                    _dataBaseService.UpdateTables(_panelTableLeft.ToArray());
                }
                return;
            }

            if ((TextBox)sender == this._mainTable.TextBox || (TextBox)sender == this._mainColomn.TextBox)
                return;

            if (this.CheckIsColomn(sender))
            {
                var colomn = this._mainTable.Colomns.FirstOrDefault(x => x.TextBox == (TextBox)sender);
                this.SetMainColomn(colomn);
                _dataBaseService.UpdateColomns(_mainTable.Colomns.ToArray());
            }
            else
            {
                var table = this._panelTableLeft.FirstOrDefault(x => x.TextBox == (TextBox)sender);
                this.SetMainTable(table);
                this.UpdatePanelColomn();
                _dataBaseService.UpdateTables(_panelTableLeft.ToArray());
            }
        }

        private void ColomnMouseUp()
        {
            int y = Cursor.Position.Y - y_mouse;
            var count = y / SizeEnums.HeightPanel;

            if (y > 0)
            {
                count += y % SizeEnums.HeightPanel > 10 ? 1 : 0;
                var colomnDown = this._mainTable.Colomns.Where(x => x.Sort > _DragAndDropColomn.Sort);
                foreach (var h in colomnDown.Where(x => x.Sort <= _DragAndDropColomn.Sort + count))
                {
                    h.Sort--;
                    h.Panel.Location = new Point(0, h.Sort * SizeEnums.HeightPanel);
                }
            }
            else
            {
                count -= y % SizeEnums.HeightPanel < -10 ? 1 : 0;
                var colomnUp = this._mainTable.Colomns.Where(x => x.Sort < _DragAndDropColomn.Sort);
                foreach (var h in colomnUp.Where(x => x.Sort >= _DragAndDropColomn.Sort - Math.Abs(count)))
                {
                    h.Sort++;
                    h.Panel.Location = new Point(0, h.Sort * SizeEnums.HeightPanel);
                }
            }
            switch (_DragAndDropColomn.Sort + count)
            {
                case var x when x >= this._mainTable.Colomns.Count:
                    _DragAndDropColomn.Sort = this._mainTable.Colomns.Count - 1;
                    break;
                case var x when x < 1:
                    _DragAndDropColomn.Sort = 0;
                    break;
                default:
                    _DragAndDropColomn.Sort = _DragAndDropColomn.Sort + count;
                    break;
            }

            _DragAndDropColomn.Panel.Location = new Point(0, _DragAndDropColomn.Sort * SizeEnums.HeightPanel);
        }

        private void TableMouseUp()
        {
            int y = Cursor.Position.Y - y_mouse;
            var count = y / SizeEnums.HeightPanel;

            if (y > 0)
            {
                count += y % SizeEnums.HeightPanel > 10 ? 1 : 0;
                var colomnDown = this._panelTableLeft.Where(x => x.Sort < _DragAndDropTable.Sort);
                foreach (var colomn in colomnDown.Where(x => x.Sort >= _DragAndDropTable.Sort - count))
                {
                    colomn.Sort++;
                }
            }
            else
            {
                count -= y % SizeEnums.HeightPanel < -10 ? 1 : 0;
                var colomnUp = this._panelTableLeft.Where(x => x.Sort > _DragAndDropTable.Sort);
                foreach (var colomn in colomnUp.Where(x => x.Sort <= _DragAndDropTable.Sort + Math.Abs(count)))
                {
                    colomn.Sort--;
                }
            }

            switch (_DragAndDropTable.Sort - count)
            {
                case var x when x >= this._panelTableLeft.Count - 1:
                    _DragAndDropTable.Sort = this._panelTableLeft.Count - 1;
                    break;
                case var x when x < 0:
                    _DragAndDropTable.Sort = 0;
                    break;
                default:
                    _DragAndDropTable.Sort -= count;
                    break;
            }
        }

        /// <summary>
        /// Установить основную таблицу.
        /// </summary>
        /// <param name="newMainTable"></param>
        private void SetMainTable(TableDto newMainTable)
        {
            this._mainTable.Panel.BackColor = Colors.PanelFon;
            this._mainTable.TextBox.BackColor = Colors.PanelFon;
            this._mainTable.TextBox.ForeColor = Color.White;
            this._mainTable.Icon.Image = IconEnums.Quest;
            this._mainTable = newMainTable;
            this._mainTable.Panel.BackColor = Colors.PanelActiveObject;
            this._mainTable.TextBox.BackColor = Colors.PanelActiveObject;
            this._mainTable.TextBox.ForeColor = Colors.PanelActiveObjectFore;
            this._mainTable.Icon.Image = IconEnums.QuestActive;

            this.textBox1.Text = this._mainTable.TextBox.Text;

            this.UpdateEndScriptTable();
            this.UpdateEndScriptColomn();
            this.UpdateEndScriptRecord(); // тут вывод скрипта

            var newMainColomn = this._mainTable.Colomns.OrderBy(x => x.Sort).FirstOrDefault();
            this.SetMainColomn(newMainColomn);
        }

        /// <summary>
        /// Установить основну колонку.
        /// </summary>
        /// <param name="newMainColomn"></param>
        private void SetMainColomn(ColomnDto newMainColomn)
        {
            this._mainColomn.Panel.BackColor = Colors.PanelFon;
            this._mainColomn.TextBox.BackColor = Colors.PanelFon;
            this._mainColomn.TextBoxCount.BackColor = Colors.PanelFon;
            this._mainColomn.TextBox.ForeColor = Color.White;
            this._mainColomn.TextBoxCount.ForeColor = Color.White;
            this._mainColomn.Icon.Image = IconEnums.Pencil;
            this._mainColomn.IconStar.Image = IconEnums.Star;
            this._mainColomn.Records = this.lineNumberRTB1.RichTextBox.Text;

            this._dataBaseService.UpdateColomns(this._mainColomn);
            this._mainColomn = newMainColomn;

            this._mainColomn.Panel.BackColor = Colors.PanelActiveObject;
            this._mainColomn.TextBox.BackColor = Colors.PanelActiveObject;
            this._mainColomn.TextBoxCount.BackColor = Colors.PanelActiveObject;
            this._mainColomn.TextBox.ForeColor = Colors.PanelActiveObjectFore;
            this._mainColomn.TextBoxCount.ForeColor = Colors.PanelActiveObjectFore;
            this._mainColomn.Icon.Image = IconEnums.PencilActive;
            this._mainColomn.IconStar.Image = IconEnums.StarActive;
            this.lineNumberRTB1.RichTextBox.Text = this._mainColomn.Records;

            this.pictureBoxAddStar.Image = this._mainColomn.IsEqualsRecordStar
                ? IconEnums.StarActive
                : IconEnums.Star;

            this.pictureBoxAddQuotes.Image = this._mainColomn.IsQuotes
                ? IconEnums.QuotesActive2
                : IconEnums.Quotes2;

            this.textBox2.Text = this._mainColomn.TextBox.Text;
            this.textBox3.Text = this._mainColomn.DirectoryTableName;
            this.textBox4.Text = this._mainColomn.DirectoryColomnName;

        }

        /// <summary>
        /// Изменение отображение таблиц.
        /// </summary>
        private void UpdatePanelTable()
        {
            var tables = this._panelTableLeft.OrderByDescending(x => x.Sort);

            foreach (var table in tables)
            {
                table.Panel.Location = new Point(0, (this._panelTableLeft.Count() - table.Sort - 1) * SizeEnums.HeightPanel);
                panel3.Controls.Add(table.Panel);
            }
        }

        /// <summary>
        /// Изменение отображения колонок.
        /// </summary>
        private void UpdatePanelColomn()
        {
            panel2.Controls.Clear();
            var colomns = this._mainTable.Colomns.OrderBy(x => x.Sort);

            foreach (var colomn in colomns)
            {
                colomn.Panel.Location = new Point(0, colomn.Sort * SizeEnums.HeightPanel);
                panel2.Controls.Add(colomn.Panel);
            }
        }

        /// <summary>
        /// При нажатии на клавишу пересчитывание количества строк.
        /// </summary>
        private void lineNumberRTB1_KeyDown(object sender, KeyEventArgs e)
            => this.RewriteCountRecord(e);

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
                this._mainColomn.Records = lineNumberRTB1.RichTextBox.Text;
                this._mainColomn.TextBoxCount.Text = this._mainColomn.CountRecords;
                this.SetCountRecordColomn(_mainColomn);
            }
        }

        /// <summary>
        /// Установить количество строк в колонке.
        /// </summary>
        /// <param name="colomns"></param>
        private void SetCountRecordColomn(ColomnDto colomn)
        {
            colomn.TextBoxCount.Text = colomn.CountRecords;
            if (colomn.TextBoxCount.Text == "0")
            {
                colomn.TextBoxCount.Text = "1";
            }
        }

        /// <summary>
        /// Изменение начало скрипта (название таблицы)
        /// </summary>
        private void UpdateEndScriptTable()
            => EndScriptTable = string.Format(BuildingScript.Table, this._mainTable.TextBox.Text);

        /// <summary>
        /// Изменение середины скрипта (записи).
        /// </summary>
        private void UpdateEndScriptRecord()
        {
            if (!_mainTable.Colomns.Any())
            {
                return;
            }

            this._mainColomn.Records = lineNumberRTB1.RichTextBox.Text;
            var maxCount = this._mainTable.Colomns.Max(x => x.CountRecords.ToInt());
            var colomnTextAndQuotes = this._mainTable.Colomns
                .OrderBy(x => x.Sort)
                .Select(x => new
                {
                    x.IsQuotes,
                    Text = x.Records.Split('\n')
                });

            List<string> recs = new List<string>();
            for (int i = 0; i < maxCount; i++)
            {
                List<string> rec = new List<string>();
                foreach (var colomn in colomnTextAndQuotes)
                {

                    rec.Add(colomn.IsQuotes
                        ? i < colomn.Text.Count() ? $"'{colomn.Text[i]}'" : "''"
                        : i < colomn.Text.Count() ? $"{colomn.Text[i]}" : string.Empty);
                }
                recs.Add($"({string.Join(", ", rec)})");
            }
            EndScriptRecord = string.Join("\n       ,", recs);
            this.OutputEndScript();
        }

        /// <summary>
        /// Изменение конца скрипта (название колонок).
        /// </summary>
        private void UpdateEndScriptColomn()
        {
            var sortColomn = this._mainTable.Colomns.OrderBy(x => x.Sort);

            if (this._mainTable.Colomns.Any(x => x.IsExistDirectory))
            {

            }

            var columnList = string.Join(", ", sortColomn.Select(x => x.TextBox.Text));
            var equalConditions = string.Join(" and ", sortColomn.Where(x => x.IsEqualsRecordStar).Select(x => string.Format(BuildingScript.Assign, x.TextBox.Text)));
            var notEqualConditions = string.Join(" or\n    ",sortColomn.Where(x => !x.IsEqualsRecordStar).Select(x => string.Format(BuildingScript.NotAssign, x.TextBox.Text)));
            var assignList = string.Join(",\n        ",sortColomn.Where(x => !x.IsEqualsRecordStar).Select(x => string.Format(BuildingScript.Assign, x.TextBox.Text)));
            var sourceColumns = string.Join(", ",sortColomn.Select(x => "source." + x.TextBox.Text));

            EndScriptColomn = string.Format(
                BuildingScript.Colomns,
                columnList,
                equalConditions,
                notEqualConditions,
                assignList,
                sourceColumns
            );

        }

        /// <summary>
        /// Установление ссылочных таблиц.
        /// </summary>
        /// <param name="colomn"></param>
        /// <param name="colomnName"></param>
        private void SetDirectoryName(string mainTableName, ColomnDto colomn, string colomnName)
        {
            int dotIndex = colomnName.IndexOf('.');
            if (dotIndex < 0 || dotIndex + 1 == colomn.TextBox.Name.Length)
            {
                colomn.IsExistDirectory = false;
                colomn.TextBox.Text = colomnName;
                return;
            }

            colomn.TextBox.Text = colomnName.Substring(0, dotIndex);
            colomn.IsExistDirectory = true;
            colomn.DirectoryColomnName = colomnName.Substring(dotIndex + 1);
            colomn.DirectoryTableName = this._dataBaseService.GetDirectoryTableName(mainTableName, colomnName);
        }

        /// <summary>
        /// Вывод скрипта.
        /// </summary>
        private void OutputEndScript()
        => richTextBox3.Text = this._mainTable.IsTemplateScript
            ? SaveScriptService.GetEndScriptWithTemplate(this._mainTable.SaveScriptModel)
            : this.GetEndScript();

        /// <summary>
        /// Навел на иконку.
        /// </summary>
        private void navel_na_ikonky_MouseMove(object sender, MouseEventArgs e) =>
            ((PictureBox)sender).Parent.BackColor = Colors.PanelMouseMoveObject;

        /// <summary>
        /// Убрал мышку с иконки.
        /// </summary>
        private void navel_na_ikonky_MouseLeave(object sender, EventArgs e) =>
            ((PictureBox)sender).Parent.BackColor = ((PictureBox)sender).Parent.Parent.BackColor;

        /// <summary>
        /// Нажал на мышь (не отпустил).
        /// </summary>
        private void navel_na_ikonky_MouseDown(object sender, MouseEventArgs e) =>
            ((PictureBox)sender).Parent.BackColor = Colors.PanelActiveObject;

        /// <summary>
        /// Отпустил мышь.
        /// </summary>
        private void navel_na_ikonky_MouseUp(object sender, MouseEventArgs e) =>
            ((PictureBox)sender).Parent.BackColor = Colors.PanelUpFon;

        /// <summary>
        /// Навел на иконку.
        /// </summary>
        private void navel_na_ikonky2_MouseMove(object sender, MouseEventArgs e) =>
            ((PictureBox)sender).Parent.BackColor = Colors.PanelMouseMoveObject2;

        /// <summary>
        /// Отпустил мышь.
        /// </summary>
        private void navel_na_ikonky2_MouseUp(object sender, MouseEventArgs e) =>
            ((PictureBox)sender).Parent.BackColor = SystemColors.ActiveCaption;

        /// <summary>
        /// Проверка, является ли textBox таблицей (или это колонка).
        /// </summary>
        /// <returns></returns>
        private bool CheckIsColomn(object sender)
            => this._mainTable.Colomns.Any(x => x.TextBox == (TextBox)sender);

        /// <summary>
        /// Изменение название таблицы.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            this._mainTable.TextBox.Text = textBox1.Text;
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
            this._mainColomn.TextBox.Text = textBox2.Text;
            this.UpdateEndScriptColomn();
            this.OutputEndScript();
        }

        /// <summary>
        /// Изменение название связанной таблицы.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox3_KeyUp(object sender, KeyEventArgs e)
        {
            this._mainColomn.DirectoryTableName= textBox3.Text;
            this.UpdateEndScriptColomn();
            this.OutputEndScript();
        }

        /// <summary>
        /// Изменение названия связанной колонки.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox4_KeyUp(object sender, KeyEventArgs e)
        {
            this._mainColomn.DirectoryColomnName = textBox4.Text;
            this.UpdateEndScriptColomn();
            this.OutputEndScript();
        }

        /// <summary>
        /// Сохранение скрипта.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBoxSaveScript_Click(object sender, EventArgs e)
        {
            _dataBaseService.UpdateTables(_mainTable);
            _dataBaseService.UpdateColomns(_mainColomn);

            this._mainTable.SaveScriptModel.Script = this.GetEndScript();
            var formDialog = new FormSaveScript(_mainTable.SaveScriptModel, _dataBaseService);
            formDialog.ShowDialog();

            _dataBaseService.UpdateTables(_mainTable);
        }

        /// <summary>
        /// Получение конечного скрипта.
        /// </summary>
        /// <returns></returns>
        private string GetEndScript()
            => EndScriptTable + EndScriptRecord + EndScriptColomn;

        /// <summary>
        /// События после закрытия формы.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormScriptMerge_FormClosed(object sender, FormClosedEventArgs e)
        {
            _dataBaseService.UpdateTables(_mainTable);
            _dataBaseService.UpdateColomns(_mainColomn);
        }
    }
}
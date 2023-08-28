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

            this.SetMainTable(new TableModelObjPanel());
            _panelTableLeft.Add(_mainTable);
            _mainTable.Sort = _panelTableLeft.Count();
            _mainTable.Icon.Image = Image.FromFile(@"images\icons\quest.png");
            _mainTable.TextBox.Text = formAddTableName.TableName;

            _mainTable.TextBox.MouseMove += MouseMove;      // Курсор на объекте, навелся, перетаскивание.
            _mainTable.TextBox.MouseDown += MouseDown;      // Нажал левую кнопку мыши, не отпустил \ перетаскивание
            _mainTable.TextBox.MouseUp += MouseUp;          // Клинкул по объекту (отпустил мышь)
            _mainTable.TextBox.MouseLeave += MouseLeave;    // Отвел курсор с объекта.

            // получение названий колонок.
            var colomNames = _formQuotesService.FormatingString(formAddTableName.ColomnNames, true, true, true, true);
            foreach (var colom in colomNames)
            {
                var colomn = new ColomnModelObjPanel();
                colomn.Sort = colomNames.Count() - _mainTable.Colomns.Count();
                colomn.Icon.Image = Image.FromFile(@"images\icons\pencil.png");
                colomn.TextBox.Text = colom;
                _mainTable.Colomns.Add(colomn);

                colomn.TextBox.MouseMove += MouseMove;      // Курсор на объекте, навелся, перетаскивание.
                colomn.TextBox.MouseDown += MouseDown;      // Нажал левую кнопку мыши, не отпустил \ перетаскивание
                colomn.TextBox.MouseUp += MouseUp;          // Клинкул по объекту (отпустил мышь)
                colomn.TextBox.MouseLeave += MouseLeave;    // Отвел курсор с объекта.
            }

            var newMainColomn = _mainTable.Colomns.FirstOrDefault(x => x.Sort == colomNames.Count());
            this.SetMainColomn(newMainColomn);
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
                this.SetMainColomn(colomn);
                this.UpdatePanelTable();
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

        private void button1_Click(object sender, EventArgs e)
        {
            lineNumberRTB1.RichTextBox.BackColor = Color.DarkOrange;
        }

        private void SetMainTable(TableModelObjPanel newMainTable)
        {
            _mainTable.Panel.BackColor = Colors.PanelFon;
            _mainTable.TextBox.BackColor = Colors.PanelFon;
            _mainTable = newMainTable;
            _mainTable.Panel.BackColor = Colors.PanelActiveObject;
            _mainTable.TextBox.BackColor = Colors.PanelActiveObject;
            richTextBox3.Text = _mainTable.EndScript;
        }

        private void SetMainColomn(ColomnModelObjPanel newMainColomn)
        {
            _mainColomn.Panel.BackColor = Colors.PanelFon;
            _mainColomn.TextBox.BackColor = Colors.PanelFon;
            _mainColomn = newMainColomn;
            _mainColomn.Panel.BackColor = Colors.PanelActiveObject;
            _mainColomn.TextBox.BackColor = Colors.PanelActiveObject;
            lineNumberRTB1.RichTextBox.Text = _mainColomn.Records;
        }

        private void UpdatePanelTable()
        {
            panel3.Controls.Clear();
            foreach (var table in _panelTableLeft)
            {
                table.Panel.Location = new Point(0, (_panelTableLeft.Count() - table.Sort) * SizeEnums.HeightPanel);
                panel3.Controls.Add(table.Panel);
            }
            this.UpdatePanelColomn();
        }

        private void UpdatePanelColomn()
        {
            panel2.Controls.Clear();
            foreach (var colomn in _mainTable.Colomns)
            {
                colomn.Panel.Location = new Point(0, (_mainTable.Colomns.Count() - colomn.Sort) * SizeEnums.HeightPanel);
                panel2.Controls.Add(colomn.Panel);
            }
        }
    }
}
using MyHelper.Dto;
using MyHelper.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyHelper
{
    public partial class FormScriptMerge : Form
    {
        /// <summary>
        /// Левая колонка с задачами.
        /// </summary>
        private List<TableModelObjPanel> panelObjectsLeft = new List<TableModelObjPanel>();

        /// <summary>
        /// Правая колонка с задачами.
        /// </summary>
        //private List<BaseModelObjPanel> panelObjectsRight = new List<BaseModelObjPanel>();

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
            FormAddTable formAddTableName = new FormAddTable();
            formAddTableName.ShowDialog();

            if (formAddTableName.TableName.Trim() == string.Empty || formAddTableName.ColomnNames.Trim() == string.Empty)
            {
                return;
            }

            var table = new TableModelObjPanel();
            table.Icon.Image = Image.FromFile(@"images\icons\quest.png");
            table.TextBox.Text = formAddTableName.TableName;
            table.Panel.Location = new Point(0, panelObjectsLeft.Count * table.Panel.Size.Height);
            table.Panel.BackColor = Colors.PanelActiveObject;
            table.TextBox.BackColor = Colors.PanelActiveObject;

            table.TextBox.MouseDown += MouseDown;
            table.TextBox.MouseMove += MouseMove;
            table.TextBox.MouseLeave += MouseLeave;

            panel3.Controls.Add(table.Panel);
            panelObjectsLeft.Add(table);

            var formQuotes = new FormQuotes();
            var colomNames = formQuotes.FormatingString(formAddTableName.ColomnNames, true, true, true, true);
            panel2.Controls.Clear();

            foreach (var coloms in colomNames)
            {
                var colomn = new BaseModelObjPanel();
                colomn.Icon.Image = Image.FromFile(@"images\icons\pencil.png");
                colomn.TextBox.Text = coloms;
                colomn.Panel.Location = new Point(0, table.Colomns.Count() * table.Panel.Size.Height);
                colomn.Sort = colomNames.Count() - table.Colomns.Count();

                colomn.TextBox.MouseDown += MouseDown;
                colomn.TextBox.MouseMove += MouseMove;
                colomn.TextBox.MouseLeave += MouseLeave;

                panel2.Controls.Add(colomn.Panel);
                table.Colomns.Add(colomn);
            }

            var firstColomn = table.Colomns.Where(x => x.Sort == colomNames.Count()).First();
            firstColomn.Panel.BackColor = Colors.PanelActiveObject;
            firstColomn.TextBox.BackColor = Colors.PanelActiveObject;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            var formDialog = new FormAddColomns(this);
            formDialog.ShowDialog();


            //var task = new BaseModelObjPanel(panelObjectsRight.Count);
            //panel2.Controls.Add(task.Panel);
            //task.Icon.Image = Image.FromFile(@"images\icons\pencil.png");

            //panelObjectsRight.Add(task);

            //task.TextBox.MouseDown += MouseDown;
            //task.TextBox.MouseMove += MouseMove;
            //task.TextBox.MouseLeave += MouseLeave;
        }

        /// <summary>
        /// Нажал левую кнопку мыши, не отпустил. \ перетаскивание
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
        /// Курсор на объекте, навелся, перетаскивание.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseMove(object sender, EventArgs e)
        {
            ((TextBox)sender).BackColor = Colors.PanelMouseMoveObject;
            ((TextBox)sender).Parent.BackColor = Colors.PanelMouseMoveObject;
        }

        /// <summary>
        /// Отвел курсор с объекта.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseLeave(object sender, EventArgs e)
        {
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
    }
}

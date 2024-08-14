using MyHelper.NewPanelComponent;

namespace MyHelper.DialogForms.ScriptMerge
{
    partial class FormSaveScript
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.textBoxPath = new MyHelper.NewPanelComponent.TextBoxEx();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxTask = new MyHelper.NewPanelComponent.TextBoxEx();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxProject = new MyHelper.NewPanelComponent.TextBoxEx();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxDescription = new MyHelper.NewPanelComponent.TextBoxEx();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.buttonSaveScript = new System.Windows.Forms.Button();
            this.textBoxFileName = new MyHelper.NewPanelComponent.TextBoxEx();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxGuid = new MyHelper.NewPanelComponent.TextBoxEx();
            this.button4 = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxNumber = new MyHelper.NewPanelComponent.TextBoxEx();
            this.textBoxSprint = new MyHelper.NewPanelComponent.TextBoxEx();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.Location = new System.Drawing.Point(559, 35);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(137, 28);
            this.button1.TabIndex = 0;
            this.button1.Text = "Обзор";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.buttonReview_Click);
            // 
            // textBoxPath
            // 
            this.textBoxPath.Font = new System.Drawing.Font("Times New Roman", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxPath.IsEmpty = false;
            this.textBoxPath.IsValid = false;
            this.textBoxPath.Location = new System.Drawing.Point(13, 35);
            this.textBoxPath.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxPath.Name = "textBoxPath";
            this.textBoxPath.Placeholder = "Укажите путь";
            this.textBoxPath.ReadOnly = true;
            this.textBoxPath.Size = new System.Drawing.Size(539, 28);
            this.textBoxPath.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Location = new System.Drawing.Point(12, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(572, 20);
            this.label1.TabIndex = 7;
            this.label1.Text = "Укажите папку, в которую нужно сохранить скрипт (указать Emias)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label2.Location = new System.Drawing.Point(12, 121);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 20);
            this.label2.TabIndex = 9;
            this.label2.Text = "Релиз (х.хх.х)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label3.Location = new System.Drawing.Point(158, 121);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 20);
            this.label3.TabIndex = 11;
            this.label3.Text = "Задача";
            // 
            // textBoxTask
            // 
            this.textBoxTask.Font = new System.Drawing.Font("Times New Roman", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxTask.IsEmpty = false;
            this.textBoxTask.IsValid = false;
            this.textBoxTask.Location = new System.Drawing.Point(162, 145);
            this.textBoxTask.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxTask.Name = "textBoxTask";
            this.textBoxTask.Placeholder = "DISP-хххх";
            this.textBoxTask.Size = new System.Drawing.Size(122, 28);
            this.textBoxTask.TabIndex = 10;
            this.textBoxTask.KeyDown += new System.Windows.Forms.KeyEventHandler(this.UpdateNameFileKeyDownAndUp);
            this.textBoxTask.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UpdateNameFileKeyDownAndUp);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label4.Location = new System.Drawing.Point(309, 121);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 20);
            this.label4.TabIndex = 13;
            this.label4.Text = "Проект";
            // 
            // textBoxProject
            // 
            this.textBoxProject.Font = new System.Drawing.Font("Times New Roman", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxProject.IsEmpty = false;
            this.textBoxProject.IsValid = false;
            this.textBoxProject.Location = new System.Drawing.Point(313, 145);
            this.textBoxProject.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxProject.Name = "textBoxProject";
            this.textBoxProject.Placeholder = "Диспансеризация";
            this.textBoxProject.Size = new System.Drawing.Size(223, 28);
            this.textBoxProject.TabIndex = 12;
            this.textBoxProject.KeyDown += new System.Windows.Forms.KeyEventHandler(this.UpdateNameFileKeyDownAndUp);
            this.textBoxProject.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UpdateNameFileKeyDownAndUp);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label5.Location = new System.Drawing.Point(555, 120);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(137, 20);
            this.label5.TabIndex = 15;
            this.label5.Text = "Номер скрипта";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label6.Location = new System.Drawing.Point(12, 185);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(212, 20);
            this.label6.TabIndex = 17;
            this.label6.Text = "Наименование скрипта ";
            // 
            // textBoxDescription
            // 
            this.textBoxDescription.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxDescription.Font = new System.Drawing.Font("Times New Roman", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxDescription.IsEmpty = false;
            this.textBoxDescription.IsValid = false;
            this.textBoxDescription.Location = new System.Drawing.Point(16, 209);
            this.textBoxDescription.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.Placeholder = "Укажите название скрипта";
            this.textBoxDescription.Size = new System.Drawing.Size(680, 28);
            this.textBoxDescription.TabIndex = 16;
            this.textBoxDescription.KeyDown += new System.Windows.Forms.KeyEventHandler(this.UpdateNameFileKeyDownAndUp);
            this.textBoxDescription.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UpdateNameFileKeyDownAndUp);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Checked = true;
            this.checkBox2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox2.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.checkBox2.Location = new System.Drawing.Point(16, 329);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(247, 20);
            this.checkBox2.TabIndex = 18;
            this.checkBox2.Text = "Открыть папку после сохранения";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button2.Location = new System.Drawing.Point(407, 359);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(164, 39);
            this.button2.TabIndex = 20;
            this.button2.Text = "Отмена";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // buttonSaveScript
            // 
            this.buttonSaveScript.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonSaveScript.Location = new System.Drawing.Point(125, 359);
            this.buttonSaveScript.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSaveScript.Name = "buttonSaveScript";
            this.buttonSaveScript.Size = new System.Drawing.Size(164, 40);
            this.buttonSaveScript.TabIndex = 19;
            this.buttonSaveScript.Text = "Сохранить";
            this.buttonSaveScript.UseVisualStyleBackColor = true;
            this.buttonSaveScript.Click += new System.EventHandler(this.buttonSaveScript_Click);
            // 
            // textBoxFileName
            // 
            this.textBoxFileName.Enabled = false;
            this.textBoxFileName.Font = new System.Drawing.Font("Times New Roman", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxFileName.IsEmpty = false;
            this.textBoxFileName.IsValid = false;
            this.textBoxFileName.Location = new System.Drawing.Point(16, 268);
            this.textBoxFileName.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxFileName.Name = "textBoxFileName";
            this.textBoxFileName.Placeholder = null;
            this.textBoxFileName.Size = new System.Drawing.Size(680, 28);
            this.textBoxFileName.TabIndex = 21;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label7.Location = new System.Drawing.Point(12, 244);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(195, 20);
            this.label7.TabIndex = 22;
            this.label7.Text = "Наименование файла";
            // 
            // textBoxGuid
            // 
            this.textBoxGuid.Font = new System.Drawing.Font("Times New Roman", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxGuid.IsEmpty = false;
            this.textBoxGuid.IsValid = false;
            this.textBoxGuid.Location = new System.Drawing.Point(13, 89);
            this.textBoxGuid.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxGuid.Name = "textBoxGuid";
            this.textBoxGuid.Placeholder = "00000000-0000-0000-0000-000000000000";
            this.textBoxGuid.Size = new System.Drawing.Size(539, 28);
            this.textBoxGuid.TabIndex = 24;
            // 
            // button4
            // 
            this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button4.Location = new System.Drawing.Point(559, 89);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(137, 28);
            this.button4.TabIndex = 25;
            this.button4.Text = "Обновить";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.buttonUpdateGuid_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label8.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label8.Location = new System.Drawing.Point(12, 67);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(326, 20);
            this.label8.TabIndex = 26;
            this.label8.Text = "Уникальный идентификатор скрипта";
            // 
            // textBoxNumber
            // 
            this.textBoxNumber.Font = new System.Drawing.Font("Times New Roman", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxNumber.IsEmpty = false;
            this.textBoxNumber.IsValid = false;
            this.textBoxNumber.Location = new System.Drawing.Point(562, 145);
            this.textBoxNumber.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxNumber.MaxLength = 3;
            this.textBoxNumber.Name = "textBoxNumber";
            this.textBoxNumber.Placeholder = "xxx";
            this.textBoxNumber.Size = new System.Drawing.Size(133, 28);
            this.textBoxNumber.TabIndex = 14;
            this.textBoxNumber.KeyDown += new System.Windows.Forms.KeyEventHandler(this.UpdateNameFileKeyDownAndUp);
            this.textBoxNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxNumber_KeyPress);
            this.textBoxNumber.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UpdateNameFileKeyDownAndUp);
            // 
            // textBoxSprint
            // 
            this.textBoxSprint.Font = new System.Drawing.Font("Times New Roman", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxSprint.IsEmpty = false;
            this.textBoxSprint.IsValid = false;
            this.textBoxSprint.Location = new System.Drawing.Point(16, 145);
            this.textBoxSprint.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxSprint.MaxLength = 6;
            this.textBoxSprint.Name = "textBoxSprint";
            this.textBoxSprint.Placeholder = "х.хх.х";
            this.textBoxSprint.Size = new System.Drawing.Size(122, 28);
            this.textBoxSprint.TabIndex = 27;
            this.textBoxSprint.TextChanged += new System.EventHandler(this.textBoxSprint_TextChanged);
            this.textBoxSprint.KeyDown += new System.Windows.Forms.KeyEventHandler(this.UpdateNameFileKeyDownAndUp);
            this.textBoxSprint.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxSprint_KeyPress);
            this.textBoxSprint.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UpdateNameFileKeyDownAndUp);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.Location = new System.Drawing.Point(702, 21);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(468, 387);
            this.richTextBox1.TabIndex = 28;
            this.richTextBox1.Text = "";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.checkBox1.Location = new System.Drawing.Point(16, 303);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(265, 20);
            this.checkBox1.TabIndex = 29;
            this.checkBox1.Text = "Добавить папку проекта (подпапка)";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // FormSaveScript
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(1182, 411);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.textBoxSprint);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.textBoxGuid);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBoxFileName);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.buttonSaveScript);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBoxDescription);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxNumber);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxProject);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxTask);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxPath);
            this.Controls.Add(this.button1);
            this.MaximumSize = new System.Drawing.Size(2000, 458);
            this.MinimumSize = new System.Drawing.Size(1200, 458);
            this.Name = "FormSaveScript";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Сохранение скрипта";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private TextBoxEx textBoxPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private TextBoxEx textBoxTask;
        private System.Windows.Forms.Label label4;
        private TextBoxEx textBoxProject;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private TextBoxEx textBoxDescription;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button buttonSaveScript;
        private TextBoxEx textBoxFileName;
        private System.Windows.Forms.Label label7;
        private TextBoxEx textBoxGuid;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label8;
        private TextBoxEx textBoxNumber;
        private TextBoxEx textBoxSprint;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}
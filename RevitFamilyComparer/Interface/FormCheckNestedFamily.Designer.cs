namespace RevitFamilyComparer.Interface
{
    partial class FormCheckNestedFamily
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxFamilyPath = new System.Windows.Forms.TextBox();
            this.buttonSelectFamily = new System.Windows.Forms.Button();
            this.textBoxLibraryPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonSelectLibrary = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Семейство для проверки:";
            // 
            // textBoxFamilyPath
            // 
            this.textBoxFamilyPath.Location = new System.Drawing.Point(13, 30);
            this.textBoxFamilyPath.Name = "textBoxFamilyPath";
            this.textBoxFamilyPath.Size = new System.Drawing.Size(274, 20);
            this.textBoxFamilyPath.TabIndex = 1;
            // 
            // buttonSelectFamily
            // 
            this.buttonSelectFamily.Location = new System.Drawing.Point(212, 56);
            this.buttonSelectFamily.Name = "buttonSelectFamily";
            this.buttonSelectFamily.Size = new System.Drawing.Size(75, 23);
            this.buttonSelectFamily.TabIndex = 2;
            this.buttonSelectFamily.Text = "Обзор";
            this.buttonSelectFamily.UseVisualStyleBackColor = true;
            this.buttonSelectFamily.Click += new System.EventHandler(this.buttonSelectFamily_Click);
            // 
            // textBoxLibraryPath
            // 
            this.textBoxLibraryPath.Location = new System.Drawing.Point(12, 110);
            this.textBoxLibraryPath.Name = "textBoxLibraryPath";
            this.textBoxLibraryPath.Size = new System.Drawing.Size(275, 20);
            this.textBoxLibraryPath.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 91);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Библиотека семейств:";
            // 
            // buttonSelectLibrary
            // 
            this.buttonSelectLibrary.Location = new System.Drawing.Point(212, 136);
            this.buttonSelectLibrary.Name = "buttonSelectLibrary";
            this.buttonSelectLibrary.Size = new System.Drawing.Size(75, 23);
            this.buttonSelectLibrary.TabIndex = 2;
            this.buttonSelectLibrary.Text = "Обзор";
            this.buttonSelectLibrary.UseVisualStyleBackColor = true;
            this.buttonSelectLibrary.Click += new System.EventHandler(this.buttonSelectLibrary_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(212, 210);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "Отмена";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(131, 210);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 6;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // FormCheckNestedFamily
            // 
            this.AcceptButton = this.buttonOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(296, 245);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxLibraryPath);
            this.Controls.Add(this.buttonSelectLibrary);
            this.Controls.Add(this.buttonSelectFamily);
            this.Controls.Add(this.textBoxFamilyPath);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormCheckNestedFamily";
            this.Text = "FormCheckNestedFamily";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxFamilyPath;
        private System.Windows.Forms.Button buttonSelectFamily;
        private System.Windows.Forms.TextBox textBoxLibraryPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonSelectLibrary;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOk;
    }
}
namespace QLSV
{
    partial class TypeAccountForm
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
            this.LabelRegister = new System.Windows.Forms.Label();
            this.ButtonConfirm = new System.Windows.Forms.Button();
            this.RadioButtonStudent = new System.Windows.Forms.RadioButton();
            this.RadioButtonHumanResource = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // LabelRegister
            // 
            this.LabelRegister.AutoSize = true;
            this.LabelRegister.Font = new System.Drawing.Font("Monotype Corsiva", 24F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelRegister.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.LabelRegister.Location = new System.Drawing.Point(226, 19);
            this.LabelRegister.Name = "LabelRegister";
            this.LabelRegister.Size = new System.Drawing.Size(328, 49);
            this.LabelRegister.TabIndex = 11;
            this.LabelRegister.Text = "Select Type Account";
            // 
            // ButtonConfirm
            // 
            this.ButtonConfirm.BackColor = System.Drawing.Color.Fuchsia;
            this.ButtonConfirm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonConfirm.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonConfirm.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.ButtonConfirm.Location = new System.Drawing.Point(314, 225);
            this.ButtonConfirm.Margin = new System.Windows.Forms.Padding(4);
            this.ButtonConfirm.Name = "ButtonConfirm";
            this.ButtonConfirm.Size = new System.Drawing.Size(154, 43);
            this.ButtonConfirm.TabIndex = 42;
            this.ButtonConfirm.Text = "Confirm";
            this.ButtonConfirm.UseVisualStyleBackColor = false;
            this.ButtonConfirm.Click += new System.EventHandler(this.ButtonConfirm_Click);
            // 
            // RadioButtonStudent
            // 
            this.RadioButtonStudent.AutoSize = true;
            this.RadioButtonStudent.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RadioButtonStudent.Location = new System.Drawing.Point(219, 126);
            this.RadioButtonStudent.Name = "RadioButtonStudent";
            this.RadioButtonStudent.Size = new System.Drawing.Size(108, 29);
            this.RadioButtonStudent.TabIndex = 43;
            this.RadioButtonStudent.TabStop = true;
            this.RadioButtonStudent.Text = "Student";
            this.RadioButtonStudent.UseVisualStyleBackColor = true;
            this.RadioButtonStudent.CheckedChanged += new System.EventHandler(this.RadioButtonStudent_CheckedChanged);
            // 
            // RadioButtonHumanResource
            // 
            this.RadioButtonHumanResource.AutoSize = true;
            this.RadioButtonHumanResource.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.RadioButtonHumanResource.Location = new System.Drawing.Point(433, 126);
            this.RadioButtonHumanResource.Name = "RadioButtonHumanResource";
            this.RadioButtonHumanResource.Size = new System.Drawing.Size(198, 29);
            this.RadioButtonHumanResource.TabIndex = 44;
            this.RadioButtonHumanResource.TabStop = true;
            this.RadioButtonHumanResource.Text = "Human Resource";
            this.RadioButtonHumanResource.UseVisualStyleBackColor = true;
            this.RadioButtonHumanResource.CheckedChanged += new System.EventHandler(this.RadioButtonHumanResource_CheckedChanged);
            // 
            // TypeAccountForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(827, 307);
            this.Controls.Add(this.RadioButtonHumanResource);
            this.Controls.Add(this.RadioButtonStudent);
            this.Controls.Add(this.ButtonConfirm);
            this.Controls.Add(this.LabelRegister);
            this.Name = "TypeAccountForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TypeAccountForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LabelRegister;
        private System.Windows.Forms.Button ButtonConfirm;
        private System.Windows.Forms.RadioButton RadioButtonStudent;
        private System.Windows.Forms.RadioButton RadioButtonHumanResource;
    }
}
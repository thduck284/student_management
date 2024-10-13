namespace QLSV
{
    partial class StaticAdminForm
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
            this.radioButtonGender = new System.Windows.Forms.RadioButton();
            this.radioButtonResult = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonView = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // radioButtonGender
            // 
            this.radioButtonGender.AutoSize = true;
            this.radioButtonGender.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonGender.Location = new System.Drawing.Point(42, 49);
            this.radioButtonGender.Name = "radioButtonGender";
            this.radioButtonGender.Size = new System.Drawing.Size(121, 33);
            this.radioButtonGender.TabIndex = 0;
            this.radioButtonGender.TabStop = true;
            this.radioButtonGender.Text = "Gender";
            this.radioButtonGender.UseVisualStyleBackColor = true;
            this.radioButtonGender.CheckedChanged += new System.EventHandler(this.radioButtonGender_CheckedChanged);
            // 
            // radioButtonResult
            // 
            this.radioButtonResult.AutoSize = true;
            this.radioButtonResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonResult.Location = new System.Drawing.Point(244, 49);
            this.radioButtonResult.Name = "radioButtonResult";
            this.radioButtonResult.Size = new System.Drawing.Size(108, 33);
            this.radioButtonResult.TabIndex = 1;
            this.radioButtonResult.TabStop = true;
            this.radioButtonResult.Text = "Result";
            this.radioButtonResult.UseVisualStyleBackColor = true;
            this.radioButtonResult.CheckedChanged += new System.EventHandler(this.radioButtonResult_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonGender);
            this.groupBox1.Controls.Add(this.radioButtonResult);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(440, 270);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(411, 100);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Student Static";
            // 
            // buttonView
            // 
            this.buttonView.BackColor = System.Drawing.Color.Lime;
            this.buttonView.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonView.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonView.Location = new System.Drawing.Point(558, 410);
            this.buttonView.Name = "buttonView";
            this.buttonView.Size = new System.Drawing.Size(133, 41);
            this.buttonView.TabIndex = 3;
            this.buttonView.Text = "View";
            this.buttonView.UseVisualStyleBackColor = false;
            this.buttonView.Click += new System.EventHandler(this.buttonView_Click);
            // 
            // StaticAdminForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(1279, 705);
            this.Controls.Add(this.buttonView);
            this.Controls.Add(this.groupBox1);
            this.Name = "StaticAdminForm";
            this.Text = "StaticAdminForm";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton radioButtonGender;
        private System.Windows.Forms.RadioButton radioButtonResult;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonView;
    }
}
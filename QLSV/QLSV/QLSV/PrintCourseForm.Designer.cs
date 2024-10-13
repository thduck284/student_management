namespace QLSV
{
    partial class PrintCourseForm
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
            this.DataGridView1 = new System.Windows.Forms.DataGridView();
            this.ButtonToFile = new System.Windows.Forms.Button();
            this.ButtonPrint = new System.Windows.Forms.Button();
            this.buttonToExcel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // DataGridView1
            // 
            this.DataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DataGridView1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridView1.Location = new System.Drawing.Point(85, 78);
            this.DataGridView1.Name = "DataGridView1";
            this.DataGridView1.RowHeadersWidth = 51;
            this.DataGridView1.RowTemplate.Height = 24;
            this.DataGridView1.Size = new System.Drawing.Size(1015, 408);
            this.DataGridView1.TabIndex = 0;
            // 
            // ButtonToFile
            // 
            this.ButtonToFile.AutoSize = true;
            this.ButtonToFile.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ButtonToFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonToFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonToFile.Location = new System.Drawing.Point(254, 543);
            this.ButtonToFile.Name = "ButtonToFile";
            this.ButtonToFile.Size = new System.Drawing.Size(177, 44);
            this.ButtonToFile.TabIndex = 1;
            this.ButtonToFile.Text = "To File";
            this.ButtonToFile.UseVisualStyleBackColor = false;
            this.ButtonToFile.Click += new System.EventHandler(this.ButtonToFile_Click);
            // 
            // ButtonPrint
            // 
            this.ButtonPrint.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ButtonPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonPrint.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonPrint.Location = new System.Drawing.Point(791, 542);
            this.ButtonPrint.Name = "ButtonPrint";
            this.ButtonPrint.Size = new System.Drawing.Size(177, 43);
            this.ButtonPrint.TabIndex = 2;
            this.ButtonPrint.Text = "Print";
            this.ButtonPrint.UseVisualStyleBackColor = false;
            this.ButtonPrint.Click += new System.EventHandler(this.ButtonPrint_Click);
            // 
            // buttonToExcel
            // 
            this.buttonToExcel.AutoSize = true;
            this.buttonToExcel.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.buttonToExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonToExcel.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonToExcel.Location = new System.Drawing.Point(529, 542);
            this.buttonToExcel.Name = "buttonToExcel";
            this.buttonToExcel.Size = new System.Drawing.Size(177, 44);
            this.buttonToExcel.TabIndex = 3;
            this.buttonToExcel.Text = "To Excel";
            this.buttonToExcel.UseVisualStyleBackColor = false;
            this.buttonToExcel.Click += new System.EventHandler(this.buttonToExcel_Click);
            // 
            // PrintCourseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1206, 640);
            this.Controls.Add(this.buttonToExcel);
            this.Controls.Add(this.ButtonPrint);
            this.Controls.Add(this.ButtonToFile);
            this.Controls.Add(this.DataGridView1);
            this.Name = "PrintCourseForm";
            this.Text = "PrintCourseForm";
            this.Load += new System.EventHandler(this.PrintCourseForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView DataGridView1;
        private System.Windows.Forms.Button ButtonToFile;
        private System.Windows.Forms.Button ButtonPrint;
        private System.Windows.Forms.Button buttonToExcel;
    }
}
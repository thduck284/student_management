namespace QLSV
{
    partial class ChartForm
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
            this.panel3 = new System.Windows.Forms.Panel();
            this.buttonStaticResult = new System.Windows.Forms.Button();
            this.buttonSwapChart = new System.Windows.Forms.Button();
            this.comboBoxSemester = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.PanelChart = new System.Windows.Forms.Panel();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.panel3.Controls.Add(this.buttonStaticResult);
            this.panel3.Controls.Add(this.buttonSwapChart);
            this.panel3.Controls.Add(this.comboBoxSemester);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(763, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(681, 678);
            this.panel3.TabIndex = 3;
            // 
            // buttonStaticResult
            // 
            this.buttonStaticResult.BackColor = System.Drawing.Color.Teal;
            this.buttonStaticResult.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonStaticResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonStaticResult.Location = new System.Drawing.Point(434, 305);
            this.buttonStaticResult.Name = "buttonStaticResult";
            this.buttonStaticResult.Size = new System.Drawing.Size(202, 46);
            this.buttonStaticResult.TabIndex = 10;
            this.buttonStaticResult.Text = "Static Result";
            this.buttonStaticResult.UseVisualStyleBackColor = false;
            this.buttonStaticResult.Click += new System.EventHandler(this.buttonStaticResult_Click);
            // 
            // buttonSwapChart
            // 
            this.buttonSwapChart.BackColor = System.Drawing.Color.Teal;
            this.buttonSwapChart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSwapChart.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSwapChart.Location = new System.Drawing.Point(98, 305);
            this.buttonSwapChart.Name = "buttonSwapChart";
            this.buttonSwapChart.Size = new System.Drawing.Size(202, 46);
            this.buttonSwapChart.TabIndex = 9;
            this.buttonSwapChart.Text = "Swap Chart";
            this.buttonSwapChart.UseVisualStyleBackColor = false;
            this.buttonSwapChart.Click += new System.EventHandler(this.buttonSwapChart_Click);
            // 
            // comboBoxSemester
            // 
            this.comboBoxSemester.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSemester.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold);
            this.comboBoxSemester.FormattingEnabled = true;
            this.comboBoxSemester.Items.AddRange(new object[] {
            "1",
            "2",
            "3"});
            this.comboBoxSemester.Location = new System.Drawing.Point(366, 140);
            this.comboBoxSemester.Name = "comboBoxSemester";
            this.comboBoxSemester.Size = new System.Drawing.Size(125, 37);
            this.comboBoxSemester.TabIndex = 1;
            this.comboBoxSemester.SelectedIndexChanged += new System.EventHandler(this.comboBoxSemester_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(213, 143);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(132, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "Semester:";
            // 
            // PanelChart
            // 
            this.PanelChart.BackColor = System.Drawing.Color.White;
            this.PanelChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelChart.Location = new System.Drawing.Point(0, 0);
            this.PanelChart.Name = "PanelChart";
            this.PanelChart.Size = new System.Drawing.Size(763, 678);
            this.PanelChart.TabIndex = 4;
            // 
            // ChartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1444, 678);
            this.Controls.Add(this.PanelChart);
            this.Controls.Add(this.panel3);
            this.Name = "ChartForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BarCharForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.ChartForm_Load);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ComboBox comboBoxSemester;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonStaticResult;
        private System.Windows.Forms.Button buttonSwapChart;
        private System.Windows.Forms.Panel PanelChart;
    }
}
namespace QLSV
{
    partial class Login_Form
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.TextBoxUsername = new System.Windows.Forms.TextBox();
            this.TextBoxPassword = new System.Windows.Forms.TextBox();
            this.btt_Login = new System.Windows.Forms.Button();
            this.btt_Cancel = new System.Windows.Forms.Button();
            this.btt_Register = new System.Windows.Forms.Button();
            this.RadioButtonHumanResource = new System.Windows.Forms.RadioButton();
            this.RadioButtonStudent = new System.Windows.Forms.RadioButton();
            this.ButtonForgetPassword = new System.Windows.Forms.Button();
            this.RadioButtonAdmin = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pictureBox1.Location = new System.Drawing.Point(32, 41);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 98);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label2.Location = new System.Drawing.Point(27, 187);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(169, 32);
            this.label2.TabIndex = 2;
            this.label2.Text = "User name:";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label3.Location = new System.Drawing.Point(40, 262);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(155, 32);
            this.label3.TabIndex = 2;
            this.label3.Text = "Password:";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.DarkSalmon;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label4.Location = new System.Drawing.Point(237, 100);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(124, 38);
            this.label4.TabIndex = 2;
            this.label4.Text = "LOGIN";
            // 
            // TextBoxUsername
            // 
            this.TextBoxUsername.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.TextBoxUsername.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBoxUsername.Location = new System.Drawing.Point(217, 187);
            this.TextBoxUsername.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TextBoxUsername.Multiline = true;
            this.TextBoxUsername.Name = "TextBoxUsername";
            this.TextBoxUsername.Size = new System.Drawing.Size(249, 34);
            this.TextBoxUsername.TabIndex = 3;
            this.TextBoxUsername.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBoxUsername_KeyDown);
            // 
            // TextBoxPassword
            // 
            this.TextBoxPassword.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.TextBoxPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBoxPassword.Location = new System.Drawing.Point(217, 262);
            this.TextBoxPassword.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TextBoxPassword.Multiline = true;
            this.TextBoxPassword.Name = "TextBoxPassword";
            this.TextBoxPassword.PasswordChar = '*';
            this.TextBoxPassword.Size = new System.Drawing.Size(249, 34);
            this.TextBoxPassword.TabIndex = 3;
            this.TextBoxPassword.TabStop = false;
            this.TextBoxPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBoxPassword_KeyDown);
            // 
            // btt_Login
            // 
            this.btt_Login.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btt_Login.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btt_Login.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btt_Login.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btt_Login.ForeColor = System.Drawing.Color.SaddleBrown;
            this.btt_Login.Location = new System.Drawing.Point(108, 411);
            this.btt_Login.Margin = new System.Windows.Forms.Padding(4);
            this.btt_Login.Name = "btt_Login";
            this.btt_Login.Size = new System.Drawing.Size(130, 41);
            this.btt_Login.TabIndex = 4;
            this.btt_Login.Text = "Login";
            this.btt_Login.UseVisualStyleBackColor = false;
            this.btt_Login.Click += new System.EventHandler(this.btt_Login_Click);
            // 
            // btt_Cancel
            // 
            this.btt_Cancel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btt_Cancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btt_Cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btt_Cancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btt_Cancel.ForeColor = System.Drawing.Color.SaddleBrown;
            this.btt_Cancel.Location = new System.Drawing.Point(312, 411);
            this.btt_Cancel.Margin = new System.Windows.Forms.Padding(4);
            this.btt_Cancel.Name = "btt_Cancel";
            this.btt_Cancel.Size = new System.Drawing.Size(131, 41);
            this.btt_Cancel.TabIndex = 5;
            this.btt_Cancel.Text = "Cancel";
            this.btt_Cancel.UseVisualStyleBackColor = false;
            this.btt_Cancel.Click += new System.EventHandler(this.btt_Cancel_Click);
            // 
            // btt_Register
            // 
            this.btt_Register.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btt_Register.BackColor = System.Drawing.Color.DarkSalmon;
            this.btt_Register.FlatAppearance.BorderSize = 0;
            this.btt_Register.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btt_Register.Font = new System.Drawing.Font("Monotype Corsiva", 13.8F, ((System.Drawing.FontStyle)(((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic) 
                | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btt_Register.ForeColor = System.Drawing.Color.Black;
            this.btt_Register.Location = new System.Drawing.Point(13, 456);
            this.btt_Register.Margin = new System.Windows.Forms.Padding(4);
            this.btt_Register.Name = "btt_Register";
            this.btt_Register.Size = new System.Drawing.Size(131, 41);
            this.btt_Register.TabIndex = 6;
            this.btt_Register.Text = "New User";
            this.btt_Register.UseVisualStyleBackColor = false;
            this.btt_Register.Click += new System.EventHandler(this.btt_Register_Click);
            // 
            // RadioButtonHumanResource
            // 
            this.RadioButtonHumanResource.AutoSize = true;
            this.RadioButtonHumanResource.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RadioButtonHumanResource.Location = new System.Drawing.Point(153, 338);
            this.RadioButtonHumanResource.Name = "RadioButtonHumanResource";
            this.RadioButtonHumanResource.Size = new System.Drawing.Size(198, 29);
            this.RadioButtonHumanResource.TabIndex = 7;
            this.RadioButtonHumanResource.TabStop = true;
            this.RadioButtonHumanResource.Text = "Human Resource";
            this.RadioButtonHumanResource.UseVisualStyleBackColor = true;
            // 
            // RadioButtonStudent
            // 
            this.RadioButtonStudent.AutoSize = true;
            this.RadioButtonStudent.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RadioButtonStudent.Location = new System.Drawing.Point(370, 338);
            this.RadioButtonStudent.Name = "RadioButtonStudent";
            this.RadioButtonStudent.Size = new System.Drawing.Size(108, 29);
            this.RadioButtonStudent.TabIndex = 8;
            this.RadioButtonStudent.TabStop = true;
            this.RadioButtonStudent.Text = "Student";
            this.RadioButtonStudent.UseVisualStyleBackColor = true;
            // 
            // ButtonForgetPassword
            // 
            this.ButtonForgetPassword.BackColor = System.Drawing.Color.DarkSalmon;
            this.ButtonForgetPassword.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ButtonForgetPassword.FlatAppearance.BorderColor = System.Drawing.Color.DarkSalmon;
            this.ButtonForgetPassword.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonForgetPassword.Font = new System.Drawing.Font("Monotype Corsiva", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonForgetPassword.Location = new System.Drawing.Point(270, 298);
            this.ButtonForgetPassword.Margin = new System.Windows.Forms.Padding(0);
            this.ButtonForgetPassword.Name = "ButtonForgetPassword";
            this.ButtonForgetPassword.Size = new System.Drawing.Size(243, 37);
            this.ButtonForgetPassword.TabIndex = 9;
            this.ButtonForgetPassword.Text = "Forget Password";
            this.ButtonForgetPassword.UseVisualStyleBackColor = false;
            this.ButtonForgetPassword.Click += new System.EventHandler(this.ButtonForgetPassword_Click);
            // 
            // RadioButtonAdmin
            // 
            this.RadioButtonAdmin.AutoSize = true;
            this.RadioButtonAdmin.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RadioButtonAdmin.Location = new System.Drawing.Point(36, 338);
            this.RadioButtonAdmin.Name = "RadioButtonAdmin";
            this.RadioButtonAdmin.Size = new System.Drawing.Size(94, 29);
            this.RadioButtonAdmin.TabIndex = 10;
            this.RadioButtonAdmin.TabStop = true;
            this.RadioButtonAdmin.Text = "Admin";
            this.RadioButtonAdmin.UseVisualStyleBackColor = true;
            // 
            // Login_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkSalmon;
            this.ClientSize = new System.Drawing.Size(533, 510);
            this.Controls.Add(this.RadioButtonAdmin);
            this.Controls.Add(this.ButtonForgetPassword);
            this.Controls.Add(this.RadioButtonStudent);
            this.Controls.Add(this.RadioButtonHumanResource);
            this.Controls.Add(this.btt_Register);
            this.Controls.Add(this.btt_Cancel);
            this.Controls.Add(this.btt_Login);
            this.Controls.Add(this.TextBoxPassword);
            this.Controls.Add(this.TextBoxUsername);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Login_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LoginForm";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TextBoxUsername;
        private System.Windows.Forms.TextBox TextBoxPassword;
        private System.Windows.Forms.Button btt_Login;
        private System.Windows.Forms.Button btt_Cancel;
        private System.Windows.Forms.Button btt_Register;
        private System.Windows.Forms.RadioButton RadioButtonHumanResource;
        private System.Windows.Forms.RadioButton RadioButtonStudent;
        private System.Windows.Forms.Button ButtonForgetPassword;
        private System.Windows.Forms.RadioButton RadioButtonAdmin;
    }
}


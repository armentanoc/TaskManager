namespace TaskManager.UI
{
    partial class StartScreen
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnExit = new Button();
            btnRegister = new Button();
            btnEnter = new Button();
            labelUser = new Label();
            labelPassword = new Label();
            txtUser = new TextBox();
            txtPassword = new TextBox();
            SuspendLayout();
            // 
            // btnExit
            // 
            btnExit.Location = new Point(280, 286);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(112, 34);
            btnExit.TabIndex = 0;
            btnExit.TabStop = false;
            btnExit.Text = "Sair";
            btnExit.UseVisualStyleBackColor = true;
            btnExit.Click += btnExit_Click;
            // 
            // btnRegister
            // 
            btnRegister.Location = new Point(416, 286);
            btnRegister.Name = "btnRegister";
            btnRegister.Size = new Size(112, 34);
            btnRegister.TabIndex = 2;
            btnRegister.Text = "Registrar";
            btnRegister.UseVisualStyleBackColor = true;
            btnRegister.Click += btnRegister_Click;
            // 
            // btnEnter
            // 
            btnEnter.Location = new Point(335, 222);
            btnEnter.Name = "btnEnter";
            btnEnter.Size = new Size(112, 34);
            btnEnter.TabIndex = 3;
            btnEnter.Text = "Entrar";
            btnEnter.UseVisualStyleBackColor = true;
            // 
            // labelUser
            // 
            labelUser.AutoSize = true;
            labelUser.Location = new Point(137, 95);
            labelUser.Name = "Usuário";
            labelUser.Size = new Size(72, 25);
            labelUser.TabIndex = 4;
            labelUser.Text = "Usuário";
            // 
            // label2
            // 
            labelPassword.AutoSize = true;
            labelPassword.Location = new Point(149, 148);
            labelPassword.Name = "Senha";
            labelPassword.Size = new Size(60, 25);
            labelPassword.TabIndex = 5;
            labelPassword.Text = "Senha";
            // 
            // txtUser
            // 
            txtUser.Location = new Point(230, 92);
            txtUser.Name = "txtUser";
            txtUser.Size = new Size(355, 31);
            txtUser.TabIndex = 6;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(230, 148);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(355, 31);
            txtPassword.TabIndex = 7;
            // 
            // StartScreen
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(txtPassword);
            Controls.Add(txtUser);
            Controls.Add(labelPassword);
            Controls.Add(labelUser);
            Controls.Add(btnEnter);
            Controls.Add(btnRegister);
            Controls.Add(btnExit);
            Name = "StartScreen";
            Text = "StartScreen";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label labelUser;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnEnter;
    }
}

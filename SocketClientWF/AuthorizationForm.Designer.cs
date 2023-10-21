namespace SocketClientWF
{
    partial class AuthorizationForm
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
            btnLogIn = new Button();
            btnCancel = new Button();
            label1 = new Label();
            tbLogin = new TextBox();
            tbPassword = new TextBox();
            label2 = new Label();
            tbRepeatPass = new TextBox();
            lblRepeat = new Label();
            SuspendLayout();
            // 
            // btnLogIn
            // 
            btnLogIn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnLogIn.Location = new Point(159, 148);
            btnLogIn.Name = "btnLogIn";
            btnLogIn.Size = new Size(75, 23);
            btnLogIn.TabIndex = 0;
            btnLogIn.Text = "Войти";
            btnLogIn.UseVisualStyleBackColor = true;
            btnLogIn.Click += btnLogIn_Click;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.Location = new Point(240, 148);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "Отмена";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(72, 29);
            label1.Name = "label1";
            label1.Size = new Size(40, 15);
            label1.TabIndex = 2;
            label1.Text = "Login:";
            // 
            // tbLogin
            // 
            tbLogin.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            tbLogin.Location = new Point(118, 26);
            tbLogin.Name = "tbLogin";
            tbLogin.Size = new Size(197, 23);
            tbLogin.TabIndex = 3;
            // 
            // tbPassword
            // 
            tbPassword.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            tbPassword.Location = new Point(118, 67);
            tbPassword.Name = "tbPassword";
            tbPassword.PasswordChar = '*';
            tbPassword.Size = new Size(197, 23);
            tbPassword.TabIndex = 5;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Location = new Point(52, 70);
            label2.Name = "label2";
            label2.Size = new Size(60, 15);
            label2.TabIndex = 4;
            label2.Text = "Password:";
            // 
            // tbRepeatPass
            // 
            tbRepeatPass.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            tbRepeatPass.BackColor = SystemColors.Window;
            tbRepeatPass.Location = new Point(118, 108);
            tbRepeatPass.Name = "tbRepeatPass";
            tbRepeatPass.PasswordChar = '*';
            tbRepeatPass.Size = new Size(197, 23);
            tbRepeatPass.TabIndex = 7;
            // 
            // lblRepeat
            // 
            lblRepeat.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblRepeat.AutoSize = true;
            lblRepeat.Location = new Point(13, 111);
            lblRepeat.Name = "lblRepeat";
            lblRepeat.Size = new Size(99, 15);
            lblRepeat.TabIndex = 6;
            lblRepeat.Text = "Repeat password:";
            // 
            // AuthorizationForm
            // 
            AcceptButton = btnLogIn;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(327, 183);
            Controls.Add(tbRepeatPass);
            Controls.Add(lblRepeat);
            Controls.Add(tbPassword);
            Controls.Add(label2);
            Controls.Add(tbLogin);
            Controls.Add(label1);
            Controls.Add(btnCancel);
            Controls.Add(btnLogIn);
            MaximumSize = new Size(343, 222);
            MinimumSize = new Size(343, 182);
            Name = "AuthorizationForm";
            Text = "Authorization";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnLogIn;
        private Button btnCancel;
        private Label label1;
        private TextBox tbLogin;
        private TextBox tbPassword;
        private Label label2;
        private TextBox tbRepeatPass;
        private Label lblRepeat;
    }
}
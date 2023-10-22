namespace SocketClientWF
{
    partial class Form1
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
            btnSend = new Button();
            rtbMain = new RichTextBox();
            rtbMessage = new RichTextBox();
            btnConnect = new Button();
            cbClients = new ComboBox();
            label3 = new Label();
            mainMenuStrip = new MenuStrip();
            AuthorizationToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripMenuItem();
            tsmiRegistration = new ToolStripMenuItem();
            optionsToolStripMenuItem = new ToolStripMenuItem();
            getAllMessagesToolStripMenuItem = new ToolStripMenuItem();
            mainMenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // btnSend
            // 
            btnSend.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSend.Location = new Point(337, 345);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(297, 35);
            btnSend.TabIndex = 0;
            btnSend.Text = "Send";
            btnSend.UseVisualStyleBackColor = true;
            btnSend.Click += btnSend_Click;
            // 
            // rtbMain
            // 
            rtbMain.Location = new Point(12, 70);
            rtbMain.Name = "rtbMain";
            rtbMain.ReadOnly = true;
            rtbMain.Size = new Size(319, 310);
            rtbMain.TabIndex = 1;
            rtbMain.Text = "";
            // 
            // rtbMessage
            // 
            rtbMessage.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            rtbMessage.Location = new Point(337, 70);
            rtbMessage.Name = "rtbMessage";
            rtbMessage.Size = new Size(297, 269);
            rtbMessage.TabIndex = 6;
            rtbMessage.Text = "";
            // 
            // btnConnect
            // 
            btnConnect.Location = new Point(511, 32);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(123, 32);
            btnConnect.TabIndex = 7;
            btnConnect.Text = "connect to server";
            btnConnect.UseVisualStyleBackColor = true;
            btnConnect.Click += btnConnect_Click;
            // 
            // cbClients
            // 
            cbClients.FormattingEnabled = true;
            cbClients.Location = new Point(60, 36);
            cbClients.Name = "cbClients";
            cbClients.Size = new Size(445, 23);
            cbClients.TabIndex = 8;
            cbClients.SelectedIndexChanged += cbClients_SelectedIndexChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 41);
            label3.Name = "label3";
            label3.Size = new Size(41, 15);
            label3.TabIndex = 9;
            label3.Text = "clients";
            // 
            // mainMenuStrip
            // 
            mainMenuStrip.Items.AddRange(new ToolStripItem[] { AuthorizationToolStripMenuItem, optionsToolStripMenuItem, getAllMessagesToolStripMenuItem });
            mainMenuStrip.Location = new Point(0, 0);
            mainMenuStrip.Name = "mainMenuStrip";
            mainMenuStrip.Size = new Size(646, 24);
            mainMenuStrip.TabIndex = 10;
            mainMenuStrip.Text = "menuStrip1";
            // 
            // AuthorizationToolStripMenuItem
            // 
            AuthorizationToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItem1, tsmiRegistration });
            AuthorizationToolStripMenuItem.Name = "AuthorizationToolStripMenuItem";
            AuthorizationToolStripMenuItem.Size = new Size(91, 20);
            AuthorizationToolStripMenuItem.Text = "Authorization";
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(180, 22);
            toolStripMenuItem1.Text = "Log In";
            toolStripMenuItem1.Click += LogInToolStripMenuItem_Click;
            // 
            // tsmiRegistration
            // 
            tsmiRegistration.Name = "tsmiRegistration";
            tsmiRegistration.Size = new Size(180, 22);
            tsmiRegistration.Text = "Registration";
            tsmiRegistration.Click += tsmiRegistration_Click;
            // 
            // optionsToolStripMenuItem
            // 
            optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            optionsToolStripMenuItem.Size = new Size(61, 20);
            optionsToolStripMenuItem.Text = "Options";
            optionsToolStripMenuItem.Click += optionsToolStripMenuItem_Click;
            // 
            // getAllMessagesToolStripMenuItem
            // 
            getAllMessagesToolStripMenuItem.Name = "getAllMessagesToolStripMenuItem";
            getAllMessagesToolStripMenuItem.Size = new Size(106, 20);
            getAllMessagesToolStripMenuItem.Text = "Get all messages";
            getAllMessagesToolStripMenuItem.Click += getAllMessagesToolStripMenuItem_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(646, 392);
            Controls.Add(label3);
            Controls.Add(cbClients);
            Controls.Add(btnConnect);
            Controls.Add(rtbMessage);
            Controls.Add(rtbMain);
            Controls.Add(btnSend);
            Controls.Add(mainMenuStrip);
            MainMenuStrip = mainMenuStrip;
            Name = "Form1";
            Text = "Form1";
            mainMenuStrip.ResumeLayout(false);
            mainMenuStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnSend;
        private RichTextBox rtbMain;
        private RichTextBox rtbMessage;
        private Button btnConnect;
        private ComboBox cbClients;
        private Label label3;
        private MenuStrip mainMenuStrip;
        private ToolStripMenuItem AuthorizationToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem tsmiRegistration;
        private ToolStripMenuItem optionsToolStripMenuItem;
        private ToolStripMenuItem getAllMessagesToolStripMenuItem;
    }
}
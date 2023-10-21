namespace SocketClientWF
{
    partial class OptionsForm
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
            btnOk = new Button();
            btnCancel = new Button();
            lblName = new Label();
            tbName = new TextBox();
            label1 = new Label();
            lblGuid = new Label();
            label2 = new Label();
            tbHost = new TextBox();
            tbPort = new TextBox();
            label3 = new Label();
            SuspendLayout();
            // 
            // btnOk
            // 
            btnOk.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnOk.Location = new Point(202, 139);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(75, 23);
            btnOk.TabIndex = 0;
            btnOk.Text = "Save";
            btnOk.UseVisualStyleBackColor = true;
            btnOk.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCancel.Location = new Point(283, 139);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Location = new Point(12, 63);
            lblName.Name = "lblName";
            lblName.Size = new Size(42, 15);
            lblName.TabIndex = 2;
            lblName.Text = "Name:";
            // 
            // tbName
            // 
            tbName.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            tbName.Location = new Point(60, 60);
            tbName.Name = "tbName";
            tbName.Size = new Size(298, 23);
            tbName.TabIndex = 3;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(19, 27);
            label1.Name = "label1";
            label1.Size = new Size(35, 15);
            label1.TabIndex = 4;
            label1.Text = "Guid:";
            // 
            // lblGuid
            // 
            lblGuid.AutoSize = true;
            lblGuid.ForeColor = Color.MidnightBlue;
            lblGuid.Location = new Point(60, 27);
            lblGuid.Name = "lblGuid";
            lblGuid.Size = new Size(0, 15);
            lblGuid.TabIndex = 5;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(16, 96);
            label2.Name = "label2";
            label2.Size = new Size(35, 15);
            label2.TabIndex = 6;
            label2.Text = "Host:";
            // 
            // tbHost
            // 
            tbHost.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            tbHost.Location = new Point(60, 93);
            tbHost.Name = "tbHost";
            tbHost.Size = new Size(150, 23);
            tbHost.TabIndex = 7;
            // 
            // tbPort
            // 
            tbPort.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            tbPort.Location = new Point(283, 93);
            tbPort.Name = "tbPort";
            tbPort.Size = new Size(75, 23);
            tbPort.TabIndex = 9;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label3.AutoSize = true;
            label3.Location = new Point(242, 96);
            label3.Name = "label3";
            label3.Size = new Size(32, 15);
            label3.TabIndex = 8;
            label3.Text = "Port:";
            // 
            // OptionsForm
            // 
            AcceptButton = btnOk;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(370, 175);
            Controls.Add(tbPort);
            Controls.Add(label3);
            Controls.Add(tbHost);
            Controls.Add(label2);
            Controls.Add(lblGuid);
            Controls.Add(label1);
            Controls.Add(tbName);
            Controls.Add(lblName);
            Controls.Add(btnCancel);
            Controls.Add(btnOk);
            MaximumSize = new Size(386, 214);
            Name = "OptionsForm";
            Text = "OptionsForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnOk;
        private Button btnCancel;
        private Label lblName;
        private TextBox tbName;
        private Label label1;
        private Label lblGuid;
        private Label label2;
        private TextBox tbHost;
        private TextBox tbPort;
        private Label label3;
    }
}
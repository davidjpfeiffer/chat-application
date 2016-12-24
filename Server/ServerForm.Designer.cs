namespace ServerForm
{
    partial class ServerForm
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
            this.messageBox = new System.Windows.Forms.TextBox();
            this.messageList = new System.Windows.Forms.ListBox();
            this.sendButton = new System.Windows.Forms.Button();
            this.toggleServerButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // messageBox
            // 
            this.messageBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.messageBox.Location = new System.Drawing.Point(13, 13);
            this.messageBox.Name = "messageBox";
            this.messageBox.Size = new System.Drawing.Size(645, 31);
            this.messageBox.TabIndex = 0;
            this.messageBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MessageBoxKeyDown);
            // 
            // messageList
            // 
            this.messageList.FormattingEnabled = true;
            this.messageList.Location = new System.Drawing.Point(12, 53);
            this.messageList.Name = "messageList";
            this.messageList.Size = new System.Drawing.Size(830, 329);
            this.messageList.TabIndex = 3;
            // 
            // sendButton
            // 
            this.sendButton.Location = new System.Drawing.Point(664, 12);
            this.sendButton.Name = "sendButton";
            this.sendButton.Size = new System.Drawing.Size(86, 31);
            this.sendButton.TabIndex = 1;
            this.sendButton.Text = "Send";
            this.sendButton.UseVisualStyleBackColor = true;
            this.sendButton.Click += new System.EventHandler(this.SendButtonOnClick);
            // 
            // toggleServerButton
            // 
            this.toggleServerButton.Location = new System.Drawing.Point(756, 12);
            this.toggleServerButton.Name = "toggleServerButton";
            this.toggleServerButton.Size = new System.Drawing.Size(86, 31);
            this.toggleServerButton.TabIndex = 2;
            this.toggleServerButton.Text = "Start Server";
            this.toggleServerButton.UseVisualStyleBackColor = true;
            this.toggleServerButton.Click += new System.EventHandler(this.ToggleServerButtonOnClick);
            // 
            // ServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(854, 394);
            this.Controls.Add(this.toggleServerButton);
            this.Controls.Add(this.sendButton);
            this.Controls.Add(this.messageList);
            this.Controls.Add(this.messageBox);
            this.Name = "ServerForm";
            this.Text = "Server";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox messageBox;
        private System.Windows.Forms.ListBox messageList;
        private System.Windows.Forms.Button sendButton;
        private System.Windows.Forms.Button toggleServerButton;
    }
}


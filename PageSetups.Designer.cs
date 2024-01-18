namespace EJ_.Forms
{
    partial class PageSetups
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
            this.lbxPageSetups = new System.Windows.Forms.ListBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.lblPrompt = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbxPageSetups
            // 
            this.lbxPageSetups.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbxPageSetups.FormattingEnabled = true;
            this.lbxPageSetups.Location = new System.Drawing.Point(26, 29);
            this.lbxPageSetups.Name = "lbxPageSetups";
            this.lbxPageSetups.Size = new System.Drawing.Size(186, 225);
            this.lbxPageSetups.TabIndex = 0;
            this.lbxPageSetups.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbxPageSetups_MouseDoubleClick);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(35, 269);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(79, 30);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "&OK";
            this.btnOK.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // Cancel
            // 
            this.Cancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(124, 269);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(79, 30);
            this.Cancel.TabIndex = 2;
            this.Cancel.Text = "&Cancel";
            this.Cancel.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // lblPrompt
            // 
            this.lblPrompt.AutoSize = true;
            this.lblPrompt.Location = new System.Drawing.Point(25, 12);
            this.lblPrompt.Name = "lblPrompt";
            this.lblPrompt.Size = new System.Drawing.Size(122, 13);
            this.lblPrompt.TabIndex = 3;
            this.lblPrompt.Text = "Select a MS plot setting.";
            // 
            // PageSetups
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(237, 306);
            this.Controls.Add(this.lblPrompt);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lbxPageSetups);
            this.Name = "PageSetups";
            this.Text = "Page Setups";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbxPageSetups;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Label lblPrompt;
    }
}
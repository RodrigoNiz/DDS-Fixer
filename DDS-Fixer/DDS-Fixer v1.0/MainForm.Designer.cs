namespace DDS_Fixer_v1._0
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.buttonStart = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.groupBoxProgress = new System.Windows.Forms.GroupBox();
            this.labelProgress = new System.Windows.Forms.Label();
            this.richTextBoxFiles = new System.Windows.Forms.RichTextBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.groupBoxButtons = new System.Windows.Forms.GroupBox();
            this.linkLabelAbout = new System.Windows.Forms.LinkLabel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.groupBoxProgress.SuspendLayout();
            this.groupBoxButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(10, 23);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(60, 23);
            this.buttonStart.TabIndex = 0;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(10, 23);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(60, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // groupBoxProgress
            // 
            this.groupBoxProgress.Controls.Add(this.labelProgress);
            this.groupBoxProgress.Controls.Add(this.richTextBoxFiles);
            this.groupBoxProgress.Controls.Add(this.progressBar);
            this.groupBoxProgress.Location = new System.Drawing.Point(12, 71);
            this.groupBoxProgress.Name = "groupBoxProgress";
            this.groupBoxProgress.Size = new System.Drawing.Size(259, 158);
            this.groupBoxProgress.TabIndex = 2;
            this.groupBoxProgress.TabStop = false;
            this.groupBoxProgress.Text = "Progress";
            // 
            // labelProgress
            // 
            this.labelProgress.AutoSize = true;
            this.labelProgress.Location = new System.Drawing.Point(6, 138);
            this.labelProgress.Name = "labelProgress";
            this.labelProgress.Size = new System.Drawing.Size(16, 13);
            this.labelProgress.TabIndex = 2;
            this.labelProgress.Text = "...";
            // 
            // richTextBoxFiles
            // 
            this.richTextBoxFiles.Location = new System.Drawing.Point(6, 52);
            this.richTextBoxFiles.Name = "richTextBoxFiles";
            this.richTextBoxFiles.ReadOnly = true;
            this.richTextBoxFiles.Size = new System.Drawing.Size(246, 80);
            this.richTextBoxFiles.TabIndex = 1;
            this.richTextBoxFiles.Text = "Let\'s get started!";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(5, 19);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(247, 23);
            this.progressBar.TabIndex = 0;
            // 
            // groupBoxButtons
            // 
            this.groupBoxButtons.Controls.Add(this.linkLabelAbout);
            this.groupBoxButtons.Controls.Add(this.buttonStart);
            this.groupBoxButtons.Controls.Add(this.buttonCancel);
            this.groupBoxButtons.Controls.Add(this.labelTitle);
            this.groupBoxButtons.Location = new System.Drawing.Point(13, 5);
            this.groupBoxButtons.Name = "groupBoxButtons";
            this.groupBoxButtons.Size = new System.Drawing.Size(259, 60);
            this.groupBoxButtons.TabIndex = 4;
            this.groupBoxButtons.TabStop = false;
            // 
            // linkLabelAbout
            // 
            this.linkLabelAbout.AutoSize = true;
            this.linkLabelAbout.Location = new System.Drawing.Point(216, 35);
            this.linkLabelAbout.Name = "linkLabelAbout";
            this.linkLabelAbout.Size = new System.Drawing.Size(35, 13);
            this.linkLabelAbout.TabIndex = 3;
            this.linkLabelAbout.TabStop = true;
            this.linkLabelAbout.Text = "About";
            this.linkLabelAbout.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelAbout_LinkClicked);
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("Impact", 22F, System.Drawing.FontStyle.Italic);
            this.labelTitle.Location = new System.Drawing.Point(76, 16);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(134, 37);
            this.labelTitle.TabIndex = 5;
            this.labelTitle.Text = "DDS-Fixer";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 241);
            this.Controls.Add(this.groupBoxButtons);
            this.Controls.Add(this.groupBoxProgress);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DDS-Fixer v1.0";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBoxProgress.ResumeLayout(false);
            this.groupBoxProgress.PerformLayout();
            this.groupBoxButtons.ResumeLayout(false);
            this.groupBoxButtons.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.GroupBox groupBoxProgress;
        private System.Windows.Forms.Label labelProgress;
        private System.Windows.Forms.RichTextBox richTextBoxFiles;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.GroupBox groupBoxButtons;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.LinkLabel linkLabelAbout;
        private System.Windows.Forms.ToolTip toolTip;
    }
}


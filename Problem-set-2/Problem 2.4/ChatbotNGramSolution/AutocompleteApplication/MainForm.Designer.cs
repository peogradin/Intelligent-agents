
namespace AutocompleteApplication
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadTrainingDataSetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadValidationDataSetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadTestDataSetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tokenizeButton = new System.Windows.Forms.ToolStripButton();
            this.generateNGramsButton = new System.Windows.Forms.ToolStripButton();
            this.computePerplexityToolStripDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.perplexityTrainingDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.perplexityValidationDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.perplexityTestDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectModeToolStripDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.defaultModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lowTempModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateSentenceButton = new System.Windows.Forms.ToolStripButton();
            this.chatbotListBox = new System.Windows.Forms.ListBox();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1200, 33);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadTrainingDataSetToolStripMenuItem,
            this.loadValidationDataSetToolStripMenuItem,
            this.loadTestDataSetToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(54, 29);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // loadTrainingDataSetToolStripMenuItem
            // 
            this.loadTrainingDataSetToolStripMenuItem.Name = "loadTrainingDataSetToolStripMenuItem";
            this.loadTrainingDataSetToolStripMenuItem.Size = new System.Drawing.Size(303, 34);
            this.loadTrainingDataSetToolStripMenuItem.Text = "Load training data set";
            this.loadTrainingDataSetToolStripMenuItem.Click += new System.EventHandler(this.loadTrainingDataSetToolStripMenuItem_Click);
            // 
            // loadValidationDataSetToolStripMenuItem
            // 
            this.loadValidationDataSetToolStripMenuItem.Enabled = false;
            this.loadValidationDataSetToolStripMenuItem.Name = "loadValidationDataSetToolStripMenuItem";
            this.loadValidationDataSetToolStripMenuItem.Size = new System.Drawing.Size(303, 34);
            this.loadValidationDataSetToolStripMenuItem.Text = "Load validation data set";
            this.loadValidationDataSetToolStripMenuItem.Click += new System.EventHandler(this.loadValidationDataSetToolStripMenuItem_Click);
            // 
            // loadTestDataSetToolStripMenuItem
            // 
            this.loadTestDataSetToolStripMenuItem.Enabled = false;
            this.loadTestDataSetToolStripMenuItem.Name = "loadTestDataSetToolStripMenuItem";
            this.loadTestDataSetToolStripMenuItem.Size = new System.Drawing.Size(303, 34);
            this.loadTestDataSetToolStripMenuItem.Text = "Load test data set";
            this.loadTestDataSetToolStripMenuItem.Click += new System.EventHandler(this.loadTestDataSetToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(303, 34);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tokenizeButton,
            this.generateNGramsButton,
            this.computePerplexityToolStripDropDownButton,
            this.selectModeToolStripDropDownButton,
            this.generateSentenceButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 33);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.toolStrip1.Size = new System.Drawing.Size(1200, 34);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tokenizeButton
            // 
            this.tokenizeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tokenizeButton.Enabled = false;
            this.tokenizeButton.Image = ((System.Drawing.Image)(resources.GetObject("tokenizeButton.Image")));
            this.tokenizeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tokenizeButton.Name = "tokenizeButton";
            this.tokenizeButton.Size = new System.Drawing.Size(83, 29);
            this.tokenizeButton.Text = "Tokenize";
            this.tokenizeButton.Click += new System.EventHandler(this.tokenizeButton_Click);
            // 
            // generateNGramsButton
            // 
            this.generateNGramsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.generateNGramsButton.Enabled = false;
            this.generateNGramsButton.Image = ((System.Drawing.Image)(resources.GetObject("generateNGramsButton.Image")));
            this.generateNGramsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.generateNGramsButton.Name = "generateNGramsButton";
            this.generateNGramsButton.Size = new System.Drawing.Size(155, 29);
            this.generateNGramsButton.Text = "Generate NGrams";
            this.generateNGramsButton.Click += new System.EventHandler(this.generateNGramsButton_Click);
            // 
            // computePerplexityToolStripDropDownButton
            // 
            this.computePerplexityToolStripDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.computePerplexityToolStripDropDownButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.perplexityTrainingDataToolStripMenuItem,
            this.perplexityValidationDataToolStripMenuItem,
            this.perplexityTestDataToolStripMenuItem});
            this.computePerplexityToolStripDropDownButton.Enabled = false;
            this.computePerplexityToolStripDropDownButton.Image = ((System.Drawing.Image)(resources.GetObject("computePerplexityToolStripDropDownButton.Image")));
            this.computePerplexityToolStripDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.computePerplexityToolStripDropDownButton.Name = "computePerplexityToolStripDropDownButton";
            this.computePerplexityToolStripDropDownButton.Size = new System.Drawing.Size(184, 29);
            this.computePerplexityToolStripDropDownButton.Text = "Compute Perplexity";
            // 
            // perplexityTrainingDataToolStripMenuItem
            // 
            this.perplexityTrainingDataToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.perplexityTrainingDataToolStripMenuItem.Name = "perplexityTrainingDataToolStripMenuItem";
            this.perplexityTrainingDataToolStripMenuItem.Size = new System.Drawing.Size(270, 34);
            this.perplexityTrainingDataToolStripMenuItem.Text = "Training data set";
            this.perplexityTrainingDataToolStripMenuItem.Click += new System.EventHandler(this.perplexityTrainingDataToolStripMenuItem_Click);
            // 
            // perplexityValidationDataToolStripMenuItem
            // 
            this.perplexityValidationDataToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.perplexityValidationDataToolStripMenuItem.Name = "perplexityValidationDataToolStripMenuItem";
            this.perplexityValidationDataToolStripMenuItem.Size = new System.Drawing.Size(270, 34);
            this.perplexityValidationDataToolStripMenuItem.Text = "Validation data set";
            this.perplexityValidationDataToolStripMenuItem.Click += new System.EventHandler(this.perplexityValidationDataToolStripMenuItem_Click);
            // 
            // perplexityTestDataToolStripMenuItem
            // 
            this.perplexityTestDataToolStripMenuItem.Name = "perplexityTestDataToolStripMenuItem";
            this.perplexityTestDataToolStripMenuItem.Size = new System.Drawing.Size(270, 34);
            this.perplexityTestDataToolStripMenuItem.Text = "Test data set";
            this.perplexityTestDataToolStripMenuItem.Click += new System.EventHandler(this.perplexityTestDataToolStripMenuItem_Click);
            // 
            // selectModeToolStripDropDownButton
            // 
            this.selectModeToolStripDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.selectModeToolStripDropDownButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.defaultModeToolStripMenuItem,
            this.lowTempModeToolStripMenuItem});
            this.selectModeToolStripDropDownButton.Enabled = false;
            this.selectModeToolStripDropDownButton.Image = ((System.Drawing.Image)(resources.GetObject("selectModeToolStripDropDownButton.Image")));
            this.selectModeToolStripDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.selectModeToolStripDropDownButton.Name = "selectModeToolStripDropDownButton";
            this.selectModeToolStripDropDownButton.Size = new System.Drawing.Size(194, 29);
            this.selectModeToolStripDropDownButton.Text = "Select running mode";
            // 
            // defaultModeToolStripMenuItem
            // 
            this.defaultModeToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.defaultModeToolStripMenuItem.Enabled = false;
            this.defaultModeToolStripMenuItem.Name = "defaultModeToolStripMenuItem";
            this.defaultModeToolStripMenuItem.Size = new System.Drawing.Size(302, 34);
            this.defaultModeToolStripMenuItem.Text = "Default mode";
            this.defaultModeToolStripMenuItem.Click += new System.EventHandler(this.defaultModeToolStripMenuItem_Click);
            // 
            // lowTempModeToolStripMenuItem
            // 
            this.lowTempModeToolStripMenuItem.Name = "lowTempModeToolStripMenuItem";
            this.lowTempModeToolStripMenuItem.Size = new System.Drawing.Size(302, 34);
            this.lowTempModeToolStripMenuItem.Text = "Low-temperature mode";
            this.lowTempModeToolStripMenuItem.Click += new System.EventHandler(this.lowTempModeToolStripMenuItem_Click);
            // 
            // generateSentenceButton
            // 
            this.generateSentenceButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.generateSentenceButton.Enabled = false;
            this.generateSentenceButton.Image = ((System.Drawing.Image)(resources.GetObject("generateSentenceButton.Image")));
            this.generateSentenceButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.generateSentenceButton.Name = "generateSentenceButton";
            this.generateSentenceButton.Size = new System.Drawing.Size(162, 29);
            this.generateSentenceButton.Text = "Generate Sentence";
            this.generateSentenceButton.ToolTipText = "Generate Sentence";
            this.generateSentenceButton.Click += new System.EventHandler(this.generateSentenceButton_Click);
            // 
            // chatbotListBox
            // 
            this.chatbotListBox.BackColor = System.Drawing.Color.Black;
            this.chatbotListBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.chatbotListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chatbotListBox.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chatbotListBox.ForeColor = System.Drawing.Color.Lime;
            this.chatbotListBox.FormattingEnabled = true;
            this.chatbotListBox.ItemHeight = 17;
            this.chatbotListBox.Location = new System.Drawing.Point(0, 67);
            this.chatbotListBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chatbotListBox.Name = "chatbotListBox";
            this.chatbotListBox.Size = new System.Drawing.Size(1200, 625);
            this.chatbotListBox.TabIndex = 6;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 692);
            this.Controls.Add(this.chatbotListBox);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "N-grams chatbot";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadTrainingDataSetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tokenizeButton;
        private System.Windows.Forms.ListBox chatbotListBox;
        private System.Windows.Forms.ToolStripButton generateNGramsButton;
        private System.Windows.Forms.ToolStripButton generateSentenceButton;
        private System.Windows.Forms.ToolStripMenuItem loadValidationDataSetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadTestDataSetToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton selectModeToolStripDropDownButton;
        private System.Windows.Forms.ToolStripMenuItem defaultModeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lowTempModeToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton computePerplexityToolStripDropDownButton;
        private System.Windows.Forms.ToolStripMenuItem perplexityTrainingDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem perplexityValidationDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem perplexityTestDataToolStripMenuItem;
    }
}


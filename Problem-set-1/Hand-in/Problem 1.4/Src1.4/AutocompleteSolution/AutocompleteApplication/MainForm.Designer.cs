
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
            this.loadDataSetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tokenizeButton = new System.Windows.Forms.ToolStripButton();
            this.generateNGramsButton = new System.Windows.Forms.ToolStripButton();
            this.startAutocompletionButton = new System.Windows.Forms.ToolStripButton();
            this.sentenceTextBox = new System.Windows.Forms.TextBox();
            this.nGramsListBox = new System.Windows.Forms.ListBox();
            this.suggestionListBox = new System.Windows.Forms.ListBox();
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
            this.loadDataSetToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(54, 29);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // loadDataSetToolStripMenuItem
            // 
            this.loadDataSetToolStripMenuItem.Name = "loadDataSetToolStripMenuItem";
            this.loadDataSetToolStripMenuItem.Size = new System.Drawing.Size(221, 34);
            this.loadDataSetToolStripMenuItem.Text = "Load data set";
            this.loadDataSetToolStripMenuItem.Click += new System.EventHandler(this.loadDataSetToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(221, 34);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tokenizeButton,
            this.generateNGramsButton,
            this.startAutocompletionButton});
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
            // startAutocompletionButton
            // 
            this.startAutocompletionButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.startAutocompletionButton.Enabled = false;
            this.startAutocompletionButton.Image = ((System.Drawing.Image)(resources.GetObject("startAutocompletionButton.Image")));
            this.startAutocompletionButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.startAutocompletionButton.Name = "startAutocompletionButton";
            this.startAutocompletionButton.Size = new System.Drawing.Size(186, 29);
            this.startAutocompletionButton.Text = "Start Autocompletion";
            this.startAutocompletionButton.Click += new System.EventHandler(this.startAutocompletionButton_Click);
            // 
            // sentenceTextBox
            // 
            this.sentenceTextBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.sentenceTextBox.Enabled = false;
            this.sentenceTextBox.Location = new System.Drawing.Point(0, 67);
            this.sentenceTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.sentenceTextBox.Name = "sentenceTextBox";
            this.sentenceTextBox.Size = new System.Drawing.Size(1200, 26);
            this.sentenceTextBox.TabIndex = 2;
            this.sentenceTextBox.TextChanged += new System.EventHandler(this.sentenceTextBox_TextChanged);
            this.sentenceTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.sentenceTextBox_KeyDown);
            // 
            // nGramsListBox
            // 
            this.nGramsListBox.BackColor = System.Drawing.Color.Black;
            this.nGramsListBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.nGramsListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nGramsListBox.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nGramsListBox.ForeColor = System.Drawing.Color.Lime;
            this.nGramsListBox.FormattingEnabled = true;
            this.nGramsListBox.ItemHeight = 17;
            this.nGramsListBox.Location = new System.Drawing.Point(0, 93);
            this.nGramsListBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.nGramsListBox.Name = "nGramsListBox";
            this.nGramsListBox.Size = new System.Drawing.Size(1200, 599);
            this.nGramsListBox.TabIndex = 6;
            // 
            // suggestionListBox
            // 
            this.suggestionListBox.ForeColor = System.Drawing.Color.Black;
            this.suggestionListBox.FormattingEnabled = true;
            this.suggestionListBox.ItemHeight = 20;
            this.suggestionListBox.Location = new System.Drawing.Point(430, 95);
            this.suggestionListBox.Margin = new System.Windows.Forms.Padding(2);
            this.suggestionListBox.Name = "suggestionListBox";
            this.suggestionListBox.Size = new System.Drawing.Size(243, 344);
            this.suggestionListBox.TabIndex = 7;
            this.suggestionListBox.TabStop = false;
            this.suggestionListBox.Visible = false;
            this.suggestionListBox.SelectedIndexChanged += new System.EventHandler(this.suggestionListBox_SelectedIndexChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 692);
            this.Controls.Add(this.suggestionListBox);
            this.Controls.Add(this.nGramsListBox);
            this.Controls.Add(this.sentenceTextBox);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Autocompletion";
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
        private System.Windows.Forms.ToolStripMenuItem loadDataSetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tokenizeButton;
        private System.Windows.Forms.TextBox sentenceTextBox;
        private System.Windows.Forms.ListBox nGramsListBox;
        private System.Windows.Forms.ToolStripButton generateNGramsButton;
        private System.Windows.Forms.ToolStripButton startAutocompletionButton;
        private System.Windows.Forms.ListBox suggestionListBox;
    }
}


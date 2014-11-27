namespace OPSYS_GUI_NThompson
{
    partial class StartForm
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
            this.goButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.scheduleCB = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.helpButton = new System.Windows.Forms.Button();
            this.quitButton = new System.Windows.Forms.Button();
            this.processorBox = new System.Windows.Forms.TextBox();
            this.ramBox = new System.Windows.Forms.TextBox();
            this.ramAst = new System.Windows.Forms.Label();
            this.scheduleAst = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.importFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // goButton
            // 
            this.goButton.Location = new System.Drawing.Point(13, 237);
            this.goButton.Margin = new System.Windows.Forms.Padding(4);
            this.goButton.Name = "goButton";
            this.goButton.Size = new System.Drawing.Size(100, 28);
            this.goButton.TabIndex = 0;
            this.goButton.Text = "Go";
            this.goButton.UseVisualStyleBackColor = true;
            this.goButton.Click += new System.EventHandler(this.goButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(71, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(153, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Number of Processors:";
            // 
            // scheduleCB
            // 
            this.scheduleCB.FormattingEnabled = true;
            this.scheduleCB.Items.AddRange(new object[] {
            "First Come First Serve",
            "Shortest Job First",
            "Priority"});
            this.scheduleCB.Location = new System.Drawing.Point(230, 137);
            this.scheduleCB.Name = "scheduleCB";
            this.scheduleCB.Size = new System.Drawing.Size(178, 24);
            this.scheduleCB.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(79, 140);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(145, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "Scheduling Algorithm:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(135, 112);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 17);
            this.label4.TabIndex = 5;
            this.label4.Text = "Size of RAM:";
            // 
            // helpButton
            // 
            this.helpButton.Location = new System.Drawing.Point(120, 236);
            this.helpButton.Name = "helpButton";
            this.helpButton.Size = new System.Drawing.Size(98, 29);
            this.helpButton.TabIndex = 6;
            this.helpButton.Text = "Help";
            this.helpButton.UseVisualStyleBackColor = true;
            this.helpButton.Click += new System.EventHandler(this.helpButton_Click);
            // 
            // quitButton
            // 
            this.quitButton.Location = new System.Drawing.Point(310, 236);
            this.quitButton.Name = "quitButton";
            this.quitButton.Size = new System.Drawing.Size(98, 29);
            this.quitButton.TabIndex = 7;
            this.quitButton.Text = "Quit";
            this.quitButton.UseVisualStyleBackColor = true;
            this.quitButton.Click += new System.EventHandler(this.quitButton_Click);
            // 
            // processorBox
            // 
            this.processorBox.Location = new System.Drawing.Point(230, 81);
            this.processorBox.Name = "processorBox";
            this.processorBox.Size = new System.Drawing.Size(178, 22);
            this.processorBox.TabIndex = 8;
            // 
            // ramBox
            // 
            this.ramBox.Location = new System.Drawing.Point(230, 109);
            this.ramBox.Name = "ramBox";
            this.ramBox.Size = new System.Drawing.Size(178, 22);
            this.ramBox.TabIndex = 10;
            // 
            // ramAst
            // 
            this.ramAst.AutoSize = true;
            this.ramAst.ForeColor = System.Drawing.Color.Red;
            this.ramAst.Location = new System.Drawing.Point(116, 112);
            this.ramAst.Name = "ramAst";
            this.ramAst.Size = new System.Drawing.Size(13, 17);
            this.ramAst.TabIndex = 11;
            this.ramAst.Text = "*";
            this.ramAst.Visible = false;
            // 
            // scheduleAst
            // 
            this.scheduleAst.AutoSize = true;
            this.scheduleAst.ForeColor = System.Drawing.Color.Red;
            this.scheduleAst.Location = new System.Drawing.Point(60, 140);
            this.scheduleAst.Name = "scheduleAst";
            this.scheduleAst.Size = new System.Drawing.Size(13, 17);
            this.scheduleAst.TabIndex = 12;
            this.scheduleAst.Text = "*";
            this.scheduleAst.Visible = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importFileToolStripMenuItem,
            this.quitToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(420, 28);
            this.menuStrip1.TabIndex = 13;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // importFileToolStripMenuItem
            // 
            this.importFileToolStripMenuItem.Name = "importFileToolStripMenuItem";
            this.importFileToolStripMenuItem.Size = new System.Drawing.Size(157, 24);
            this.importFileToolStripMenuItem.Text = "Import Instructions....";
            this.importFileToolStripMenuItem.Click += new System.EventHandler(this.importFileToolStripMenuItem_Click);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(49, 24);
            this.quitToolStripMenuItem.Text = "Quit";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
            // 
            // StartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 278);
            this.Controls.Add(this.scheduleAst);
            this.Controls.Add(this.ramAst);
            this.Controls.Add(this.ramBox);
            this.Controls.Add(this.processorBox);
            this.Controls.Add(this.quitButton);
            this.Controls.Add(this.helpButton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.scheduleCB);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.goButton);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "StartForm";
            this.Text = "Operating Systems Project, Part 2";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button goButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox scheduleCB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button helpButton;
        private System.Windows.Forms.Button quitButton;
        private System.Windows.Forms.TextBox processorBox;
        private System.Windows.Forms.TextBox ramBox;
        private System.Windows.Forms.Label ramAst;
        private System.Windows.Forms.Label scheduleAst;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem importFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
    }
}


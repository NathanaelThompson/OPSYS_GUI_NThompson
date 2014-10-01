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
            this.label2 = new System.Windows.Forms.Label();
            this.scheduleCB = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.helpButton = new System.Windows.Forms.Button();
            this.quitButton = new System.Windows.Forms.Button();
            this.processorBox = new System.Windows.Forms.TextBox();
            this.coreBox = new System.Windows.Forms.TextBox();
            this.ramBox = new System.Windows.Forms.TextBox();
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
            this.label1.Location = new System.Drawing.Point(71, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(153, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Number of Processors:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(212, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Number of Cores per Processor:";
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
            this.processorBox.Enabled = false;
            this.processorBox.Location = new System.Drawing.Point(230, 53);
            this.processorBox.Name = "processorBox";
            this.processorBox.Size = new System.Drawing.Size(178, 22);
            this.processorBox.TabIndex = 8;
            // 
            // coreBox
            // 
            this.coreBox.Enabled = false;
            this.coreBox.Location = new System.Drawing.Point(230, 81);
            this.coreBox.Name = "coreBox";
            this.coreBox.Size = new System.Drawing.Size(178, 22);
            this.coreBox.TabIndex = 9;
            // 
            // ramBox
            // 
            this.ramBox.Location = new System.Drawing.Point(230, 109);
            this.ramBox.Name = "ramBox";
            this.ramBox.Size = new System.Drawing.Size(178, 22);
            this.ramBox.TabIndex = 10;
            // 
            // StartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 278);
            this.Controls.Add(this.ramBox);
            this.Controls.Add(this.coreBox);
            this.Controls.Add(this.processorBox);
            this.Controls.Add(this.quitButton);
            this.Controls.Add(this.helpButton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.scheduleCB);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.goButton);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "StartForm";
            this.Text = "Operating Systems Project, Part 1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button goButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox scheduleCB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button helpButton;
        private System.Windows.Forms.Button quitButton;
        private System.Windows.Forms.TextBox processorBox;
        private System.Windows.Forms.TextBox coreBox;
        private System.Windows.Forms.TextBox ramBox;
    }
}


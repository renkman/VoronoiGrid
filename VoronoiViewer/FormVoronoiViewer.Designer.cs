namespace VoronoiViewer
{
    partial class FormVoronoiViewer
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
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.buttonVoronoiDiagram = new System.Windows.Forms.Button();
            this.textBoxHeight = new System.Windows.Forms.TextBox();
            this.textBoxWidth = new System.Windows.Forms.TextBox();
            this.labelWidth = new System.Windows.Forms.Label();
            this.labelHeight = new System.Windows.Forms.Label();
            this.buttonGenerateSites = new System.Windows.Forms.Button();
            this.textBoxSites = new System.Windows.Forms.TextBox();
            this.labelSites = new System.Windows.Forms.Label();
            this.tabControlResult = new System.Windows.Forms.TabControl();
            this.tabPageDiagram = new System.Windows.Forms.TabPage();
            this.tabPageLog = new System.Windows.Forms.TabPage();
            this.textBoxLog = new System.Windows.Forms.TextBox();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.tabControlResult.SuspendLayout();
            this.tabPageLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.buttonVoronoiDiagram);
            this.splitContainer.Panel1.Controls.Add(this.textBoxHeight);
            this.splitContainer.Panel1.Controls.Add(this.textBoxWidth);
            this.splitContainer.Panel1.Controls.Add(this.labelWidth);
            this.splitContainer.Panel1.Controls.Add(this.labelHeight);
            this.splitContainer.Panel1.Controls.Add(this.buttonGenerateSites);
            this.splitContainer.Panel1.Controls.Add(this.textBoxSites);
            this.splitContainer.Panel1.Controls.Add(this.labelSites);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer.Panel2.Controls.Add(this.tabControlResult);
            this.splitContainer.Size = new System.Drawing.Size(1427, 858);
            this.splitContainer.SplitterDistance = 366;
            this.splitContainer.TabIndex = 0;
            // 
            // buttonVoronoiDiagram
            // 
            this.buttonVoronoiDiagram.Location = new System.Drawing.Point(100, 126);
            this.buttonVoronoiDiagram.Name = "buttonVoronoiDiagram";
            this.buttonVoronoiDiagram.Size = new System.Drawing.Size(100, 35);
            this.buttonVoronoiDiagram.TabIndex = 5;
            this.buttonVoronoiDiagram.Text = "Create Voronoi Diagram";
            this.buttonVoronoiDiagram.UseVisualStyleBackColor = true;
            this.buttonVoronoiDiagram.Click += new System.EventHandler(this.buttonVoronoiDiagram_Click);
            // 
            // textBoxHeight
            // 
            this.textBoxHeight.Location = new System.Drawing.Point(100, 6);
            this.textBoxHeight.Name = "textBoxHeight";
            this.textBoxHeight.Size = new System.Drawing.Size(100, 20);
            this.textBoxHeight.TabIndex = 1;
            // 
            // textBoxWidth
            // 
            this.textBoxWidth.Location = new System.Drawing.Point(100, 34);
            this.textBoxWidth.Name = "textBoxWidth";
            this.textBoxWidth.Size = new System.Drawing.Size(100, 20);
            this.textBoxWidth.TabIndex = 2;
            // 
            // labelWidth
            // 
            this.labelWidth.AutoSize = true;
            this.labelWidth.Location = new System.Drawing.Point(12, 37);
            this.labelWidth.Name = "labelWidth";
            this.labelWidth.Size = new System.Drawing.Size(41, 13);
            this.labelWidth.TabIndex = 4;
            this.labelWidth.Text = "Widtht:";
            // 
            // labelHeight
            // 
            this.labelHeight.AutoSize = true;
            this.labelHeight.Location = new System.Drawing.Point(12, 9);
            this.labelHeight.Name = "labelHeight";
            this.labelHeight.Size = new System.Drawing.Size(41, 13);
            this.labelHeight.TabIndex = 3;
            this.labelHeight.Text = "Height:";
            // 
            // buttonGenerateSites
            // 
            this.buttonGenerateSites.Location = new System.Drawing.Point(100, 97);
            this.buttonGenerateSites.Name = "buttonGenerateSites";
            this.buttonGenerateSites.Size = new System.Drawing.Size(100, 23);
            this.buttonGenerateSites.TabIndex = 4;
            this.buttonGenerateSites.Text = "Generate sites";
            this.buttonGenerateSites.UseVisualStyleBackColor = true;
            this.buttonGenerateSites.Click += new System.EventHandler(this.buttonGenerateSites_Click);
            // 
            // textBoxSites
            // 
            this.textBoxSites.Location = new System.Drawing.Point(100, 62);
            this.textBoxSites.Name = "textBoxSites";
            this.textBoxSites.Size = new System.Drawing.Size(100, 20);
            this.textBoxSites.TabIndex = 3;
            // 
            // labelSites
            // 
            this.labelSites.AutoSize = true;
            this.labelSites.Location = new System.Drawing.Point(11, 65);
            this.labelSites.Name = "labelSites";
            this.labelSites.Size = new System.Drawing.Size(83, 13);
            this.labelSites.TabIndex = 0;
            this.labelSites.Text = "Number of sites:";
            // 
            // tabControlResult
            // 
            this.tabControlResult.Controls.Add(this.tabPageDiagram);
            this.tabControlResult.Controls.Add(this.tabPageLog);
            this.tabControlResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlResult.Location = new System.Drawing.Point(0, 0);
            this.tabControlResult.Name = "tabControlResult";
            this.tabControlResult.SelectedIndex = 0;
            this.tabControlResult.Size = new System.Drawing.Size(1057, 858);
            this.tabControlResult.TabIndex = 0;
            // 
            // tabPageDiagram
            // 
            this.tabPageDiagram.AutoScroll = true;
            this.tabPageDiagram.Location = new System.Drawing.Point(4, 22);
            this.tabPageDiagram.Name = "tabPageDiagram";
            this.tabPageDiagram.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDiagram.Size = new System.Drawing.Size(1049, 832);
            this.tabPageDiagram.TabIndex = 0;
            this.tabPageDiagram.Text = "Diagram";
            this.tabPageDiagram.UseVisualStyleBackColor = true;
            this.tabPageDiagram.Paint += new System.Windows.Forms.PaintEventHandler(this.tabPageDiagram_Paint);
            // 
            // tabPageLog
            // 
            this.tabPageLog.Controls.Add(this.textBoxLog);
            this.tabPageLog.Location = new System.Drawing.Point(4, 22);
            this.tabPageLog.Name = "tabPageLog";
            this.tabPageLog.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageLog.Size = new System.Drawing.Size(630, 520);
            this.tabPageLog.TabIndex = 1;
            this.tabPageLog.Text = "Log";
            this.tabPageLog.UseVisualStyleBackColor = true;
            // 
            // textBoxLog
            // 
            this.textBoxLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxLog.Location = new System.Drawing.Point(3, 3);
            this.textBoxLog.Multiline = true;
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.ReadOnly = true;
            this.textBoxLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxLog.Size = new System.Drawing.Size(624, 514);
            this.textBoxLog.TabIndex = 6;
            // 
            // FormVoronoiViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1427, 858);
            this.Controls.Add(this.splitContainer);
            this.Name = "FormVoronoiViewer";
            this.Text = "Voronoi Viewer";
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel1.PerformLayout();
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.ResumeLayout(false);
            this.tabControlResult.ResumeLayout(false);
            this.tabPageLog.ResumeLayout(false);
            this.tabPageLog.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.TextBox textBoxSites;
        private System.Windows.Forms.Label labelSites;
        private System.Windows.Forms.Button buttonGenerateSites;
        private System.Windows.Forms.Label labelWidth;
        private System.Windows.Forms.Label labelHeight;
        private System.Windows.Forms.TextBox textBoxHeight;
        private System.Windows.Forms.TextBox textBoxWidth;
        private System.Windows.Forms.Button buttonVoronoiDiagram;
        private System.Windows.Forms.TabControl tabControlResult;
        private System.Windows.Forms.TabPage tabPageDiagram;
        private System.Windows.Forms.TabPage tabPageLog;
        private System.Windows.Forms.TextBox textBoxLog;
    }
}


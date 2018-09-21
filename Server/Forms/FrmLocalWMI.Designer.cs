namespace xServer.Forms
{
    partial class FrmLocalWMI
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
            this.line5 = new xServer.Controls.Line();
            this.label7 = new System.Windows.Forms.Label();
            this.txtNamespace = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtQuery = new System.Windows.Forms.TextBox();
            this.line1 = new xServer.Controls.Line();
            this.line2 = new xServer.Controls.Line();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTarget = new System.Windows.Forms.TextBox();
            this.txtResults = new System.Windows.Forms.TextBox();
            this.line3 = new xServer.Controls.Line();
            this.label3 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.executeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.txtProperties = new System.Windows.Forms.TextBox();
            this.line4 = new xServer.Controls.Line();
            this.label4 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // line5
            // 
            this.line5.LineAlignment = xServer.Controls.Line.Alignment.Horizontal;
            this.line5.Location = new System.Drawing.Point(71, 34);
            this.line5.Name = "line5";
            this.line5.Size = new System.Drawing.Size(436, 13);
            this.line5.TabIndex = 18;
            this.line5.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 31);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(64, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "Namespace";
            // 
            // txtNamespace
            // 
            this.txtNamespace.Location = new System.Drawing.Point(15, 47);
            this.txtNamespace.Name = "txtNamespace";
            this.txtNamespace.Size = new System.Drawing.Size(270, 20);
            this.txtNamespace.TabIndex = 19;
            this.toolTip1.SetToolTip(this.txtNamespace, "The namespace of the query to be performed. ex.) root/cimv2");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 76);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Query";
            // 
            // txtQuery
            // 
            this.txtQuery.Location = new System.Drawing.Point(15, 93);
            this.txtQuery.Name = "txtQuery";
            this.txtQuery.Size = new System.Drawing.Size(270, 20);
            this.txtQuery.TabIndex = 21;
            this.toolTip1.SetToolTip(this.txtQuery, "The WMI Query to be executed.");
            // 
            // line1
            // 
            this.line1.LineAlignment = xServer.Controls.Line.Alignment.Horizontal;
            this.line1.Location = new System.Drawing.Point(43, 79);
            this.line1.Name = "line1";
            this.line1.Size = new System.Drawing.Size(464, 13);
            this.line1.TabIndex = 22;
            this.line1.TabStop = false;
            // 
            // line2
            // 
            this.line2.LineAlignment = xServer.Controls.Line.Alignment.Horizontal;
            this.line2.Location = new System.Drawing.Point(46, 127);
            this.line2.Name = "line2";
            this.line2.Size = new System.Drawing.Size(461, 13);
            this.line2.TabIndex = 24;
            this.line2.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 124);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 23;
            this.label2.Text = "Target";
            // 
            // txtTarget
            // 
            this.txtTarget.Location = new System.Drawing.Point(15, 140);
            this.txtTarget.Name = "txtTarget";
            this.txtTarget.Size = new System.Drawing.Size(270, 20);
            this.txtTarget.TabIndex = 25;
            this.toolTip1.SetToolTip(this.txtTarget, "The target to run this query against.");
            // 
            // txtResults
            // 
            this.txtResults.BackColor = System.Drawing.Color.Black;
            this.txtResults.ForeColor = System.Drawing.Color.White;
            this.txtResults.Location = new System.Drawing.Point(12, 252);
            this.txtResults.Multiline = true;
            this.txtResults.Name = "txtResults";
            this.txtResults.ReadOnly = true;
            this.txtResults.Size = new System.Drawing.Size(495, 179);
            this.txtResults.TabIndex = 26;
            // 
            // line3
            // 
            this.line3.LineAlignment = xServer.Controls.Line.Alignment.Horizontal;
            this.line3.Location = new System.Drawing.Point(53, 233);
            this.line3.Name = "line3";
            this.line3.Size = new System.Drawing.Size(454, 13);
            this.line3.TabIndex = 28;
            this.line3.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 230);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 27;
            this.label3.Text = "Results";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.executeToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(519, 24);
            this.menuStrip1.TabIndex = 29;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // executeToolStripMenuItem
            // 
            this.executeToolStripMenuItem.Name = "executeToolStripMenuItem";
            this.executeToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.executeToolStripMenuItem.Text = "Execute";
            this.executeToolStripMenuItem.Click += new System.EventHandler(this.executeToolStripMenuItem_Click);
            // 
            // txtProperties
            // 
            this.txtProperties.Location = new System.Drawing.Point(15, 191);
            this.txtProperties.Name = "txtProperties";
            this.txtProperties.Size = new System.Drawing.Size(270, 20);
            this.txtProperties.TabIndex = 32;
            this.toolTip1.SetToolTip(this.txtProperties, "List of properties to be displayed in the results separated by a comma.");
            // 
            // line4
            // 
            this.line4.LineAlignment = xServer.Controls.Line.Alignment.Horizontal;
            this.line4.Location = new System.Drawing.Point(148, 177);
            this.line4.Name = "line4";
            this.line4.Size = new System.Drawing.Size(359, 13);
            this.line4.TabIndex = 31;
            this.line4.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 175);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(138, 13);
            this.label4.TabIndex = 30;
            this.label4.Text = "Comma Delimited Properties";
            // 
            // FrmLocalWMI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(519, 443);
            this.Controls.Add(this.txtProperties);
            this.Controls.Add(this.line4);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.line3);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtResults);
            this.Controls.Add(this.txtTarget);
            this.Controls.Add(this.line2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.line1);
            this.Controls.Add(this.txtQuery);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtNamespace);
            this.Controls.Add(this.line5);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FrmLocalWMI";
            this.Text = "Run WMI Command (Local)";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Controls.Line line5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtNamespace;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtQuery;
        private Controls.Line line1;
        private Controls.Line line2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTarget;
        private System.Windows.Forms.TextBox txtResults;
        private Controls.Line line3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem executeToolStripMenuItem;
        private System.Windows.Forms.TextBox txtProperties;
        private Controls.Line line4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
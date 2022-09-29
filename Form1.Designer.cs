namespace Cafenet {
    partial class Form1 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.timeLeftToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.turnOffToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.add15MinutesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.add30MinutesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.add1HourToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.add2HoursToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.add4HoursToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.keepEnabledToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.keepScreenOnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Text = "Cafenet";
            this.notifyIcon1.Visible = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.timeLeftToolStripMenuItem,
            this.keepScreenOnToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(181, 98);
            // 
            // timeLeftToolStripMenuItem
            // 
            this.timeLeftToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.turnOffToolStripMenuItem,
            this.add15MinutesToolStripMenuItem,
            this.add30MinutesToolStripMenuItem,
            this.add1HourToolStripMenuItem,
            this.add2HoursToolStripMenuItem,
            this.add4HoursToolStripMenuItem,
            this.keepEnabledToolStripMenuItem});
            this.timeLeftToolStripMenuItem.Name = "timeLeftToolStripMenuItem";
            this.timeLeftToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.timeLeftToolStripMenuItem.Text = "ThisShouldChange";
            // 
            // turnOffToolStripMenuItem
            // 
            this.turnOffToolStripMenuItem.Name = "turnOffToolStripMenuItem";
            this.turnOffToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.turnOffToolStripMenuItem.Text = "Turn &off";
            this.turnOffToolStripMenuItem.Click += new System.EventHandler(this.turnOffToolStripMenuItem_Click);
            // 
            // add15MinutesToolStripMenuItem
            // 
            this.add15MinutesToolStripMenuItem.Name = "add15MinutesToolStripMenuItem";
            this.add15MinutesToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.add15MinutesToolStripMenuItem.Text = "Add 15 minutes (&Q)";
            this.add15MinutesToolStripMenuItem.Click += new System.EventHandler(this.add15MinutesToolStripMenuItem_Click);
            // 
            // add30MinutesToolStripMenuItem
            // 
            this.add30MinutesToolStripMenuItem.Name = "add30MinutesToolStripMenuItem";
            this.add30MinutesToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.add30MinutesToolStripMenuItem.Text = "Add 30 minutes (&H)";
            this.add30MinutesToolStripMenuItem.Click += new System.EventHandler(this.add30MinutesToolStripMenuItem_Click);
            // 
            // add1HourToolStripMenuItem
            // 
            this.add1HourToolStripMenuItem.Name = "add1HourToolStripMenuItem";
            this.add1HourToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.add1HourToolStripMenuItem.Text = "Add &1 hour";
            this.add1HourToolStripMenuItem.Click += new System.EventHandler(this.add1HourToolStripMenuItem_Click);
            // 
            // add2HoursToolStripMenuItem
            // 
            this.add2HoursToolStripMenuItem.Name = "add2HoursToolStripMenuItem";
            this.add2HoursToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.add2HoursToolStripMenuItem.Text = "Add &2 hours";
            this.add2HoursToolStripMenuItem.Click += new System.EventHandler(this.add2HoursToolStripMenuItem_Click);
            // 
            // add4HoursToolStripMenuItem
            // 
            this.add4HoursToolStripMenuItem.Name = "add4HoursToolStripMenuItem";
            this.add4HoursToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.add4HoursToolStripMenuItem.Text = "Add &4 hours";
            this.add4HoursToolStripMenuItem.Click += new System.EventHandler(this.add4HoursToolStripMenuItem_Click);
            // 
            // keepEnabledToolStripMenuItem
            // 
            this.keepEnabledToolStripMenuItem.Name = "keepEnabledToolStripMenuItem";
            this.keepEnabledToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.keepEnabledToolStripMenuItem.Text = "Perp&etually";
            this.keepEnabledToolStripMenuItem.Click += new System.EventHandler(this.keepEnabledToolStripMenuItem_Click);
            // 
            // keepScreenOnToolStripMenuItem
            // 
            this.keepScreenOnToolStripMenuItem.CheckOnClick = true;
            this.keepScreenOnToolStripMenuItem.Name = "keepScreenOnToolStripMenuItem";
            this.keepScreenOnToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.keepScreenOnToolStripMenuItem.Text = "Keep &screen on";
            this.keepScreenOnToolStripMenuItem.CheckedChanged += new System.EventHandler(this.keepScreenOnToolStripMenuItem_CheckedChanged);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 5000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem keepScreenOnToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem timeLeftToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem turnOffToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem add15MinutesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem add30MinutesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem add1HourToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem add2HoursToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem add4HoursToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem keepEnabledToolStripMenuItem;
        private System.Windows.Forms.Timer timer1;
    }
}


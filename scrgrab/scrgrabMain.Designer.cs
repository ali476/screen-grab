namespace scrgrab
{
    partial class formMain
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
                m_bdstimer.Dispose();
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
            this.label1 = new System.Windows.Forms.Label();
            this.labelWorkingFolder = new System.Windows.Forms.LinkLabel();
            this.textWorkingFolder = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textHistory = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textInterval = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonLog = new System.Windows.Forms.Button();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuShow = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLogNow = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.label6 = new System.Windows.Forms.Label();
            this.comboHFrom = new System.Windows.Forms.ComboBox();
            this.comboMFrom = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.comboHTo = new System.Windows.Forms.ComboBox();
            this.comboMTo = new System.Windows.Forms.ComboBox();
            this.buttonView = new System.Windows.Forms.Button();
            this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
            this.contextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(462, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Activity logger captures screen image at regular intervals";
            // 
            // labelWorkingFolder
            // 
            this.labelWorkingFolder.AutoSize = true;
            this.labelWorkingFolder.Location = new System.Drawing.Point(11, 39);
            this.labelWorkingFolder.Name = "labelWorkingFolder";
            this.labelWorkingFolder.Size = new System.Drawing.Size(76, 13);
            this.labelWorkingFolder.TabIndex = 1;
            this.labelWorkingFolder.TabStop = true;
            this.labelWorkingFolder.Text = "Working folder";
            this.labelWorkingFolder.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // textWorkingFolder
            // 
            this.textWorkingFolder.Location = new System.Drawing.Point(93, 36);
            this.textWorkingFolder.Name = "textWorkingFolder";
            this.textWorkingFolder.ReadOnly = true;
            this.textWorkingFolder.Size = new System.Drawing.Size(381, 20);
            this.textWorkingFolder.TabIndex = 2;
            this.textWorkingFolder.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Max History";
            // 
            // textHistory
            // 
            this.textHistory.Location = new System.Drawing.Point(93, 62);
            this.textHistory.Name = "textHistory";
            this.textHistory.Size = new System.Drawing.Size(50, 20);
            this.textHistory.TabIndex = 4;
            this.textHistory.TextChanged += new System.EventHandler(this.configuration_Changed);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(149, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "days";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 91);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Interval";
            // 
            // textInterval
            // 
            this.textInterval.Location = new System.Drawing.Point(93, 88);
            this.textInterval.Name = "textInterval";
            this.textInterval.Size = new System.Drawing.Size(50, 20);
            this.textInterval.TabIndex = 7;
            this.textInterval.TextChanged += new System.EventHandler(this.configuration_Changed);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(149, 91);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "minutes";
            // 
            // buttonLog
            // 
            this.buttonLog.Location = new System.Drawing.Point(398, 86);
            this.buttonLog.Name = "buttonLog";
            this.buttonLog.Size = new System.Drawing.Size(75, 23);
            this.buttonLog.TabIndex = 15;
            this.buttonLog.Text = "Log Now!";
            this.buttonLog.UseVisualStyleBackColor = true;
            this.buttonLog.Click += new System.EventHandler(this.LogNow_Clicked);
            // 
            // notifyIcon
            // 
            this.notifyIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon.BalloonTipText = "Activity Logger is still working ...";
            this.notifyIcon.BalloonTipTitle = "ActivityLogger";
            this.notifyIcon.ContextMenuStrip = this.contextMenuStrip;
            this.notifyIcon.Text = "Activity Logger";
            this.notifyIcon.Visible = true;
            this.notifyIcon.DoubleClick += new System.EventHandler(this.NotifyIcon_DoubleClick);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuShow,
            this.menuLogNow,
            this.toolStripSeparator1,
            this.menuExit});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(126, 76);
            // 
            // menuShow
            // 
            this.menuShow.Name = "menuShow";
            this.menuShow.Size = new System.Drawing.Size(125, 22);
            this.menuShow.Text = "Show";
            this.menuShow.Click += new System.EventHandler(this.NotifyIcon_DoubleClick);
            // 
            // menuLogNow
            // 
            this.menuLogNow.Name = "menuLogNow";
            this.menuLogNow.Size = new System.Drawing.Size(125, 22);
            this.menuLogNow.Text = "Log Now!";
            this.menuLogNow.Click += new System.EventHandler(this.LogNow_Clicked);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(122, 6);
            // 
            // menuExit
            // 
            this.menuExit.Name = "menuExit";
            this.menuExit.Size = new System.Drawing.Size(125, 22);
            this.menuExit.Text = "Exit";
            this.menuExit.Click += new System.EventHandler(this.menuExit_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 117);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Operation";
            // 
            // comboHFrom
            // 
            this.comboHFrom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboHFrom.FormattingEnabled = true;
            this.comboHFrom.Location = new System.Drawing.Point(93, 114);
            this.comboHFrom.Name = "comboHFrom";
            this.comboHFrom.Size = new System.Drawing.Size(50, 21);
            this.comboHFrom.TabIndex = 10;
            this.comboHFrom.SelectedIndexChanged += new System.EventHandler(this.configuration_Changed);
            // 
            // comboMFrom
            // 
            this.comboMFrom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboMFrom.FormattingEnabled = true;
            this.comboMFrom.Location = new System.Drawing.Point(149, 114);
            this.comboMFrom.Name = "comboMFrom";
            this.comboMFrom.Size = new System.Drawing.Size(50, 21);
            this.comboMFrom.TabIndex = 11;
            this.comboMFrom.SelectedIndexChanged += new System.EventHandler(this.configuration_Changed);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(205, 117);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(16, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "to";
            // 
            // comboHTo
            // 
            this.comboHTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboHTo.FormattingEnabled = true;
            this.comboHTo.Location = new System.Drawing.Point(227, 114);
            this.comboHTo.Name = "comboHTo";
            this.comboHTo.Size = new System.Drawing.Size(50, 21);
            this.comboHTo.TabIndex = 13;
            this.comboHTo.SelectedIndexChanged += new System.EventHandler(this.configuration_Changed);
            // 
            // comboMTo
            // 
            this.comboMTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboMTo.FormattingEnabled = true;
            this.comboMTo.Location = new System.Drawing.Point(283, 114);
            this.comboMTo.Name = "comboMTo";
            this.comboMTo.Size = new System.Drawing.Size(50, 21);
            this.comboMTo.TabIndex = 14;
            this.comboMTo.SelectedIndexChanged += new System.EventHandler(this.configuration_Changed);
            // 
            // buttonView
            // 
            this.buttonView.Location = new System.Drawing.Point(398, 112);
            this.buttonView.Name = "buttonView";
            this.buttonView.Size = new System.Drawing.Size(75, 23);
            this.buttonView.TabIndex = 16;
            this.buttonView.Text = "View";
            this.buttonView.UseVisualStyleBackColor = true;
            this.buttonView.Click += new System.EventHandler(this.ButtonView_Click);
            // 
            // fileSystemWatcher1
            // 
            this.fileSystemWatcher1.EnableRaisingEvents = true;
            this.fileSystemWatcher1.SynchronizingObject = this;
            // 
            // formMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(485, 147);
            this.Controls.Add(this.buttonView);
            this.Controls.Add(this.comboMTo);
            this.Controls.Add(this.comboHTo);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.comboMFrom);
            this.Controls.Add(this.comboHFrom);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.buttonLog);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textInterval);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textHistory);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textWorkingFolder);
            this.Controls.Add(this.labelWorkingFolder);
            this.Controls.Add(this.label1);
            this.MinimizeBox = false;
            this.Name = "formMain";
            this.Text = "Activity Logger";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Shown += new System.EventHandler(this.FormMain_Shown);
            this.Resize += new System.EventHandler(this.FormMain_Resize);
            this.contextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel labelWorkingFolder;
        private System.Windows.Forms.TextBox textWorkingFolder;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textHistory;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textInterval;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonLog;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem menuShow;
        private System.Windows.Forms.ToolStripMenuItem menuLogNow;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuExit;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboHFrom;
        private System.Windows.Forms.ComboBox comboMFrom;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboHTo;
        private System.Windows.Forms.ComboBox comboMTo;
        private System.Windows.Forms.Button buttonView;
        private System.IO.FileSystemWatcher fileSystemWatcher1;
    }
}


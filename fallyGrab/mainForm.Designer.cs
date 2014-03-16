namespace fallyGrab
{
    partial class mainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainForm));
            this.notificare = new System.Windows.Forms.NotifyIcon(this.components);
            this.meniu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.historyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uploadFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openSaveLocationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.preferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkForUpdatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openErrorLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.meniu.SuspendLayout();
            this.SuspendLayout();
            // 
            // notificare
            // 
            this.notificare.ContextMenuStrip = this.meniu;
            this.notificare.Icon = ((System.Drawing.Icon)(resources.GetObject("notificare.Icon")));
            this.notificare.Text = "fallyGrab";
            this.notificare.Visible = true;
            this.notificare.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notification_MouseDoubleClick);
            // 
            // meniu
            // 
            this.meniu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.meniu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.historyToolStripMenuItem,
            this.uploadFileToolStripMenuItem,
            this.openSaveLocationToolStripMenuItem,
            this.preferencesToolStripMenuItem,
            this.checkForUpdatesToolStripMenuItem,
            this.aboutToolStripMenuItem,
            this.openErrorLogToolStripMenuItem,
            this.helpToolStripMenu,
            this.exitToolStripMenuItem});
            this.meniu.Name = "meniu";
            this.meniu.Size = new System.Drawing.Size(184, 296);
            // 
            // historyToolStripMenuItem
            // 
            this.historyToolStripMenuItem.Image = global::fallyGrab.orange.finished_work;
            this.historyToolStripMenuItem.Name = "historyToolStripMenuItem";
            this.historyToolStripMenuItem.Size = new System.Drawing.Size(183, 30);
            this.historyToolStripMenuItem.Text = "History";
            this.historyToolStripMenuItem.Click += new System.EventHandler(this.historyToolStripMenuItem_Click);
            // 
            // uploadFileToolStripMenuItem
            // 
            this.uploadFileToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.uploadFileToolStripMenuItem.Image = global::fallyGrab.orange.upcoming_work;
            this.uploadFileToolStripMenuItem.Name = "uploadFileToolStripMenuItem";
            this.uploadFileToolStripMenuItem.Size = new System.Drawing.Size(183, 30);
            this.uploadFileToolStripMenuItem.Text = "Upload file";
            this.uploadFileToolStripMenuItem.Click += new System.EventHandler(this.uploadFileToolStripMenuItem_Click);
            // 
            // openSaveLocationToolStripMenuItem
            // 
            this.openSaveLocationToolStripMenuItem.Image = global::fallyGrab.orange.archives;
            this.openSaveLocationToolStripMenuItem.Name = "openSaveLocationToolStripMenuItem";
            this.openSaveLocationToolStripMenuItem.Size = new System.Drawing.Size(183, 30);
            this.openSaveLocationToolStripMenuItem.Text = "Open save location";
            this.openSaveLocationToolStripMenuItem.Click += new System.EventHandler(this.openSaveLocationToolStripMenuItem_Click);
            // 
            // preferencesToolStripMenuItem
            // 
            this.preferencesToolStripMenuItem.Image = global::fallyGrab.orange.settings;
            this.preferencesToolStripMenuItem.Name = "preferencesToolStripMenuItem";
            this.preferencesToolStripMenuItem.Size = new System.Drawing.Size(183, 30);
            this.preferencesToolStripMenuItem.Text = "Preferences";
            this.preferencesToolStripMenuItem.Click += new System.EventHandler(this.preferencesToolStripMenuItem_Click);
            // 
            // checkForUpdatesToolStripMenuItem
            // 
            this.checkForUpdatesToolStripMenuItem.Image = global::fallyGrab.orange.refresh;
            this.checkForUpdatesToolStripMenuItem.Name = "checkForUpdatesToolStripMenuItem";
            this.checkForUpdatesToolStripMenuItem.Size = new System.Drawing.Size(183, 30);
            this.checkForUpdatesToolStripMenuItem.Text = "Check for updates";
            this.checkForUpdatesToolStripMenuItem.Click += new System.EventHandler(this.checkForUpdatesToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Image = global::fallyGrab.orange.category;
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(183, 30);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // openErrorLogToolStripMenuItem
            // 
            this.openErrorLogToolStripMenuItem.Image = global::fallyGrab.orange.current_work;
            this.openErrorLogToolStripMenuItem.Name = "openErrorLogToolStripMenuItem";
            this.openErrorLogToolStripMenuItem.Size = new System.Drawing.Size(183, 30);
            this.openErrorLogToolStripMenuItem.Text = "Open error log";
            this.openErrorLogToolStripMenuItem.Click += new System.EventHandler(this.openErrorLogToolStripMenuItem_Click);
            // 
            // helpToolStripMenu
            // 
            this.helpToolStripMenu.Image = global::fallyGrab.orange.lightbulb;
            this.helpToolStripMenu.Name = "helpToolStripMenu";
            this.helpToolStripMenu.Size = new System.Drawing.Size(183, 30);
            this.helpToolStripMenu.Text = "Help";
            this.helpToolStripMenu.Click += new System.EventHandler(this.helpToolStripMenu_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Image = global::fallyGrab.orange.logout;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(183, 30);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "Image files|*.png;*.jpg;*.bmp;*.gif";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(442, 228);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Opacity = 0D;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "fallyGrab";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.meniu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip meniu;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uploadFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem preferencesToolStripMenuItem;
        public System.Windows.Forms.NotifyIcon notificare;
        private System.Windows.Forms.ToolStripMenuItem openSaveLocationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem historyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkForUpdatesToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem openErrorLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenu;
    }
}


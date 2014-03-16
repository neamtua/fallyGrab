namespace fallyGrab
{
    partial class cropperForm
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
            System.Windows.Forms.Label label1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(cropperForm));
            label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.BackColor = System.Drawing.Color.Transparent;
            label1.CausesValidation = false;
            label1.Dock = System.Windows.Forms.DockStyle.Bottom;
            label1.Enabled = false;
            label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            label1.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            label1.ForeColor = System.Drawing.Color.Black;
            label1.Location = new System.Drawing.Point(0, 213);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(548, 54);
            label1.TabIndex = 0;
            label1.Text = "Press ENTER / DOUBLE CLICK to save or ESCAPE to quit";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cropper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 267);
            this.Controls.Add(label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "cropper";
            this.Opacity = 0.4D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "fallyGrab";
            this.TopMost = true;
            this.Deactivate += new System.EventHandler(this.cropper_Deactivate);
            this.Load += new System.EventHandler(this.cropper_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.cropper_Paint);
            this.DoubleClick += new System.EventHandler(this.cropper_DoubleClick);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cropper_KeyPress);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cropper_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.cropper_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.cropper_MouseUp);
            this.Resize += new System.EventHandler(this.cropper_Resize);
            this.ResumeLayout(false);

        }

        #endregion

    }
}
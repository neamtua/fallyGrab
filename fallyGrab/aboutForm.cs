using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace fallyGrab
{
    public partial class aboutForm : Form
    {
        public aboutForm()
        {
            InitializeComponent();
            label2.Text = "version " + Application.ProductVersion;
        }

        private void about_Load(object sender, EventArgs e)
        {
            label1.Focus();
            try
            {
                textBox1.Text += "\r\n\r\n" + System.IO.File.ReadAllText(Application.StartupPath + @"\changelog.txt");
            }
            catch (Exception ex)
            {
                commonFunctions.writeLog(ex.Message, ex.StackTrace);
            }
            
        }

        public void Dispose()
        {
            System.GC.SuppressFinalize(this);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://fallygrab.com");
        }
    }
}

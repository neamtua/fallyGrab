using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;

namespace fallyGrab
{
    public partial class upload : Form
    {
        public string file = "";
        public string ssfolder = "";
        public string linkreturned = "";
        public string fullname = "";

        public upload()
        {
            InitializeComponent();
            
        }

        private void upload_Load(object sender, EventArgs e)
        {
            this.Hide();
            
            if (file != "")
            {
                // try uploading
                if (fallyGrab.Properties.Settings.Default.ftpFolder != "")
                    Upload(fallyGrab.Properties.Settings.Default.ftpServer+commonFunctions.cleanFtpFolder(fallyGrab.Properties.Settings.Default.ftpFolder), fallyGrab.Properties.Settings.Default.ftpUsername, Security.DecryptString(fallyGrab.Properties.Settings.Default.ftpPassword,Security.encryptionPassw), ssfolder + @"\" + file);
                else
                    Upload(fallyGrab.Properties.Settings.Default.ftpServer, fallyGrab.Properties.Settings.Default.ftpUsername, Security.DecryptString(fallyGrab.Properties.Settings.Default.ftpPassword, Security.encryptionPassw), ssfolder + @"\" + file);
            }
            
        }

        

        private void Upload(string ftpServer, string userName, string password, string filename)
        {
            try
            {
                using (System.Net.WebClient client = new System.Net.WebClient())
                {
                    ftpServer = commonFunctions.cleanUri(ftpServer);
                    client.Credentials = new System.Net.NetworkCredential(userName, password);
                    client.UploadFile(commonFunctions.cleanUri(ftpServer) + new FileInfo(filename).Name, "STOR", filename);
                    client.Dispose();
                }
                // Url shortening
                string shorturl = "";
                string normalurl = commonFunctions.cleanPublicUri(fallyGrab.Properties.Settings.Default.ftpPublic) + new FileInfo(filename).Name;
                if (fallyGrab.Properties.Settings.Default.shortenUrls==1)
                    shorturl = ShortUrl.shortenUrl(commonFunctions.cleanPublicUri(fallyGrab.Properties.Settings.Default.ftpPublic) + new FileInfo(filename).Name);

                // copy to clipboard
                if (shorturl != "")
                {
                    System.Windows.Forms.Clipboard.SetText(shorturl);
                    linkreturned = shorturl;
                }
                else
                {
                    System.Windows.Forms.Clipboard.SetText(commonFunctions.cleanPublicUri(fallyGrab.Properties.Settings.Default.ftpPublic) + new FileInfo(filename).Name);
                    linkreturned = commonFunctions.cleanPublicUri(fallyGrab.Properties.Settings.Default.ftpPublic) + new FileInfo(filename).Name;
                }
                // show alert
                fallyToast.Toaster alertformup = new fallyToast.Toaster();
                alertformup.Show("fallyGrab", "File uploaded to FTP server. The link has been copied to your clipboard.", 8, "Fade", "Up",normalurl,fullname);
            }
            catch (Exception e)
            {
                // show alert
                fallyToast.Toaster alertformuperr = new fallyToast.Toaster();
                alertformuperr.Show("fallyGrab", "Error: "+e.Message, 5, "Fade", "Up");
                commonFunctions.writeLog(e.Message, e.StackTrace);
            }
            
            
        }

        public void Dispose()
        {
            System.GC.SuppressFinalize(this);
        }
        
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using Microsoft.Win32;
using System.Threading;
using System.Diagnostics;
using Facebook;
using System.Dynamic;
using fallyToast;
using System.Data.SQLite;
using System.Collections;

namespace fallyGrab
{
    public partial class mainForm : Form
    {
        
        public static mainForm staticVar = null;
        public static string ssfolder = "";
        public static KeyboardHook hook = new KeyboardHook();
        private static Bitmap bmpScreenshot;
        private static Graphics gfxScreenshot;
        

        public mainForm()
        {
            InitializeComponent();
            staticVar = this;
            if (fallyGrab.Properties.Settings.Default.saveLocation != "")
                ssfolder = fallyGrab.Properties.Settings.Default.saveLocation;
            else ssfolder = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            if (ssfolder.Substring(ssfolder.Length - 1, 1) == @"\")
                ssfolder = ssfolder.Substring(0, ssfolder.Length - 2);
            // do not show
            this.Hide();
            // register the event that is fired after the key press.
            hook.KeyPressed += new EventHandler<KeyPressedEventArgs>(hook_KeyPressed);
            // register the control + alt + F12 combination as hot key.
            hook.RegisterHotKey((ModifierKeys)2 | (ModifierKeys)4, Properties.Settings.Default.keyShortcut);
            hook.RegisterHotKey((ModifierKeys)2 | (ModifierKeys)4, Properties.Settings.Default.keyShortcut2);
            // get appdata path
            string appdatapath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            // check if fallygrab folder exists in appdata and create if not
            if (!Directory.Exists(appdatapath + @"\fallyGrab"))
                Directory.CreateDirectory(appdatapath + @"\fallyGrab");
            // check if database file exists in the fallyGrab folder and if not, copy it from the current location
            if (!File.Exists(appdatapath + @"\fallyGrab\fallygrab.s3db"))
                File.Copy("fallygrab.s3db",appdatapath+@"\fallyGrab\fallygrab.s3db");
            // windows startup
            string shortcutName = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.Programs), "\\Andrei Neamtu\\fallyGrab\\fallyGrab.appref-ms");
            RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (fallyGrab.Properties.Settings.Default.startup == 1)
            {
                if (rkApp.GetValue("fallyGrab") == null)
                    rkApp.SetValue("fallyGrab", shortcutName);
                    
                else
                {
                    rkApp.DeleteValue("fallyGrab", false);
                    rkApp.SetValue("fallyGrab", shortcutName);
                }
            }
            else
            {
                if (rkApp.GetValue("fallyGrab") != null)
                    rkApp.DeleteValue("fallyGrab", false);
            }
            rkApp.Close();
        }

        public static void ReregisterKeys() {
            hook.DisposeIds();
            hook.RegisterHotKey((ModifierKeys)2 | (ModifierKeys)4, Properties.Settings.Default.keyShortcut);
        }

        public static void ResetVars()
        {
            if (fallyGrab.Properties.Settings.Default.saveLocation != "")
                ssfolder = fallyGrab.Properties.Settings.Default.saveLocation;
            else ssfolder = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            if (ssfolder.Substring(ssfolder.Length - 1, 1) == @"\")
                ssfolder = ssfolder.Substring(0, ssfolder.Length - 2);
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

        void hook_KeyPressed(object sender, KeyPressedEventArgs e)
        {
            // print full screen
            if (e.Key == fallyGrab.Properties.Settings.Default.keyShortcut)
            {
                // check if save folder exists
                try
                {
                    // get file name
                    string file = commonFunctions.fileName();
                    // set the bitmap object to the size of the screen
                    ImageCodecInfo myImageCodecInfo;
                    System.Drawing.Imaging.Encoder myEncoder;
                    EncoderParameter myEncoderParameter;
                    EncoderParameters myEncoderParameters;
                    bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppRgb);
                    // create a graphics object from the bitmap
                    gfxScreenshot = Graphics.FromImage(bmpScreenshot);
                    // take the screenshot from the upper left corner to the right bottom corner
                    gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
                    // save the screenshot to the specified path that the user has chosen
                    if (fallyGrab.Properties.Settings.Default.imageFormat == "JPG")
                        myImageCodecInfo = GetEncoderInfo("image/jpeg");
                    else
                        myImageCodecInfo = GetEncoderInfo("image/png");
                    
                     myEncoder = System.Drawing.Imaging.Encoder.Quality;
                     myEncoderParameters = new EncoderParameters(1);
                     myEncoderParameter = new EncoderParameter(myEncoder, Convert.ToInt64(fallyGrab.Properties.Settings.Default.quality));
                     myEncoderParameters.Param[0] = myEncoderParameter;
                     bmpScreenshot.Save(ssfolder + "\\fallyGrab-" + file, myImageCodecInfo, myEncoderParameters);

                    string urlReturned = "";
                    // facebook
                    if (fallyGrab.Properties.Settings.Default.uploadType == "Facebook")
                        uploadFacebook(ssfolder + "\\fallyGrab-" + file);
                    else
                        urlReturned = commonFunctions.useScreenshot(ssfolder + "\\fallyGrab-" + file,ssfolder);
                    // add link to history if returned
                    if (urlReturned != "")
                    {
                        writeHistory(urlReturned);
                    }
                    
                }
                catch (Exception ex)
                {
                    fallyToast.Toaster alertformfolder = new fallyToast.Toaster();
                    alertformfolder.Show("fallyGrab", "Error: "+ex.Message, -1, "Fade", "Up","","","error");
                    commonFunctions.writeLog(ex.Message, ex.StackTrace);
                }
            }
            else if (e.Key == Properties.Settings.Default.keyShortcut2)
            {
                if (!isCropperOpen())
                {
                    cropperForm cropperForm = new cropperForm(this);
                    cropperForm.ssfolder = ssfolder;
                    cropperForm.ShowDialog();

                    // add to history
                    if (cropperForm.urlCrop != "")
                    {
                        string urlReturned = "";
                        // facebook
                        if (fallyGrab.Properties.Settings.Default.uploadType == "Facebook")
                            uploadFacebook(ssfolder + "\\fallyGrab-" + cropperForm.urlCrop);
                        else
                            urlReturned = commonFunctions.useScreenshot(ssfolder + "\\fallyGrab-" + cropperForm.urlCrop, ssfolder);
                        // add link to history if returned
                        if (urlReturned != "")
                        {
                            writeHistory(urlReturned);
                        }
                    }

                }
            }

            // garbage collector
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();
        }

        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!isPreferencesOpen())
            {
                settingsForm settingsform = new settingsForm();
                settingsform.Show();
            }
        }   

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!isAboutOpen())
            {
                aboutForm aboutform = new aboutForm();
                aboutform.Show();
                aboutform.Dispose();
                // garbage collector
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
            }
        }

        private void openSaveLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string myPath = ssfolder;
            string windir = Environment.GetEnvironmentVariable("WINDIR");
            System.Diagnostics.Process prc = new System.Diagnostics.Process();
            prc.StartInfo.FileName = windir + @"\explorer.exe";
            prc.StartInfo.Arguments = myPath+@"\";
            prc.Start();
        }

        private void notification_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string deftray = Properties.Settings.Default.defaction;
            if (deftray == "Open history manager")
            {
                if (!isHistoryOpen())
                {
                    historyManagerForm historyform = new historyManagerForm();
                    historyform.Show();

                    // garbage collector
                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                }
            }
            else if (deftray == "Open file upload")
            {
                try
                {
                    this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
                    openFileDialog1.ShowDialog();
                    if (File.Exists(openFileDialog1.FileName))
                    {
                        // copy file to save location
                        File.Copy(openFileDialog1.FileName, ssfolder + "\\" + new FileInfo(openFileDialog1.FileName).Name, true);
                        // process the screenshot
                        string urlReturned = commonFunctions.useScreenshot(ssfolder + "\\" + new FileInfo(openFileDialog1.FileName).Name, ssfolder);
                        // add link to history if returned
                        if (urlReturned != "")
                        {
                            writeHistory(urlReturned);
                        }
                    }
                }
                catch (Exception exx)
                {
                    // display alert
                    fallyToast.Toaster alertformup2 = new fallyToast.Toaster();
                    alertformup2.Show("fallyGrab", "Error: " + exx.Message, -1, "Fade", "Up", "", "", "error");
                    commonFunctions.writeLog(exx.Message, exx.StackTrace);
                }
            }
            else if (deftray == "Open preferences")
            {
                if (!isPreferencesOpen())
                {
                    settingsForm settingsform = new settingsForm();
                    settingsform.Show();
                }
            }
            else
            {
                string myPath = ssfolder;
                string windir = Environment.GetEnvironmentVariable("WINDIR");
                System.Diagnostics.Process prc = new System.Diagnostics.Process();
                prc.StartInfo.FileName = windir + @"\explorer.exe";
                prc.StartInfo.Arguments = myPath + @"\";
                prc.Start();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void historyMenu(string url) {
            var menuItem = new ToolStripMenuItem();
            menuItem.Image = orange.link;
            menuItem.Click += (System.EventHandler)historyClick;
            menuItem.Text = url;

            historyToolStripMenuItem.DropDownItems.Add(menuItem);
        }

        private void historyClick(object sender, EventArgs e)
        {
            var m1 = (ToolStripMenuItem)sender;
            System.Windows.Forms.Clipboard.SetText(m1.Text);
            fallyToast.Toaster alerthistoryclick = new fallyToast.Toaster();
            alerthistoryclick.Show("fallyGrab", "The link has been copied to your clipboard.", 5, "Fade", "Up");

            // garbage collector
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();
        }

        private void historyHover(object sender, EventArgs e)
        {
            
        }

        private void historyUnhover(object sender, EventArgs e)
        {
           
        }

        private void writeHistory(string url)
        {
            if (Properties.Settings.Default.history == 1)
            {
                // add to database
                SQLiteDatabase db = new SQLiteDatabase();
                Dictionary<String, String> data = new Dictionary<String, String>();
                data.Add("link", url);
                try
                {
                    db.Insert("history", data);
                }
                catch (Exception crap)
                {
                    fallyToast.Toaster alertdb = new fallyToast.Toaster();
                    alertdb.Show("fallyGrab", crap.Message, -1, "Fade", "Up", "", "", "error");
                    commonFunctions.writeLog(crap.Message, crap.StackTrace);
                }
            }

        }

        private void getHistory()
        {
            int counter = 0;
            
            string line;
            List<string> history = new List<string>();

            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\fallygrabhistory.txt"))
            {
                System.IO.StreamReader file = new System.IO.StreamReader(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\fallygrabhistory.txt");
                while ((line = file.ReadLine()) != null)
                {
                    if (line != "") history.Add(line);
                    counter++;
                }

                file.Close();
            }
            counter = 0;
            if (history.Count > 0)
            {
                for (int i = history.Count - 1; i >= 0; i--)
                {
                    historyMenu(history[i]);
                    counter++;
                    if (counter == 5) break;
                }
            }
        }

        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            update.InstallUpdateSyncWithInfo();
        }

        private void uploadFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
                openFileDialog1.ShowDialog();
                if (File.Exists(openFileDialog1.FileName))
                {
                    // copy file to save location
                    File.Copy(openFileDialog1.FileName, ssfolder + "\\" + new FileInfo(openFileDialog1.FileName).Name, true);
                    // process the screenshot
                    string urlReturned = commonFunctions.useScreenshot(ssfolder + "\\" + new FileInfo(openFileDialog1.FileName).Name, ssfolder);
                    // add link to history if returned
                    if (urlReturned != "")
                    {
                        writeHistory(urlReturned);
                    }
                }
            }
            catch (Exception exx)
            {
                fallyToast.Toaster alertformup2 = new fallyToast.Toaster();
                alertformup2.Show("fallyGrab", "Error: " + exx.Message, -1, "Fade", "Up", "", "", "error");
                commonFunctions.writeLog(exx.Message, exx.StackTrace);
            }
            
        }

        private void openErrorLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string appdatapath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            if (File.Exists(appdatapath + "\\fallyGrab\\fallygrab_errorlog.txt"))
            {
                Process.Start(appdatapath + "\\fallyGrab\\fallygrab_errorlog.txt");
            }
            else
            {
                fallyToast.Toaster alertformhe = new fallyToast.Toaster();
                alertformhe.Show("fallyGrab", "The error log file has not been generated", 5, "Fade", "Up");
            }
        }

        private void uploadFacebook(string file)
        {
            if (fallyGrab.Properties.Settings.Default.uploadType == "Facebook")
            {
                if (fallyGrab.Properties.Settings.Default.fbToken != "")
                {

                    var fb = new FacebookClient(Security.DecryptString(fallyGrab.Properties.Settings.Default.fbToken, Security.encryptionPassw));

                    fb.PostCompleted += (o, e) =>
                    {
                        if (e.Cancelled)
                        {
                            var cancellationError = e.Error;
                        }
                        else if (e.Error != null)
                        {
                            // error occurred
                            this.BeginInvoke(new MethodInvoker(
                                                 () =>
                                                 {
                                                     fallyToast.Toaster alertformfbe = new fallyToast.Toaster();
                                                     alertformfbe.Show("fallyGrab", e.Error.Message, -1, "Fade", "Up", "", "", "error");
                                                 }));
                        }
                        else
                        {
                            // the request was completed successfully

                            this.BeginInvoke(new MethodInvoker(
                                                 () =>
                                                 {
                                                     dynamic result = e.GetResultData();
                                                     string linkFB = "https://www.facebook.com/photo.php?fbid=" + result.id;
                                                     // Url shortening
                                                     string shorturl = "";
                                                     if (fallyGrab.Properties.Settings.Default.shortenUrls == 1)
                                                         shorturl = ShortUrl.shortenUrl(linkFB);

                                                     // copy to clipboard
                                                     string urlnotif = "";
                                                     if (shorturl != "")
                                                     {
                                                         System.Windows.Forms.Clipboard.SetText(shorturl);
                                                         historyMenu(shorturl);
                                                         writeHistory(shorturl);
                                                         urlnotif = shorturl;
                                                     }
                                                     else
                                                     {
                                                         System.Windows.Forms.Clipboard.SetText(linkFB);
                                                         historyMenu(linkFB);
                                                         writeHistory(linkFB);
                                                         urlnotif = linkFB;
                                                     }
                                 
                                                     fallyToast.Toaster alertformfacebook = new fallyToast.Toaster();
                                                     alertformfacebook.Show("fallyGrab", "File has been uploaded to Facebook. The link has been copied to your clipboard.", 8, "Fade", "Up",linkFB,file);
                                                 }));
                        }
                    };
                    string type = "";
                    if (Properties.Settings.Default.imageFormat == "JPG")
                        type = "image/jpeg";
                    else if (Properties.Settings.Default.imageFormat == "PNG")
                        type = "image/png";
                    dynamic parameters = new ExpandoObject();
                    parameters.message = "";
                    parameters.source = new FacebookMediaObject
                    {
                        ContentType = type,
                        FileName = Path.GetFileName(file)
                    }.SetValue(File.ReadAllBytes(file));

                    fb.PostAsync("me/photos", parameters);
                }
                else
                {
                    fallyToast.Toaster alertformfb = new fallyToast.Toaster();
                    alertformfb.Show("fallyGrab", "You must login to Facebook from the settings panel", -1, "Fade", "Up", "", "", "error");
                }
            }
        }

        private void helpToolStripMenu_Click(object sender, EventArgs e)
        {
            Process.Start("http://fallygrab.com/about");
        }

        private bool isCropperOpen()
        {
            foreach (Form f in Application.OpenForms)
            {
                if (f is cropperForm)
                {
                    f.Focus();
                    return true;
                }
            }
            return false;
        }

        private bool isPreferencesOpen()
        {
            foreach (Form f in Application.OpenForms)
            {
                if (f is settingsForm)
                {
                    f.Focus();
                    return true;
                }
            }
            return false;
        }

        private bool isAboutOpen()
        {
            foreach (Form f in Application.OpenForms)
            {
                if (f is aboutForm)
                {
                    f.Focus();
                    return true;
                }
            }
            return false;
        }

        private bool isHistoryOpen()
        {
            foreach (Form f in Application.OpenForms)
            {
                if (f is historyManagerForm)
                {
                    f.Focus();
                    return true;
                }
            }
            return false;
        }

        private void historyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!isHistoryOpen())
            {
                historyManagerForm historyform = new historyManagerForm();
                historyform.Show();
                historyform.BringToFront();
                historyform.Focus();
                
                // garbage collector
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
            }
        }
    }

    
}

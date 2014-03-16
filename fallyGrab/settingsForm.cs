using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using Facebook;
using System.Dynamic;

namespace fallyGrab
{
    public partial class settingsForm : Form
    {
        public Keys shortcut;
        public Keys shortcut2;
        // Facebook settings
        private const string AppId = api.Default.facebook;
        private const string ExtendedPermissions = "user_about_me,publish_stream";
        
        public settingsForm()
        {
            InitializeComponent();
        }

        private void setari_Load(object sender, EventArgs e)
        {
            // luam setarile vechi
            textBox1.Text = Properties.Settings.Default.saveLocation;
            if (Properties.Settings.Default.imageFormat == "JPG")
            {
                radioButton1.Checked = true;
                radioButton2.Checked = false;
            }
            else if (Properties.Settings.Default.imageFormat == "PNG")
            {
                radioButton2.Checked = true;
                radioButton1.Checked = false;
            }
            else
            {
                radioButton1.Checked = true;
                radioButton2.Checked = false;
            }
            char s1 = (char)Properties.Settings.Default.keyShortcut;
            char s2 = (char)Properties.Settings.Default.keyShortcut2;
            if (s1.ToString().Trim()=="" || (int)s1 <= 127)
                textBox4.Text = Properties.Settings.Default.keyShortcut.ToString();
            else
                textBox4.Text = s1.ToString();
            if (s2.ToString().Trim() == "" || (int)s2 <= 127)
                textBox14.Text = Properties.Settings.Default.keyShortcut2.ToString();
            else
                textBox14.Text = s2.ToString();
            shortcut = Properties.Settings.Default.keyShortcut;
            shortcut2 = Properties.Settings.Default.keyShortcut2;
            if (Properties.Settings.Default.uploadType == "FTP")
            {
                radioButton3.Checked = true;
                radioButton4.Checked = false;
                radioButton5.Checked = false;
                radioButton6.Checked = false;
                radioButton7.Checked = false;
            }
            else if (Properties.Settings.Default.uploadType == "Dropbox")
            {
                radioButton3.Checked = false;
                radioButton4.Checked = true;
                radioButton5.Checked = false;
                radioButton6.Checked = false;
                radioButton7.Checked = false;
            }
            else if (Properties.Settings.Default.uploadType == "Imageshack")
            {
                radioButton3.Checked = false;
                radioButton4.Checked = false ;
                radioButton5.Checked = false;
                radioButton6.Checked = true;
                radioButton7.Checked = false;
            }
            else if (Properties.Settings.Default.uploadType == "Facebook")
            {
                radioButton3.Checked = false;
                radioButton4.Checked = false;
                radioButton5.Checked = false;
                radioButton6.Checked = false;
                radioButton7.Checked = true;
            }
            else
            {
                radioButton3.Checked = false;
                radioButton4.Checked = false;
                radioButton5.Checked = true;
                radioButton6.Checked = false;
                radioButton7.Checked = false;
            }
            textBox5.Text = Properties.Settings.Default.ftpServer;
            textBox6.Text = Properties.Settings.Default.ftpUsername;
            if (Properties.Settings.Default.ftpPassword != "")
                textBox9.Text = Security.DecryptString(Properties.Settings.Default.ftpPassword, Security.encryptionPassw);
            else
                textBox9.Text = Properties.Settings.Default.ftpPassword;
            textBox7.Text = Properties.Settings.Default.ftpFolder;
            textBox8.Text = Properties.Settings.Default.ftpPublic;
            textBox10.Text = Properties.Settings.Default.dropboxRoot;
            textBox11.Text = Properties.Settings.Default.dropboxUser;
            if (Properties.Settings.Default.is_regkey != "")
                textBox2.Text = Security.DecryptString(Properties.Settings.Default.is_regkey, Security.encryptionPassw);
            else
                textBox2.Text = Properties.Settings.Default.is_regkey; 
            if (Properties.Settings.Default.shortenUrls == 1)
                checkBox1.Checked = true;
            else
                checkBox1.Checked = false;
            if (Properties.Settings.Default.startup == 1)
                checkBox2.Checked = true;
            else
                checkBox2.Checked = false;
            if (Properties.Settings.Default.is_public == "yes")
                checkBox3.Checked = false;
            else
                checkBox3.Checked = true;
            // facebook
            if (Properties.Settings.Default.fbToken != "")
            {
                button5.Visible = false;
                showFBName(Security.DecryptString(Properties.Settings.Default.fbToken,Security.encryptionPassw));
            }
            // quality
            trackBar1.Value = Convert.ToInt32(Properties.Settings.Default.quality);
            // default tray action
            string deftray = Properties.Settings.Default.defaction;
            if (deftray == "Open save location" || deftray == "Open history manager" || deftray == "Open file upload" || deftray=="Open preferences")
                comboBox1.Text = deftray;
            else comboBox1.Text = "Open save location";
            // history
            if (Properties.Settings.Default.history == 1)
                checkBox4.Checked = true;
            else
                checkBox4.Checked = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            textBox1.Text = folderBrowserDialog1.SelectedPath;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string validate = validateSettings();
            if (validate!="")
                MessageBox.Show(validate,"Validation errors",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            else
            {
                // save settings
                Properties.Settings.Default.saveLocation = textBox1.Text;
                if (radioButton1.Checked == true)
                    Properties.Settings.Default.imageFormat = "JPG";
                else
                    Properties.Settings.Default.imageFormat = "PNG";
                Properties.Settings.Default.keyShortcut = shortcut;
                Properties.Settings.Default.keyShortcut2 = shortcut2;
                if (radioButton3.Checked == true)
                    Properties.Settings.Default.uploadType = "FTP";
                else if (radioButton4.Checked == true)
                    Properties.Settings.Default.uploadType = "Dropbox";
                else if (radioButton6.Checked == true)
                    Properties.Settings.Default.uploadType = "Imageshack";
                else if (radioButton7.Checked == true)
                    Properties.Settings.Default.uploadType = "Facebook";
                else
                    Properties.Settings.Default.uploadType = "none";
                Properties.Settings.Default.ftpServer = textBox5.Text;
                Properties.Settings.Default.ftpUsername = textBox6.Text;
                if (textBox9.Text != "")
                    Properties.Settings.Default.ftpPassword = Security.EncryptString(textBox9.Text, Security.encryptionPassw);
                else
                    Properties.Settings.Default.ftpPassword = "";
                Properties.Settings.Default.ftpFolder = textBox7.Text;
                Properties.Settings.Default.ftpPublic = textBox8.Text;
                Properties.Settings.Default.dropboxRoot = textBox10.Text;
                Properties.Settings.Default.dropboxUser = textBox11.Text;
                if (checkBox1.Checked == true)
                    Properties.Settings.Default.shortenUrls = 1;
                else
                    Properties.Settings.Default.shortenUrls = 0;
                if (checkBox2.Checked == true)
                    Properties.Settings.Default.startup = 1;
                else
                    Properties.Settings.Default.startup = 0;
                if (checkBox3.Checked == true)
                    Properties.Settings.Default.is_public = "no";
                else
                    Properties.Settings.Default.is_public = "yes";
                if (textBox2.Text != "")
                    Properties.Settings.Default.is_regkey = Security.EncryptString(textBox2.Text, Security.encryptionPassw);
                else
                    Properties.Settings.Default.is_regkey = "";
                Properties.Settings.Default.quality = trackBar1.Value.ToString();
                // default tray action
                string deftray = comboBox1.Text;
                if (deftray == "Open save location" || deftray == "Open history manager" || deftray == "Open file upload" || deftray == "Open preferences")
                    Properties.Settings.Default.defaction = deftray;
                else Properties.Settings.Default.defaction = "Open save location";
                // history
                if (checkBox4.Checked == true)
                    Properties.Settings.Default.history = 1;
                else
                    Properties.Settings.Default.history = 0;

                Properties.Settings.Default.Save();

                // try to register new hotkeys
                mainForm.ReregisterKeys();

                // reset variables
                mainForm.ResetVars();

                // bug workaround/restart app
                Application.Restart();

                // close form
                this.Close();
                this.Dispose();
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            shortcut = (Keys)(byte)char.ToUpper(e.KeyChar);
            if (shortcut2 == shortcut) {
                fallyToast.Toaster alertformsettings2 = new fallyToast.Toaster();
                alertformsettings2.Show("fallyGrab", "You cannot use the same key for both shortcuts", -1, "Fade", "Up", "", "", "error");
            }
            else
            {
                if (e.KeyChar.ToString().Trim() == "" || (int)e.KeyChar <= 127)
                    textBox4.Text = shortcut.ToString();
                else
                    textBox4.Text = e.KeyChar.ToString();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            textBox10.Text = folderBrowserDialog1.SelectedPath;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void radioButton1_Click(object sender, EventArgs e)
        {
            
                radioButton2.Checked = false;
                radioButton1.Checked = true;
            
        }

        private void radioButton2_Click(object sender, EventArgs e)
        {
           
                radioButton1.Checked = false;
                radioButton2.Checked = true;
            
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox14_KeyPress(object sender, KeyPressEventArgs e)
        {
            shortcut2 = (Keys)(byte)char.ToUpper(e.KeyChar);
            if (shortcut2 == shortcut)
            {
                fallyToast.Toaster alertformsettings3 = new fallyToast.Toaster();
                alertformsettings3.Show("fallyGrab", "You cannot use the same key for both shortcuts", -1, "Fade", "Up", "", "", "error");
            }
            else
            {
                if (e.KeyChar.ToString().Trim() == "" || (int)e.KeyChar <= 127)
                    textBox14.Text = shortcut2.ToString();
                else
                    textBox14.Text = e.KeyChar.ToString();
            }
        }

        public void Dispose()
        {
            System.GC.SuppressFinalize(this);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Validates settings
        /// </summary>
        /// <returns></returns>
        private string validateSettings()
        {
            string message = "";

            // check for shortcut keys overlapping
            if (shortcut2 == shortcut)
                message += "! - You cannot use the same key for both shortcuts. Your settings have not been saved." + "\n";

            // check to see if the save location exists
            if (textBox1.Text.Trim() != "" && !Directory.Exists(textBox1.Text))
                message += "! - The screenshot save location you have selected does not exist." + "\n";

            // check to see if facebook token is set
            if (radioButton7.Checked == true && Properties.Settings.Default.fbToken =="")
                message += "! - You have not logged in on Facebook" + "\n";
            
            // check to see if dropbox settings are ok
            if (radioButton4.Checked == true)
            {
                // check if save location is not empty
                if (textBox1.Text.Trim() != "")
                {
                    // check if the root folder has been selected and it's valid
                    if (textBox10.Text.Trim() != "")
                    {
                        if (!Directory.Exists(textBox10.Text))
                            message += "! - The Dropbox root folder you have selected does not exist." + "\n";
                        else
                        {
                            // check to see if the folder for picture saving is inside the root Dropbox folder
                            string saveLocation = textBox1.Text;
                            string dbRoot = textBox10.Text;
                            if (saveLocation.Substring(saveLocation.Length - 1, 1) == @"\")
                                saveLocation = saveLocation.Substring(0, saveLocation.Length - 2);
                            if (dbRoot.Substring(dbRoot.Length - 1, 1) == @"\")
                                dbRoot = dbRoot.Substring(0, dbRoot.Length - 2);
                            if (saveLocation.IndexOf(dbRoot) == -1)
                                message += "! - The screenshot save location is not located inside your Dropbox folder." + "\n";
                        }
                    }
                    else message += "! - You have not selected the Dropbox root folder." + "\n";
                }
                else message += "! - You have not selected a save location within your Dropbox folder." + "\n";

                // check to see if a user number has been entered
                if (textBox11.Text.Trim() == "")
                    message += "! - You have not entered your Dropbox user number." + "\n";
            }

            // check to see if Imageshack settings are ok
            if (radioButton6.Checked == true)
            {
                if (textBox2.Text.Trim() == "")
                    message += "! - You have not entered your Imageshack registration code." + "\n";
            }

            // check to see if FTP settings are ok
            if (radioButton3.Checked == true)
            {
                int flag = 0;
                if (textBox5.Text.Trim() == "")
                {
                    message += "! - You have not entered an FTP server address." + "\n";
                    flag = 1;
                }
                if (!commonFunctions.validUri(textBox5.Text))
                {
                    message += "! - The FTP server address is not valid" + "\n";
                    flag = 1;
                }
                if (textBox6.Text.Trim() == "")
                {
                    message += "! - You have not entered an FTP username." + "\n";
                    flag = 1;
                }
                if (textBox7.Text.Trim() == "")
                {
                    message += "! - You have not entered an FTP folder location." + "\n";
                    flag = 1;
                }
                if (textBox8.Text.Trim() == "")
                {
                    message += "! - You have not entered a public link for the FTP folder." + "\n";
                    flag = 1;
                }
                if (!commonFunctions.validUri(textBox8.Text))
                {
                    message += "! - The FTP server address is not valid" + "\n";
                    flag = 1;
                }

                // check to see if we can connect using the credentials
                if (flag==0)
                {
                    try
                    {
                        using (System.Net.WebClient client = new System.Net.WebClient())
                        {
                            string ftpServer = commonFunctions.cleanUri(textBox5.Text);
                            client.Credentials = new System.Net.NetworkCredential(textBox6.Text, textBox9.Text);
                            client.Dispose();
                        }
                    }
                    catch
                    {
                        message += "! - A connection to the FTP server could not be established. Please check your settings." + "\n";
                    }
                       
                }
            }

            return message;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // open the Facebook Login Dialog and ask for user permissions.
            var fbLoginDlg = new facebookLoginForm(AppId, ExtendedPermissions);
            fbLoginDlg.ShowDialog();

            // The user has taken action, either allowed/denied or cancelled the authorization,
            // which can be known by looking at the dialogs FacebookOAuthResult property.
            // Depending on the result take appropriate actions.
            TakeLoggedInAction(fbLoginDlg.FacebookOAuthResult);
        }

        private void TakeLoggedInAction(FacebookOAuthResult facebookOAuthResult)
        {
            if (facebookOAuthResult == null)
            {
                // the user closed the FacebookLoginDialog, so do nothing.
                MessageBox.Show("Cancelled!");
                return;
            }

            // Even though facebookOAuthResult is not null, it could had been an 
            // OAuth 2.0 error, so make sure to check IsSuccess property always.
            if (facebookOAuthResult.IsSuccess)
            {
                // since our respone_type in FacebookLoginDialog was token,
                // we got the access_token
                // The user now has successfully granted permission to our app. facebookOAuthResult.AccessToken

                // set token in settings
                Properties.Settings.Default.fbToken = Security.EncryptString(facebookOAuthResult.AccessToken, Security.encryptionPassw);
                Properties.Settings.Default.Save();

                // hide login button
                button5.Visible = false;

                // call function that returns the name of the logged person
                showFBName(facebookOAuthResult.AccessToken);
            }
            else
            {
                // for some reason we failed to get the access token.
                // most likely the user clicked don't allow.
                MessageBox.Show(facebookOAuthResult.ErrorDescription);
            }
        }

        private void showFBName(string token)
        {
            var fb = new FacebookClient(token);

            fb.GetCompleted += (o, e) =>
            {
                if (e.Cancelled)
                {
                    // ignore
                }
                else if (e.Error != null)
                {
                    // error occurred
                    this.BeginInvoke(new MethodInvoker(
                                                 () =>
                                                 {
                                                     button5.Visible = true;
                                                     label23.Visible = false;
                                                     label24.Visible = false;
                                                     button6.Visible = false;
                                                    // delete token from settings
                                                     fallyGrab.Properties.Settings.Default.fbToken = "";
                                                     fallyGrab.Properties.Settings.Default.Save();

                                                 }));
                }
                else
                {
                    // the request was completed successfully
                    dynamic result = e.GetResultData();
                    var firstName = result.first_name;
                    var lastName = result["last_name"];

                    // set labels
                    this.BeginInvoke(new MethodInvoker(
                                         () =>
                                         {
                                             label24.Text = firstName + " " + lastName;
                                             label23.Visible = true;
                                             label24.Visible = true;
                                             button6.Visible = true;
                                         }));
                }
            };

            dynamic parameters = new ExpandoObject();
            parameters.fields = "first_name,last_name";

            fb.GetAsync("me", parameters);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var fb = new FacebookClient();

            var logoutUrl = fb.GetLogoutUrl(new
            {
                next = "https://www.facebook.com/connect/login_success.html",
                access_token = Security.DecryptString(fallyGrab.Properties.Settings.Default.fbToken,Security.encryptionPassw)
            });
            var webBrowser = new WebBrowser();
            webBrowser.Navigated += (o, args) =>
            {
                if (args.Url.AbsoluteUri == "https://www.facebook.com/connect/login_success.html")
                {
                    // update token settings
                    fallyGrab.Properties.Settings.Default.fbToken = "";
                    fallyGrab.Properties.Settings.Default.Save();
                    button5.Visible = true;
                    label23.Visible = false;
                    label24.Visible = false;
                    button6.Visible = false;
                }
            };

            webBrowser.Navigate(logoutUrl.AbsoluteUri);
        }

       
    }
}

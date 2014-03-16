using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using fallyToast;

namespace fallyGrab
{
    class commonFunctions
    {
        public static string useScreenshot(string file, string ssfolder)
        {
            string retLink = "";
            // uploading
            // FTP
            if (fallyGrab.Properties.Settings.Default.uploadType == "FTP")
            {
                // validation
                if (fallyGrab.Properties.Settings.Default.ftpServer != "" && fallyGrab.Properties.Settings.Default.ftpUsername != "" && fallyGrab.Properties.Settings.Default.ftpPassword != "" && fallyGrab.Properties.Settings.Default.ftpPublic != "")
                {
                    upload upform = new upload();
                    upform.file = new FileInfo(file).Name;
                    upform.ssfolder = ssfolder;
                    upform.fullname = file;
                    upform.Show();
                    // return link for menu
                    if (upform.linkreturned != "")
                        retLink = upform.linkreturned;
                    upform.Close();
                }
                else
                {
                    fallyToast.Toaster alertform = new fallyToast.Toaster();
                    alertform.Show("fallyGrab", "You must enter your ftp server details in the application preferences", -1, "Fade", "Up", "", "", "error");
                }
            }
            // Dropbox
            else if (fallyGrab.Properties.Settings.Default.uploadType == "Dropbox")
            {
                if (fallyGrab.Properties.Settings.Default.dropboxRoot != "" && fallyGrab.Properties.Settings.Default.dropboxUser != "")
                {
                    string dbRoot = fallyGrab.Properties.Settings.Default.dropboxRoot;
                    string saveLocation = ssfolder;
                    if (saveLocation.Substring(saveLocation.Length - 1, 1) == @"\")
                        saveLocation = saveLocation.Substring(0, saveLocation.Length - 2);
                    if (dbRoot.Substring(dbRoot.Length - 1, 1) == @"\")
                        dbRoot = dbRoot.Substring(0, dbRoot.Length - 2);

                    string folder = saveLocation.Replace(dbRoot, "");

                    folder = folder.Replace(@"\Public", "");

                    if (folder != "" && folder.Substring(0, 1) == @"\")
                        folder = folder.Substring(1, folder.Length - 1);
                    if (folder != "" && folder.Substring(folder.Length - 1, 1) != @"\")
                        folder = folder + "/";

                    // Url shortening
                    string shorturl = "";
                    string normalurl = "http://dl.dropbox.com/u/" + fallyGrab.Properties.Settings.Default.dropboxUser + "/" + folder + new FileInfo(file).Name;
                    if (fallyGrab.Properties.Settings.Default.shortenUrls == 1)
                        shorturl = ShortUrl.shortenUrl("http://dl.dropbox.com/u/" + fallyGrab.Properties.Settings.Default.dropboxUser + "/" + folder + new FileInfo(file).Name);

                    // copy to clipboard
                    if (shorturl != "")
                    {
                        System.Windows.Forms.Clipboard.SetText(shorturl);
                        retLink = shorturl;
                    }
                    else
                    {
                        System.Windows.Forms.Clipboard.SetText("http://dl.dropbox.com/u/" + fallyGrab.Properties.Settings.Default.dropboxUser + "/" + folder + new FileInfo(file).Name);
                        retLink = "http://dl.dropbox.com/u/" + fallyGrab.Properties.Settings.Default.dropboxUser + "/" + folder + new FileInfo(file).Name;
                    }

                    // show notification
                    fallyToast.Toaster alertformdropbox = new fallyToast.Toaster();
                    alertformdropbox.Show("fallyGrab", "File has been uploaded to Dropbox. The link has been copied to your clipboard.", 8, "Fade", "Up", normalurl, file);
                }
                else
                {
                    fallyToast.Toaster alertformdberror = new fallyToast.Toaster();
                    alertformdberror.Show("fallyGrab", "You must enter your Dropbox details", -1, "Fade", "Up", "", "", "error");
                }
            }
            // Imgur
            else if (fallyGrab.Properties.Settings.Default.uploadType == "Imgur")
            {
                // do the upload
                string url = Imgur.UploadImage(file);

                // Url shortening
                string shorturl = "";
                if (fallyGrab.Properties.Settings.Default.shortenUrls == 1)
                    shorturl = ShortUrl.shortenUrl(url);

                // copy to clipboard
                if (shorturl != "")
                {
                    System.Windows.Forms.Clipboard.SetText(shorturl);
                    retLink = shorturl;
                }
                else
                {
                    System.Windows.Forms.Clipboard.SetText(url);
                    retLink = url;
                }

                // show notification
                fallyToast.Toaster alertformimgur = new fallyToast.Toaster();
                alertformimgur.Show("fallyGrab", "File has been uploaded to Imgur. The link has been copied to your clipboard.", 8, "Fade", "Up", url, file);
            }
            // none
            else
            {
                fallyToast.Toaster alertformnone = new fallyToast.Toaster();
                alertformnone.Show("fallyGrab", "Your screenshot has been saved", 8, "Fade", "Up", "", file);
                retLink = file;
            }
            return retLink;
        }

        public static string fileName()
        {
            DateTime current = DateTime.Now;
            // format as file
            string name = String.Format("{0:d-M-yyyy-HH-mm-ss}", current);
            if (fallyGrab.Properties.Settings.Default.imageFormat == "JPG")
                return name + ".jpg";
            else
                return name + ".png";
        }

        public static string cleanUri(string uri)
        {
            // check if it starts with ftp
            if (uri.Substring(0, 6) != "ftp://")
                uri = "ftp://" + uri;
            // check for trailing slash and add it
            if (uri.Substring(uri.Length - 1, 1) != "/")
                uri = uri + "/";
            // return
            return uri;
        }

        public static string cleanPublicUri(string uri)
        {
            // check for trailing slash and add it
            if (uri.Substring(uri.Length - 1, 1) != "/")
                uri = uri + "/";
            // return
            return uri;
        }

        public static string cleanFtpFolder(string uri)
        {
            // check for starting slash and add it
            if (uri.Substring(0, 1) != "/")
                uri = "/" + uri;
            // check for starting slash and add it
            if (uri.Substring(uri.Length - 1, 1) != "/")
                uri = uri + "/";
            // return
            return uri;
        }

        public static bool validUri(string uri)
        {
            Regex pattern = new Regex(@"(http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?");
            return pattern.IsMatch(uri);
        }

        public static void writeLog(string error, string stacktrace)
        {
            // get appdata path
            string appdatapath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            // check if fallygrab folder exists in appdata and create if not
            if (!Directory.Exists(appdatapath + @"\fallyGrab"))
                Directory.CreateDirectory(appdatapath + @"\fallyGrab");

            DateTime current = DateTime.Now;
            string data = String.Format("{0:d-M-yyyy HH:mm:ss}", current);
            System.IO.StreamWriter file = new System.IO.StreamWriter(appdatapath + "\\fallyGrab\\fallygrab_errorlog.txt", true);
            file.WriteLine("--------------------------------------");
            file.WriteLine(data);
            file.WriteLine("--------------------------------------");
            file.WriteLine(error);
            file.WriteLine("-------------------");
            file.WriteLine(stacktrace);
            file.Close();
        }

    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using fallyToast;
using System.Text.RegularExpressions;

namespace fallyGrab
{
    class Imgur
    {
        public static string ClientId = Security.DecryptString(Properties.Settings.Default.api_imgur_clientid, Security.encryptionPassw);
        public static string UploadImage(string image)
        {
            WebClient w = new WebClient();
            w.Headers.Add("Authorization", "Client-ID " + ClientId);
            System.Collections.Specialized.NameValueCollection Keys = new System.Collections.Specialized.NameValueCollection();
            try
            {
                Keys.Add("image", Convert.ToBase64String(File.ReadAllBytes(image)));
                byte[] responseArray = w.UploadValues("https://api.imgur.com/3/image", Keys);
                dynamic result = Encoding.ASCII.GetString(responseArray);
                System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("link\":\"(.*?)\"");
                Match match = reg.Match(result);
                string url = match.ToString().Replace("link\":\"", "").Replace("\"", "").Replace("\\/", "/");
                return url;
            }
            catch (Exception s)
            {
                fallyToast.Toaster alertform = new fallyToast.Toaster();
                alertform.Show("fallyGrab", "Imgur error: "+s.Message, -1, "Fade", "Up", "", "", "error");
                return "Failed!";
            }
        }

    }
}

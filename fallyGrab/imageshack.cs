using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Xml.Linq;
using System.IO;
using System.Collections.Specialized;
using System.Xml;

namespace fallyGrab
{
    class imageshack
    {
        public static string api_key = api.Default.imageshack;

        public static bool isValidRegistration(string regkey)
        {
            string xml = "";
            xml = doPOST("http://my.imageshack.us/setlogin.php", "login=" + regkey + "&xml=yes");
            XElement elem = XElement.Parse(xml);
            var val1 = elem.Element("exists");
            var Value1 = val1 != null ? val1.Value : "no data found";
            if (Value1 == "yes") return true;
            return false;
        }

        public static string doPOST(string url, string parameters)
        {
            string response = "";

            HttpWebRequest HttpWReq = (HttpWebRequest)WebRequest.Create(url);
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] data = encoding.GetBytes(parameters);

            HttpWReq.Method = "POST";
            HttpWReq.ContentType = "application/x-www-form-urlencoded";
            HttpWReq.ContentLength = data.Length;

            Stream newStream = HttpWReq.GetRequestStream();
            newStream.Write(data, 0, data.Length);
            newStream.Close();

            WebResponse answer = HttpWReq.GetResponse();
            newStream = answer.GetResponseStream();
            StreamReader reader = new StreamReader(newStream);
            response = reader.ReadToEnd();
            reader.Close();
            newStream.Close();
            answer.Close();

            return response;
        }

        public static string UploadFileToImageShack(string fileName, string regkey = "", string ispublic="yes")
        {
            try
            {

                string contentType = null;
                CookieContainer cookie = new CookieContainer();
                NameValueCollection col = new NameValueCollection();
                col["MAX_FILE_SIZE"] = "3145728";
                col["refer"] = "";
                col["brand"] = "";
                col["optimage"] = "1";
                col["rembar"] = "1";
                col["submit"] = "host it!";
                if (regkey!="" && imageshack.isValidRegistration(regkey) == true)
                    col["cookie"] = regkey;
                col["public"] = ispublic;
                List<string> l = new List<string>();
                switch (fileName.Substring(fileName.Length - 3, 3))
                {
                    case "jpg":
                        contentType = "image/jpeg";
                        break;
                    case "peg":
                        contentType = "image/jpeg";
                        break;
                    case "gif":
                        contentType = "image/gif";
                        break;
                    case "png":
                        contentType = "image/png";
                        break;
                    case "bmp":
                        contentType = "image/bmp";
                        break;
                    case "tif":
                        contentType = "image/tiff";
                        break;
                    case "iff":
                        contentType = "image/tiff";
                        break;
                    default:
                        contentType = "image/unknown";
                        break;
                }

                string resp;
                col["optsize"] = "resample";
                resp = UploadFileEx(fileName,
                                               "http://www.imageshack.us/upload_api.php",
                                               "fileupload",
                                               contentType,
                                               col,
                                               cookie);
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(resp);
                XmlNodeList link = xmlDoc.GetElementsByTagName("image_link");
                return link[0].InnerText;
            }
            catch (Exception ex)
            {
                commonFunctions.writeLog(ex.Message, ex.StackTrace);
                return "";
                
            }
        }

        public static string UploadFileEx(string uploadfile, string url,
           string fileFormName, string contenttype, System.Collections.Specialized.NameValueCollection querystring,
           CookieContainer cookies)
        {
            if ((fileFormName == null) ||
                (fileFormName.Length == 0))
            {
                fileFormName = "file";
            }

            if ((contenttype == null) ||
                (contenttype.Length == 0))
            {
                contenttype = "application/octet-stream";
            }


            string postdata;
            postdata = "?";
            if (querystring != null)
            {
                foreach (string key in querystring.Keys)
                {
                    postdata += key + "=" + querystring.Get(key) + "&";
                }
            }
            Uri uri = new Uri(url + postdata);


            string boundary = "----------" + DateTime.Now.Ticks.ToString("x");
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(uri);
            webrequest.CookieContainer = cookies;
            webrequest.ContentType = "multipart/form-data; boundary=" + boundary;
            webrequest.Method = "POST";


            // build up the post message header
            StringBuilder sb = new StringBuilder();
            sb.Append("--");
            sb.Append(boundary);
            sb.Append("\r\n");
            sb.Append("Content-Disposition: form-data; name=\"");
            sb.Append(fileFormName);
            sb.Append("\"; filename=\"");
            sb.Append(Path.GetFileName(uploadfile));
            sb.Append("\"");
            sb.Append("\r\n");
            sb.Append("Content-Type: ");
            sb.Append(contenttype);
            sb.Append("\r\n");
            sb.Append("\r\n");

            string postHeader = sb.ToString();
            byte[] postHeaderBytes = Encoding.UTF8.GetBytes(postHeader);

            // build the trailing boundary string as a byte array
            // ensuring the boundary appears on a line by itself
            byte[] boundaryBytes =
                   Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            FileStream fileStream = new FileStream(uploadfile,
                                        FileMode.Open, FileAccess.Read);
            long length = postHeaderBytes.Length + fileStream.Length +
                                                   boundaryBytes.Length;
            webrequest.ContentLength = length;

            Stream requestStream = webrequest.GetRequestStream();

            // write out our post header
            requestStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);

            // write out the file contents
            byte[] buffer = new Byte[checked((uint)Math.Min(4096,
                                     (int)fileStream.Length))];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                requestStream.Write(buffer, 0, bytesRead);

            // write out the trailing boundary
            requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
            WebResponse responce = webrequest.GetResponse();
            Stream s = responce.GetResponseStream();
            StreamReader sr = new StreamReader(s);

            return sr.ReadToEnd();
        }
    }
}

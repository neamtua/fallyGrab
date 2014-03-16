using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace fallyGrab
{
    class ShortUrl
    {
        // goo.gl api key
        private static string apiKey = api.Default.googl;

        public static string shortenUrl(string url)
        {
            WebRequest request = WebRequest.Create("https://www.googleapis.com/urlshortener/v1/url");
            request.Method = "POST";
            string postData = "{\"longUrl\":\"" + url + "\",\"key\":\"" + apiKey + "\"}";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentType = "application/json";
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            reader.Close();
            dataStream.Close();
            response.Close();

            return parseJson(responseFromServer);
        }

        private static string parseJson(string json)
        {
            string[] lines = json.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            string url = "";
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Length>=4 && lines[i].Substring(2, 2) == "id")
                {
                    string[] split = lines[i].Split(new string[] { "\"" }, StringSplitOptions.None);
                    for (int j = 0; j < split.Length; j++)
                    {
                        if (split[j].Length >= 4 && split[j].Substring(0, 4) == "http")
                            url = split[j];
                    }
                }
            }
            return url;
        }
    }
}

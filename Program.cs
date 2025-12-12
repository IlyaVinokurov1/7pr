using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using HtmlAgilityPack;

namespace HttpNewsPAT
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Cookie token = SingIn("user", "user");
            GetContent(token);
            Console.Read();
            //WebRequest Request = WebRequest.Create("");

            //using(HttpWebResponse Response = (HttpWebResponse)Request.GetResponse())
            //{
            //    Console.WriteLine(Response.StatusDescription);

            //    using(Stream DataStream = Response.GetResponseStream())
            //    {
            //        using(StreamReader Reader = new StreamReader(DataStream))
            //        {
            //            string ResponseFromServer = Reader.ReadToEnd();
            //            Console.WriteLine(ResponseFromServer);
            //        }
            //    }

            //}
       
        }

        public static void ParsingHtml(string htmlCode)
        {
            var html = new HtmlDocument();
            html.LoadHtml(htmlCode);
            var Document = html.DocumentNode;
            IEnumerable<HtmlNode> divsNews = Document.Descendants(0).Where(n => n.HasClass("news"));
            foreach (var DivsNews in divsNews)
            {
                var src = DivsNews.ChildNodes[1].GetAttributeValue("src", "none");
                var name = DivsNews.ChildNodes[3].InnerText;
                var Description = DivsNews.ChildNodes[5].InnerText;
                Console.WriteLine(name + "\n" + "Изображение: " + src + "\n" + "Описание: " + Description + "\n");
            }
        }

        public static void GetContent(Cookie Token)
        {

            string url = "http://10.111.20.114/main";
            Debug.WriteLine($"Выполняем запрос: {url}");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.CookieContainer = new CookieContainer();
            request.CookieContainer.Add(Token);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Debug.WriteLine($"Статус выполнения: {response.StatusCode}");
            string responseFromServer = new StreamReader(response.GetResponseStream()).ReadToEnd();
            Console.WriteLine(responseFromServer);
        }
        public static Cookie SingIn(string Login, string Password)
        {
            Cookie token = null;
            string url = "http://10.111.20.114/ajax/login.php";
            Debug.WriteLine($"Выполняю запрос: {url}");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.CookieContainer = new CookieContainer();
            string postData = $"login={Login}&password={Password}";
            byte[] Data = Encoding.ASCII.GetBytes(postData);
            request.ContentLength = Data.Length;
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(Data, 0, Data.Length);
            }
            using (HttpWebResponse Response = (HttpWebResponse)request.GetResponse())
            {
                Debug.WriteLine($"Статус выполенения: {Response.StatusCode}");
                string ResponseFromServer = new StreamReader(Response.GetResponseStream()).ReadToEnd();
                Console.WriteLine(ResponseFromServer);
                token = Response.Cookies["token"];
            }
            return token;
        }
    }
}

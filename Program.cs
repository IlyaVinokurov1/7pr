using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace HttpNewsPAT
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Cookie token = SingIn("user", "user");
            string Content = GetContent(token);
            ParsingHtml(Content);
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
            Console.Read();
        }

        public static void ParsingHtml(string htmlCode)
        {
            var Html = new HtmlDocument();
            Html.LoadHtml(htmlCode);

            var Document = Html.DocumentNode;
            IEnumerable<HtmlNode> DivNews = Document.Descendants(0).Where(x => x.HasClass("news"));

            foreach(var DivNew in DivNews)
            {
                var src = DivNew.ChildNodes[1].GetAttributes("src", "node");
                var name = DivNew.ChildNodes[3].InnerHtml;
                var description = DivNew.ChildNodes[5].InnerHtml;

                Console.WriteLine($"{name} \nИзображение: {src} \nОписание: {description}");
            }
        }

        public static string GetContent(Cookie token)
        {
            string Content = null;
            string Url = "";

            Debug.WriteLine($"Выполняем запрос: {Url}");

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.CookieContainer = new CookieContainer();
            request.CookieContainer.Add(token);

            using (HttpWebResponse Response = (HttpWebResponse)request.GetResponse())
            {
                Debug.WriteLine($"Статус выполнения: {Response.StatusCode}");

                Content = new StreamReader(Response.GetResponseStream()).ReadToEnd();
                
            }
            return Content;
        }
        public static void SingIn(string login, string password)
        {
            string Url = "";

            Debug.WriteLine($"Выполняем запрос: {Url}");

            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(Url);
            Request.Method = "POST";
            Request.ContentType = "application/x-www-from-urlencoded";
            Request.CookieContainer = new CookieContainer();
            byte[] Data = Encoding.ASCII.GetBytes($"login ={login}&password={password}");
            Request.ContentLength = Data.Length;

            using (Stream stream = Request.GetRequestStream())
            {
                stream.Write(Data, 0, Data.Length);
            }
            using (HttpWebResponse response = (HttpWebResponse)Request.GetResponse())
            {
                Debug.WriteLine($"Статус выполнения: {response.StatusCode}");

                string ResponseFromServer = new StreamReader(response.GetResponseStream()).ReadToEnd();
                Console.WriteLine(ResponseFromServer);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HttpNewsPAT
{
    internal class Program
    {
        static void Main(string[] args)
        {
            WebRequest Request = WebRequest.Create("");
            
            using(HttpWebResponse Response = (HttpWebResponse)Request.GetResponse())
            {
                Console.WriteLine(Response.StatusDescription);

                using(Stream DataStream = Response.GetResponseStream())
                {
                    using(StreamReader Reader = new StreamReader(DataStream))
                    {
                        string ResponseFromServer = Reader.ReadToEnd();
                        Console.WriteLine(ResponseFromServer);
                    }
                }
               
            }
            Console.Read();
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

using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using System.Security.Cryptography;

namespace sanic_test
{
    class Program
    {
        static readonly HttpClient client = new HttpClient();
        static async Task Main(string[] args)
        {

            //url 範例 http://111.253.224.213:9000/api
            //ip 可能過幾天就會跳掉
            //以下url都是localhost的測試,使用前要改

            //post方法
            string url_post = "http://127.0.0.1:1234/sign_up";
            HttpWebRequest post = (HttpWebRequest)WebRequest.Create(url_post);
            post.Method = "POST";
            post.ContentType = "application/json";

            string password = "test0934";//玩家註冊時輸入
            byte[] hashsource;
            byte[] tmphash;
            //密碼轉換成ASCII的byteArray
            hashsource = Encoding.ASCII.GetBytes(password);
            //加密
            tmphash = new MD5CryptoServiceProvider().ComputeHash(hashsource);
            //轉換成String
            string sign_password = ByteArrayToString(tmphash);

            var postData = new
            {
                account = "test01",
                password = sign_password,
                name = "hash_user"
            };
            string postBody = JsonConvert.SerializeObject(postData);
            byte[] byteArray = Encoding.UTF8.GetBytes(postBody);
            using (Stream reqStream = post.GetRequestStream())
            {
                reqStream.Write(byteArray, 0, byteArray.Length);
                reqStream.Close();
            }
            string response_str = "";
            using (WebResponse response = post.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    response_str = reader.ReadToEnd();
                }
            }
            Console.WriteLine(response_str);
            /*
            //hash加密
            //以及怎麼對hash值進行比對
            string password = "";
            byte[] tmpsource;
            byte[] tmphash;
            password = "testpassword0123";
            tmpsource = Encoding.ASCII.GetBytes(password);
            tmphash = new  MD5CryptoServiceProvider().ComputeHash(tmpsource);
            Console.WriteLine(password);
            Console.WriteLine(ByteArrayToString(tmphash));
            password = "87146test01";
            tmpsource = Encoding.ASCII.GetBytes(password);
            byte[] tmpnewhash;
            tmpnewhash = new MD5CryptoServiceProvider().ComputeHash(tmpsource);
            Console.WriteLine(password);
            Console.WriteLine(ByteArrayToString(tmpnewhash));
            Console.WriteLine(ByteArrayToString(tmpnewhash).Length);
            bool chk = false;
            if (tmphash.Length == tmpnewhash.Length) 
            {
                int i = 0;
                while ((i <= tmphash.Length) && (tmphash[i]) == tmpnewhash[i])
                {
                    i += 1;
                }
                if (i ==tmphash.Length)
                {
                    chk = true;
                }
            }
            if (chk)
                Console.WriteLine("password is same");
            else
                Console.WriteLine("password is different");
            */
        }
        //byteArray轉String
        //常規的Encoding.Default.GetString();會亂碼
        static string ByteArrayToString(byte[] arrInput)
        {
            StringBuilder Output = new StringBuilder(arrInput.Length);
            for (int i = 0; i < arrInput.Length - 1; i++)
            {
                Output.Append(arrInput[i].ToString("x2"));
                //X為16進制
                //2為一次兩位,例：0xA會變成0x0A
            }
            return Output.ToString();
        }
    }
}

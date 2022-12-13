using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Demo_NetEaseYoudaoAPI
{
    class Program
    {
        public static void Main()
        {
            var result = @"{
    ""returnPhrase"":[
        ""text translation""
    ],
    ""query"":""text translation"",
    ""errorCode"":""0"",
    ""l"":""en2zh-CHS"",
    ""tSpeakUrl"":""https://openapi.youdao.com/ttsapi?q=%E6%96%87%E6%9C%AC%E7%BF%BB%E8%AF%91&amp;langType=zh-CHS&amp;sign=5F7B8F1446922E2AB3555C94584284E4&amp;salt=1670913065963&amp;voice=4&amp;format=mp3&amp;appKey=169bbb38a747f8f5&amp;ttsVoiceStrict=false"",
    ""web"":[
        {
            ""value"":[
                ""否需要转换""
            ],
            ""key"":""text translation""
        },
        {
            ""value"":[
                ""专业笔译""
            ],
            ""key"":""Professional Text Translation""
        },
        {
            ""value"":[
                ""语篇意识""
            ],
            ""key"":""text translation sense""
        }
    ],
    ""requestId"":""efd0d594-9b8d-4223-ab66-ca313b04e395"",
    ""translation"":[
        ""文本翻译""
    ],
    ""dict"":{
        ""url"":""yddict://m.youdao.com/dict?le=eng&amp;q=text+translation""
    },
    ""webdict"":{
        ""url"":""http://mobile.youdao.com/dict?le=eng&amp;q=text+translation""
    },
    ""basic"":{
        ""us-phonetic"":""tekst trænzˈleɪʃn"",
        ""uk-speech"":""https://openapi.youdao.com/ttsapi?q=text+translation&amp;langType=en&amp;sign=1FEF6F0E17F6798BBD2079450248CD17&amp;salt=1670913065963&amp;voice=5&amp;format=mp3&amp;appKey=169bbb38a747f8f5&amp;ttsVoiceStrict=false"",
        ""explains"":[
            ""语篇翻译""
        ],
        ""us-speech"":""https://openapi.youdao.com/ttsapi?q=text+translation&amp;langType=en&amp;sign=1FEF6F0E17F6798BBD2079450248CD17&amp;salt=1670913065963&amp;voice=6&amp;format=mp3&amp;appKey=169bbb38a747f8f5&amp;ttsVoiceStrict=false""
    },
    ""isWord"":true,
    ""speakUrl"":""https://openapi.youdao.com/ttsapi?q=text+translation&amp;langType=en&amp;sign=1FEF6F0E17F6798BBD2079450248CD17&amp;salt=1670913065963&amp;voice=4&amp;format=mp3&amp;appKey=169bbb38a747f8f5&amp;ttsVoiceStrict=false""
}";
            var value = "";
            var errmsg = "";
            try
            {
                var apiRequest = JsonConvert.DeserializeObject<NetEaseYoudaoAPIRequest>(result);
                if (apiRequest.errorCode == "0")
                {
                    value = apiRequest.translation[0];
                }
                errmsg = $"错误代码:{apiRequest.errorCode}";
            }
            catch (Exception ex)
            {
                throw;
            }
            //Send();
        }

        private static void Send()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            string url = "https://openapi.youdao.com/api";
            string q = "text translation";
            string appKey = "169bbb38a747f8f5";
            string appSecret = "0tGfin1eQcUo4teOPGFEAZjcjmnq6434";
            string salt = DateTime.Now.Millisecond.ToString();
            dic.Add("from", "en");
            dic.Add("to", "zh-CHS");
            dic.Add("signType", "v3");
            TimeSpan ts = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            long millis = (long)ts.TotalMilliseconds;
            string curtime = Convert.ToString(millis / 1000);
            dic.Add("curtime", curtime);
            string signStr = appKey + Truncate(q) + salt + curtime + appSecret; ;
            string sign = ComputeHash(signStr, new SHA256CryptoServiceProvider());
            dic.Add("q", System.Web.HttpUtility.UrlEncode(q));
            dic.Add("appKey", appKey);
            dic.Add("salt", salt);
            dic.Add("sign", sign);
            Post(url, dic);
        }

        protected static string ComputeHash(string input, HashAlgorithm algorithm)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashedBytes = algorithm.ComputeHash(inputBytes);
            string result = BitConverter.ToString(hashedBytes).Replace("-", "");
            return result;
        }

        protected static void Post(string url, Dictionary<string, string> dic)
        {
            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            StringBuilder builder = new StringBuilder();
            int i = 0;
            foreach (var item in dic)
            {
                if (i > 0)
                    builder.Append("&");
                builder.AppendFormat("{0}={1}", item.Key, item.Value);
                i++;
            }
            byte[] data = Encoding.UTF8.GetBytes(builder.ToString());
            req.ContentLength = data.Length;
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            if (resp.ContentType.ToLower().Equals("audio/mp3"))
            {
                SaveBinaryFile(resp, @"E:\Desktop\audio");
            }
            else
            {
                Stream stream = resp.GetResponseStream();
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    result = reader.ReadToEnd();
                }
                Console.WriteLine(result);
                WriteTxtByText(text: result);
            }
        }

        protected static string Truncate(string q)
        {
            if (q == null)
            {
                return null;
            }
            int len = q.Length;
            return len <= 20 ? q : (q.Substring(0, 10) + len + q.Substring(len - 10, 10));
        }

        private static bool SaveBinaryFile(WebResponse response, string FileName)
        {
            string FilePath = FileName + DateTime.Now.Millisecond.ToString() + ".mp3";
            bool Value = true;
            byte[] buffer = new byte[1024];

            try
            {
                if (File.Exists(FilePath))
                    File.Delete(FilePath);
                Stream outStream = System.IO.File.Create(FilePath);
                Stream inStream = response.GetResponseStream();

                int l;
                do
                {
                    l = inStream.Read(buffer, 0, buffer.Length);
                    if (l > 0)
                        outStream.Write(buffer, 0, l);
                }
                while (l > 0);

                outStream.Close();
                inStream.Close();
            }
            catch
            {
                Value = false;
            }
            return Value;
        }

        public static void WriteTxtByText(string fileName = "", string text = "")
        {
            string date = $"{DateTime.Now:yyyy-MM-dd}";
            string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $@"files");
            if (!Directory.Exists(logPath))//如果不存在就创建file文件夹
                Directory.CreateDirectory(logPath);
            string filePath = Path.Combine(logPath, $"{fileName}_{date}.txt");
            using (StreamWriter stream = File.AppendText(filePath))
            {
                stream.WriteLine("===============================================================");
                stream.WriteLine(text);
                stream.Flush();
            }
        }
    }

    public class NetEaseYoudaoAPIRequest
    {
        public string[] returnPhrase { get; set; }
        public string query { get; set; }
        public string errorCode { get; set; }
        public string l { get; set; }
        public string tSpeakUrl { get; set; }
        public Web[] web { get; set; }
        public string requestId { get; set; }
        public string[] translation { get; set; }
        public Dict dict { get; set; }
        public Webdict webdict { get; set; }
        public Basic basic { get; set; }
        public bool isWord { get; set; }
        public string speakUrl { get; set; }
    }

    public class Dict
    {
        public string url { get; set; }
    }

    public class Webdict
    {
        public string url { get; set; }
    }

    public class Basic
    {
        public string usphonetic { get; set; }
        public string ukspeech { get; set; }
        public string[] explains { get; set; }
        public string usspeech { get; set; }
    }

    public class Web
    {
        public string[] value { get; set; }
        public string key { get; set; }
    }
}
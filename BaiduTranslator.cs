using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace Translator
{
    internal class BaiduTranslator
    {
        private class Result
        {
            public class TransResult
            {
                public string src = null;

                public string dst = null;
            }

            public string from                = null;

            public string to                  = null;

            public TransResult[] trans_result = null;

            public int? error_code            = null;

            public string error_msg           = null;
        }

        internal static string Translate(string url)
        {
            var data   = Encoding.UTF8.GetString(new WebClient().DownloadData(url));

            var result = JsonConvert.DeserializeObject<Result>(data);

            if (result.error_code == null)
            {
                return string.Join("\n", result.trans_result.Select(e => e.dst));
            }
            else
            {
                return $"Error Code {result.error_code}: {result.error_msg}";
            }
        }

        internal static string GetURL(string from, string to, string appid, string q, int salt, string key)
        {
            var data = appid + q + salt + key;

            var sign = string.Concat(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(data))
                                                 .Select(x => x.ToString("x2")));

            return $"http://api.fanyi.baidu.com/api/trans/vip/translate?"
                + $"{nameof(q)}={HttpUtility.UrlEncode(q)}"
                + $"&{nameof(from)}={from}"
                + $"&{nameof(to)}={to}"
                + $"&{nameof(appid)}={appid}"
                + $"&{nameof(salt)}={salt}"
                + $"&{nameof(sign)}={sign}";
        }

        internal static (string, string) GetFromAndTo(string optionText)
        {
            switch (optionText)
            {
                case "自动-英文":
                    return ("auto", "en");
                case "自动-中文":
                    return ("auto", "zh");
                case "中文-英文":
                    return ("zh", "en");
                case "英文-中文":
                    return ("en", "zh");
                case "中文-日文":
                    return ("zh", "jp");
                case "日文-中文":
                    return ("jp", "zh");
                case "英文-日文":
                    return ("en", "jp");
                case "日文-英文":
                    return ("jp", "en");
                default:
                    return (null, null);
            }
        }
    }
}

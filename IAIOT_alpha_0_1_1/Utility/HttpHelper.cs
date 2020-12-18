using System.IO;
using System.Net;
using System.Text;

namespace Final_project_IAIOTCloud.Utility
{
    public class HttpHelper
    {
        public static HttpResponseEntity Get(string url, HttpRequestEntity entity, string token = null)
        {
            HttpResponseEntity httpResponseEntity = new HttpResponseEntity();
            //设置请求基本参数
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url); httpWebRequest.ContentType = string.IsNullOrEmpty(entity.ContentType) ? "application/json" : entity.ContentType;
            httpWebRequest.Method = entity.Method.ToString();
            if (token != null)
            {
                httpWebRequest.Headers.Add("Authorization", "Bearer " + token);
            }
            //接收返回的数据
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using var streamReader = new StreamReader(httpResponse.GetResponseStream());
            httpResponseEntity.Bodys = streamReader.ReadToEnd().ToString();
            return httpResponseEntity;
        }
        public static HttpResponseEntity Post(string url, HttpRequestEntity requestEntity, string content = null, string token = null)
        {
            HttpResponseEntity responseEntity = new HttpResponseEntity();
            //设置请求基本参数
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = string.IsNullOrEmpty(requestEntity.ContentType) ? "application/json" : requestEntity.ContentType;
            httpWebRequest.Method = requestEntity.Method.ToString();
            Encoding encoding = requestEntity.Encoding ?? Encoding.UTF8;
            if (token != null)
            {
                httpWebRequest.Headers.Add("Authorization", "Bearer " + token);
            }
            //请求体发送json格式字符   就创建置空就可以
            using (Stream stream = httpWebRequest.GetRequestStream())
            {
                if (content != null)
                {
                    stream.Write(encoding.GetBytes(content));
                    stream.Flush();
                    stream.Close();
                }
            }

            //接收数据
            var httpresponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using var streamReader = new StreamReader(httpresponse.GetResponseStream());
            responseEntity.Bodys = streamReader.ReadToEnd().ToString();
            return responseEntity;
        }
    }
}

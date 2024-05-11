using Infrastructure.Globals;
using Newtonsoft.Json;
using Services.Models;
using Services.Models.Responses.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Utilities.Logs;

namespace Services
{
    public class BaseServices
    {
        private readonly string _baseAddress;
        public BaseServices(string baseUrl)
        {
            _baseAddress = baseUrl;
        }

        #region query
        public BaseResponse<T> PostQuery<T>(string url, object data = null, string token = null, bool writelog = false)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_baseAddress);
                    client.Timeout = TimeSpan.FromSeconds(10000);
                    client.DefaultRequestHeaders.FormatHeaders(token);
                    if (data != null)
                        url += "?" + data.GetQueryString();
                    if (writelog)
                        NLogManager.LogMessage(string.Format("Request: {0}", url));
                    var response = client.PostAsync(url, null).Result;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    if (writelog)
                        NLogManager.LogMessage(string.Format("Response: {0} || Data: {1}", url, responseBody));
                    var result = JsonConvert.DeserializeObject<BaseResponse<T>>(responseBody);
                    if (result != null)
                        return result;
                }
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            return new BaseResponse<T>();
        }
        public BaseResponse<List<MediaResponse>> UploadMedia(string url, object data, dynamic file)
        {
            var result = new BaseResponse<List<MediaResponse>>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_baseAddress);
                    if (data != null)
                        url += "?" + data.GetQueryString();
                    using (var content = new MultipartFormDataContent())
                    {
                        byte[] Bytes = new byte[file.InputStream.Length + 1];
                        file.InputStream.Read(Bytes, 0, Bytes.Length);
                        var fileContent = new ByteArrayContent(Bytes);
                        fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = file.FileName };
                        content.Add(fileContent);
                        var response = client.PostAsync(url, content).Result;
                        var responseString = response.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<BaseResponse<List<MediaResponse>>>(responseString.Result);
                    }
                }
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            return result;
        }
        #endregion

        #region old response
        public BaseResponse<T> Post<T>(string url, object data = null, string token = null, bool writelog = false)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_baseAddress);
                    client.Timeout = TimeSpan.FromSeconds(10000);
                    client.DefaultRequestHeaders.FormatHeaders(token);
                    //Format content
                    var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                    if (writelog)
                        NLogManager.LogMessage(string.Format("Request: {0} || Token: {1} || Data: {2}", url, token, JsonConvert.SerializeObject(data)));
                    var response = client.PostAsync(url, content).Result;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    if (writelog)
                        NLogManager.LogMessage(string.Format("Response: {0} || Data: {1}", url, responseBody));
                    var result = JsonConvert.DeserializeObject<BaseResponse<T>>(responseBody);
                    if (result != null)
                        return result;
                }
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            return new BaseResponse<T>();
        }

        public BaseResponse<T> Put<T>(string url, object data = null, string token = null, bool writelog = false)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_baseAddress);
                    client.Timeout = TimeSpan.FromSeconds(10000);
                    client.DefaultRequestHeaders.FormatHeaders(token);
                    //Format content
                    var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                    if (writelog)
                        NLogManager.LogMessage(string.Format("Request: {0} || Token: {1} || Data: {2}", url, token, JsonConvert.SerializeObject(data)));
                    var response = client.PutAsync(url, content).Result;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    if (writelog)
                        NLogManager.LogMessage(string.Format("Response: {0} || Data: {1}", url, responseBody));
                    var result = JsonConvert.DeserializeObject<BaseResponse<T>>(responseBody);
                    if (result != null)
                        return result;
                }
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            return new BaseResponse<T>();
        }

        public BaseResponse<T> Get<T>(string url, object data = null, string token = null, bool writelog = false)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_baseAddress);
                    client.Timeout = TimeSpan.FromSeconds(10000);
                    client.DefaultRequestHeaders.FormatHeaders(token);

                    if (data != null)
                        url += "?" + data.GetQueryString();
                    if (writelog)
                        NLogManager.LogMessage(string.Format("Request: {0}", url));
                    var response = client.GetAsync(url).Result;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    if (writelog)
                        NLogManager.LogMessage(string.Format("Response: {0} || Data: {1}", url, responseBody));
                    var result = JsonConvert.DeserializeObject<BaseResponse<T>>(responseBody);
                    if (result != null)
                        return result;
                }
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            return new BaseResponse<T>();
        }

        public BaseResponse<T> Delete<T>(string url, object data = null, string token = null, bool writelog = false)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_baseAddress);
                    client.Timeout = TimeSpan.FromSeconds(10000);
                    client.DefaultRequestHeaders.FormatHeaders(token);

                    if (writelog)
                        NLogManager.LogMessage(string.Format("Request: {0} || Token: {1}", url, token));
                    var response = client.DeleteAsync(url).Result;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    if (writelog)
                        NLogManager.LogMessage(string.Format("Response: {0} || Data: {1}", url, responseBody));
                    var result = JsonConvert.DeserializeObject<BaseResponse<T>>(responseBody);
                    if (result != null)
                        return result;
                }
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            return new BaseResponse<T>();
        }
        #endregion

        #region new response
        public BaseResponse<T> PostApi<T>(string url, object data = null, string token = null, bool writelog = false)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_baseAddress);
                    client.Timeout = TimeSpan.FromSeconds(10000);
                    client.DefaultRequestHeaders.FormatHeaders(token);
                    //Format content
                    var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                    if (writelog)
                        NLogManager.LogMessage(string.Format("Request: {0} || Token: {1} || Data: {2}", url, token, JsonConvert.SerializeObject(data)));
                    var response = client.PostAsync(url, content).Result;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    if (writelog)
                        NLogManager.LogMessage(string.Format("Response: {0} || Data: {1}", url, responseBody));
                    var result = JsonConvert.DeserializeObject<BaseResponse<T>>(responseBody);
                    if (result != null)
                    {
                        result.Code = (int)response.StatusCode;
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            return new BaseResponse<T>();
        }

        public BaseResponse<T> PutApi<T>(string url, object data = null, string token = null, bool writelog = false)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_baseAddress);
                    client.Timeout = TimeSpan.FromSeconds(10000);
                    client.DefaultRequestHeaders.FormatHeaders(token);
                    //Format content
                    var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                    if (writelog)
                        NLogManager.LogMessage(string.Format("Request: {0} || Token: {1} || Data: {2}", url, token, JsonConvert.SerializeObject(data)));
                    var response = client.PutAsync(url, content).Result;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    if (writelog)
                        NLogManager.LogMessage(string.Format("Response: {0} || Data: {1}", url, responseBody));
                    var result = JsonConvert.DeserializeObject<BaseResponse<T>>(responseBody);
                    if (result != null)
                    {
                        result.Code = (int)response.StatusCode;
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            return new BaseResponse<T>();
        }

        public BaseResponse<T> GetApi<T>(string url, object data = null, string token = null, bool writelog = false)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_baseAddress);
                    client.Timeout = TimeSpan.FromSeconds(10000);
                    client.DefaultRequestHeaders.FormatHeaders(token);
                    if (data != null)
                        url += "?" + data.GetQueryString();
                    if (writelog)
                        NLogManager.LogMessage(string.Format("Request: {0}", url));
                    var response = client.GetAsync(url).Result;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    if (writelog)
                        NLogManager.LogMessage(string.Format("Response: {0} || Data: {1}", url, responseBody));
                    var result = JsonConvert.DeserializeObject<BaseResponse<T>>(responseBody);
                    if (result != null)
                    {
                        result.Code = (int)response.StatusCode;
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            return new BaseResponse<T>();
        }

        public BaseResponse<T> DeleteApi<T>(string url, object data = null, string token = null, bool writelog = false)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_baseAddress);
                    client.Timeout = TimeSpan.FromSeconds(10000);
                    client.DefaultRequestHeaders.FormatHeaders(token);
                    if (data != null)
                        url += "?" + data.GetQueryString();
                    if (writelog)
                        NLogManager.LogMessage(string.Format("Request: {0} || Token: {1}", url, token));
                    var response = client.DeleteAsync(url).Result;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    if (writelog)
                        NLogManager.LogMessage(string.Format("Response: {0} || Data: {1}", url, responseBody));
                    var result = JsonConvert.DeserializeObject<BaseResponse<T>>(responseBody);
                    if (result != null)
                    {
                        result.Code = (int)response.StatusCode;
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            return new BaseResponse<T>();
        }
        #endregion

        #region crawl
        public BaseResponse<string> Get(string url)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_baseAddress);
                    client.Timeout = TimeSpan.FromSeconds(10000);

                    var response = client.GetAsync(url).Result;
                    string responseBody = response.Content.ReadAsStringAsync().Result;

                    return new BaseResponse<string>
                    {
                        Code = (int)response.StatusCode,
                        Data = responseBody
                    };
                }
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            return new BaseResponse<string>();
        }
        #endregion
    }
    public static class ServicesExtension
    {
        public static string GetQueryString(this object obj)
        {
            var properties = from p in obj.GetType().GetProperties()
                             where p.GetValue(obj, null) != null
                             select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());
            return String.Join("&", properties.ToArray());
        }
        public static void FormatHeaders(this HttpRequestHeaders header, string token)
        {
            if (!string.IsNullOrEmpty(token))
                header.Authorization = new AuthenticationHeaderValue("Bearer", token);
            header.Add("ClientId", Globals.ClientId);
            header.Add("ClientSecret", Globals.ClientSecret);
        }
    }
    public class BaseImg
    {
        public int LinkOption { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string Url { get; set; }
        public string Alt { get; set; }
        public int Type { get; set; }
        public int Idx { get; set; }
    }
}

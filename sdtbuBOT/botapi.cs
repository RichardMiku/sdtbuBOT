using Newtonsoft.Json;
using System.Text;
using System.Text.Json;

namespace sdtbuBOT
{
    public class botapi
    {
        private static readonly string webhookUrl = Environment.GetEnvironmentVariable("WEBHOOK_URL");

        /// <summary>
        /// 发送群消息_目标id_文本消息
        /// </summary>
        /// <param name="uid">目标id</param>
        /// <param name="message">消息内容</param>
        /// <returns></returns>
        public static async Task<bool> SendGroupMessage(string uid, string message)
        {
            using (HttpClient _client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Post, $"{webhookUrl}");
                var payload = new
                {
                    to = new { id = uid },
                    isRoom = true,
                    data = new { content = message }
                };
                request.Content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

                var response = await _client.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
        }

        /// <summary>
        /// 发送消息_目标id_文本消息
        /// </summary>
        /// <param name="uid">目标id</param>
        /// <param name="message">消息内容</param>
        /// <returns></returns>
        public static async Task<bool> SendMessage(string uid,string message)
        {
            using (HttpClient _client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Post, $"{webhookUrl}");
                var payload = new
                {
                    to = new { id = uid },
                    data = new { content = message }
                };
                request.Content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

                var response = await _client.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
        }
    }
}

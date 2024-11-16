using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace zhipuLib
{
    public class api_AI
    {
        /// <summary>
        /// 读取智普清言AI返回文本内容
        /// </summary>
        /// <param name="zhipu_result">智普清言AI源内容</param>
        /// <returns></returns>
        public static async Task<string> zhipu_ReadContent(string PromatContent)
        {
            var jsonObject = JsonConvert.DeserializeObject<dynamic>(await ZHIPUpostAsync(PromatContent));
            string content = jsonObject.choices[0].message.content;
            return content;
        }

        /// <summary>
        /// 智普清言生成式AI
        /// </summary>
        /// <param name="promat">请求内容</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">未正确配置AI环境变量</exception>
        public static async Task<string> ZHIPUpostAsync(string promat)
        {
            using (var client = new HttpClient())
            {
                string api_key = Environment.GetEnvironmentVariable("ZHIPU_API_KEY");
                if (string.IsNullOrEmpty(api_key))
                {
                    throw new InvalidOperationException("API key is not set in the environment variables.");
                }

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", api_key);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var requestBody = new
                {
                    model = "glm-4-flash",
                    messages = new[]
                    {
                        new { role = "user", content = promat }
                    }
                };

                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("https://open.bigmodel.cn/api/paas/v4/chat/completions", content);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
        }
    }
}

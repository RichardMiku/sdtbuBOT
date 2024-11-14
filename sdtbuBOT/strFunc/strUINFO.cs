using sdtbuhelperLib;
using System.Text.RegularExpressions;

namespace sdtbuBOT.strFunc
{
    public class strUINFO
    {
        /// <summary>
        /// 绑定智慧山商
        /// </summary>
        /// <param name="wxid">微信昵称</param>
        /// <param name="bindMSG">绑定命令</param>
        /// <returns></returns>
        public static string BindSDTBU(string wxid, string bindMSG)
        {
            string input = bindMSG;
            // 正则表达式解释：
            // "绑定" - 匹配命令 "绑定"
            // @"\s+" - 匹配一个或多个空白字符
            // "(\p{L}+)" - 匹配一个或多个字母字符（包括中文）
            // @"\s+" - 再次匹配一个或多个空白字符
            // "(\p{L}+)" - 再次匹配一个或多个字母字符（包括中文）
            Regex regex = new Regex(@"信息绑定\s+([\p{L}\p{N}\p{P}\p{S}]+)\s+([\p{L}\p{N}\p{P}\p{S}]+)");
            Match match = regex.Match(input);

            if (match.Success)
            {
                string SDTBUaccount = match.Groups[1].Value; // 第一个参数 "123"
                string SDTBUpasswd = match.Groups[2].Value; // 第二个参数 "abc"
                api_sqlite _sqliteAPI =new api_sqlite("Data Source=USERS.db");
                _sqliteAPI.CreateTable();
                _sqliteAPI.InsertUser(wxid, SDTBUaccount, SDTBUpasswd);
                if (true)
                {
                    return "绑定成功！";
                }
                else
                {
                    return "绑定失败";
                }
            }
            else
            {
                return "可能存在错误的输入信息";
            }
        }

        /// <summary>
        /// 判断消息开头是否为信息绑定
        /// </summary>
        /// <param name="msgcontent"></param>
        /// <returns></returns>
        public static bool BindRegex(string msgcontent)
        {
            string input = msgcontent;
            // 正则表达式解释：
            // "绑定" - 匹配命令 "绑定"
            // @"\s+" - 匹配一个或多个空白字符
            // "(\p{L}+)" - 匹配一个或多个字母字符（包括中文）
            // @"\s+" - 再次匹配一个或多个空白字符
            // "(\p{L}+)" - 再次匹配一个或多个字母字符（包括中文）
            Regex regex = new Regex(@"信息绑定\s+([\p{L}\p{N}\p{P}\p{S}]+)\s+([\p{L}\p{N}\p{P}\p{S}]+)");
            Match match = regex.Match(input);
            if (match.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

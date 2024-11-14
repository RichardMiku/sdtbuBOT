using sdtbuhelperLib;
using System.Text.RegularExpressions;

namespace sdtbuBOT.strFunc
{
    public class strUINFO
    {
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="wxid">微信id</param>
        /// <returns></returns>
        public async static Task<string> USRINFO(string wxid)
        {
            // 创建SQLite API实例，连接到USERS.db数据库
            api_sqlite _sqliteAPI = new api_sqlite("Data Source=USERS.db");
            // 创建表格（如果不存在）
            _sqliteAPI.CreateTable();
            // 获取用户信息
            Dictionary<string, object> _user = _sqliteAPI.GetUser(wxid);
            // 创建智慧山商API实例，使用用户的学号和密码
            api_sdtbu _sdtbuAPI = new api_sdtbu(_user["STUID"].ToString(), _user["PASSWD"].ToString());
            // 获取用户详细信息
            Dictionary<string, string> _userinfo = await _sdtbuAPI.GetUserInfo();
            // 初始化用户信息字符串
            string usrinfo = "ℹ个人信息\n";
            // 定义字段前的emoji符号
            string[] Femoji = { "📓", "🏷", "⚧", "🏫" };
            // 定义每个字段后的换行符
            string[] newLine = { "\n", "\n", "\n", "" };
            // 遍历用户信息字典，拼接信息字符串
            for (int i = 0; i < _userinfo.Count; i++)
            {
                usrinfo += Femoji[i] + _userinfo.ElementAt(i).Key + ":" + _userinfo.ElementAt(i).Value + newLine[i];
            }
            // 返回用户信息字符串
            return usrinfo;
        }

        /// <summary>
        /// 检查是否绑定
        /// </summary>
        /// <param name="wxid">微信uid</param>
        /// <returns></returns>
        public static bool isBIND(string wxid)
        {
            // 创建SQLite API实例，连接到USERS.db数据库
            api_sqlite _sqliteAPI = new api_sqlite("Data Source=USERS.db");
            // 创建表格（如果不存在）
            _sqliteAPI.CreateTable();
            // 获取用户信息
            Dictionary<string, object> _user = _sqliteAPI.GetUser(wxid);
            // 检查用户的CHECKID是否与微信id匹配
            if (_user["CHECKID"].ToString() == wxid)
            {
                // 如果匹配，返回true
                return true;
            }
            else
            {
                // 如果不匹配，返回false
                return false;
            }
        }

        /// <summary>
        /// 绑定智慧山商
        /// </summary>
        /// <param name="wxid">微信昵称</param>
        /// <param name="bindMSG">绑定命令</param>
        /// <returns></returns>
        public static string BindSDTBU(string wxid, string bindMSG)
        {
            // 获取绑定命令的输入
            string input = bindMSG;
            // 定义正则表达式匹配绑定命令
            // 正则表达式解释：
            // "绑定" - 匹配命令 "绑定"
            // @"\s+" - 匹配一个或多个空白字符
            // "(\p{L}+)" - 匹配一个或多个字母字符（包括中文）
            // @"\s+" - 再次匹配一个或多个空白字符
            // "(\p{L}+)" - 再次匹配一个或多个字母字符（包括中文）
            Regex regex = new Regex(@"信息绑定\s+([\p{L}\p{N}\p{P}\p{S}]+)\s+([\p{L}\p{N}\p{P}\p{S}]+)");
            // 匹配输入命令
            Match match = regex.Match(input);

            // 如果匹配成功
            if (match.Success)
            {
                // 获取学号和密码
                string SDTBUaccount = match.Groups[1].Value; // 第一个参数 "123"
                string SDTBUpasswd = match.Groups[2].Value; // 第二个参数 "abc"
                                                            // 创建SQLite API实例，连接到USERS.db数据库
                api_sqlite _sqliteAPI = new api_sqlite("Data Source=USERS.db");
                // 创建表格（如果不存在）
                _sqliteAPI.CreateTable();
                // 插入用户信息
                _sqliteAPI.InsertUser(SDTBUaccount, SDTBUpasswd, wxid);
                // 检查是否绑定成功
                if (isBIND(wxid))
                {
                    // 返回绑定成功信息
                    return "绑定成功！";
                }
                else
                {
                    // 返回绑定失败信息
                    return "绑定失败";
                }
            }
            else
            {
                // 返回输入信息错误提示
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
            // 获取消息内容
            string input = msgcontent;
            // 定义正则表达式匹配绑定命令
            // 正则表达式解释：
            // "绑定" - 匹配命令 "绑定"
            // @"\s+" - 匹配一个或多个空白字符
            // "(\p{L}+)" - 匹配一个或多个字母字符（包括中文）
            // @"\s+" - 再次匹配一个或多个空白字符
            // "(\p{L}+)" - 再次匹配一个或多个字母字符（包括中文）
            Regex regex = new Regex(@"信息绑定\s+([\p{L}\p{N}\p{P}\p{S}]+)\s+([\p{L}\p{N}\p{P}\p{S}]+)");
            // 匹配输入命令
            Match match = regex.Match(input);
            // 如果匹配成功，返回true，否则返回false
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

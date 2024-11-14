using sdtbuhelperLib;

namespace sdtbuBOT.strFunc
{
    public class strUCOURSE
    {
        public async static Task<string> USRCOURSE(string wxid)
        {
            // 创建SQLite API实例，连接到USERS.db数据库
            api_sqlite _sqliteAPI = new api_sqlite("Data Source=USERS.db");
            // 创建表格（如果不存在）
            _sqliteAPI.CreateTable();
            // 获取用户信息
            Dictionary<string, object> _user = _sqliteAPI.GetUser(wxid);
            // 创建智慧山商API实例，使用用户的学号和密码
            api_sdtbu _sdtbuAPI = new api_sdtbu(_user["STUID"].ToString(), _user["PASSWD"].ToString());
            // 获取课程列表字符串
            var course = await _sdtbuAPI.CourseListString();
            // 初始化课程信息字符串
            string _COURSE = "";
            // 遍历课程列表
            for (int i = 0; i < course.Count; i++)
            {
                // 拼接课程信息字符串
                _COURSE += "📖" + course[i];
                // 如果不是最后一门课程，添加换行符
                if (i < course.Count - 1)
                {
                    _COURSE += "\n\n";
                }
            }
            // 返回课程信息字符串
            return _COURSE;
        }
    }
}

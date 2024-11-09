using AngleSharp.Html.Parser;
using Jint;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Json;

namespace sdtbuhelperLib
{
    public class api_sdtbu : IDisposable
    {
        private readonly HttpClient _client;
        private readonly string _username;
        private readonly string _password;
        private readonly Engine _jsEngine;
        private bool _loginStatus;
        private Dictionary<string, string> _userInfo;
        private HttpResponseMessage _loginCookie;
        private List<Dictionary<string, object>> _COURSEINFOLIST;
        /// <summary>
        /// 智慧山商API类
        /// </summary>
        /// <param name="username">学号_用户名</param>
        /// <param name="password">密码</param>
        public api_sdtbu(string username, string password)
        {
            _client = new HttpClient();//创建HttpClient对象
            //设置请求头User-Agent
            _client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36 Edg/120.0.0.0");
            _username = username;//初始化学号_用户名
            _password = password;//初始化密码
            _userInfo = null;//初始化用户信息为空
            var jsCode = des.desjscontent();//获取des.js文件内容
            _jsEngine = new Engine().Execute(jsCode);//创建Jint引擎并执行js代码
            _loginStatus = Login().Result;//调用登录方法
        }

        /// <summary>
        /// 将上课星期的数字转换为对应的中文星期名称
        /// </summary>
        /// <param name="weekDay">上课星期的数字</param>
        /// <returns>中文星期名称</returns>
        private string GetWeekDayName(int weekDay)
        {
            return weekDay switch
            {
                1 => "星期一",
                2 => "星期二",
                3 => "星期三",
                4 => "星期四",
                5 => "星期五",
                6 => "星期六",
                7 => "星期日",
                _ => "未知星期"
            };
        }

        /// <summary>
        /// 获取下节课信息
        /// </summary>
        /// <returns>下节课程信息</returns>
        public async Task<string> NextCourse()
        {
            // 强行定义时间进行测试
            //var now = new DateTime(2023, 10, 31, 20, 0, 0); // 2023年10月30日，周一，上午10点

            // 获取当前时间
            var now = DateTime.Now;

            // 调用 OrderAsCourseTime 方法获取按上课时间排序的课程信息列表
            var classInfoList = await OrderAsCourseTime();

            // 遍历课程信息列表，找到当前时间之后的第一节课
            foreach (var course in classInfoList)
            {
                // 获取课程的上课星期和上课时间
                int courseWeek = (int)course["CourseWeek"];
                int courseTime = (int)course["CourseTime"];

                // 获取当前星期几
                int currentWeek = (int)now.DayOfWeek;
                if (currentWeek == 0) currentWeek = 7; // 将星期日转换为7

                // 获取上课时间的具体时间段
                TimeSpan startTime;
                switch (courseTime)
                {
                    case 1:
                        startTime = new TimeSpan(8, 0, 0);
                        break;
                    case 3:
                        startTime = new TimeSpan(9, 50, 0);
                        break;
                    case 5:
                        startTime = new TimeSpan(14, 0, 0);
                        break;
                    case 7:
                        startTime = new TimeSpan(15, 50, 0);
                        break;
                    case 9:
                        startTime = new TimeSpan(19, 0, 0);
                        break;
                    default:
                        continue;
                }

                // 构造课程的上课日期时间
                var courseDateTime = now.Date.AddDays(courseWeek - currentWeek).Add(startTime);

                // 如果课程时间在当前时间之后，则返回该课程信息
                if (courseDateTime > now)
                {
                    return $"下节课信息：\n" +
                        $"课程名称：{course["CourseName"]}\n" +
                        $"教学班级：{course["CourseClass"]}\n" +
                        $"上课时间：{startTime}\n" +
                        $"上课地点：{course["Location"]}\n" +
                        $"上课星期：{GetWeekDayName(courseWeek)}";
                }
            }

            return "本周没有更多的课程了。";
        }

        /// <summary>
        /// 课程信息_字符串列表
        /// </summary>
        /// <returns>课程字符串列表</returns>
        public async Task<List<string>> CourseListString()
        {
            List<string> CourseList = new List<string>();//课程字符串列表
            var CourseINFOList = await OrderAsCourseTime();//获取按上课时间排序的课程信息列表
            string CourseTimeName = "";//上课时间名称
            //遍历课程信息列表
            foreach (var CourseDictionary in CourseINFOList)
            {
                //根据上课时间获取对应的具体上课时间
                switch ((int)CourseDictionary["CourseTime"])
                {
                    case 1:
                        CourseTimeName = "8:00-9:30";
                        break;
                    case 2:
                        CourseTimeName = "第二节";
                        break;
                    case 3:
                        CourseTimeName = "9:50-11:20";
                        break;
                    case 4:
                        CourseTimeName = "第四节";
                        break;
                    case 5:
                        CourseTimeName = "14:00-15:30";
                        break;
                    case 6:
                        CourseTimeName = "第六节";
                        break;
                    case 7:
                        CourseTimeName = "15:50-17:20";
                        break;
                    case 8:
                        CourseTimeName = "第八节";
                        break;
                    case 9:
                        CourseTimeName = "19:00-20:30";
                        break;
                    case 10:
                        CourseTimeName = "第十节";
                        break;
                }
                CourseList.Add($"课程名称：{CourseDictionary["CourseName"]}" +
                    $"\n教学班级：{CourseDictionary["CourseClass"]}" +
                    $"\n上课时间：{CourseTimeName}" +
                    $"\n上课地点：{CourseDictionary["Location"]}" +
                    $"\n上课星期：{GetWeekDayName((int)CourseDictionary["CourseWeek"])}");
            }
            return CourseList;
        }

        /// <summary>
        /// 课程信息_按上课时间排序
        /// </summary>
        /// <returns></returns>
        public async Task<List<Dictionary<string, object>>> OrderAsCourseTime()
        {
            // 调用 OrderAsCourseWeek 方法获取按上课星期排序的课程信息列表
            var classInfoList = await OrderAsCourseWeek();

            // 使用 LINQ 对列表在上课星期排序不变的基础上按上课时间进行排序
            var sortedList = classInfoList
                .OrderBy(classInfo => (int)classInfo["CourseWeek"])//第一次排序_按上课星期排序
                .ThenBy(classInfo => (int)classInfo["CourseTime"])//第二次排序_按上课时间排序
                .ToList();

            return sortedList;
        }

        /// <summary>
        /// 课程信息_按上课星期排序
        /// </summary>
        /// <returns></returns>
        public async Task<List<Dictionary<string, object>>> OrderAsCourseWeek()
        {
            // 调用 ReadClassInfoAsList 方法获取课程信息列表
            var classInfoList = _COURSEINFOLIST ?? await ReadClassInfoAsList();

            // 使用 LINQ 对列表按 CourseWeek 进行排序
            var sortedList = classInfoList.OrderBy(classInfo => (int)classInfo["CourseWeek"]).ToList();

            return sortedList;
        }

        /// <summary>
        /// 读取课程信息
        /// </summary>
        /// <returns>课程信息字典列表</returns>
        public async Task<List<Dictionary<string, object>>> ReadClassInfoAsList()
        {
            var list = new List<Dictionary<string, object>>();//课程信息列表
            Dictionary<string, object> ClassDictionary;//课程信息字典
            JArray CourseINFO = await GetClassInfo();//获取课程信息JSON数组
            //遍历课程信息JSON数组
            foreach (JObject courses in CourseINFO)
            {
                string courseName = (string)courses["KCMC"];//课程名称
                string witchclass = (string)courses["JXBMC"];//教学班级
                int CourseTime = (int)courses["SKJC"];//上课时间
                string location = (string)courses["JXDD"];//上课地点
                int weekDay = (int)courses["SKXQ"];//上课星期

                //构造课程信息字典
                ClassDictionary = new Dictionary<string, object>
                {
                    { "CourseName", courseName },
                    { "CourseClass", witchclass },
                    { "CourseTime", CourseTime },
                    { "Location", location },
                    { "CourseWeek", weekDay}
                };
                //添加课程信息到列表
                list.Add(ClassDictionary);
            };

            _COURSEINFOLIST = list;//保存课程信息列表，便于后续处理不用再次获取

            return list;
        }

        /// <summary>
        /// 获取课程信息JSON数组
        /// </summary>
        /// <returns>课程信息JSON数组</returns>
        public async Task<JArray> GetClassInfo()
        {
            /*
            获取课程信息
            JSXM: 教师姓名
            JXBMC: 教学班名称
            ZZZ: 未知(应该是总课次)
            XH: 学号
            KCMC: 课程名称
            JXDD: 教学地点
            KKXND: 开课学年度
            JXBH: 教学班号
            KKXQM: 开课学期名
            JSGH: 教师工号
            CXJC: 未知(应该是一节课的时长)
            QSZ: 起始周
            ZCSM: 上课的周次
            SKXQ: 上课星期
            SKJC: 上课节次
            KCH: 课程号
            */
            //构造当前上课周请求体
            var learnweekPostBoay = new
            {
                mapping = "getSemesterbyDate"
            };
            //发送请求获取当前上课周
            var learnWeekJSONbody = new StringContent(System.Text.Json.JsonSerializer.Serialize(learnweekPostBoay), System.Text.Encoding.UTF8, "application/json");
            var learnWeekResponse = await _client.PostAsync("https://zhss.sdtbu.edu.cn/tp_up/up/widgets/getLearnweekbyDate", learnWeekJSONbody);
            var learnWeek = await learnWeekResponse.Content.ReadFromJsonAsync<Dictionary<string, string>>();
            //构造获取所有课程信息请求体
            var ClassPostData_ALL = new
            {
                learnWeek = learnWeek["learnWeek"],
                schoolYear = learnWeek["schoolYear"],
                semester = learnWeek["semester"]
            };
            //发送请求获取所有课程信息
            var ClassJSONbody_ALL = new StringContent(System.Text.Json.JsonSerializer.Serialize(ClassPostData_ALL), System.Text.Encoding.UTF8, "application/json");
            var classInfoResponse_ALL = await _client.PostAsync("https://zhss.sdtbu.edu.cn/tp_up/up/widgets/getClassbyUserInfo", ClassJSONbody_ALL);
            var classInfoData_ALL = await classInfoResponse_ALL.Content.ReadAsStringAsync();
            //构造获取当前上课周课程信息请求体
            var ClassPostData_NOW = new
            {
                learnWeek = learnWeek["learnWeek"],
                schoolYear = learnWeek["schoolYear"],
                semester = learnWeek["semester"],
                classList = JArray.Parse(classInfoData_ALL)
            };
            //发送请求获取当前上课周课程信息
            var ClassJSONbody_NOW = new StringContent(JsonConvert.SerializeObject(ClassPostData_NOW), System.Text.Encoding.UTF8, "application/json");
            var classInfoResponse_NOW = await _client.PostAsync("https://zhss.sdtbu.edu.cn/tp_up/up/widgets/getClassbyTime", ClassJSONbody_NOW);
            var classInfoData_NOW = await classInfoResponse_NOW.Content.ReadAsStringAsync();
            //返回课程信息列表JSON数组
            JArray classList = JArray.Parse(classInfoData_NOW);
            return classList;
            //throw new NotImplementedException("MyMethod is not implemented yet.");
            //return classInfoData2 ?? new List<Dictionary<string, string>>();
        }


        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns>用户信息字典</returns>
        public async Task<Dictionary<string, string>> GetUserInfo()
        {
            //如果用户信息不为空，直接返回
            if (_userInfo != null)
            {
                return _userInfo;
            }

            //构造请求体
            var postData = new
            {
                BE_OPT_ID = _jsEngine.Invoke("strEnc", _username, "tp", "des", "param").AsString()
            };
            try
            {
                //发送请求
                var jsonContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(postData), System.Text.Encoding.UTF8, "application/json");
                //添加请求头
                var request = new HttpRequestMessage(HttpMethod.Post, "https://zhss.sdtbu.edu.cn/tp_up/sys/uacm/profile/getUserInfo")
                {
                    Content = jsonContent
                };

                var response = await _client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();

                _userInfo = new Dictionary<string, string>
                {
                    { "学号", result["ID_NUMBER"] },
                    { "姓名", result["USER_NAME"] },
                    { "性别", result["USER_SEX"] },
                    { "学院", result["UNIT_NAME"] }
                };
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unexpected error: {e.Message}");
            }

            return _userInfo;
        }

        /// <summary>
        /// 登录方法，获取登录状态
        /// </summary>
        /// <returns>登录是否成功</returns>
        private async Task<bool> Login()
        {
            var response = await _client.GetAsync("https://cas.sdtbu.edu.cn/cas/login");//获取登录页面
            var content = await response.Content.ReadAsStringAsync();//获取页面内容
            var parser = new HtmlParser();
            var document = parser.ParseDocument(content);//解析页面内容

            var lt = document.QuerySelector("#lt")?.GetAttribute("value");//获取lt_lt位于页面中的隐藏input标签中
            if (lt == null)
            {
                return false;
            }

            var rsa = _jsEngine.Invoke("strEnc", $"{_username}{_password}{lt}", "1", "2", "3").AsString();//调用js加密函数
            var postData = new Dictionary<string, string>
                {
                    { "rsa", rsa },
                    { "ul", _username.Length.ToString() },
                    { "pl", _password.Length.ToString() },
                    { "lt", lt },
                    { "execution", "e1s1" },
                    { "_eventId", "submit" }
                };

            //发送登录请求
            var loginResponse = await _client.PostAsync("https://cas.sdtbu.edu.cn/cas/login?service=https://zhss.sdtbu.edu.cn/tp_up/", new FormUrlEncodedContent(postData));
            //判断是否登录成功
            if (!loginResponse.Headers.Contains("Set-Cookie"))
            {
                return false;
            }
            //重定向到智慧山商主页获取cookie
            var ForwaRedict = await _client.GetAsync("https://zhss.sdtbu.edu.cn/tp_up/");

            _loginCookie = loginResponse;//无用的登录cookie，暂时保留

            return true;
        }

        /// <summary>
        /// 获取登录状态
        /// </summary>
        /// <returns>是否登录</returns>
        public bool IsLogin()
        {
            return _loginStatus;
        }

        /// <summary>
        /// 释放HttpClient资源
        /// </summary>
        public void Dispose()
        {
            _client.Dispose();
        }


    }
}

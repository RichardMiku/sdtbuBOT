using Microsoft.AspNetCore.Mvc;

namespace sdtbuBOT.strFunc
{
    public class strMenu
    {
        /// <summary>
        /// 主菜单
        /// </summary>
        /// <returns></returns>
        public static string MENU_Index()
        {
            string menTXT = "🤖菜单🤖\n" +
                "🚀智慧山商🚀\n" +
                "👋一言(不可用)👋\n" +
                "❔功能信息(不可用)❔";
            return menTXT;
        }

        /// <summary>
        /// 智慧山商菜单
        /// </summary>
        /// <returns></returns>
        public static string MENU_sdtbu()
        {
            string sdtbumenu = "🚀智慧山商🚀\n" +
                "ℹ个人信息ℹ\n" +
                "💯成绩查询(不可用)💯\n" +
                "📖课表查询📖\n" +
                "📖下节课📖\n" +
                "💬账号绑定(不可用)💬";
            return sdtbumenu;
        }

        /// <summary>
        /// 功能预开发列表
        /// </summary>
        /// <returns></returns>
        public static string FuncDEVINFO()
        {
            string FuncMenu = "🛠️功能实现🛠️\n" +
                "✅主菜单\n" +
                "✅一言\n" +
                "✅智慧山商菜单\n" +
                "✅个人信息\n" +
                "❎成绩查询\n" +
                "✅课表查询\n" +
                "❎课程提醒\n" +
                "====\n" +
                "✅已实现功能\n" +
                "❎未实现功能";
            return FuncMenu;
        }

        public static string INFObindMSG()
        {
            string FuncMenu = "💬信息绑定方法：\n" +
                "按照以下格式向机器人发送消息：\n" +
                "💬信息绑定 您的账号 您的密码💬\n" +
                "例如 信息绑定 202xxxxx abc123";
            return FuncMenu;
        }

    }
}

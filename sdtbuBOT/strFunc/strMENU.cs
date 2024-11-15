using Microsoft.AspNetCore.Mvc;

namespace sdtbuBOT.strFunc
{
    public class strMenu
    {
        /// <summary>
        /// 机器人版本
        /// </summary>
        public static string INFO_BOTVERSION = "v1.0.2";

        /// <summary>
        /// 使用说明
        /// </summary>
        /// <returns></returns>
        public static string MENU_HELP()
        {
            string helpTXT = "🤖使用说明🤖\n" +
                "发送“菜单”查看机器人功能，发送与功能相关消息即可开始使用！\n" +
                "\n" +
                "🤖常见问题🤖：\n" +
                "❔Q:为什么需要智慧山商密码？\n" +
                "A:因为机器人的|智慧山商|相关内容从校园网智慧山商中实时获取，机器人使用您的账号登录为您获取信息。\n" +
                "\n" +
                "❔Q:为什么不使用扫码或验证码登录？\n" +
                "A:机器人每次获取信息都是新的登录，其他用户发送查询指令时，您的登录将会被退出，若您希望再次使用机器人将需要重新登录，这将大大降低机器人效率。\n" +
                "\n" +
                "❔Q:我的密码安全如何保证？\n" +
                "A:机器人在后台用数据库存储您的密码，机器人并不会泄露您的密码，且密码不可从外部取得。当然，在进行账号绑定时，建议您优先选择私聊机器人。\n" +
                "\n" +
                "❔Q:如何进行机器人绑定？\n" +
                "A:请您在聊天框中编辑“信息绑定 <您的学号> <您的密码>”并向机器人发送，若机器人返回您的信息即绑定成功。在输入时请注意使用空格将消息隔开，示例\n“信息绑定 2023123456 Abc1234”\n" +
                "\n" +
                "❔Q:为什么有时需要重新绑定？\n" +
                "A:因机器人框架原因，机器人在更新或重启后会导致用户唯一识别码更改，您的绑定数据会被清除，因此需要重新绑定。\n" +
                "\n" +
                "©RichardMiku 2024";
            return helpTXT;
        }

        /// <summary>
        /// 主菜单
        /// </summary>
        /// <returns></returns>
        public static string MENU_Index()
        {
            string menTXT = "🤖菜单🤖\n" +
                "🚀智慧山商🚀\n" +
                "👋一言(暂不可用)👋\n" +
                "❔功能设想❔\n" +
                "🤖版本信息🤖\n" +
                "更多帮助请发送：“/使用说明”";
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
                "💯成绩查询(暂不可用)💯\n" +
                "📖课表查询📖\n" +
                "📖下节课📖\n" +
                "💬账号绑定💬";
            return sdtbumenu;
        }

        /// <summary>
        /// 功能预开发列表
        /// </summary>
        /// <returns></returns>
        public static string FuncDEVINFO()
        {
            string FuncMenu = "🛠️功能设想🛠️\n" +
                "✅主菜单\n" +
                "❎一言\n" +
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

        /// <summary>
        /// 如何绑定信息
        /// </summary>
        /// <returns></returns>
        public static string INFObindMSG()
        {
            string FuncMenu = "💬信息绑定方法：\n" +
                "请您按照以下格式私聊向机器人发送消息：\n" +
                "💬信息绑定 您的账号 您的密码💬\n" +
                "例如 “信息绑定 202xxxxx abc123”";
            return FuncMenu;
        }

    }
}

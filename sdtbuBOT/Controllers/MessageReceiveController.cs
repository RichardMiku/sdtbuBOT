using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using sdtbuBOT.strFunc;

namespace sdtbuBOT.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MessageReceiveController : ControllerBase
    {
        // 定义命令处理程序字典
        private readonly Dictionary<string, Func<string, Task<IActionResult>>> commandHandlers;

        public MessageReceiveController()
        {
            commandHandlers = new Dictionary<string, Func<string, Task<IActionResult>>>
            {
                { "个人信息", HandlePersonalInfo },
                { "当前时间", HandleCurrentTime },
                { "智慧山商", HandleSdtbuMenu }
            };
        }

        /// <summary>
        /// 处理接收到的消息
        /// </summary>
        /// <param name="msgReceive">接收到的消息</param>
        /// <returns>处理结果</returns>
        [HttpPost(Name = "msgreceive")]
        public async Task<IActionResult> Post([FromForm] MessageReceive msgReceive)
        {
            JObject jsource = JObject.Parse(msgReceive.Source);//解析来源信息
            string? fromid = (string?)jsource["from"]?["id"];//获取来源id

            if (fromid == null)
            {
                return BadRequest("来源ID不能为空");
            }

            // 检查命令处理程序字典
            if (commandHandlers.TryGetValue(msgReceive.Content, out var handler))
            {
                return await handler(fromid);
            }

            // 功能-智慧山商-下一节课
            if (strCMD.cmd_NextCourse(msgReceive.Content))
            {
                return await HandleNextCourse(fromid);
            }

            // 功能-智慧山商-课表查询
            if (strCMD.cmd_COURSE(msgReceive.Content))
            {
                return await HandleCourse(fromid);
            }

            // 功能-智慧山商-绑定智慧山商
            if (strUINFO.BindRegex(msgReceive.Content))
            {
                return await HandleBindSdtbu(fromid, msgReceive.Content);
            }

            // 菜单-主菜单
            if (strCMD.cmd_MENU(msgReceive.Content))
            {
                return HandleMenu();
            }

            // 加好友请求
            if (msgReceive.Type == "friendship")
            {
                return HandleFriendship();
            }

            // 默认返回值，报告框架消息已处理
            var noneresponse = new
            {
                success = true
            };

            return new JsonResult(noneresponse);
        }

        /// <summary>
        /// 处理个人信息命令
        /// </summary>
        /// <param name="fromid">来源ID</param>
        /// <returns>处理结果</returns>
        private async Task<IActionResult> HandlePersonalInfo(string fromid)
        {
            if (await strUINFO.isBIND(fromid))
            {
                string usrinfo = strUINFO.BINDmsg;
                var response = new
                {
                    success = true,
                    data = new { type = "text", content = usrinfo }
                };

                return new JsonResult(response);
            }
            else
            {
                var response = new
                {
                    success = true,
                    data = new { type = "text", content = strMenu.INFObindMSG() }
                };

                return new JsonResult(response);
            }
        }

        /// <summary>
        /// 处理当前时间命令
        /// </summary>
        /// <param name="fromid">来源ID</param>
        /// <returns>处理结果</returns>
        private async Task<IActionResult> HandleCurrentTime(string fromid)
        {
            await botapi.SendMessage(fromid, DateTime.Now.ToString());
            return Ok();
        }

        /// <summary>
        /// 处理智慧山商菜单命令
        /// </summary>
        /// <param name="fromid">来源ID</param>
        /// <returns>处理结果</returns>
        private async Task<IActionResult> HandleSdtbuMenu(string fromid)
        {
            var response = new
            {
                success = true,
                data = new { type = "text", content = strMenu.MENU_sdtbu() }
            };

            return new JsonResult(response);
        }

        /// <summary>
        /// 处理下一节课命令
        /// </summary>
        /// <param name="fromid">来源ID</param>
        /// <returns>处理结果</returns>
        private async Task<IActionResult> HandleNextCourse(string fromid)
        {
            if (await strUINFO.isBIND(fromid))
            {
                string usrcourse = await strUCOURSE.USRNEXTCOURSE(fromid);
                var response = new
                {
                    success = true,
                    data = new { type = "text", content = usrcourse }
                };

                return new JsonResult(response);
            }
            else
            {
                var response = new
                {
                    success = true,
                    data = new { type = "text", content = strMenu.INFObindMSG() }
                };

                return new JsonResult(response);
            }
        }

        /// <summary>
        /// 处理课表查询命令
        /// </summary>
        /// <param name="fromid">来源ID</param>
        /// <returns>处理结果</returns>
        private async Task<IActionResult> HandleCourse(string fromid)
        {
            if (await strUINFO.isBIND(fromid))
            {
                string usrcourse = await strUCOURSE.USRCOURSE(fromid);
                var response = new
                {
                    success = true,
                    data = new { type = "text", content = usrcourse }
                };

                return new JsonResult(response);
            }
            else
            {
                var response = new
                {
                    success = true,
                    data = new { type = "text", content = strMenu.INFObindMSG() }
                };

                return new JsonResult(response);
            }
        }

        /// <summary>
        /// 处理绑定智慧山商命令
        /// </summary>
        /// <param name="fromid">来源ID</param>
        /// <param name="content">消息内容</param>
        /// <returns>处理结果</returns>
        private async Task<IActionResult> HandleBindSdtbu(string fromid, string content)
        {
            var response = new
            {
                success = true,
                data = new { type = "text", content = await strUINFO.BindSDTBU(fromid, content) }
            };

            return new JsonResult(response);
        }

        /// <summary>
        /// 处理主菜单命令
        /// </summary>
        /// <returns>处理结果</returns>
        private IActionResult HandleMenu()
        {
            var response = new
            {
                success = true,
                data = new { type = "text", content = strMenu.MENU_Index() }
            };

            return new JsonResult(response);
        }

        /// <summary>
        /// 处理加好友请求
        /// </summary>
        /// <returns>处理结果</returns>
        private IActionResult HandleFriendship()
        {
            var response = new
            {
                success = true
            };
            return new JsonResult(response);
        }
    }
}

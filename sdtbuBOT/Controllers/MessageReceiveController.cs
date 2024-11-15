using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using sdtbuBOT.Funny;
using sdtbuBOT.strFunc;

namespace sdtbuBOT.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MessageReceiveController : ControllerBase
    {
        [HttpPost(Name = "msgreceive")]
        public async Task<IActionResult> Post([FromForm] MessageReceive msgReceive)
        {
            JObject jsource = JObject.Parse(msgReceive.Source);//解析来源信息
            string fromid = (string)jsource["from"]["id"];//获取来源id
            //string room = (string)jsource["room"]["id"];//获取群id_

            //功能-娱乐-一言
            if (msgReceive.Content == "一言")
            {
                hitokoto _hitokoto = new hitokoto();
                var response = new
                {
                    success = true,
                    data = new { type = "text", content = await _hitokoto.hitokotoGetAsync() }
                };

                return new JsonResult(response);
            }

            //菜单-版本信息
            if(msgReceive.Content == "版本信息")
            {
                var response = new
                {
                    success = true,
                    data = new { type = "text", content = strMenu.INFO_BOTVERSION }
                };

                return new JsonResult(response);
            }

            //菜单-功能设想
            if (msgReceive.Content == "功能设想")
            {
                var response = new
                {
                    success = true,
                    data = new { type = "text", content = strMenu.FuncDEVINFO() }
                };

                return new JsonResult(response);
            }

            //功能-使用说明
            if (msgReceive.Content == "/使用说明")
            {
                var response = new
                {
                    success = true,
                    data = new { type = "text", content = strMenu.MENU_HELP() }
                };

                return new JsonResult(response);
            }

            //功能-智慧山商-下一节课
            if (strCMD.cmd_NextCourse(msgReceive.Content)) 
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

            //功能-智慧山商-课表查询
            if (strCMD.cmd_COURSE(msgReceive.Content))
            {
                if(await strUINFO.isBIND(fromid))
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

            //功能-智慧山商-个人信息
            if (msgReceive.Content == "个人信息")
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

            //功能-智慧山商-绑定智慧山商
            if (strUINFO.BindRegex(msgReceive.Content))
            {
                var response = new
                {
                    success = true,
                    data = new { type = "text", content =await strUINFO.BindSDTBU(fromid, msgReceive.Content) }
                };

                return new JsonResult(response);
            }


            //菜单-主菜单
            if (strCMD.cmd_MENU(msgReceive.Content)) 
            {
                var response = new
                {
                    success = true,
                    data = new { type = "text", content = strMenu.MENU_Index() }
                };

                return new JsonResult(response);
            }

            //菜单-智慧山商
            if (msgReceive.Content == "智慧山商")
            {
                var response = new
                {
                    success = true,
                    data = new { type = "text", content = strMenu.MENU_sdtbu() }
                };

                return new JsonResult(response);
            }

            //加好友请求
            if(msgReceive.Type== "friendship")
            {
                var response = new
                {
                    success = true
                };
                return new JsonResult(response);
            }

            //测试功能-当前时间
            if (msgReceive.Content == "当前时间")
            {
                await botapi.SendMessage((string)jsource["from"]["id"], DateTime.Now.ToString());
                return Ok();
            }


            //默认返回值，报告框架消息已处理
            var noneresponse = new
            {
                success = true
            };

            return new JsonResult(noneresponse);//返回成功值
        }
    }
}

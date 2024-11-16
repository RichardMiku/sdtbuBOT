using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using sdtbuBOT.Funny;
using sdtbuBOT.strFunc;
using zhipuLib;

namespace sdtbuBOT.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MessageReceiveController : ControllerBase
    {
        [HttpPost(Name = "msgreceive")]
        public async Task<IActionResult> Post([FromForm] MessageReceive msgReceive)
        {
            //JObject jsource = JObject.Parse(msgReceive.Source);//解析来源信息
            //string fromid = (string)jsource["from"]["id"];//获取来源id
            var JsonSource = JsonConvert.DeserializeObject<dynamic>(msgReceive.Source);//动态解析来源信息为对象
            string fromid = JsonSource.from.id;//获取来源id

            //加好友请求
            if (msgReceive.Type == "friendship")
            {
                var response = new
                {
                    success = true
                };
                return new JsonResult(response);
            }

            switch (msgReceive.Content)
            {
                //菜单-GPT说明
                case "GPT说明":
                    {
                        var response = new
                        {
                            success = true,
                            data = new { type = "text", content = strMenu.MENU_GPT() }
                        };

                        return new JsonResult(response);
                    }
                //功能-智慧山商-下一节课
                case "下节课":
                    {
                        return await HandleNextCourse(fromid);
                    }
                //功能-智慧山商-课表查询
                case "课表查询":
                    {
                        return await HandleCourseQuery(fromid);
                    }
                //菜单-主菜单
                case "菜单":
                    {
                        var response = new
                        {
                            success = true,
                            data = new { type = "text", content = strMenu.MENU_Index() }
                        };

                        return new JsonResult(response);
                    }
                //功能-娱乐-一言
                case "一言":
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
                case "版本信息":
                    {
                        var response = new
                        {
                            success = true,
                            data = new { type = "text", content = strMenu.INFO_BOTVERSION }
                        };

                        return new JsonResult(response);
                    }
                //菜单-功能设想
                case "功能设想":
                    {
                        var response = new
                        {
                            success = true,
                            data = new { type = "text", content = strMenu.FuncDEVINFO() }
                        };

                        return new JsonResult(response);
                    }
                //菜单-使用说明
                case "/使用说明":
                    {
                        var response = new
                        {
                            success = true,
                            data = new { type = "text", content = strMenu.MENU_HELP() }
                        };

                        return new JsonResult(response);
                    }
                //功能-智慧山商-个人信息
                case "个人信息":
                    {
                        return await HandlePersonalInfo(fromid);
                    }
                //菜单-智慧山商
                case "智慧山商":
                    {
                        var response = new
                        {
                            success = true,
                            data = new { type = "text", content = strMenu.MENU_sdtbu() }
                        };

                        return new JsonResult(response);
                    }
                case "当前时间":
                    {
                        await botapi.SendMessage(JsonSource.from.id, DateTime.Now.ToString());
                        var noneresponse = new
                        {
                            success = true
                        };

                        return new JsonResult(noneresponse);//返回成功值
                    }
                default:
                    {
                        //功能-智普清言AI-私聊回复
                        if (JsonSource.room.id == null) 
                        {
                            var response = new
                            {
                                success = true,
                                data = new { type = "text", content = await api_AI.zhipu_ReadContent(msgReceive.Content ?? "你好") }
                            };

                            return new JsonResult(response);
                        }
                        //功能-智普清言AI-被@时回复
                        if (msgReceive.IsMentioned == "1")
                        {
                            var response = new
                            {
                                success = true,
                                data = new { type = "text", content = await api_AI.zhipu_ReadContent(msgReceive.Content ?? "你好") }
                            };

                            return new JsonResult(response);
                        }

                        //功能-智慧山商-绑定智慧山商
                        if (strUINFO.BindRegex(msgReceive.Content))
                        {
                            var response = new
                            {
                                success = true,
                                data = new { type = "text", content = await strUINFO.BindSDTBU(fromid, msgReceive.Content) }
                            };

                            return new JsonResult(response);
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

        /// <summary>
        /// 消息处理-智慧山商-下一节课
        /// </summary>
        /// <param name="fromid">来源id</param>
        /// <returns></returns>
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
        /// 消息处理-智慧山商-课表查询
        /// </summary>
        /// <param name="fromid">来源id</param>
        /// <returns></returns>
        private async Task<IActionResult> HandleCourseQuery(string fromid)
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
        /// 消息处理-智慧山商-个人信息
        /// </summary>
        /// <param name="fromid"></param>
        /// <returns></returns>
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
    }
}

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
        [HttpPost(Name = "msgreceive")]
        public async Task<IActionResult> Post([FromForm] MessageReceive msgReceive)
        {
            JObject jsource = JObject.Parse(msgReceive.Source);

            //菜单-绑定智慧山商
            if(strUINFO.BindRegex(msgReceive.Content))
            {
                strUINFO.BindSDTBU((string)jsource["from"]["id"], msgReceive.Content);
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

            if (msgReceive.Content == "测试")
            {
                await botapi.SendMessage((string)jsource["from"]["id"],"测试成功");
            }

            return Ok();
        }
    }
}

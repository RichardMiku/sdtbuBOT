using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

            return Ok();
        }
    }
}

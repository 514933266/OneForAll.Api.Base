using Microsoft.AspNetCore.Mvc;

namespace Base.Host.Controllers
{
    /// <summary>
    /// 项目启动
    /// </summary>
    [Route("api/[controller]")]
    public class StartupsController : Controller
    {
        public StartupsController()
        {
        }

        [HttpGet]
        public string Get()
        {
            return "项目启动成功...";
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using UV_Backend.Logic.Uv;
using System.Threading.Tasks;

namespace UV_Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UVController : Controller
    {
        private readonly UVLogic _uvLogic;
        public UVController(UVLogic uvLogic)
        {
            _uvLogic = uvLogic;
        }
        [HttpGet("[action]")]
        public async Task<JsonResult> FillUVForecast()
        {
            return Json(await _uvLogic.RegisterUVForecast());
        }
    }
}

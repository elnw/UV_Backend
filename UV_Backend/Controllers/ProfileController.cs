using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using UV_Backend.Entities.Requests.Profile;
using UV_Backend.Entities.Responses.Profile;
using UV_Backend.Logic.Profile;
using UV_Backend.Logic.Uv;
using UV_Backend.Models;

namespace UV_Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProfileController : Controller
    {
        private readonly ProfileLogic _profileLogic;
        private readonly UVLogic _uvLogic;

        public ProfileController(ProfileLogic profileLogic, UVLogic uvLogic)
        {
            _profileLogic = profileLogic;
            _uvLogic = uvLogic;
        }


        [HttpGet("[action]")]
        public async Task<JsonResult> GetQuestionary()
        {
            return Json(await _profileLogic.RetrieveQuestionary());
        }

        [HttpGet("[action]")]
        public async Task<JsonResult> GetProfilesByUSer([FromQuery] string userId)
        {
            return Json(await _profileLogic.RetrieveProfilesByUser(userId));
        }

        [HttpPost("[action]")]
        public async Task<JsonResult> CreateProfile([FromBody] CreateProfileRequest createProfileRequest)
        {
            return Json(await _profileLogic.CreateProfile(createProfileRequest));
        }

        [HttpPatch("[action]")]
        public async Task<JsonResult> UpdateProfile([FromBody] Profile profile)
        {
            return Json(await _profileLogic.UpdateProfile(profile));
        }

        [HttpDelete("[action]")]
        public async Task<JsonResult> DeleteProfile([FromQuery] int profileId)
        {
            return Json(await _profileLogic.DeleteProfile(profileId));
        }

        [HttpGet("[action]")]
        public async Task<JsonResult> GetExtraInfoByProfile([FromQuery] int profileId, float? input_uvi)
        {
            float uvi = input_uvi ?? await _uvLogic.GetCurrentUVI();
            return Json(new ProfileInfoResponse
            {
                ExposureTime = await _uvLogic.CalculateExposureTime(await _profileLogic.RetrieveScoreDescriptionByProfile(profileId), uvi),
                uvi = uvi,
                falseAdvices = new List<string>{"No puedes sufrir quemaduras en un dia nublado",
                    "No puedes sufrir quemaduras dentro del agua", 
                    "La radiación UV durante el invierno no es peligrosa", 
                    "Si no sientes los rayos solares no sufriras quemaduras"}
,               truthAdvices = new List<string>{"Hasta el 80% de la radiación UV puede penetrar las nubes. La neblina en la atmosfera incluso puede incrementar la exposición a la radiación UV", 
                                "El agua ofrece protección minma ante la radiación UV y el reflejo puede aumentar dicha exposición a la radiación",
                                "La radiación UV es mas baja durante el invierno, pero la nieve puede rflejar el doble de la exposición total, especialmente en altas altitudes. Presta atención cuando en la primavera la temperatura es baja pero los rayos son fuertes",
                                "Las quemaduras solares son causas por la radiación UV, la cual no se puede sentir. La sensación de calor es causada por la radiación infraroja del sol y no por la radiación UV"
                               
                },
                fps = await _uvLogic.CalculateRecommendedFPS(await _profileLogic.RetrieveScoreDescriptionByProfile(profileId))
            });
        }

    }
}

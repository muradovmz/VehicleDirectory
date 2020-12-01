
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace API.Controllers
{
    public class CheckLocalizationController:BaseApiController
    {
        private readonly IStringLocalizer<CheckLocalizationController> _localizer;

        public CheckLocalizationController(IStringLocalizer<CheckLocalizationController> localizer)
        {
            _localizer = localizer;
        }

        [HttpGet]
        public string Get()
        {
            var ragaca = _localizer["WelcomeMsg"].Value;
            return ragaca;
        }
    }
}
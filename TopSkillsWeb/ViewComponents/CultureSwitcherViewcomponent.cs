using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TopSkillsWeb.Models;

namespace TopSkillsWeb.ViewComponents
{
    public class CultureSwitcherViewcomponent : ViewComponent
    {
        private readonly IOptions<RequestLocalizationOptions> localizationOptions;
        public CultureSwitcherViewcomponent(IOptions<RequestLocalizationOptions> localizationOptions) =>
            this.localizationOptions = localizationOptions;

        public IViewComponentResult Invoke()
        {
            var cultureFeature = HttpContext.Features.Get<IRequestCultureFeature>();
            var model = new CultureSwitcherModel
            {
                SupportedCultures = localizationOptions.Value.SupportedUICultures.ToList(),
                CurrentUICulture = cultureFeature.RequestCulture.UICulture
            };
            return View(model);
        }
    }
}

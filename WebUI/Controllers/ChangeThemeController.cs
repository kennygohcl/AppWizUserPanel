using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dFrontierAppWizard.WebUI.Controllers
{
    public class ChangeThemeController : Controller
    {
        private const string DefaultTheme = "gui2";

        private const string CookieName = "appwizardtheme";

        //theme, jqueryUiTheme
        readonly IDictionary<string, string> themes = new Dictionary<string, string>
            {
                {"gui","base"},
                {"gui2", "base"}, 
                {"gui3", "start"}, 
                {"compact", "base"}, 
                {"start","start"}, 
                {"black-tie","black-tie"}, 
            };

        public ActionResult Index()
        {
            var currentTheme = DefaultTheme;

            if (Request.Cookies[CookieName] != null)
                currentTheme = Request.Cookies[CookieName].Value;

            return View(themes.Select(theme => new SelectListItem
            {
                Text = theme.Key,
                Value = theme.Key + "|" + theme.Value,
                Selected = theme.Key == currentTheme
            }));
        }

        [HttpPost]
        public ActionResult Change(string s)
        {
            Response.Cookies.Add(new HttpCookie(CookieName, s) { Expires = DateTime.Now.AddDays(30) });
            return new EmptyResult();
        }

        public string GetTheme()
        {
            var s = DefaultTheme;
            if (Request.Cookies[CookieName] != null && themes.ContainsKey(Request.Cookies[CookieName].Value))
            {
                s = Request.Cookies[CookieName].Value;
            }

            return s;
        }

        public string GetJqTheme()
        {
            var s = DefaultTheme;
            if (Request.Cookies[CookieName] != null && themes.ContainsKey(Request.Cookies[CookieName].Value))
            {
                s = Request.Cookies[CookieName].Value;
            }

            return themes[s];
        }
    }
}
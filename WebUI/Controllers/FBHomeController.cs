using System.Collections.Generic;
using System.Web.Mvc;
using Facebook;


namespace WebUI1.Controllers
{
    public class FBHomeController : Controller
    {
        private readonly FacebookClient _fb;
       
        public FBHomeController()
            : this(new FacebookClient())
        {
        }

        public FBHomeController(FacebookClient fb)
        {
              _fb = fb;
             
        }

        public ActionResult Index()
        {

            return View();

        }

     


        [FacebookAuthorize]
        public ActionResult FacebookAccessTokens()
        {
            try
            {
                ViewBag.access_token = _fb.AccessToken;

                dynamic accounts = _fb.Get("me/accounts");
                ViewBag.accounts = accounts;
            }
            catch (FacebookOAuthException)
            {
                // log exception
                return new HttpUnauthorizedResult();
            }

            return View();
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            // it would be better to set the _fb.AccessToken using your favoirte IoC
            _fb.AccessToken = Session["fb_access_token"] as string;
            
           
        }
    }
}



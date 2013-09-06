// using System;
// using System.Collections.Generic;
// using System.Text;
// using System.Web.Mvc;
// using Facebook;
// using System.Web;
//using System.Diagnostics.Contracts;


//using WebUI1.Models;
//using System.Web.Security;

//using System.Linq;
//using Omu.AwesomeMvc;
//using Bartlers.Core.Model;

//using Newtonsoft.Json.Linq;
//using System.Net;

//namespace WebUI1.Controllers
//{
   

//    public class FBAccountController : Controller
//    {
//       private const string AppId = "281645771915615";
//       private const string Appsecret = "efeb1474c160da80d78bd7b3174dfc57";


//        private const string Scope = "user_about_me,user_birthday,email,user_status,publish_stream,manage_pages";
//        private const string RedirectUri = "https://lahabra.pworklink.com/bartlers/fbAccount/FacebookCallback";
//        private readonly FacebookClient _fb;

//        public FBAccountController()
//            : this(new FacebookClient())
//        {
//        }

//        public FBAccountController(FacebookClient fb)
//        {

//            _fb = fb;
//        }

//        public ActionResult LogOn(string returnUrl)
//        {
//            var csrfToken = Guid.NewGuid().ToString();
//            Session["fb_csrf_token"] = csrfToken;

//            var state = Convert.ToBase64String(Encoding.UTF8.GetBytes(_fb.SerializeJson(new { returnUrl = returnUrl, csrf = csrfToken })));

//            var fbLoginUrl = _fb.GetLoginUrl(
//                new
//                    {
//                        client_id = AppId,
//                        client_secret = Appsecret,
//                        redirect_uri = RedirectUri,
//                        response_type = "code",
//                        scope = Scope,
//                        state = state
//                    });

//             return Redirect(fbLoginUrl.AbsoluteUri);  
//        }

//        public ActionResult FacebookCallback(string code, string state)
//        {
//            if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(state))
//                return RedirectToAction("Index", "Home");

//            // first validate the csrf token
//            dynamic decodedState;
//            try
//            {
//                decodedState = _fb.DeserializeJson(Encoding.UTF8.GetString(Convert.FromBase64String(state)), null);
//                var exepectedCsrfToken = Session["fb_csrf_token"] as string;
//                // make the fb_csrf_token invalid
//                Session["fb_csrf_token"] = null;

//                if (!(decodedState is IDictionary<string, object>) || !decodedState.ContainsKey("csrf") || string.IsNullOrWhiteSpace(exepectedCsrfToken) || exepectedCsrfToken != decodedState.csrf)
//                {
//                    return RedirectToAction("Index", "Home");
//                }
//            }
//            catch
//            {
//                // log exception
//                return RedirectToAction("Index", "Home");
//            }

//            try
//            {
//                dynamic result = _fb.Post("oauth/access_token",
//                                          new
//                                              {
//                                                  client_id = AppId,
//                                                  client_secret = Appsecret,
//                                                  redirect_uri = RedirectUri,
//                                                  code = code,
//                                                  scope = Scope,
//                                                  state = state
//                                              });

//                Session["fb_access_token"] = result.access_token;
               
//                if (result.ContainsKey("expires"))
//                    Session["fb_expires_in"] = DateTime.Now.AddSeconds(result.expires);

//                if (decodedState.ContainsKey("returnUrl"))
//                {
//                    if (Url.IsLocalUrl(decodedState.returnUrl))
//                        return Redirect(decodedState.returnUrl);
//                }

//                var request = WebRequest.Create("https://graph.facebook.com/me?fields=email,name,first_name&access_token=" + Uri.EscapeDataString(result.access_token));
//                using (var response = request.GetResponse())
//                {
//                    using (var responseStream = response.GetResponseStream())
//                    {
//                        System.IO.StreamReader streamReader = new System.IO.StreamReader(responseStream, true);
//                        string MyStr = streamReader.ReadToEnd();
//                        JObject userInfo = JObject.Parse(MyStr);

                    
//                        IssueFormsAuthenticationTicket((string)userInfo["name"], (string)userInfo["email"], (string)userInfo["id"], (string)userInfo["first_name"]);
//                    }
//                }


                
               
//                return RedirectToAction("Index", "Home");
//            }
//            catch
//            {
//                // log exception
//                return RedirectToAction("Index", "Home");
//            }
//        }


//        #region IssueFormsAuthenticationTicket
//        /// <summary>
//        /// Issues the FormsAuthenticationTicket to let ASP.NET know that a user is logged in.
//        /// </summary>
//        /// <param name="userName">User that has logged in.</param>
//        private void IssueFormsAuthenticationTicket(string userName, string email, string id, string first_name)
//        {
//            // We need to make a FormsAuthenticationTicket.
//            // To store UserInfo data in it we use the 2nd overload.
//            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
//                1,
//               userName,
//                DateTime.Now,
//                DateTime.Now.AddDays(14),
//                true,
//                FromUser(userName,  email,  first_name).ToString());

//            // Now we encrypt the ticket so no one can read it...
//            string encTicket = FormsAuthentication.Encrypt(ticket);

//            // ...make a cookie and add it. ASP.NET will now know that our user is logged in.
//            Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
//        }
//        #endregion

//        /// <summary>
//        /// Creates a UserInfo class from the User class.
//        /// </summary>
//        /// <param name="openIdUser">User who's data is used to create the UserInfo.</param>
//        /// <returns>UserInfo created from User.</returns>
//        public static UserInfo FromUser(string userName, string email,  string first_name)
//        {
//            //Contract.Requires<ArgumentNullException>(openIdUser != null);

//            return new UserInfo
//            {
//                LoginName = first_name,
//                Email = email,
//                FullName = userName,

//            };
//        }

//    }
//}


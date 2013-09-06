using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace dFrontierAppWizard.WebUI.Controllers
{
    public class DoggyController : Controller
    {
        public class Message
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }

        public IEnumerable<Message> Specific = new[]
                                                   {
                                                       new Message{Key = "promotionindex", Value = "click on <u>add promotion</u> to add a promotion"},
                                                       new Message{Key = "productindex", Value = "to change the picture click on <u>picture</u> link under the picture"},
                                                       new Message{Key = "promotionindex", Value = "you can search for promotions from the 'Search for a promotion' box"},
                                                       new Message{Key = "promotionindex", Value = "to get more results click on the 'more' button on the bottom"},
                                                       new Message{Key = "promotionindex", Value = "you can delete any promotion by clicking on the 'X' button"},
                                                       new Message{Key = "promotionindex", Value = "you can select multiple products by using up/down buttons or with drag and drop"},
                                                       new Message{Key = "productindex", Value = "you can see more products if you click on the 'more' button on the bottom"},
                                                       new Message{Key = "productindex", Value = "you can delete any product by clicking on the 'X' button"},
                                                   };

        public string[] Generic = new[] { 
            "login and you will be able to restore deleted products, and other stuff", 
            "sign in with <br/>login: o <br/> password: 1 <br/> and you will be able to do much more",
            "you can change the UI language from the right top corner",
            "you can change the UI Theme from the right top corner",
            "click on this box to show more hints",
            "click on me to <b>hide/show</b> this box",
            "hovering the red validation bulbs will show the messages",
            "click on the message to show more tips; click on me to hide"
        };
            
        public ActionResult Tell(string c, string a)
        {
            var r = new Random();

            if (r.Next(2) == 1 && Specific.Where(o => o.Key == c + a).Count() > 0)
            {
                var src = Specific.Where(o => o.Key == c + a);
                return Json(new { o = src.ToArray()[r.Next(src.Count())].Value });
            }
            return Json(new { o = Generic[r.Next(Generic.Count())] });
        }
    }
}
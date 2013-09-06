using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Omu.AwesomeMvc;
using cm=dFrontierAppWizard.Core.Model;
using dFrontierAppWizard.Core.Repository;
using dFrontierAppWizard.Core.Service;

namespace dFrontierAppWizard.WebUI.Controllers.Awesome
{
    public class ApplicationsAjaxListController : Controller
    {
        private readonly IRepo<cm.Application> repo;
        private new readonly IUserService userService;
        public ApplicationsAjaxListController(IRepo<cm.Application> repo, IUserService userService)
        {
            this.repo = repo;
            this.userService = userService;
        }

        public ActionResult Search( int page)
        {
            int userId = userService.GetUserIdByName(HttpContext.User.Identity.Name) == 0 ? 0 : userService.GetUserIdByName(HttpContext.User.Identity.Name);
            var list = repo.Where(o => o.UserId.Equals(userId), User.IsInRole("admin")).OrderByDescending(o => o.Id);
            
            return Json(new AjaxListResult
            {
                Content = this.RenderView("ListItems/Application", list.Page(page, 10).ToList()),
                More = list.Count() > page * 10
            });
        }
    }



    public class PromotionsReadOnlyAjaxListController : Controller
    {
        private readonly IRepo<cm.Promotion> repo;
        private new readonly IUserService userService;
        private readonly IRepo<cm.Product> repoProducts;
        public PromotionsReadOnlyAjaxListController(IRepo<cm.Promotion> repo, IUserService userService, IRepo<cm.Product> repoProducts)
        {
            this.repo = repo;
            this.userService = userService;
            this.repoProducts = repoProducts;
        }

        public ActionResult Search(string search, int? productCategory, int[] products, int page)
        {
            var list = repo.Where(o => o.Description.Contains(search), User.IsInRole("admin"));
            int userId = userService.GetUserIdByName(HttpContext.User.Identity.Name) == 0
                             ? 0
                             : userService.GetUserIdByName(HttpContext.User.Identity.Name);
            if (productCategory.HasValue)
            {
                //list = list.Where(o => o.Products.All(b => b.ProductCategoryId.Equals(productCategory)));
                list = list.Where(o => o.Products.All(b => b.ProductCategoryId == productCategory));
                int cn = list.Count();
            }
            if (products != null)
            {
                list = list.Where(o => products.All(m => o.Products.Select(g => g.Id).Contains(m) ));
                list = list.Where(o => products.All(m => o.Products.Select(g => g.Userid).Contains(userId)));
            }
            else
            {
                list = list.Where(o => o.Products.All(m => m.Userid.Equals(userId)));
            }
            int cn1 = list.Count();
            list = list.OrderByDescending(o => o.Id);

            if (productCategory != null)
            {
                ViewBag.CategoryId = productCategory;
            }

                return Json(new AjaxListResult
                    {
                        Content = this.RenderView("ListItems/PromotionReadOnly", list.Page(page, 4).ToList()),
                        More = list.Count() > page*4
                    });
           
        }
    }


    public class PromotionsAjaxListController : Controller
    {
        private readonly IRepo<cm.Promotion> repo;
        private new readonly IUserService userService;
        private readonly IRepo<cm.Product> repoProducts;
        public PromotionsAjaxListController(IRepo<cm.Promotion> repo, IUserService userService, IRepo<cm.Product> repoProducts)
        {
            this.repo = repo;
            this.userService = userService;
            this.repoProducts = repoProducts;
        }

        public ActionResult Search(string search, int? productCategory, int[] products, int page)
        {
            var list = repo.Where(o => o.Description.Contains(search), User.IsInRole("admin"));
            int userId = userService.GetUserIdByName(HttpContext.User.Identity.Name) == 0
                             ? 0
                             : userService.GetUserIdByName(HttpContext.User.Identity.Name);
            if (productCategory.HasValue)
            {
                //list = list.Where(o => o.Products.All(b => b.ProductCategoryId.Equals(productCategory)));
                list = list.Where(o => o.Products.All(b => b.ProductCategoryId == productCategory));
                int cn = list.Count();
            }
            if (products != null)
            {
                list = list.Where(o => products.All(m => o.Products.Select(g => g.Id).Contains(m)));
            }
            else
            {
                list = list.Where(o => o.Products.All(m => m.Userid.Equals(userId)));
            }
            int cn1 = list.Count();
            list = list.OrderByDescending(o => o.Id);

            if (productCategory != null)
            {
                ViewBag.CategoryId = productCategory;
            }

            return Json(new AjaxListResult
            {
                Content = this.RenderView("ListItems/Promotion", list.Page(page, 7).ToList()),
                More = list.Count() > page * 7
            });
        }
    }

  
    public class ProductsAjaxListController : Controller
    {
        private readonly IRepo<cm.Product> repo;
        private new readonly IUserService userService;
        public ProductsAjaxListController(IRepo<cm.Product> repo, IUserService userService)
        {
            this.repo = repo;
            this.userService = userService;
        }

        public ActionResult Search(string search, int? productCategory,  int page)
        {
            int userId =userService.GetUserIdByName(HttpContext.User.Identity.Name) == 0 ? 0: userService.GetUserIdByName(HttpContext.User.Identity.Name);
            var list = repo.Where(o => o.Name.Contains(search) && o.Userid.Equals(userId), User.IsInRole("admin")).OrderByDescending(o => o.Id);

            if (productCategory!=null)
            {
                list = repo.Where(o => o.ProductCategoryId==productCategory).OrderByDescending(o => o.Id);
            }
            return Json(new AjaxListResult
            {
                Content = this.RenderView("ListItems/Product", list.Page(page, 10).ToList()),
                More = list.Count() > page * 10
            });
        }
    }

    public class BranchsAjaxListController : Controller
    {
        private readonly IRepo<cm.Branch> repo;

        public BranchsAjaxListController(IRepo<cm.Branch> repo)
        {
            this.repo = repo;
        }

        public ActionResult Search(string search, int page, bool isTheadEmpty)
        {
            var list = repo.Where(o => (o.BranchName + " " + o.Address).Contains(search), User.IsInRole("admin"))
                .OrderBy(o => o.BranchName);

            var result = new AjaxListResult
                             {
                                 Content = this.RenderView("ListItems/Branch", list.Page(page, 10).ToList()),
                                 More = list.Count() > page * 10
                             };
            if (isTheadEmpty) result.Thead = this.RenderView("ListItems/BranchThead");

            return Json(result);
        }
    }

    public class ProductCategoriesAjaxListController : Controller
    {
        private readonly IRepo<cm.ProductCategory> repo;

        public ProductCategoriesAjaxListController(IRepo<cm.ProductCategory> repo)
        {
            this.repo = repo;
        }

        public ActionResult Search(string search, int page, bool isTheadEmpty)
        {
            var list = repo.Where(o => (o.Description).Contains(search), User.IsInRole("admin"))
                .OrderByDescending(o => o.Id);

            var result = new AjaxListResult
            {
                Content = this.RenderView("ListItems/ProductCategory", list.Page(page, 10).ToList()),
                More = list.Count() > page * 10
            };
            if (isTheadEmpty) result.Thead = this.RenderView("ListItems/ProductCategoryThead");

            return Json(result);
        }
    }
    
    public class CountriesAjaxListController : Controller
    {
        private readonly IRepo<cm.Country> repo;

        public CountriesAjaxListController(IRepo<cm.Country> repo)
        {
            this.repo = repo;
        }

        public ActionResult Search(string search, int page)
        {
            var list = repo.Where(o => o.Name.StartsWith(search), User.IsInRole("admin"))
                .OrderByDescending(o => o.Id);

            var result = new AjaxListResult
                             {
                                 Content = this.RenderView("ListItems/Country", list.Page(page, 10).ToList()),
                                 More = list.Count() > page * 10
                             };

            return Json(result);
        }
    }

    public class UsersAjaxListController : Controller
    {
        private readonly IRepo<cm.User> repo;
        private new readonly IUserService userService;

        public UsersAjaxListController(IRepo<cm.User> repo, IUserService userService)
        {
            this.repo = repo;
            this.userService = userService;
        }

        public ActionResult Search(string search, int page, bool isTheadEmpty, int userTypeId)
        {
            var list = repo.Where(o => o.Login.StartsWith(search) && o.UserTypeId==userTypeId, User.IsInRole("admin")).OrderByDescending(o => o.Id);
            var result = new AjaxListResult
            {
                Content = this.RenderView("ListItems/User", list.Page(page, 10).ToList()),
                More = list.Count() > page * 10
            };
            if (isTheadEmpty) result.Thead = this.RenderView("ListItems/UserThead");

            return Json(result);
        }
    }

    public class RetailersAjaxListController : Controller
    {
        private readonly IRepo<cm.User> repo;
        private new readonly IUserService userService;

        public RetailersAjaxListController(IRepo<cm.User> repo, IUserService userService)
        {
            this.repo = repo;
            this.userService = userService;
        }

        public ActionResult Search(string search, int page, bool isTheadEmpty, int userTypeId)
        {
            var list = repo.Where(o => o.Login.StartsWith(search) && o.UserTypeId == userTypeId, User.IsInRole("admin")).OrderByDescending(o => o.Id);
            var result = new AjaxListResult
            {
                Content = this.RenderView("ListItems/Retailer", list.Page(page, 10).ToList()),
                More = list.Count() > page * 10
            };
            if (isTheadEmpty) result.Thead = this.RenderView("ListItems/RetailerThead");

            return Json(result);
        }
    }


    public class FeedbackAjaxListController : Controller
    {
        private readonly IRepo<cm.Feedback> repo;

        public FeedbackAjaxListController(IRepo<cm.Feedback> repo)
        {
            this.repo = repo;
        }

        public ActionResult Search(string search, int page)
        {
            var list = repo.GetAll().OrderByDescending(o => o.Id);

            var result = new AjaxListResult
            {
                Content = this.RenderView("ListItems/Feedback", list.Page(page, 10).ToList()),
                More = list.Count() > page * 10
            };

            return Json(result);
        }
    }
}
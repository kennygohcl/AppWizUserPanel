using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using cm=dFrontierAppWizard.Core.Model;
using dFrontierAppWizard.Core.Repository;
using dFrontierAppWizard.Core.Service;
using dFrontierAppWizard.Data;
using dFrontierAppWizard.WebUI.Dto;
using dFrontierAppWizard.WebUI.Mappers;
using dFrontierAppWizard.WebUI.Utils;

namespace dFrontierAppWizard.WebUI.Controllers
{
    public class PromotionController : Cruder<cm.Promotion, PromotionInput>
    {
        private new readonly IUserService userService;
        private new readonly IRepo<cm.ProductCategory> categoryService;
        private new readonly IRepo<cm.Product> productService;
        public PromotionController(ICrudService<cm.Promotion> service, IMapper<cm.Promotion, PromotionInput> v, IUserService userService, IRepo<cm.ProductCategory> categoryService, IRepo<cm.Product> productService)
            : base(service, v)
        {
            this.userService = userService;
            this.categoryService = categoryService;
            this.productService = productService;
        }

        public PromotionInput promotionInput = new PromotionInput();
        private readonly ICollection<int> _pd = new Collection<int>();


        //public ActionResult Create(int productId)
        //{

        //}

        public ActionResult CreatePromotion(string description, decimal origPrice, int productId)
        {
            var pd = productService.Where(o=>o.Id==productId).SingleOrDefault();
            _pd.Add(pd.Id);
            promotionInput.Description = description;
            promotionInput.OriginalPrice = origPrice;
            promotionInput.Products = _pd;
            promotionInput.Price = pd.Price;
            promotionInput.Start = DateTime.Now;
            promotionInput.End = DateTime.Now;

            ViewBag.pid = pd.Id;
  
            return View(promotionInput);
            
        }
        private readonly ICollection<cm.Product> _prod = new Collection<cm.Product>();

        [HttpPost]
        public   ActionResult CreatePromotion(string start, string end, string description, decimal price, int discount, int pid)
        {
                DateTime startDate = Convert.ToDateTime(start);
                DateTime endDate = Convert.ToDateTime(end);
                var product = productService.Where(o => o.Id == pid).SingleOrDefault();
                _prod.Add(product);
                var promotion = service.Create(new cm.Promotion
                {
                    Start = startDate,
                    End = endDate,
                    Price = price,
                    DiscountPercentage = discount,
                    Description = description,
                    Products = _prod,
                    IsActive = true,
                    IsPublished = true
                });

                service.Save();
            return View();
           //return RedirectToAction("EShop","Application");
        }

        [HttpPost]
        public ActionResult EditPromotion(PromotionInput promotionInput)
        {
          ////  DateTime startDate = Convert.ToDateTime(start);
          // // DateTime endDate = Convert.ToDateTime(end);
          //  var product = productService.Where(o => o.Id == pid).SingleOrDefault();
          //  _prod.Add(product);
          //  var promotion = service.Create(new Promotion
          //  {
          //      Start = startDate,
          //      End = endDate,
          //      Price = price,
          //      DiscountPercentage = discount,
          //      Description = description,
          //      Products = _prod,
          //      IsActive = true,
          //      IsPublished = true
          //  });
            
          //  service.Save();
          //  Promotion prom = service.Get(pid);
          //   prom.Start = startDate,
          //   prom.End = endDate,
          //   prom.Price = price,
          //   prom.DiscountPercentage = discount,
          //   prom.Description = description,
          //   prom.Products = _prod,
          //   prom.IsActive = true,
          //   prom.IsPublished = true

          //  service.Save();


          // // return View();
            return RedirectToAction("EShop","Application");
        }

        public override ActionResult Index()
        {
            int userId = userService.GetUserIdByName(HttpContext.User.Identity.Name);
            var pcategories = categoryService.Where(o => o.UserId.Equals(userId) && o.IsDeleted == false);
            List<cm.ProductCategory> categories = new List<cm.ProductCategory>();
            foreach (var pp in pcategories)
            {
                cm.ProductCategory pCat = new cm.ProductCategory();
                pCat.Id = pp.Id;
                pCat.Description = pp.Description;
                categories.Add(pCat);
               
            }
            ViewBag.Categories = categories; 

            return View();
        }

        public ActionResult IndexNoHeader()
        {
            int userId = userService.GetUserIdByName(HttpContext.User.Identity.Name);
            var pcategories = categoryService.Where(o => o.UserId.Equals(userId) && o.IsDeleted == false);
            List<cm.ProductCategory> categories = new List<cm.ProductCategory>();
            foreach (var pp in pcategories)
            {
                cm.ProductCategory pCat = new cm.ProductCategory();
                pCat.Id = pp.Id;
                pCat.Description = pp.Description;
                categories.Add(pCat);
               
            }
            ViewBag.Categories = categories; 

            return View();
        }

        
        [HttpPost]
        public string GetPromotionsByRetailerId(int id)
        {

            var cats = categoryService.Where(o => o.UserId == id && o.IsDeleted ==false);

            List<int> catids = new List<int>();
            foreach (var c in cats)
            {
                catids.Add(c.Id);
            }

            var list = service.Where(o => o.Products.All(m => m.Userid == id && m.IsDeleted == false && catids.Contains(m.ProductCategoryId)));

          
            
           // list = service.Where(p => p.Products.All(x => catids.Contains(x.ProductCategoryId)));

          
            var query = from pro in list
                        select new
                        {
                            pro.Id,
                            pro.Description,
                            End = DateConverter.GetJsonDate(pro.End),
                            pro.Price,
                            Start = DateConverter.GetJsonDate(pro.Start),
                            pro.Products.First().Picture,
                            pro.Products.First().Name,
                            pro.Products.First().ProductCategoryId,
                            pro.Products.First().LikesCounter,
                            productId=pro.Products.First().Id,
                            pro.DiscountPercentage
                        };
            var oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string sJson = oSerializer.Serialize(query);
            sJson = @"{""Data"":" + sJson + @",""success"":true}";
            return sJson;

        }

      
       [HttpPost]
        public string GetPromotionsByRetailerIdAndPCategory(int id, int pCat)
        {
            var list = service.Where(o => o.Products.All(m => m.Userid == id && m.ProductCategoryId == pCat && m.IsDeleted == false));
            var query = from pro in list
                        select new
                        {
                            pro.Id,
                            pro.Description,
                            End=DateConverter.GetJsonDate(pro.End),
                            pro.Price,
                            Start= DateConverter.GetJsonDate(pro.Start),
                            pro.Products.First().Picture,
                            pro.Products.First().ProductCategoryId,
                            pro.Products.First().LikesCounter,
                            productId = pro.Products.First().Id
                        };
            var oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string sJson = oSerializer.Serialize(query);
            sJson = @"{""Data"":" + sJson + @",""success"":true}";
            return sJson;
        }
        

        public ActionResult ShowGrid()
        {
            return View();
        }

        protected override string RowViewName
        {
            get { return "ListItems/Promotion"; }
        }
    }

}
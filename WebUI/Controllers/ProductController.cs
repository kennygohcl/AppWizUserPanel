using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Omu.AwesomeMvc;
using dFrontierAppWizard.Core.Model;
using dFrontierAppWizard.Core.Repository;
using dFrontierAppWizard.Core.Service;
using dFrontierAppWizard.Data;
using dFrontierAppWizard.WebUI.Dto;
using dFrontierAppWizard.WebUI.Mappers;
using System.Collections.Generic;

namespace dFrontierAppWizard.WebUI.Controllers
{
    public class ProductController : Cruder<Product, ProductInput>
    {
        private new readonly IProductService service;
        private new readonly IUserService userService;
        private new readonly IApplicationService applicationService;
        private readonly IFileManagerService fileManagerService;
        private new readonly IRepo<ProductCategory> categoryService;
        private new readonly IRepo<Product> productService;
        
        public ProductController(IProductService service, IMapper<Product, ProductInput> v, IFileManagerService fileManagerService, IUserService userService, IApplicationService applicationService, IRepo<ProductCategory> categoryService, IRepo<Product> productService)
            : base(service, v)
        {
            this.service = service;
            this.userService = userService;
            this.fileManagerService = fileManagerService;
            this.applicationService = applicationService;
            this.categoryService = categoryService;
            this.productService = productService;
         
        }

        public override ActionResult Index()
        {
            int userId = userService.GetUserIdByName(HttpContext.User.Identity.Name);
            var pcategories = categoryService.Where(o => o.UserId.Equals(userId) && o.IsDeleted==false);
            List<ProductCategory> categories = new List<ProductCategory>();
            foreach (var pp in pcategories)
            {
                ProductCategory pCat = new ProductCategory();
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
            List<ProductCategory> categories = new List<ProductCategory>();
            foreach (var pp in pcategories)
            {
                ProductCategory pCat = new ProductCategory();
                pCat.Id = pp.Id;
                pCat.Description = pp.Description;
                categories.Add(pCat);

            }
            ViewBag.Categories = categories;

            return View();
        }
        
        

        public ProductInput productInput = new ProductInput();

        public override ActionResult Create()
        {
            return View(productInput);
        }

        [HttpPost]
        [ValidateInput(false)]
        public override ActionResult Create(ProductInput input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }
         
            var product = service.Create(new Product { Name = input.Name, Description = input.Description, ProductCategoryId = input.ProductCategoryId, Userid = userService.GetUserIdByName(HttpContext.User.Identity.Name), Price = input.Price });
            service.Save();

            return Json(new { });

        }

        [HttpPost]
        public string GetProductByRetailerId(int id)
        {
            var list = service.Where(o => o.Userid == id && o.IsDeleted == false).AsEnumerable();
            var query = from pro in list
                        select new
                        {
                            pro.Id,
                            pro.Name,
                            pro.Description,
                            pro.Picture,
                            pro.Price,
                            pro.ProductCategoryId
                        };
            var oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string sJson = oSerializer.Serialize(query);
            sJson = @"{""Data"":" + sJson + @",""success"":true}";
            return sJson;
        }

        [HttpPost]
        public string SetProductLike(int productId)
        {
         
       
      
  
            Product prod = productService.Where(o=>o.Id==productId).First();
            prod.LikesCounter += 1;
            productService.Save();
           
          //  service.SetLike(productId);

           

         /*   db.Users.Attach(updatedUser);
            var entry = db.Entry(updatedUser);
            entry.Property(e => e.Email).IsModified = true;
            // other changed properties
            db.SaveChanges();*/


            var list = service.Where(o => o.Id == productId).AsEnumerable();
            var query = from pro in list
                        select new
                        {
                            pro.LikesCounter
                        };
            var oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string sJson = oSerializer.Serialize(query);
            sJson = @"{""Data"":" + sJson + @",""success"":true}";
            return sJson;
        }


        [HttpPost]
        public string GetProductById(int id)
        {
            var list = service.Where(o => o.Id == id && o.IsDeleted == false).AsEnumerable();
            var query = from pro in list
                        select new
                        {
                            pro.Id,
                            pro.Name,
                            pro.Description,
                            pro.Picture,
                            pro.Price,
                            pro.ProductCategoryId
                        };
            var oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string sJson = oSerializer.Serialize(query);
            sJson = @"{""Data"":" + sJson + @",""success"":true}";
            return sJson;
        }

        protected override string RowViewName
        {
            get { return "ListItems/Product"; }
        }
        
        public ActionResult GetProduct(int id)
        {
            return Json(new { Id = id, Content = this.RenderView(RowViewName, new[] { service.Get(id) }), Type = "product" });
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            int w, h;
            var name = fileManagerService.SaveTempJpeg(Globals.PicturesPath, file.InputStream, out w, out h);
            return Json(new {name, type = file.ContentType, size = file.ContentLength, w, h});
        }

        public ActionResult ChangePicture(int id)
        {
            return View(service.Get(id));
        }

      
        

        [HttpPost]
        public ActionResult Crop(int x, int y, int w, int h, string filename, int id)
        {
            service.SetPicture(id, Globals.PicturesPath, filename, x, y, w, h);
            return Json(new {name = filename});
        }



        #region used only by internet explorer and opera (in header.ascx .cool and .notcool from rows are hidden/showed)

        public ActionResult OChangePicture(int id)
        {
            return View(service.Get(id));
        }

        [HttpPost]
        public ActionResult OChangePicture()
        {
            var file = Request.Files["fileUpload"];
            var id = Convert.ToInt32(Request.Form["id"]);

            if (file.ContentLength > 0)
            {
                int w, h;
                var name = fileManagerService.SaveTempJpeg(Globals.PicturesPath, file.InputStream, out w, out h);
                return RedirectToAction("ocrop",
                                        new CropInput {ImageWidth = w, ImageHeight = h, Id = id, FileName = name});
            }

            return RedirectToAction("Index");
        }

        public ActionResult OCrop(CropInput cropDisplay)
        {
            return View(cropDisplay);
        }

        [HttpPost]
        public ActionResult OCrop(int x, int y, int w, int h, int id, string filename)
        {
            service.SetPicture(id, Globals.PicturesPath, filename, x, y, w, h);
            return RedirectToAction("ochangepicture", new {id});
        }

        #endregion
        
    }
}
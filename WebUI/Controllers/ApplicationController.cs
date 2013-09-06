using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Omu.AwesomeMvc;
using cm=dFrontierAppWizard.Core.Model;
using dFrontierAppWizard.Core.Repository;
using dFrontierAppWizard.Core.Service;
using dFrontierAppWizard.Data;
using dFrontierAppWizard.WebUI.Dto;
using dFrontierAppWizard.WebUI.Mappers;
using dFrontierAppWizard.WebUI.Utils;

namespace dFrontierAppWizard.WebUI.Controllers
{
    public class ApplicationController : Cruder<cm.Application, ApplicationInput>
    {
        private new readonly IRepo<cm.Application> repoApplication;
        private new readonly IRepo<cm.ServiceUpdate> repoServiceUpdate;
        private new readonly IRepo<cm.Module> moduleService;
        private new readonly IRepo<cm.Promotion> promotionService;
        private new readonly IRepo<cm.Product> productService;
        private new readonly IRepo<cm.Branch> branchService;
        private new readonly IRepo<cm.Consumer> consumerService;
        private new readonly IRepo<cm.ProductCategory> categoryService;
        private new readonly IRepo<cm.ConsumerLoyalty> consumerLoyaltyService;
        private new readonly IApplicationService service;
        private new readonly IUserService userService;
        private readonly IFileManagerService fileManagerService;
        private readonly IRepo<cm.ServiceUpdate> serverUpdatesService;
        private new readonly IRepo<cm.MediaDocument> mediaDocumentService;
        public ApplicationController(IApplicationService service, IMapper<cm.Application, ApplicationInput> v, IFileManagerService fileManagerService, IUserService userService, IRepo<cm.ProductCategory> categoryService, IRepo<cm.Module> moduleService, IRepo<cm.Promotion> promotionService, IRepo<cm.Product> productService, IRepo<cm.Branch> branchService, IRepo<cm.ConsumerLoyalty> consumerLoyaltyService, IRepo<cm.ServiceUpdate> serverUpdatesService, IRepo<cm.Application> repoApplication, IRepo<cm.ServiceUpdate> repoServiceUpdate, IRepo<cm.MediaDocument> mediaDocumentService, IRepo<cm.Consumer> consumerService)
            : base(service, v)
        {
            this.service = service;
            this.userService = userService;
            this.fileManagerService = fileManagerService;
            this.categoryService = categoryService;
            this.moduleService = moduleService;
            this.promotionService = promotionService;
            this.productService = productService;
            this.branchService = branchService;
            this.consumerLoyaltyService = consumerLoyaltyService;
            this.serverUpdatesService = serverUpdatesService;
            this.repoApplication = repoApplication;
            this.repoServiceUpdate = repoServiceUpdate;
            this.mediaDocumentService = mediaDocumentService;
            this.consumerService = consumerService;
        }

        
        public ActionResult DataManagementPromo()
        {
            return View();
        }
        
        private readonly ApplicationInput applicationInput = new ApplicationInput();

        private readonly ProductCategoryInput productCategoryInput = new ProductCategoryInput();

        public ActionResult CreateProductCategory()
        {
            return View(productCategoryInput);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateProductCategory(ProductCategoryInput input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            var productCat = categoryService.Insert(new cm.ProductCategory { Description = input.Description, UserId = userService.GetUserIdByName(HttpContext.User.Identity.Name) });
            categoryService.Save();

            return Json(new { });

        }
        
        [HttpPost]
        public string SetConsumerLoyalty(int appId, int consumerId, string loyaltyPassword )
        {
            int loyaltyCount = 0;
            var appliction = service.Where(o => o.Id.Equals(appId)).FirstOrDefault();
            if (appliction!=null)
            {
                if (appliction.LoyaltyPassword==loyaltyPassword)
                {
                    consumerLoyaltyService.Insert(new cm.ConsumerLoyalty()
                    {
                        ApplicationId = appId,
                        ConsumerId = consumerId,
                        DateCreated = DateTime.Now
                    });

                    consumerLoyaltyService.Save();
                    loyaltyCount = consumerLoyaltyService.Where(o => o.ApplicationId == appId && o.ConsumerId == consumerId).Count();
                }
            }

            var oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string sJson = oSerializer.Serialize(loyaltyCount);
            if (loyaltyCount==0)
            {
                sJson = @"{""Data"":" + sJson + @",""success"":false}";
            }
            else
            {
                sJson = @"{""Data"":" + sJson + @",""success"":true}";
            }
            return sJson;
        }

        [HttpPost]
        public ActionResult GetMediaNameById(int id)
        {
            var sv = mediaDocumentService.Get(id);
            if (sv!=null)
            {
                return Json(new { results = sv.DocName });
            }
            else
            {
                return Json(new { results = "" });
            }
        }

        [HttpPost]
        public string GetApplicationById(int appId)
        {
            var appliction = service.Where(o => o.Id.Equals(appId)).AsEnumerable();
            
            var query = from selectedApplication in appliction.AsEnumerable()
                        select new
                        {
                            selectedApplication.Id,
                            selectedApplication.ApplicationName,
                            selectedApplication.UserId,
                            selectedApplication.AppStartUpMediaType,
                            selectedApplication.AppLogo,
                            selectedApplication.AppVideo,
                            selectedApplication.MediaTypeId,
                            selectedApplication.MediaLinkImage,
                            selectedApplication.MediaLinkVideo,
                            selectedApplication.SocialMedia1Link,
                            selectedApplication.SocialMedia1Image,
                            selectedApplication.SocialMedia2Link,
                            selectedApplication.SocialMedia2Image,
                            selectedApplication.SocialMedia3Link,
                            selectedApplication.SocialMedia3Image,
                            selectedApplication.VoucherMessage,
                            selectedApplication.VourcherMessageStart,
                            selectedApplication.VourcherMessageEnd,
                            selectedApplication.VoucherMessageFrequency,
                            selectedApplication.VourhcerMessageTriggerPoint,
                            selectedApplication.TermsAndConditions,
                            selectedApplication.IsLoyaltyVisible,
                            selectedApplication.LoyaltyTermsAndConditions,
                            selectedApplication.LoyaltyGiftImage,
                            selectedApplication.LoyaltyPassword,
                            selectedApplication.TabBackgroundColor,
                            selectedApplication.TabFontColor,
                            selectedApplication.TabFont,
                            selectedApplication.TabFontSize,
                            selectedApplication.AppBackGroundColor,
                            selectedApplication.AppMainTitleFontSize,
                            selectedApplication.AppMainTitleFont,
                            selectedApplication.AppHeaderFontSize,
                            selectedApplication.AppHeaderFont,
                            selectedApplication.AppDataFont,
                            selectedApplication.AppDataFontSize,
                            selectedApplication.AppMainTitleColor,
                            selectedApplication.IsTermandConditionsAgreed
                        };

            var oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string sJson = oSerializer.Serialize(query);
            sJson = @"{""Data"":" + sJson + @",""success"":true}";
            return sJson;
            //return Json(new {sJson});
        }

        [HttpPost]
        public ActionResult UploadFiles()
        {
          var file = Request.Files[0];
          int userId = userService.GetUserIdByName(HttpContext.User.Identity.Name);
          var ms = mediaDocumentService.Where(o => o.DocName == file.FileName).FirstOrDefault();
        
            if (ms!=null)
            {
                ViewBag.ErrorMessage = "File exists";
            }
            else
            {
                if (file != null && file.ContentLength > 0)
                {

                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Globals.PicturesPath, "Retailers\\" + fileName);
                
                    var app = service.Where(o => o.UserId == userId).FirstOrDefault();
                    if (file.ContentType.Contains("video"))
                      {
                          var mds = mediaDocumentService.Insert(new cm.MediaDocument()
                          {
                              ApplicationId = app.Id,
                              DocName = fileName,
                              MediaType = "Video",
                              Module = "RetailerInfo",
                              DateCreated = DateTime.Now
                          });
                          serverUpdatesService.Save();

                        if (app != null)
                          {
                              cm.Application appl = repoApplication.Get(app.Id);
                              appl.MediaLinkVideo = fileName;
                              appl.MediaLinkVideoId = mds.Id;
                              repoApplication.Save();
                          }
                         file.SaveAs(path);

                        
                      }
                    else if (file.ContentType.Contains("image"))
                     {
                         var mds = mediaDocumentService.Insert(new cm.MediaDocument()
                         {
                             ApplicationId = app.Id,
                             DocName = fileName,
                             MediaType = "Image",
                             Module = "RetailerInfo",
                             DateCreated = DateTime.Now
                         });
                         serverUpdatesService.Save();

                         if (app != null)
                         {
                             cm.Application appl = repoApplication.Get(app.Id);
                             appl.MediaLinkImage = fileName;
                             appl.MediaLinkImageId = mds.Id;
                             repoApplication.Save();
                         }
                         file.SaveAs(path);
                        
                     }
                }
            }
            return RedirectToAction("RetailerInfo");
        }


        [HttpPost]
        public ActionResult UploadFilesMainAppLogoOrVideo()
        {
            var file = Request.Files[0];
            int userId = userService.GetUserIdByName(HttpContext.User.Identity.Name);
            var ms = mediaDocumentService.Where(o => o.DocName == file.FileName).FirstOrDefault();

            if (ms != null)
            {
                ViewBag.ErrorMessage = "File exists";
            }
            else
            {
                if (file != null && file.ContentLength > 0)
                {

                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Globals.PicturesPath, "Retailers\\" + fileName);

                    var app = service.Where(o => o.UserId == userId).FirstOrDefault();
                    if (file.ContentType.Contains("video"))
                    {
                        var mds = mediaDocumentService.Insert(new cm.MediaDocument()
                        {
                            ApplicationId = app.Id,
                            DocName = fileName,
                            MediaType = "Video",
                            Module = "Index",
                            DateCreated = DateTime.Now
                        });
                        serverUpdatesService.Save();

                        if (app != null)
                        {
                            cm.Application appl = repoApplication.Get(app.Id);
                            appl.AppVideo = fileName;
                            appl.AppVideoId = mds.Id;
                            appl.AppStartUpMediaType = "Video";
                            repoApplication.Save();
                        }
                        file.SaveAs(path);

                       
                    }
                    else if (file.ContentType.Contains("image"))
                    {
                        var mds = mediaDocumentService.Insert(new cm.MediaDocument()
                        {
                            ApplicationId = app.Id,
                            DocName = fileName,
                            MediaType = "Image",
                            Module = "Index",
                            DateCreated = DateTime.Now
                        });
                        serverUpdatesService.Save();

                        if (app != null)
                        {
                            cm.Application appl = repoApplication.Get(app.Id);
                            appl.AppLogo = fileName;
                            appl.AppLogoId = mds.Id;
                            appl.AppStartUpMediaType = "Video";
                            repoApplication.Save();
                        }
                        file.SaveAs(path);
                      
                    }
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult RemoveFilesMainAppLogoOrVideo(int id)
        {
            int userId = userService.GetUserIdByName(HttpContext.User.Identity.Name);
            var app = service.Where(o => o.UserId == userId).FirstOrDefault();

            cm.MediaDocument doc = mediaDocumentService.Get(id);
            if (doc.ApplicationId == app.Id)
            {
                var fileName = doc.DocName;

                doc.IsDeleted = true;
                mediaDocumentService.Save();

                
                var path = Path.Combine(Globals.PicturesPath, "Retailers\\" + fileName);
                System.IO.File.Delete(path);
            }
            

            if (app != null)
            {
                
                cm.Application appl = repoApplication.Get(app.Id);
                if (appl.AppVideoId == id)
                {
                    appl.AppVideo = "";
                    appl.AppVideoId = 0;
                    repoApplication.Save();
                }
                if (appl.AppLogoId == id)
                {
                    appl.AppLogo = "";
                    appl.AppLogoId= 0;
                    repoApplication.Save();
                }
            }

            return Json("{\"success\":\"1\"}");
        }



        [HttpPost]
        public ActionResult UploadFilesLoyalty()
        {
            var file = Request.Files[0];
            int userId = userService.GetUserIdByName(HttpContext.User.Identity.Name);
            var ms = mediaDocumentService.Where(o => o.DocName == file.FileName).FirstOrDefault();

            if (ms != null)
            {
                ViewBag.ErrorMessage = "File exists";
            }
            else
            {
                if (file != null && file.ContentLength > 0)
                {

                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Globals.PicturesPath, "Retailers\\" + fileName);

                    var app = service.Where(o => o.UserId == userId).FirstOrDefault();

                    var mds = mediaDocumentService.Insert(new cm.MediaDocument()
                        {
                            ApplicationId = app.Id,
                            DocName = fileName,
                            MediaType = "Image",
                            Module = "Loyalty",
                            DateCreated = DateTime.Now
                        });
                        serverUpdatesService.Save();

                        if (app != null)
                        {
                            cm.Application appl = repoApplication.Get(app.Id);
                            appl.LoyaltyGiftImage = fileName;
                            appl.LoyaltyGiftImageId = mds.Id;
                            repoApplication.Save();
                        }
                        file.SaveAs(path);

                    
                }
            }
            return RedirectToAction("Loyalty");
        }


        [HttpPost]
        public ActionResult UploadFilesFeedback()
        {
            var file = Request.Files[0];
            int userId = userService.GetUserIdByName(HttpContext.User.Identity.Name);
            var ms = mediaDocumentService.Where(o => o.DocName == file.FileName).FirstOrDefault();

            if (ms != null)
            {
                ViewBag.ErrorMessage = "File exists";
            }
            else
            {
                if (file != null && file.ContentLength > 0)
                {

                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Globals.PicturesPath, "Retailers\\" + fileName);

                    var app = service.Where(o => o.UserId == userId).FirstOrDefault();

                    var mds = mediaDocumentService.Insert(new cm.MediaDocument()
                    {
                        ApplicationId = app.Id,
                        DocName = fileName,
                        MediaType = "Image",
                        Module = "Feedback",
                        DateCreated = DateTime.Now
                    });
                    serverUpdatesService.Save();

                    if (app != null)
                    {
                        cm.Application appl = repoApplication.Get(app.Id);
                        appl.FeedbackGiftImage = fileName;
                        appl.FeedbackGiftImageId = mds.Id;
                        repoApplication.Save();
                    }
                    file.SaveAs(path);


                }
            }
            return RedirectToAction("Feedback");
        }

       
 

        [HttpPost]
        public string GetWhenLastModifiedByAppId(int appId)
        {
            var whenLastModified = serverUpdatesService.Where(o => o.Id.Equals(appId)).SingleOrDefault();

            if (whenLastModified==null)
            {
                var su = serverUpdatesService.Insert(new cm.ServiceUpdate()
                    {
                        ApplicationId = appId,
                        ProductCategories=DateTime.Now,
                        Products=DateTime.Now,
                        Promotions=DateTime.Now,
                        Consumers=DateTime.Now,
                        ConsumerFeedbacks=DateTime.Now,
                        ConsumerLoyalties=DateTime.Now,
                        Branches = DateTime.Now,
                        LastModifiedAll=DateTime.Now
                    });
                serverUpdatesService.Save();
              
            }
            else
            {
                var whenLastModifiedAll= serverUpdatesService.Where(o => o.Id.Equals(appId)).SingleOrDefault();
                if (whenLastModifiedAll != null)
                {

                    cm.ServiceUpdate sUpdate = repoServiceUpdate.Get(whenLastModifiedAll.Id);
                     sUpdate.Branches = DateTime.Now;
                     sUpdate.ConsumerFeedbacks = DateTime.Now;
                     sUpdate.ConsumerLoyalties = DateTime.Now;
                     sUpdate.Consumers = DateTime.Now;
                     sUpdate.ProductCategories = DateTime.Now;
                     sUpdate.Products = DateTime.Now;
                     sUpdate.Promotions = DateTime.Now;
                     sUpdate.LastModifiedAll = DateTime.Now; 
                     repoServiceUpdate.Save();
                }
            }
    
      var query = from selectedAppModifications in serverUpdatesService.Where(o => o.Id.Equals(appId)).AsEnumerable()
                        select new
                        {
                            ProductCategories =DateConverter.GetJsonDate(selectedAppModifications.ProductCategories),
                            Products= DateConverter.GetJsonDate(selectedAppModifications.Products),
                            Promotions= DateConverter.GetJsonDate(selectedAppModifications.Promotions),
                            Consumers= DateConverter.GetJsonDate(selectedAppModifications.Consumers),
                            ConsumerFeedbacks= DateConverter.GetJsonDate(selectedAppModifications.ConsumerFeedbacks),
                            ConsumerLoyalties=DateConverter.GetJsonDate(selectedAppModifications.ConsumerLoyalties),
                            Branches= DateConverter.GetJsonDate(selectedAppModifications.Branches),
                            LastModifiedAll = DateConverter.GetJsonDate(selectedAppModifications.LastModifiedAll)
                        };

            var oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string sJson = oSerializer.Serialize(query);
            sJson = @"{""Data"":" + sJson + @",""success"":true}";
            return sJson;

            //return Json(new {sJson});
        }

        [HttpPost]
        public string GetWhenLastModifiedByAppIdAndServiceName(int appId, string serviceName)
        {
            var whenLastModified = serverUpdatesService.Where(o => o.Id.Equals(appId)).SingleOrDefault();

            if (whenLastModified == null)
            {
                var su = serverUpdatesService.Insert(new cm.ServiceUpdate()
                {
                    ApplicationId = appId,
                    ProductCategories = DateTime.Now,
                    Products = DateTime.Now,
                    Promotions = DateTime.Now,
                    Consumers = DateTime.Now,
                    ConsumerFeedbacks = DateTime.Now,
                    ConsumerLoyalties = DateTime.Now,
                    Branches = DateTime.Now,
                    LastModifiedAll = DateTime.Now
                });
                serverUpdatesService.Save();

            }

            if (serviceName=="Branches")
            {
                var whenLastModifiedBranch = serverUpdatesService.Where(o => o.Id.Equals(appId)).SingleOrDefault();
                if (whenLastModifiedBranch!=null)
                {
                    cm.ServiceUpdate sUpdate = repoServiceUpdate.Get(whenLastModifiedBranch.Id);
                    sUpdate.Branches = DateTime.Now;
                    repoServiceUpdate.Save();
                }
            }
            else if (serviceName == "ConsumerLoyalties")
            {
                var whenLastModifiedConsumerLoyalties = serverUpdatesService.Where(o => o.Id.Equals(appId)).SingleOrDefault();
                if (whenLastModifiedConsumerLoyalties != null)
                {
                    cm.ServiceUpdate sUpdate = repoServiceUpdate.Get(whenLastModifiedConsumerLoyalties.Id);
                    sUpdate.ConsumerLoyalties = DateTime.Now;
                    repoServiceUpdate.Save();


                }
            }
            else if (serviceName == "ConsumerFeedbacks")
            {
                var whenLastModifiedConsumerFeedbacks = serverUpdatesService.Where(o => o.Id.Equals(appId)).SingleOrDefault();
                if (whenLastModifiedConsumerFeedbacks != null)
                {
                    cm.ServiceUpdate sUpdate = repoServiceUpdate.Get(whenLastModifiedConsumerFeedbacks.Id);
                    sUpdate.ConsumerFeedbacks = DateTime.Now;
                    repoServiceUpdate.Save();
                }
            }
            else if (serviceName == "Consumers")
            {
                var whenLastModifiedConsumers = serverUpdatesService.Where(o => o.Id.Equals(appId)).SingleOrDefault();
                if (whenLastModifiedConsumers != null)
                {
                    cm.ServiceUpdate sUpdate = repoServiceUpdate.Get(whenLastModifiedConsumers.Id);
                    sUpdate.Consumers = DateTime.Now;
                    repoServiceUpdate.Save();
                }
            }
            else if (serviceName == "Promotions")
            {
                var whenLastModifiedPromotions = serverUpdatesService.Where(o => o.Id.Equals(appId)).SingleOrDefault();
                if (whenLastModifiedPromotions != null)
                {
                    cm.ServiceUpdate sUpdate = repoServiceUpdate.Get(whenLastModifiedPromotions.Id);
                    sUpdate.Promotions = DateTime.Now;
                    repoServiceUpdate.Save();
                }
            }
            else if (serviceName == "Products")
            {
                var whenLastModifiedProducts = serverUpdatesService.Where(o => o.Id.Equals(appId)).SingleOrDefault();
                if (whenLastModifiedProducts != null)
                {
                    cm.ServiceUpdate sUpdate = repoServiceUpdate.Get(whenLastModifiedProducts.Id);
                    sUpdate.Products = DateTime.Now;
                    repoServiceUpdate.Save();
                }
            }
            else if (serviceName == "ProductCategories")
            {
                var whenLastModifiedProductCategories = serverUpdatesService.Where(o => o.Id.Equals(appId)).SingleOrDefault();
                if (whenLastModifiedProductCategories != null)
                {
                    cm.ServiceUpdate sUpdate = repoServiceUpdate.Get(whenLastModifiedProductCategories.Id);
                    sUpdate.ProductCategories = DateTime.Now;
                    repoServiceUpdate.Save();
                }
            }

            var whenLastModifiedAll= serverUpdatesService.Where(o => o.Id.Equals(appId)).SingleOrDefault();
            cm.ServiceUpdate sUpdateAll = repoServiceUpdate.Get(whenLastModifiedAll.Id);
            sUpdateAll.LastModifiedAll = DateTime.Now;
            repoServiceUpdate.Save();

            var query = from selectedAppModifications in serverUpdatesService.Where(o => o.Id.Equals(appId)).AsEnumerable()
                        select new
                        {
                            ProductCategories = DateConverter.GetJsonDate(selectedAppModifications.ProductCategories),
                            Products = DateConverter.GetJsonDate(selectedAppModifications.Products),
                            Promotions = DateConverter.GetJsonDate(selectedAppModifications.Promotions),
                            Consumers = DateConverter.GetJsonDate(selectedAppModifications.Consumers),
                            ConsumerFeedbacks = DateConverter.GetJsonDate(selectedAppModifications.ConsumerFeedbacks),
                            ConsumerLoyalties = DateConverter.GetJsonDate(selectedAppModifications.ConsumerLoyalties),
                            Branches = DateConverter.GetJsonDate(selectedAppModifications.Branches),
                            LastModifiedAll = DateConverter.GetJsonDate(selectedAppModifications.LastModifiedAll),
                        };
            
            var oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string sJson = oSerializer.Serialize(query);
            sJson = @"{""Data"":" + sJson + @",""success"":true}";
            return sJson;

            //return Json(new {sJson});
        }

        public ActionResult EShop()
        {
            PopulateEShopViewBags();

            return PartialView();
        }



        private void PopulateEShopViewBags()
        {
             List<ModulesInput> modules = new List<ModulesInput>();

            var modulesList = moduleService.GetAll().OrderBy(o => o.Id);

            foreach (var module in modulesList)
            {
                ModulesInput mInput = new ModulesInput();
                mInput.Code = module.Code;
                mInput.Description = module.Description;
                modules.Add(mInput);
            }
            ViewBag.Modules = modules;

            int userId = userService.GetUserIdByName(HttpContext.User.Identity.Name);
            var pcategories = categoryService.Where(o => o.UserId.Equals(userId));
            List<cm.ProductCategory> categories = new List<cm.ProductCategory>();
            foreach (var pp in pcategories)
            {
                cm.ProductCategory pCat = new cm.ProductCategory();
                pCat.Id = pp.Id;
                pCat.Description = pp.Description;
                categories.Add(pCat);

            }
            ViewBag.Categories = categories; 

            List<String> eshopTabs = new List<String>();

       //    eshopTabs.Add("Categories");
            eshopTabs.Add("Products");
            eshopTabs.Add("HotDeals");

            ViewBag.EshopTabs = eshopTabs; 
            
        }


       
        public ActionResult Feedback()
        {
            int id = userService.GetUserIdByName(HttpContext.User.Identity.Name);
            var app = service.Where(o => o.UserId.Equals(id)).SingleOrDefault();
            if (app != null)
            {
                applicationInput.Id = app.Id;
                applicationInput.FeedbackGiftImage = app.FeedbackGiftImage;
                applicationInput.FeedbackGiftImageId = app.FeedbackGiftImageId;
               
            }
            return View(applicationInput);
           
        }

        [HttpPost]
        public ActionResult Feedback(ApplicationInput input)
        {
            int id = userService.GetUserIdByName(HttpContext.User.Identity.Name);
            var app = service.Where(o => o.UserId.Equals(id)).SingleOrDefault();
            if (app != null)
            {

                cm.Application appl = repoApplication.Get(app.Id);
                appl.FeedbackGiftImage = input.FeedbackGiftImage;
                appl.FeedbackGiftImageId = input.FeedbackGiftImageId;
                repoApplication.Save();

            }
            return View(applicationInput);

        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            int w, h;
           
            if (file.ContentType.Contains("video"))
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath(Globals.PicturesPath), fileName);
                file.SaveAs(path);
                return RedirectToAction("RetailerInfo");
            }
            else {
                 var name = fileManagerService.SaveTempJpeg(Globals.PicturesPath, file.InputStream, out w, out h);
                 return Json(new { name, type = file.ContentType, size = file.ContentLength, w, h });
            }
           
        }

      
        [HttpPost]
        public ActionResult UploadVideo(string fileName1, HttpPostedFileBase file)
        {
            if (file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath(Globals.PicturesPath), fileName);
                file.SaveAs(path);
            }

            return RedirectToAction("RetailerInfo");
        }


        

        public ActionResult HotDeals()
        {
            int userId = userService.GetUserIdByName(HttpContext.User.Identity.Name);
            var pcategories = categoryService.Where(o => o.UserId.Equals(userId));
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

        public override ActionResult Index()
        {
            int id = userService.GetUserIdByName(HttpContext.User.Identity.Name);
            var app = service.Where(o => o.UserId.Equals(id)).SingleOrDefault();
            if (app!=null)
            {
                applicationInput.ApplicationName = app.ApplicationName;
                applicationInput.Id = app.Id;
                Session["appId"] = applicationInput.Id;
                Session["appName"] = applicationInput.ApplicationName;

                applicationInput.UserId = id;
                applicationInput.AppStartUpMediaType = app.AppStartUpMediaType;
                applicationInput.AppLogo = app.AppLogo;
                applicationInput.AppVideo = app.AppVideo;
                applicationInput.AppLogoId = app.AppLogoId;
                applicationInput.AppVideoId = app.AppVideoId;
                applicationInput.TabBackgroundColor = app.TabBackgroundColor;
                applicationInput.TabFontColor = app.TabFontColor;
                applicationInput.TabFont = app.TabFont;
                applicationInput.TabFontSize = app.TabFontSize;
                applicationInput.AppBackGroundColor = app.AppBackGroundColor;
                applicationInput.AppMainTitleFontSize = app.AppMainTitleFontSize;
                applicationInput.AppMainTitleFont = app.AppMainTitleFont;
                applicationInput.AppHeaderFontSize = app.AppHeaderFontSize;
                applicationInput.AppMainTitleColor = app.AppMainTitleColor;
                applicationInput.AppHeaderFont = app.AppHeaderFont;
                applicationInput.AppDataFont = app.AppDataFont;
                applicationInput.AppDataFontSize = app.AppDataFontSize;
                applicationInput.AppButtonFont = app.AppButtonFont;
                applicationInput.AppButtonColor = app.AppButtonColor;
                applicationInput.AppButtonFontSize = app.AppButtonFontSize;
                applicationInput.AppTextBoxColor = app.AppTextBoxColor;
                applicationInput.AppTextBoxFontSize = app.AppTextBoxFontSize;
                applicationInput.AppTextBoxFont = app.AppTextBoxFont;
            }
            


             /* ICollection<FontSize> fontSizes = new List<FontSize>();
              int initialFontSize = 8;
              for (int i = 0; i < 10; i++ )
              {*/
          //        FontSize fs = new FontSize();
           //       fs.Size = "8"; //initialFontSize.ToString();
             //     initialFontSize =initialFontSize + 2;
               //   fontSizes.Add(fs);
            //  }
          //  ViewBag.FontSizes = fs;
              //ViewBag.FontSizes = fontSizes.AsEnumerable(); 

            return View(applicationInput);
        }

        [HttpPost]
        public  ActionResult Index(ApplicationInput input)
        {
            var app = service.Where(o => o.UserId.Equals(input.UserId) && o.Id== input.Id).SingleOrDefault();

            if (app!=null)
            {
                cm.Application appl = repoApplication.Get(app.Id);
                appl.ApplicationName = input.ApplicationName;
                appl.AppMainTitleColor = input.AppMainTitleColor;
                appl.AppMainTitleFont = input.AppMainTitleFont;
                appl.AppMainTitleFontSize = input.AppMainTitleFontSize;
                appl.AppBackGroundColor = input.AppBackGroundColor;
                appl.AppStartUpMediaType = input.AppStartUpMediaType;
                appl.AppLogoId = input.AppLogoId;
                appl.AppVideoId = input.AppVideoId;
                repoApplication.Save();
            }

            return View(input);
           // return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult SetApplicationName(int hiddenApplicationId, string newAppName,
            string appBackGroundColor, int appMainTitleFont, int appMainTitleFontSize, int tabFont,
            int tabFontSize, string tabFontColor, int appHeaderFontSize, int appDataFont,
            int appHeaderFont, int appDataFontSize, string tabBackgroundColor, string appMainTitleColor,
            int appButtonFont, string appButtonColor, int appButtonFontSize, string appTextBoxColor,
            int appTextBoxFontSize, int appTextBoxFont, string appStartUpMediaType,
            int appLogoId, int appVideoId
            )
        {
          

            var app = service.Where(o => o.Id == hiddenApplicationId).FirstOrDefault();

            if (app != null)
            {
                cm.Application appl = repoApplication.Get(app.Id);
                appl.ApplicationName = newAppName;
                appl.AppBackGroundColor= appBackGroundColor;
                appl.AppMainTitleFont = appMainTitleFont;
                appl.AppMainTitleFontSize = appMainTitleFontSize;
                appl.TabFont = tabFont;
                appl.TabFontSize = tabFontSize;
                appl.TabFontColor = tabFontColor;
                appl.AppHeaderFontSize = appHeaderFontSize;
                appl.AppDataFont = appDataFont;
                appl.AppHeaderFont = appHeaderFont;
                appl.AppDataFontSize = appDataFontSize;
                appl.TabBackgroundColor = tabBackgroundColor;
                appl.AppMainTitleColor = appMainTitleColor;
                appl.AppButtonFont = appButtonFont;
                appl.AppButtonColor = appButtonColor;
                appl.AppButtonFontSize = appButtonFontSize;
                appl.AppTextBoxColor = appTextBoxColor;
                appl.AppTextBoxFontSize = appTextBoxFontSize;
                appl.AppTextBoxFont = appTextBoxFont;
                appl.AppStartUpMediaType = appStartUpMediaType;
                appl.AppLogoId = appLogoId;
                appl.AppVideoId = appVideoId;
                appl.AppLogo = service.GetMediaValue(appLogoId);
                appl.AppVideo = service.GetMediaValue(appVideoId);
                repoApplication.Save();
            }
            return Json(new { applicationName = newAppName });
           // return RedirectToAction("Index", "Application");
        }

    
        [HttpPost]
        public ActionResult SetRetailerInfo(int hiddenIdValue, string txtSocialMedia1Link,
            string txtSocialMedia2Link, string txtSocialMedia3Link, string mediaTypeId,
            int mediaLinkImageId, int mediaLinkVideoId)
        {
            var app = service.Where(o => o.Id == hiddenIdValue).FirstOrDefault();


            if (app != null)
            {
                cm.Application appl = repoApplication.Get(app.Id);
                appl.SocialMedia1Link = txtSocialMedia1Link;
                appl.SocialMedia2Link = txtSocialMedia2Link;
                appl.SocialMedia3Link = txtSocialMedia3Link;
                appl.MediaTypeId = mediaTypeId;
                appl.MediaLinkImageId = mediaLinkImageId;
                appl.MediaLinkVideoId = mediaLinkVideoId;
                appl.MediaLinkImage = service.GetMediaValue(mediaLinkImageId); ;
                appl.MediaLinkVideo = service.GetMediaValue(mediaLinkVideoId);
                repoApplication.Save();
            }
            return Json(new { appId = app.Id});
            
        }


        [HttpPost]
        public ActionResult SetLoyalty(int hiddenIdValue, int loyaltyGiftImageId,
            string loyaltyPassword)
        {
            var app = service.Where(o => o.Id == hiddenIdValue).FirstOrDefault();

            if (app != null)
            {
                cm.Application appl = repoApplication.Get(app.Id);
                appl.LoyaltyPassword = loyaltyPassword;
                appl.LoyaltyGiftImageId = loyaltyGiftImageId;
                appl.LoyaltyGiftImage = service.GetMediaValue(loyaltyGiftImageId); 
                repoApplication.Save();
            }
            return Json(new { appId = app.Id });

        }

        [HttpPost]
        public ActionResult SetFeedback(int hiddenIdValue, int feedbackGiftImageId)
        {
             
            var app = service.Where(o => o.Id == hiddenIdValue).FirstOrDefault();

            if (app != null)
            {
                cm.Application appl = repoApplication.Get(app.Id);
                appl.FeedbackGiftImageId = feedbackGiftImageId;
                appl.LoyaltyGiftImage = service.GetMediaValue(feedbackGiftImageId);
                repoApplication.Save();
                

              
            }
            return Json(new { appId = app.Id });

        }


        [HttpPost]
        public ActionResult SetPushNotification(int hiddenIdValue, string voucherMessage, DateTime start, DateTime end, int triggerRadiusId, string apiKey)
        {
            var app = service.Where(o => o.Id == hiddenIdValue).FirstOrDefault();

            if (app != null)
            {
                cm.Application appl = repoApplication.Get(app.Id);
                appl.VoucherMessage = voucherMessage;
                appl.VourcherMessageEnd = end;
                appl.VourcherMessageStart = start;
                appl.ApiKey = apiKey;
                if (triggerRadiusId>0)
                {
                    appl.VourhcerMessageTriggerPoint = service.GetTriggerPoint(triggerRadiusId);
                }
                appl.TriggerRadiusId = triggerRadiusId;
                repoApplication.Save();
            }
            return Json(new { appId = app.Id });

        }
    



        public ActionResult MainScreen()
        {
            List<ModulesInput> modules = new List<ModulesInput>();

            var modulesList = moduleService.GetAll().OrderBy(o=>o.Id);

            foreach (var module in modulesList)
            {
                ModulesInput mInput = new ModulesInput();
                mInput.Code = module.Code;
                mInput.Description = module.Description;
                modules.Add(mInput);
            }
            ViewBag.Modules = modules;

            int userId = userService.GetUserIdByName(HttpContext.User.Identity.Name);
            var pcategories = categoryService.Where(o => o.UserId.Equals(userId));
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

        public ActionResult Payment()
        {
            return View();
        }

        public ActionResult ProductDetailStatic()
        {
            return View();
        }

        public ActionResult ProductDetail(int promoId)
        {
            int userId = userService.GetUserIdByName(HttpContext.User.Identity.Name);
            var promoProducts = promotionService.Where(o => o.Id==promoId).SingleOrDefault();
            string tempProductCategory = null;
            List<cm.Product> products = new List<cm.Product>();

         /*   foreach (var promoProduct in promoProducts)
            {
                foreach (var product in promoProduct.Products)
                {
                    products.Add(product);
                    tempProductCategory = product.ProductCategory.Description;
                }
            }*/
            
           /* var query = from pro in promoProducts
                        select new
                        {
                            pro.Id,
                            pro.Description,
                            pro.End,
                            pro.Price,
                            pro.Start,
                            pro.Products.First().Picture
                        };*/
            
            ViewBag.PromoId = promoId;
            ViewBag.Category = promoProducts.Products.First().ProductCategory.Description;
            ViewBag.Id = promoProducts.Id;
            ViewBag.Description = promoProducts.Description;
            ViewBag.End = promoProducts.End;
            ViewBag.Price = promoProducts.Price;
            ViewBag.Start = promoProducts.Start;
            ViewBag.Picture = promoProducts.Products.First().Picture;
            ViewBag.Name = promoProducts.Products.First().Name;
            return View();
        }

        protected override string RowViewName
        {
            get { return "ListItems/Application"; }
        }

        [HttpPost]
        public ActionResult Crop(int x, int y, int w, int h, string filename, int id)
        {
            service.SetMediaLink(id, Globals.PicturesPath, filename, x, y, w, h);
            return Json(new { name = filename });
        }

        public ActionResult PushNotification()
        {
            int id = userService.GetUserIdByName(HttpContext.User.Identity.Name);
            var app = service.Where(o => o.UserId.Equals(id)).SingleOrDefault();
            if (app != null)
            {
                applicationInput.Id = app.Id;
                applicationInput.MediaTypeId = app.MediaTypeId;
                applicationInput.MediaLinkImage = app.MediaLinkImage;
                if (Session["appId"] == null)
                {
                    Session["appId"] = applicationInput.Id;
                    Session["appName"] = applicationInput.ApplicationName;
                }
                else
                {
                    applicationInput.ApplicationName = Session["appName"].ToString();
                }

                applicationInput.ApplicationName = app.ApplicationName;
                applicationInput.VoucherMessage = app.VoucherMessage;
                applicationInput.VoucherMessageFrequency = app.VoucherMessageFrequency;
                applicationInput.VourcherMessageEnd = app.VourcherMessageEnd;
                applicationInput.VourcherMessageStart = app.VourcherMessageStart;
                applicationInput.VourhcerMessageTriggerPoint = app.VourhcerMessageTriggerPoint;
                applicationInput.ApiKey = app.ApiKey;

            }
            return View(applicationInput);
        }

        [HttpPost]
        public ActionResult PushNotification(ApplicationInput input)
        {
            int id = userService.GetUserIdByName(HttpContext.User.Identity.Name);

            var app = service.Where(o => o.UserId.Equals(input.UserId) && o.Id == input.Id).SingleOrDefault();

            if (app != null)
            {
                cm.Application appl = repoApplication.Get(app.Id);
                appl.MediaTypeId = input.MediaTypeId;
                appl.VoucherMessage = app.VoucherMessage;
                appl.VoucherMessageFrequency = app.VoucherMessageFrequency;
                appl.VourcherMessageEnd = app.VourcherMessageEnd;
                appl.VourcherMessageStart = app.VourcherMessageStart;
                appl.VourhcerMessageTriggerPoint = app.VourhcerMessageTriggerPoint;
                appl.ApiKey = input.ApiKey;
                repoApplication.Save();
                
            }

            var branches = branchService.Where(o => o.UserId.Equals(id) && o.IsDeleted == false);
            List<cm.Branch> branchesList = new List<cm.Branch>();
            foreach (var pp in branches)
            {
                cm.Branch branchRetailerInfo = new cm.Branch();
                branchRetailerInfo.Id = pp.Id;
                branchRetailerInfo.Address = pp.Address;
                branchRetailerInfo.BranchName = pp.BranchName;
                branchRetailerInfo.Country = pp.Country;
                branchesList.Add(branchRetailerInfo);

            }
            ViewBag.Branches = branchesList;
            applicationInput.UserId = id;
            return View(input);
        }


        public ActionResult RetailerInfo()
        {
            int id = userService.GetUserIdByName(HttpContext.User.Identity.Name);
            var app = service.Where(o => o.UserId.Equals(id)).SingleOrDefault();
            if (app != null)
            {
                applicationInput.Id = app.Id;
                applicationInput.MediaTypeId = app.MediaTypeId;
                applicationInput.MediaLinkImage = app.MediaLinkImage;
                if (Session["appId"] == null)
                {
                    Session["appId"] = applicationInput.Id;
                    Session["appName"] = applicationInput.ApplicationName;
                }
                else
                {
                    applicationInput.ApplicationName = Session["appName"].ToString();
                }
                applicationInput.AppStartUpMediaType = app.AppStartUpMediaType;
                applicationInput.AppVideo = app.AppVideo;
                applicationInput.AppLogo = app.AppLogo;
                applicationInput.MediaLinkImageId = app.MediaLinkImageId;
                applicationInput.MediaLinkVideoId = app.MediaLinkVideoId;
                applicationInput.ApplicationName=app.ApplicationName;
                applicationInput.FeedbackGiftImage = app.FeedbackGiftImage;
                applicationInput.IsFeedbackVisible = app.IsFeedbackVisible;
                applicationInput.IsLoyaltyVisible = app.IsLoyaltyVisible;
                applicationInput.IsPaymentVisible = app.IsPaymentVisible;
                applicationInput.LoyaltyGiftImage = app.LoyaltyGiftImage;
                applicationInput.LoyaltyPassword = app.LoyaltyPassword;
                applicationInput.LoyaltyTermsAndConditions = app.LoyaltyTermsAndConditions;
                applicationInput.SocialMedia1Image = app.SocialMedia1Image;
                applicationInput.SocialMedia1Link = app.SocialMedia1Link;
                applicationInput.SocialMedia2Image = app.SocialMedia2Image;
                applicationInput.SocialMedia2Link = app.SocialMedia2Link;
                applicationInput.SocialMedia3Image = app.SocialMedia3Image;
                applicationInput.SocialMedia3Link = app.SocialMedia3Link;
                applicationInput.TermsAndConditions = app.TermsAndConditions;
                applicationInput.VoucherMessage = app.VoucherMessage;
                applicationInput.VoucherMessageFrequency = app.VoucherMessageFrequency;
                applicationInput.VourcherMessageEnd = app.VourcherMessageEnd;
                applicationInput.VourcherMessageStart = app.VourcherMessageStart;
                applicationInput.VourhcerMessageTriggerPoint = app.VourhcerMessageTriggerPoint;

            }

            var branches = branchService.Where(o => o.UserId.Equals(id));
            List<cm.Branch> branchesList = new List<cm.Branch>();
            foreach (var pp in branches)
            {
                cm.Branch branchRetailerInfo = new cm.Branch();
                branchRetailerInfo.Id = pp.Id;
                branchRetailerInfo.Address = pp.Address;
                branchRetailerInfo.BranchName = pp.BranchName;
                branchRetailerInfo.Country = pp.Country;
                branchesList.Add(branchRetailerInfo);

            }
            ViewBag.Branches = branchesList;
        
            applicationInput.UserId = id;
            return View(applicationInput);
        }

        [HttpPost]
        public ActionResult RetailerInfo(ApplicationInput input)
        {
            int id = userService.GetUserIdByName(HttpContext.User.Identity.Name);

            var app = service.Where(o => o.UserId.Equals(input.UserId) && o.Id == input.Id).SingleOrDefault();

            if (app != null)
            {
                cm.Application appl = repoApplication.Get(app.Id);
                appl.SocialMedia1Link = input.SocialMedia1Link;
                appl.SocialMedia2Link = input.SocialMedia2Link;
                appl.SocialMedia3Link = input.SocialMedia3Link;
                appl.MediaTypeId = input.MediaTypeId;
                appl.AppStartUpMediaType = input.AppStartUpMediaType;
                appl.AppVideo = input.AppVideo;
                appl.AppLogo = input.AppLogo;
                appl.MediaLinkImageId = input.MediaLinkImageId;
                appl.MediaLinkVideoId = input.MediaLinkVideoId;
                repoApplication.Save();
                
            }

            var branches = branchService.Where(o => o.UserId.Equals(id) && o.IsDeleted==false);
            List<cm.Branch> branchesList = new List<cm.Branch>();
            foreach (var pp in branches)
            {
                cm.Branch branchRetailerInfo = new cm.Branch();
                branchRetailerInfo.Id = pp.Id;
                branchRetailerInfo.Address = pp.Address;
                branchRetailerInfo.BranchName = pp.BranchName;
                branchRetailerInfo.Country = pp.Country;
                branchesList.Add(branchRetailerInfo);

            }
            ViewBag.Branches = branchesList;
            applicationInput.UserId = id;
            return View(input);
        }




        public ActionResult ShoppingCart()
        {
            return View();
        }
        
        public ActionResult Loyalty()
        {
            int id = userService.GetUserIdByName(HttpContext.User.Identity.Name);
            var app = service.Where(o => o.UserId.Equals(id)).SingleOrDefault();
            if (app != null)
            {
                applicationInput.Id = app.Id;
                applicationInput.LoyaltyGiftImage = app.LoyaltyGiftImage;
                applicationInput.LoyaltyGiftImageId = app.LoyaltyGiftImageId;
                applicationInput.LoyaltyPassword = app.LoyaltyPassword;
                applicationInput.LoyaltyTermsAndConditions = app.LoyaltyTermsAndConditions;
            }
            return View(applicationInput);
           
        }

        [HttpPost]
        public ActionResult Loyalty(ApplicationInput input)
        {
            int id = userService.GetUserIdByName(HttpContext.User.Identity.Name);
            var app = service.Where(o => o.UserId.Equals(id)).SingleOrDefault();
            if (app != null)
            {
                cm.Application appl = repoApplication.Get(app.Id);
                appl.LoyaltyGiftImage = input.LoyaltyGiftImage;
                appl.LoyaltyPassword = input.LoyaltyPassword;
                appl.LoyaltyTermsAndConditions = input.LoyaltyTermsAndConditions;
                appl.LoyaltyGiftImageId = input.LoyaltyGiftImageId;
                repoApplication.Save();

            }
            return View(applicationInput);

        }

        public ActionResult TermsAndConditions()
        {
            int id = userService.GetUserIdByName(HttpContext.User.Identity.Name);
            var app = service.Where(o => o.UserId.Equals(id)).SingleOrDefault();
            if (app != null)
            {
                applicationInput.Id = app.Id;
                applicationInput.TermsAndConditions = app.TermsAndConditions;
                applicationInput.IsTermandConditionsAgreed = app.IsTermandConditionsAgreed;
                
            }
            return View(applicationInput);
           
        }

        public static JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();


        [HttpPost]
        public string SendNotification(string apiKey, List<string> regid, string messageTitle, string messageDetail) 
        {

            var consumer = consumerService.GetAll();

            foreach (var con in consumer)
            {
                if (con.DeviceId!=null)
                {
                    if (con.DeviceId.Length>50)
                    {
                        regid.Add(con.DeviceId);
                    }
                }
            }

            var SENDER_ID = "855050158200";
                cm.PostData postData = new cm.PostData();
                ServicePointManager.ServerCertificateValidationCallback = (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true;

                WebRequest request = WebRequest.Create("https://android.googleapis.com/gcm/send");
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add(string.Format("Authorization: key={0}", apiKey));
                request.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));
                //request.Credentials = new NetworkCredential("dev.dfrontier@gmail.com", "d-frontier");
                request.UseDefaultCredentials = true;
                request.PreAuthenticate = true;
                request.Credentials = CredentialCache.DefaultCredentials;
                postData.collapse_key = "score_update";
                postData.time_to_live = 108;
                postData.delay_while_idle = true;
                postData.data = new cm.JsonData() { data = messageDetail, title = messageTitle };
                postData.registration_ids = regid;

                byte[] byteArray = Encoding.UTF8.GetBytes(javaScriptSerializer.Serialize(postData));
                request.ContentLength = byteArray.Length;

                string responseFromServer = null;

                try
                {
                    using (Stream dataStream = request.GetRequestStream())
                    {
                        dataStream.Write(byteArray, 0, byteArray.Length);

                        using (WebResponse response = request.GetResponse())
                        {
                            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                            {
                                responseFromServer = reader.ReadToEnd();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    responseFromServer = ex.Message;
                    ex = null;
                }
                return responseFromServer;
          
        }


        //[HttpPost]
        //public string SendNotification(string deviceId, string message, string googleAppId, string senderId)
        //{
        //    //deviceId= APA91bGqrYNBmNfjth7h0OgK2a-u8irifOTjyzmg3Eq4bp-kL6wTUSEBrcn_BcgNOpE5U34b-SYquNjqE3NsUWAgXmzWtjeiVD7dRcfe--IB29NrsqOcMstY2xsgeYCrsMRSJYmt2GcX0gQJSFWUZAkpBuWSgWSBgg
        //    //string GoogleAppID = "AIzaSyBsKhy_h1dD8MG_9XHBvq-6K8cn7hTzajw";
        //  //  var SENDER_ID = "471481279758";
        //    var value = message;
        //    WebRequest tRequest;

        //    //   String message = intent.getStringExtra("Data");
        //    //   String title = intent.getStringExtra("Title");

        //    tRequest = WebRequest.Create("https://android.googleapis.com/gcm/send");
        //    tRequest.Method = "post";
        //    tRequest.ContentType = " application/x-www-form-urlencoded;charset=UTF-8";
        //    tRequest.Headers.Add(string.Format("Authorization: key={0}",  googleAppId));

        //   tRequest.Headers.Add(string.Format("Sender: id={0}",  senderId));

        //    string postData = "collapse_key=score_update&time_to_live=108&delay_while_idle=1&data.message=" + value + "&data.time=" + System.DateTime.Now.ToString() + "&registration_id=" + deviceId + "";
        //    Byte[] byteArray = Encoding.UTF8.GetBytes(postData);
        //    tRequest.ContentLength = byteArray.Length;

        //    Stream dataStream = tRequest.GetRequestStream();
        //    dataStream.Write(byteArray, 0, byteArray.Length);
        //    dataStream.Close();
            

        //    WebResponse tResponse = tRequest.GetResponse();

        //    dataStream = tResponse.GetResponseStream();

        //    StreamReader tReader = new StreamReader(dataStream);

        //    String sResponseFromServer = tReader.ReadToEnd();


        //    tReader.Close();
        //    dataStream.Close();
        //    tResponse.Close();
        //    return sResponseFromServer;
        //}

         [HttpPost]
        public ActionResult TermsAndConditions(ApplicationInput input)
        {
            int id = userService.GetUserIdByName(HttpContext.User.Identity.Name);
            var app = service.Where(o => o.UserId.Equals(id)).SingleOrDefault();
            if (app != null)
            {

                cm.Application appl = repoApplication.Get(app.Id);
                appl.TermsAndConditions = input.TermsAndConditions;
                appl.IsTermandConditionsAgreed = app.IsTermandConditionsAgreed;
                repoApplication.Save();

            }
            return View(applicationInput);

        }
        
        public ActionResult UserProfile()
        {
            int id = userService.GetUserIdByName(HttpContext.User.Identity.Name);
            var app = service.Where(o => o.UserId.Equals(id)).SingleOrDefault();
            if (app != null)
            {
                applicationInput.ApplicationName = app.ApplicationName;
                applicationInput.Id = app.Id;
                Session["appId"] = applicationInput.Id;
                Session["appName"] = applicationInput.ApplicationName;
            }


            return View(applicationInput);
        }


        public ActionResult GetApplication(int id)
        {
            return Json(new { Id = id, Content = this.RenderView(RowViewName, new[] { service.Get(id) }), Type = "product" });
        }


        public ActionResult ChangePicture(int id)
        {
            var result = service.Get(id);
            ViewBag.MediaTypeId = "Video";
            if (result.MediaTypeId.Equals("Image"))
            {
                ViewBag.MediaTypeId = "Image";    
            }

            return View(result);
        }


       
    }
}

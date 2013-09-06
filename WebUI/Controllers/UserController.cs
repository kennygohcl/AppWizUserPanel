using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using dFrontierAppWizard.Core.Model;
using dFrontierAppWizard.Core.Repository;
using dFrontierAppWizard.Core.Security;
using dFrontierAppWizard.Core.Service;
using dFrontierAppWizard.WebUI.Dto;
using dFrontierAppWizard.WebUI.Mappers;
using System;
using System.Collections.Generic;


namespace dFrontierAppWizard.WebUI.Controllers
{
    //[Authorize(Roles = "admin")]
    public class UserController : Crudere<User, UserCreateInput, UserEditInput>
    {
        private new readonly IUserService service;
        private readonly IRepo<Role> repo;
        private readonly IRepo<Industry> repoIndustry;
        public UserCreateInput newUserCreateInput = new UserCreateInput();
        private readonly ICollection<Industry> _ind = new Collection<Industry>();
        private readonly ICollection<int> _indList = new Collection<int>();

        public UserController(IMapper<User, UserCreateInput> v, IMapper<User, UserEditInput> ve, IUserService service, IRepo<Role> repo, IRepo<Industry> repoIndustry)
            : base(service, v, ve)
        {
            this.service = service;
            this.repo = repo;
            this.repoIndustry = repoIndustry;

        }

        public ActionResult ChangePassword(int id)
        {
            return View();
        }

        public override ActionResult Index()
        {
            //int userTypeId = service.GetUserTypeId(HttpContext.User.Identity.Name);
            ViewBag.userTypeId = 1; 
             
            return View();
        }


        public ActionResult EditSingle()
        {
            var userId = service.GetUserIdByName(HttpContext.User.Identity.Name);
            var user = service.Get(userId);

            foreach (var a in user.Industries)
            {
                var repoValue = repoIndustry.Where(o => o.Id == a.Id).SingleOrDefault();
                if (repoValue!=null)
                {  
                    _indList.Add(repoValue.Id);
                }
            }
            newUserCreateInput.Id = user.Id;
            newUserCreateInput.Address = user.Address;
            newUserCreateInput.Company = user.Company;
            newUserCreateInput.ConfirmEmail = user.Email;
            newUserCreateInput.Email = user.Email;
            newUserCreateInput.Login = user.Login;
            newUserCreateInput.Industries = _indList;
            newUserCreateInput.Phone = user.Phone;

            return View(newUserCreateInput);
        }
      
        [HttpPost]
        public ActionResult EditSingle(UserCreateInput input)
        {

            foreach (var a in input.Industries)
            {
                _ind.Add(repoIndustry.Where(o => o.Id == a).SingleOrDefault());

            }
            
            User user = service.Get(input.Id);

            user.Address = input.Address;
            user.Company = input.Company;
            user.Email = input.Email;
            user.Login = input.Login;
            user.Industries = _ind;
            user.Phone = input.Phone;
            service.Save();

            return View(input);
        }

       
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordInput input)
        {
            if (!ModelState.IsValid) return View(input);
            service.ChangePassword(input.Id, input.Password);
            return Json(new{ Login = service.Get(input.Id).Login});
        }

        protected override string RowViewName
        {
             
            get { return "ListItems/User"; }
        }

     
       
        public ActionResult CreateUser()
        {
            List<int> roleList = new List<int>();
            roleList.Add(3);
            newUserCreateInput.Roles = roleList;
            return View(newUserCreateInput);
        }


        [HttpPost]
        public ActionResult CreateUser(UserCreateInput input)
        {
            

            if (!ModelState.IsValid)
            {
                return View(input);
            }

            var list = service.Where(o => o.Email.Equals(input.Email)).SingleOrDefault();

            if (list != null)
            {
                ViewBag.EmailDuplicate = "Yes";
                return View(input);

            }

  
         /*   var repoIndustries = repoIndustry.Where(o => o.Id.Equals(userId));
            List<Industry> industries = new List<Industry>();
            foreach (var pp in pcategories)
            {
                ProductCategory pCat = new ProductCategory();
                pCat.Id = pp.Id;
                pCat.Description = pp.Description;
                categories.Add(pCat);

            }*/

            foreach (var a in input.Industries)
            {
                _ind.Add(repoIndustry.Where(o => o.Id == a).SingleOrDefault());

            }
            
            var user = service.Create(new User
            {
                Login = input.Login,
                Email = input.Email,
                Password = input.Password,
                Company = input.Company,
                Phone = input.Phone,
                UserTypeId = 2,
                Address = input.Address,
                Industries = _ind,
                DateCreated = DateTime.Now,
                PaymentStatus = "NA"
            });
             service.Save();


             using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Db"].ConnectionString))
             using (var command = new SqlCommand("sp_send_dbmail", conn)
             {
                 CommandType = CommandType.StoredProcedure
             })
             {
                 conn.Open();
                 command.Parameters.Add(new SqlParameter("@profile_name", "DBMail Profile"));
                 command.Parameters.Add(new SqlParameter("@recipients", input.Email));
                 command.Parameters.Add(new SqlParameter("@subject", "AppWiz Registration"));
                 command.Parameters.Add(new SqlParameter("@from_address", ConfigurationManager.AppSettings["EmailHost"]));
                 command.Parameters.Add(new SqlParameter("@Body", "Welcome to AppWiz.."));
                 command.ExecuteNonQuery();
                 conn.Close();
             }

             return RedirectToAction("SignIn", "Account");
           

        }

 

       

    }
}
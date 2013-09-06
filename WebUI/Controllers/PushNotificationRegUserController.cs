using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using cm = dFrontierAppWizard.Core.Model;
using dFrontierAppWizard.Core.Repository;
using dFrontierAppWizard.Core.Service;

namespace dFrontierAppWizard.WebUI.Controllers
{
    public class PushNotificationRegUserController : Controller
    {
        //
        // GET: /PushNotificationRegUser/
        private IRepo<cm.PushNotificationRegUser> repo;

        public PushNotificationRegUserController(IRepo<cm.PushNotificationRegUser> repo)
        {
            this.repo = repo;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string RegisterApp(int UserID, string RegId, string DeviceId)
        {
            using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Db"].ToString()))
            {
                conn.Open();

                string sql = "INSERT INTO PushNotificationRegUser (UserId,RegId,DeviceId,CreatedTime,IsDeleted)  VALUES (@UserId,@RegId,@DeviceId,@CreatedTime,@IsDeleted)";
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@UserId", UserID);
                cmd.Parameters.AddWithValue("@RegId", RegId);
                cmd.Parameters.AddWithValue("@DeviceId", DeviceId);
                cmd.Parameters.AddWithValue("@CreatedTime", DateTime.UtcNow);
                cmd.Parameters.AddWithValue("@IsDeleted", "0");
                cmd.ExecuteNonQuery();

            }

            string sJson = @"{""success"":true}";
            return sJson;
            //return Json(new {sJson});
        }
    }
}


using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Web.Mvc;
using dFrontierAppWizard.Core.Model;
using dFrontierAppWizard.Core.Service;
using dFrontierAppWizard.Data;
using dFrontierAppWizard.WebUI.Dto;
using dFrontierAppWizard.WebUI.Mappers;
//using AuthorizeNet;
//using AuthorizeNet.Helpers;
using System.Configuration;
using System.Collections.Generic;


namespace dFrontierAppWizard.WebUI.Controllers
{
    //[Authorize(Roles = "admin")]
    public class UserBillingInformationController :
        Crudere<UserBillingInformation, UserBillingInformationInput, UserBillingInformationEditInput>
    {
        private new readonly IUserBillingInformationService service;
        private new readonly IUserService serviceUser;
        private const String MERCHANT_ID = "appwiz";

        private const String TRANSACTION_KEY =
            "n9/NGwfeyW5ZXAClzZC1856fGivsoziu+qR4IGCZCP21UYPwoF4Xsv2WuHYIEIgrS9yTEzMSzzKqIm3+kLfcg846WF1hY6kk4BAZ3K/TR8inI2/hIwwdTmrKlmI3RbPgm7yUZRHukzd9BtmZA9S0axw/txIIgn+89SfX+xFfUeypPktVqD1ou3YybjpckLXznp8aK+yjOK76pHggYJkI/bVRg/CgXhey/Za4dggQiCtL3JMTMxLPMqoibf6Qt9yDzjpYXWFjqSTgEBncr9NHyKcjb+EjDB1OasqWYjdFs+CbvJRlEe6TN30G2ZkD1LRrHD+3EgiCf7z1J9f7EV9R7A==";

        //  public UserController(IMapper<User, UserCreateInput> v, IMapper<User, UserEditInput> ve, IUserService service) : base(service, v, ve)
        public UserBillingInformationController(IMapper<UserBillingInformation, UserBillingInformationInput> v,
                                                IMapper<UserBillingInformation, UserBillingInformationEditInput> ve,
                                                IUserBillingInformationService service, IUserService serviceUser)
            : base(service, v, ve)
        {
            this.serviceUser = serviceUser;
            this.service = service;
        }


        public UserBillingInformationInput userBillingInformationInput = new UserBillingInformationInput();
        private Role role = new Role();

        public ActionResult CreateBilling()
        {
            ViewBag.UserId = serviceUser.GetUserIdByName(HttpContext.User.Identity.Name);

            var list = service.Where(o => o.UserId.Equals(userBillingInformationInput.UserId)).SingleOrDefault();

            if (list != null)
            {
                return RedirectToAction("Index", "Application");
            }


            return View(userBillingInformationInput);
        }


        public ActionResult EditBilling()
        {
            var id = serviceUser.GetUserIdByName(HttpContext.User.Identity.Name);
            ViewBag.UserId = id;

            var list = service.Where(o => o.UserId.Equals(id)).SingleOrDefault();
            userBillingInformationInput.BillingAddress = list.BillingAddress;
            userBillingInformationInput.BillingContactEmail = list.BillingContactEmail;
            userBillingInformationInput.BillingContactNumber = list.BillingContactNumber;
            userBillingInformationInput.BillingZipCode = list.BillingZipCode;
            userBillingInformationInput.FirstName = list.FirstName;
            userBillingInformationInput.LastName = list.LastName;
            userBillingInformationInput.Id = list.Id;
            userBillingInformationInput.Country = list.Country;
            userBillingInformationInput.City = list.City;

            return View(userBillingInformationInput);
        }

        [HttpPost]
        public ActionResult EditBilling(UserBillingInformationInput input)
        {
            UserBillingInformation ub = service.Get(input.Id);

            ub.BillingAddress = input.BillingAddress;
            ub.BillingContactEmail = input.BillingContactEmail;
            ub.BillingContactNumber = input.BillingContactNumber;
            ub.BillingZipCode = input.BillingZipCode;
            ub.FirstName = input.FirstName;
            ub.LastName = input.LastName;
          service.Save();
          return View(input);
        }

        protected override string RowViewName
        {
            get { return "ListItems/UserBillingInformation"; }
        }

        /*   //pretend this is injected with IoC
           private IGateway OpenGateway()
           {
               //we used the form builder so we can now just load it up
               //using the form reader

               var login = ConfigurationManager.AppSettings["ApiLogin"];
               var transactionKey = ConfigurationManager.AppSettings["TransactionKey"];

               //this is set to test mode - change as needed.
               var gate = new Gateway(login, transactionKey, true);
               return gate;
           }

           private string GetCardType(int cardId)
           {
               string ctype = "";
               switch (cardId)
               {
                   case 1:
                       ctype = "Visa";
                       break;
                   case 2:
                       ctype = "Master Card";
                       break;
                   case 3:
                       ctype = "American Express";
                       break;
               }
               return ctype;
           }*/

        [HttpPost]
        public ActionResult CreateBilling(UserBillingInformationInput input)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.errMessages = string.Join("; ", ModelState.Values
                                                            .SelectMany(x => x.Errors)
                                                            .Select(x => x.ErrorMessage));

                return View(input);
            }

            // var xPrice = (decimal)input.SpecialPostingCost;
            if (!ModelState.IsValid)
                return View(input);

             //be sure the amount paid is the amount required*/

            RequestMessage request = new RequestMessage();

            request.merchantID = MERCHANT_ID;

            // To help us troubleshoot any problems that you may encounter,
            // please include the following information about your application.
            request.clientLibrary = ".NET WCF";
            request.clientLibraryVersion = Environment.Version.ToString();
            request.clientEnvironment =
                Environment.OSVersion.Platform +
                Environment.OSVersion.Version.ToString();

            // This section contains a sample transaction request for the authorization 
            // service with complete billing, payment card, and purchase (two items) information.
            request.ccAuthService = new CCAuthService();
            request.ccAuthService.run = "true";


            BillTo billTo = new BillTo();
            billTo.firstName = input.FirstName;
            billTo.lastName = input.LastName;
            billTo.street1 = input.BillingAddress;
            billTo.city = input.City;
            billTo.postalCode = input.BillingZipCode;
            billTo.country = input.Country;
            billTo.email = input.BillingContactEmail;
            billTo.ipAddress = GetPublicIP();
            billTo.phoneNumber = input.BillingContactNumber;

            request.billTo = billTo;

            Card card = new Card();
            card.accountNumber = input.CreditCardNumber; // "4111111111111111";
            card.expirationMonth = input.ExpiryDate.PadLeft(2);
            card.expirationYear = input.ExpiryDate.PadRight(4);
            card.cvNumber = input.CardCode;
            request.card = card;

            PurchaseTotals purchaseTotals = new PurchaseTotals();
            purchaseTotals.currency = "SGD";
            if (input.Subscription.Equals("100$/Month"))
            {
                purchaseTotals.originalAmount = "100";
            }
            else if (input.Subscription.Equals("600$/Semi-Annual"))
            {
                purchaseTotals.originalAmount = "600";
            }
            else if (input.Subscription.Equals("1200$/Annual"))
            {
                purchaseTotals.originalAmount = "1200";
            }
            request.purchaseTotals = purchaseTotals;

            // Before using this example, replace the generic value with your
            // reference number for the current transaction.
            request.merchantReferenceCode = "U" + input.UserId + "A" + purchaseTotals.originalAmount;
            request.item = new Item[1];

            Item item = new Item();
            item.id = "0";
            item.unitPrice = purchaseTotals.originalAmount;
            request.item[0] = item;


            try
            {
                TransactionProcessorClient proc = new TransactionProcessorClient();

                proc.ChannelFactory.Credentials.UserName.UserName = request.merchantID;
                proc.ChannelFactory.Credentials.UserName.Password = TRANSACTION_KEY;

                ReplyMessage reply = proc.runTransaction(request);

                if (reply.decision == "ACCEPT")
                {
                     SaveOrderState();
                      ProcessReply(reply);
                    var userBillingInformation = service.Create(new UserBillingInformation
                        {
                            FirstName = input.FirstName,
                            LastName = input.LastName,
                            BillingAddress = input.BillingAddress,
                            BillingZipCode = input.BillingZipCode,
                            BillingContactNumber = input.BillingContactNumber,
                            BillingContactEmail = input.BillingContactEmail,
                            CreditCardNumber = input.CreditCardNumber,
                            ExpiryDate = input.ExpiryDate,
                            UserId = input.UserId
                        });

                    service.Save();

                    var user = serviceUser.Where(o => o.Id.Equals(input.UserId)).SingleOrDefault();

                    User selectedUser = serviceUser.Get(user.Id);
                    selectedUser.PaymentStatus = "Active";
                    serviceUser.Save();


                    using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Db"].ConnectionString))
                    using (var command = new SqlCommand("sp_send_dbmail", conn)
                        {
                            CommandType = CommandType.StoredProcedure
                        })
                    {
                        conn.Open();
                        command.Parameters.Add(new SqlParameter("@profile_name", "DBMail Profile"));
                        command.Parameters.Add(new SqlParameter("@recipients", input.BillingContactEmail));
                        command.Parameters.Add(new SqlParameter("@subject", "AppWiz Billing Info registration"));
                        command.Parameters.Add(new SqlParameter("@from_address",
                                                                ConfigurationManager.AppSettings["EmailHost"]));
                        command.Parameters.Add(new SqlParameter("@Body", "AppWiz payment acknowledgement."));
                        command.ExecuteNonQuery();
                        conn.Close();
                    }
                    return RedirectToAction("Index", "Application");
                }
            }
            catch (TimeoutException e)
            {
                //Console.WriteLine("TimeoutException: " + e.Message + "\n" + e.StackTrace);
                // return Json("TimeoutException: " + e.Message + "\n" + e.StackTrace);
                return Json(false);
            }
            catch (FaultException e)
            {
                //Console.WriteLine("FaultException: " + e.Message + "\n" + e.StackTrace);
                return Json(false);
            }
            catch (CommunicationException e)
            {
                //Console.WriteLine("CommunicationException: " + e.Message + "\n" + e.StackTrace);
                return Json(false);
            }

            return Json(false);
        }

        public string GetPublicIP()
        {
            String direction = "";
            WebRequest request = WebRequest.Create("http://checkip.dyndns.org/");
            using (WebResponse response = request.GetResponse())
            using (StreamReader stream = new StreamReader(response.GetResponseStream()))
            {
                direction = stream.ReadToEnd();
            }

            //Search for the ip in the html
            int first = direction.IndexOf("Address: ") + 9;
            int last = direction.LastIndexOf("</body>");
            direction = direction.Substring(first, last - first);

            return direction;
        }

        private static void SaveOrderState()
        {
            /*
             * This is where you store the order state in your system for
             * post-transaction analysis.  Information to store include the
             * invoice, the values of the reply fields, or the details of the
             * exception that occurred, if any.
             */
        }

        private static void ProcessReply(ReplyMessage reply)
        {
            string template = GetTemplate(
                (reply.decision));
            string content = GetContent(reply);

            /*
             * Display result of transaction.  Being a console application,
             * this sample simply prints out some text on the screen.  Use
             * what is appropriate for your system (e.g. ASP.NET pages).
             */
            Console.WriteLine(template, content);
        }

        private static string GetTemplate(string decision)
        {
            /*
             * This is where you retrieve the HTML template that corresponds
             * to the decision.  This template has 'boiler-plate' wording and
             * can be stored in files or a database.  This is just one way to
             * retrieve feedback pages.  Use what is appropriate for your
             * system (e.g. ASP.NET pages).
             */

            if ("ACCEPT".Equals(decision))
            {
                return ("The transaction succeeded.{0}");
            }

            if ("REJECT".Equals(decision))
            {
                return ("Your order was not approved.{0}");
            }

            // ERROR
            return (
                       "Your order could not be completed at this time.{0}" +
                       "\nPlease try again later.");
        }

        private static string GetContent(ReplyMessage reply)
        {
            /*
             * This is where you retrieve the content that will be plugged
             * into the template.
             * 
             * The strings returned in this sample are mostly to demonstrate
             * how to retrieve the reply fields.  Your application should
             * display user-friendly messages.
             */

            int reasonCode = int.Parse(reply.reasonCode);
            switch (reasonCode)
            {
                    // Success
                case 100:
                    return (
                               "\nRequest ID: " + reply.requestID +
                               "\nAuthorization Code: " +
                               reply.ccAuthReply +
                               "\nCapture Request Time: " +
                               reply.ccCaptureReply +
                               "\nCaptured Amount: " +
                               reply.ccCaptureReply);


                    // Missing field(s)
                case 101:
                    return (
                               "\nThe following required field(s) are missing: " +
                               EnumerateValues(reply, "missingField"));

                    // Invalid field(s)
                case 102:
                    return (
                               "\nThe following field(s) are invalid: " +
                               EnumerateValues(reply, "invalidField"));

                    // Insufficient funds
                case 204:
                    return (
                               "\nInsufficient funds in the account.  Please use a " +
                               "different card or select another form of payment.");

                    // add additional reason codes here that you need to handle
                    // specifically.

                default:
                    // For all other reason codes, return an empty string,
                    // in which case, the template will be displayed with no
                    // specific content.
                    return (String.Empty);
            }
        }


        private static string EnumerateValues(
            ReplyMessage reply, string fieldName)
        {
            StringBuilder sb = new System.Text.StringBuilder();

            foreach (var vf in fieldName)
            {
                sb.Append(vf.ToString());
            }
            return (sb.ToString());
        }

    }
}


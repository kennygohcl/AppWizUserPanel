using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Web.Mvc;
using mm=dFrontierAppWizard.Core.Model;
using dFrontierAppWizard.Core.Repository;
using dFrontierAppWizard.Core.Service;
using dFrontierAppWizard.Data;
using dFrontierAppWizard.WebUI.Dto;
using dFrontierAppWizard.WebUI.Mappers;
//using AuthorizeNet;
using AuthorizeNet.Helpers;
using System.Configuration;
using System.Collections.Generic;


namespace dFrontierAppWizard.WebUI.Controllers
{
    //[Authorize(Roles = "admin")]
    public class ConsumerBillingInformationController :
        Crudere<mm.ConsumerBillingInformation, ConsumerBillingInformationInput, ConsumerBillingInformationEditInput>
    {
        private new readonly IConsumerBillingInformationService service;
        private new readonly IRepo<mm.Transaction> serviceTransaction;
        private const String MERCHANT_ID = "appwiz";
        private const String TRANSACTION_KEY = "n9/NGwfeyW5ZXAClzZC1856fGivsoziu+qR4IGCZCP21UYPwoF4Xsv2WuHYIEIgrS9yTEzMSzzKqIm3+kLfcg846WF1hY6kk4BAZ3K/TR8inI2/hIwwdTmrKlmI3RbPgm7yUZRHukzd9BtmZA9S0axw/txIIgn+89SfX+xFfUeypPktVqD1ou3YybjpckLXznp8aK+yjOK76pHggYJkI/bVRg/CgXhey/Za4dggQiCtL3JMTMxLPMqoibf6Qt9yDzjpYXWFjqSTgEBncr9NHyKcjb+EjDB1OasqWYjdFs+CbvJRlEe6TN30G2ZkD1LRrHD+3EgiCf7z1J9f7EV9R7A==";
        private new readonly IRepo<mm.Order> repoOrder;


        public ConsumerBillingInformationController(
            IMapper<mm.ConsumerBillingInformation, ConsumerBillingInformationInput> v,
            IMapper<mm.ConsumerBillingInformation, ConsumerBillingInformationEditInput> ve,
            IConsumerBillingInformationService service, IRepo<mm.Transaction> serviceTransaction, IRepo<mm.Order> repoOrder)
            : base(service, v, ve)
        {

            this.service = service;
            this.serviceTransaction = serviceTransaction;
            this.repoOrder = repoOrder;
        }


        public ConsumerBillingInformationInput consumerBillingInformationInput = new ConsumerBillingInformationInput();
        private mm.Role role = new mm.Role();

        /*  public ActionResult CreateBilling()
          {
              ViewBag.UserId = serviceUser.GetUserIdByName(HttpContext.User.Identity.Name);

              var list = service.Where(o => o.UserId.Equals(userBillingInformationInput.UserId)).SingleOrDefault();

              if (list != null)
              {
                  return RedirectToAction("Index", "Home");
              }


              return View(userBillingInformationInput);
          }*/

        protected override string RowViewName
        {
            get { return "ListItems/UserBillingInformation"; }
        }

        ////pretend this is injected with IoC
        //private IGateway OpenGateway()
        //{
        //    //we used the form builder so we can now just load it up
        //    //using the form reader

        //    var login = ConfigurationManager.AppSettings["ApiLogin"];
        //    var transactionKey = ConfigurationManager.AppSettings["TransactionKey"];

        //    //this is set to test mode - change as needed.
        //    var gate = new Gateway(login, transactionKey, true);
        //    return gate;
        //}

        //private string GetCardType(int cardId)
        //{
        //    string ctype = "";
        //    switch (cardId)
        //    {
        //        case 1:
        //            ctype = "Visa";
        //            break;
        //        case 2:
        //            ctype = "Master Card";
        //            break;
        //        case 3:
        //            ctype = "American Express";
        //            break;
        //    }
        //    return ctype;
        //}



        [HttpPost]
        public string GetBillingInfo(int consumerId)
        {
            var billingInfo = service.Where(o => o.ConsumerId.Equals(consumerId)).AsEnumerable();
            var query = from selectedBillingInfo in billingInfo.AsEnumerable()
                        select new
                        {
                            selectedBillingInfo.Id,
                            selectedBillingInfo.BillingAddress,
                            selectedBillingInfo.BillingContactNumber,
                            selectedBillingInfo.BillingZipCode,
                            selectedBillingInfo.ConsumerId,
                            selectedBillingInfo.FirstName,
                            selectedBillingInfo.LastName
                        };

            var oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string sJson = oSerializer.Serialize(query);
            sJson = @"{""Data"":" + sJson + @",""success"":true}";
            return sJson;
        }


        [HttpPost]
        public ActionResult SetPayment(int consumerId, string firstName, string lastName, string billingAddress,
                                       string billingZipCode, long creditCardNumber, string creditCardSecurityCode,
                                       string expiryDate, decimal amount, long phone, string email, string ipAddress, string city, string country)
        {

            if (firstName.Length > 0 && lastName.Length > 0 && billingAddress.Length > 0 && billingZipCode.Length > 0 &&
                creditCardNumber > 0 && creditCardSecurityCode.Length > 0 && ipAddress.Length>0 && city.Length>0)
            {
                //Authorize.net start
                // var gate = OpenGateway();

                // //build the request from the Form post
                //var apiRequest = CheckoutFormReaders.BuildAuthAndCaptureFromPost();

                //apiRequest.Amount = amount.ToString();

                //apiRequest.DelimData = "tRUE";
                //apiRequest.DelimChar = "|";
                //apiRequest.RelayResponse = "FALSE";
                //apiRequest.Type = "AUTH_CAPTURE";
                //apiRequest.Method = "CC";
                //apiRequest.CardNum = creditCardNumber.ToString();
                //apiRequest.ExpDate = expiryDate;

                //apiRequest.Description = "Payment for purchase from ....";
                //apiRequest.FirstName = firstName;
                //apiRequest.LastName = lastName;
                //apiRequest.Address = billingAddress;
                //apiRequest.Phone = phone.ToString();
                //apiRequest.Email = email;
                //apiRequest.Zip = billingZipCode;
                //apiRequest.CardCode = creditCardSecurityCode;

                ////send to Auth.NET
                //var response = gate.Send(apiRequest);

                ////be sure the amount paid is the amount required

                //if (response.Approved)
                //Authorize.net end

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
                billTo.firstName = firstName;
                billTo.lastName = lastName;
                billTo.street1 = billingAddress;
                billTo.city = city;
                billTo.postalCode = billingZipCode;
                billTo.country = country;
                billTo.email = email;
                billTo.ipAddress = ipAddress;
                billTo.phoneNumber = phone.ToString();
                request.billTo = billTo;

                Card card = new Card();
                card.accountNumber = creditCardNumber.ToString(); // "4111111111111111";
                card.expirationMonth =  expiryDate.Substring(0,2);
                card.expirationYear = expiryDate.Substring(3, 4);
               
                card.cvNumber = creditCardSecurityCode;
                request.card = card;

                PurchaseTotals purchaseTotals = new PurchaseTotals();
                purchaseTotals.currency = "SGD";
                purchaseTotals.originalAmount = amount.ToString();
                request.purchaseTotals = purchaseTotals;

                var totals = serviceTransaction.Where(o => o.ConsumerId == consumerId && o.Status == "Pending").FirstOrDefault();
                var orders = repoOrder.Where(x => x.ConsumerId==consumerId && x.TransactionId==totals.Id);

                // Before using this example, replace the generic value with your
                // reference number for the current transaction.
                request.merchantReferenceCode = "T"+totals.Id.ToString()+"C"+totals.ConsumerId.ToString()+"D"+totals.DateofTransaction.ToString();
                request.item = new Item[orders.Count()];

                int ctr = 0;
                foreach (var order in orders)
                {
                    Item item = new Item();
                    item.id = ctr.ToString();
                    item.unitPrice = order.Total.ToString();
                    request.item[ctr] = item;
                    ctr++;
                }
                
                try
                {
                    TransactionProcessorClient proc = new TransactionProcessorClient();

                    proc.ChannelFactory.Credentials.UserName.UserName = request.merchantID;
                    proc.ChannelFactory.Credentials.UserName.Password = TRANSACTION_KEY;

                    ReplyMessage reply = proc.runTransaction(request);

                    if (reply.decision=="ACCEPT")
                    {
                    SaveOrderState();
                    ProcessReply(reply);
                    var billingInfo = service.Where(o => o.ConsumerId == consumerId).FirstOrDefault();
         
                    if (billingInfo==null)
                    {
                        var consumerBillingInformation = service.Create(new mm.ConsumerBillingInformation
                        {
                            FirstName = firstName,
                            LastName = lastName,
                            BillingAddress = billingAddress,
                            BillingZipCode = billingZipCode,
                            BillingContactNumber = phone.ToString(),
                            BillingContactEmail = email,
                            CreditCardNumber = creditCardNumber.ToString(),
                            ExpiryDate = expiryDate,
                            ConsumerId = consumerId
                        });

                        service.Save();
                     }
                
                  
                     mm.Transaction selectedTransaction= serviceTransaction.Get(totals.Id);
                     selectedTransaction.Status = "Paid";
                     serviceTransaction.Save();

                     using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Db"].ConnectionString))
                     using (var command = new SqlCommand("sp_send_dbmail", conn)
                     {
                           CommandType = CommandType.StoredProcedure
                     })
                     {
                         conn.Open();
                         command.Parameters.Add(new SqlParameter("@profile_name", "DBMail Profile"));
                         command.Parameters.Add(new SqlParameter("@recipients", email));
                         command.Parameters.Add(new SqlParameter("@subject", "Some application purchase"));
                         command.Parameters.Add(new SqlParameter("@from_address", ConfigurationManager.AppSettings["EmailHost"]));
                         command.Parameters.Add(new SqlParameter("@Body", "temporary message."));
                         command.ExecuteNonQuery();
                         conn.Close();
                      }
                         return Json(true);
                      }
                      else
                      {
                         return Json(false);
                      }

                    //// To retrieve individual reply fields, follow these examples.
                    //Console.WriteLine("decision = " + reply.decision);
                    //Console.WriteLine("reasonCode = " + reply.reasonCode);
                    //Console.WriteLine("requestID = " + reply.requestID);
                    //Console.WriteLine("requestToken = " + reply.requestToken);
                    //Console.WriteLine("ccAuthReply.reasonCode = " + reply.ccAuthReply.reasonCode);
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
            }
             return Json(false);
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


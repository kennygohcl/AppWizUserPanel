using System;
using System.Collections.Generic;
using System.IO;
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

namespace dFrontierAppWizard.WebUI.Controllers
{
    public class OrderController : Cruder<Order, OrderInput>
    {
        private new readonly IRepo<Transaction> repoTransaction;
        public OrderController(ICrudService<Order> service, IMapper<Order, OrderInput> v, IRepo<Transaction> repoTransaction)
            : base(service, v)
        {
            this.repoTransaction = repoTransaction;
        }

        [HttpPost]
        public string GetOrders(int consumerId)
        {
            var trans = repoTransaction.Where(o => o.ConsumerId == consumerId && o.Status == "Pending").FirstOrDefault();
            var list = service.Where(o => o.ConsumerId == consumerId && o.TransactionId==trans.Id).AsEnumerable();
            var query = from ords in list
                        select new
                        {
                            ords.ConsumerId,
                            ords.DateCreated,
                            ords.DateOfOrder,
                            ords.PromotionId,
                            ords.Quantity,
                            ords.Total,
                            ords.TransactionId
                        };
            var oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string sJson = oSerializer.Serialize(query);
            sJson = @"{""Data"":" + sJson + @",""success"":true}";
            return sJson;
        }


        [HttpPost]
        public string SetOrder(int consumerId, int promotionId, string dateOfOrder, int quantity, Decimal total, DateTime dateCreated)
        {
            var trans = repoTransaction.Where(o => o.ConsumerId == consumerId && o.Status == "Pending").FirstOrDefault();
            int transId = 0;
            DateTime dateOfOrder1 = Convert.ToDateTime(dateOfOrder); 
            if (trans==null)
            {
                var newTrans = repoTransaction.Insert(new Transaction
                {
                    ConsumerId = consumerId,
                    PaymentModeId = 0,
                    DateofTransaction =  DateTime.Now,
                    PaymentReference = "NA",
                    TotalAmount = 0,
                    DiscountOfTotalPercent = 0,
                    Status = "Pending"
                });

                repoTransaction.Save();
              

                var newTransSaved = repoTransaction.Where(m => m.ConsumerId == consumerId && m.Status == "Pending").FirstOrDefault();
                transId = newTransSaved.Id;
            }   
            else
            {
                transId = trans.Id;
            }
         
            var order = service.Create(new Order
            {
                ConsumerId = consumerId,
                PromotionId = promotionId,
                DateOfOrder = dateOfOrder1,
                Quantity = quantity,
                Total = total,
                TransactionId = transId,
                DateCreated = DateTime.Now
            });
            service.Save();

            var od = service.Where(o => o.ConsumerId.Equals(consumerId) && o.TransactionId == transId).OrderByDescending(m => m.Id).FirstOrDefault();
            var oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string sJson = oSerializer.Serialize(od.Id);
            sJson = @"{""Data"":" + sJson + @",""success"":true}";
            return sJson;
            
        }

        protected override string RowViewName
        {
            get { return "ListItems/Order"; }
        }
        

       
    }
}

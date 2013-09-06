using System;

namespace dFrontierAppWizard.Core.Model
{
    public class Order : DelEntity
    {
        public int ConsumerId { get; set; }
        public int PromotionId { get; set; }
        public int TransactionId { get; set; }
        public DateTime DateOfOrder { get; set; }
        public int Quantity { get; set; }
        public Decimal Total { get; set; }
        public DateTime DateCreated { get; set; }
      
    }
}
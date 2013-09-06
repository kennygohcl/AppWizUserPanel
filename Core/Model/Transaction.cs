using System;

namespace dFrontierAppWizard.Core.Model
{
    public class Transaction : DelEntity
    {
        public int ConsumerId { get; set; }
        public int PaymentModeId { get; set; }
        public DateTime DateofTransaction { get; set; }
        public string PaymentReference { get; set; }
        public Decimal TotalAmount { get; set; }
        public int DiscountOfTotalPercent { get; set; }
        public Decimal FinalAmount { get; set; }
        public string Status { get; set; }
    }
}
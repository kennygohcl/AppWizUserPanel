using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dFrontierAppWizard.Core.Model
{
    public class UserBillingInformation : DelEntity
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BillingContactEmail { get; set; }
        public string BillingAddress { get; set; }
        public string BillingZipCode { get; set; }
        public string BillingContactNumber { get; set; }
        public string CreditCardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
      
    }
}

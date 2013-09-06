using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dFrontierAppWizard.Core.Model
{
    public class PaymentGateway : DelEntity
    {
      public int UserId { get; set; }
      public string ApiLogin { get; set; }
      public string TransactionKey { get; set; }
      public string Type { get; set; }
    }
}

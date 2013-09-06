using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dFrontierAppWizard.Core.Model
{
   public class ConsumerLoyalty : DelEntity
    {
        public int ConsumerId { get; set; }
        public int ApplicationId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}



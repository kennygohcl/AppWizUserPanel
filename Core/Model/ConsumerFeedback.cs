using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dFrontierAppWizard.Core.Model
{
   public class ConsumerFeedback : DelEntity
    {
        public int ConsumerId { get; set; }
        public string Comments { get; set; }
        public DateTime DatePosted { get; set; }
        public virtual Consumer Consumer { get; set; }
        public string Status { get; set; }
      
    }
}



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dFrontierAppWizard.Core.Model
{
    public class Ad : DelEntity
    {
      public int ApplicationId { get; set; }
      public virtual Application Application { get; set; }
      public string WebSiteReference { get; set; }
      public string  Banner { get; set; }
      public DateTime Start { get; set; }
      public DateTime End { get; set; }
    
    }
}

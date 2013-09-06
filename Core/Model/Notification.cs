using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dFrontierAppWizard.Core.Model
{
  
    public class Notification : DelEntity
    {
      public int ApplicationId { get; set; }
      public int BranchId { get; set; }
      public string MessageTitle { get; set; }
      public string  Message { get; set; }
      public DateTime DateCreated { get; set; }
      public bool IsActive { get; set; }

    }
}

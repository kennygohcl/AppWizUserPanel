using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dFrontierAppWizard.Core.Model
{
    public class Result //Android push notification
    {
        public string message_id { get; set; }
        public string error { get; set; }
        public string registration_id { get; set; }
    }
}

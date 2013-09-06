using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dFrontierAppWizard.Core.Model
{
    public class PostData //Android push notification
    {
        public string collapse_key { get; set; }
        public int time_to_live { get; set; }
        public bool delay_while_idle { get; set; }
        public JsonData data { get; set; }
        public List<string> registration_ids { get; set; }
    }
}

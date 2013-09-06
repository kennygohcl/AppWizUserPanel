using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dFrontierAppWizard.WebUI.Utils
{
    public static class DateConverter
    {



        public static string GetJsonDate(DateTime selectedDate)
        {
             //var baseDate = new DateTime(1970, 1, 1); 
            // var currentDate = DateTime.Now.ToUniversalTime();
           //  TimeSpan ts = new TimeSpan(selectedDate.Ticks - baseDate.Ticks);
           return selectedDate.ToString("MM-dd-yyyy HH:mm:ss");
           

        }
       
    }
}
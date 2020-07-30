using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CovidTracker.Models
{
    public class CovidModel
    {

        public string Country { get; set; }
        public string Cases { get; set; }
        public string TodayCases { get; set; }
        public string deaths { get; set; }
        public string TodayDeaths { get; set; }
        public string recovered { get; set; }
        public string active { get; set; }
        public string critical { get; set; }

       

    }

    public class CovidGlobal
    {
        public string cases { get; set; }
        public string deaths { get; set; }
        public string recovered { get; set; }

    }

    
}

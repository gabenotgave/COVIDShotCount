using System;
using System.Collections.Generic;
using System.Text;

namespace COVIDVaccinationCount.ScrapeObjects
{
    public class CDCAjax
    {
        public string runid { get; set; }
        public CDCVaccinationData[] vaccination_data { get; set; }
    }
}

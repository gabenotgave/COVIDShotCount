using System;
using System.Collections.Generic;
using System.Text;

namespace COVIDVaccinationCount.ScrapeObjects
{
    public class CDCVaccinationData
    {
        public DateTime Date { get; set; }
        public int MMWR_week { get; set; }
        public string Location { get; set; }
        public string ShortName { get; set; }
        public string LongName { get; set; }
        public int Doses_Distributed { get; set; }
        public int Doses_Administered { get; set; }
        public int Dist_Per_100K { get; set; }
        public int Admin_Per_100K { get; set; }
        public int Census2019 { get; set; }
    }
}

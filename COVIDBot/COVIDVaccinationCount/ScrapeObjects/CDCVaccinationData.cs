using System;

namespace COVIDVaccinationCount.ScrapeObjects
{
    public class CDCVaccinationData
    {
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public string ShortName { get; set; }
        public string LongName { get; set; }
        public int Doses_Distributed { get; set; }
        public int Doses_Administered { get; set; }
        public int Administered_Dose1_Recip { get; set; }
        public int Administered_Dose2_Recip { get; set; }
        public int Series_Complete_Yes { get; set; }
        public int Series_Complete_18Plus { get; set; }
        public int Administered_Moderna { get; set; }
        public int Administered_Pfizer { get; set; }
        public int Administered_Janssen { get; set; }
        public int Census2019 { get; set; }
    }
}

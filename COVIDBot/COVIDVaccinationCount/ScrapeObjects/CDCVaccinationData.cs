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
        public int Administered_Dose1 { get; set; }
        public int Administered_Dose1_Per_100K { get; set; }
        public int Administered_Dose2 { get; set; }
        public int Administered_Dose2_Per_100K { get; set; }
        public int Administered_Moderna { get; set; }
        public int Administered_Pfizer { get; set; }
        public int Administered_Unk_Manuf { get; set; }
        public int Dist_Per_100K { get; set; }
        public int Admin_Per_100K { get; set; }
        public decimal Administered_Dose1_Pop_Pct { get; set; }
        public decimal Administered_Dose2_Pop_Pct { get; set; }
        public int Census2019 { get; set; }
    }
}

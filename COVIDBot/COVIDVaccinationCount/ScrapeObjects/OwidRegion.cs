namespace COVIDVaccinationCount.ScrapeObjects
{
    public class OwidRegion
    {
        public string country { get; set; }
        public string iso_code { get; set; }
        public OwidVaccinationData[] data { get; set; }
    }
}

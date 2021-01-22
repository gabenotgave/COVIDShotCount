namespace COVIDVaccinationCount.ScrapeObjects
{
    public class CDCAjax
    {
        public string runid { get; set; }
        public CDCVaccinationData[] vaccination_data { get; set; }
    }
}

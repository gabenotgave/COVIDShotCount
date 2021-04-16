using System;

namespace COVIDVaccinationCount.ScrapeObjects
{
    public class OwidVaccinationData
    {
        public DateTime date { get; set; }
        public int total_vaccinations { get; set; }
        public int people_vaccinated { get; set; }
        public int people_fully_vaccinated { get; set; }
        public int daily_vaccinations_raw { get; set; }
        public int daily_vaccinations { get; set; }
        public decimal total_vaccinations_per_hundred { get; set; }
        public decimal people_vaccinated_per_hundred { get; set; }
        public decimal people_fully_vaccinated_per_hundred { get; set; }
        public int daily_vaccinations_per_million { get; set; }
    }
}

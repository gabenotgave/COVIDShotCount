using COVIDVaccinationCount.ScrapeObjects;

namespace COVIDVaccinationCount
{
    public class Owid
    {
        private OwidRegion[] owidData;
        public Owid()
        {
            this.owidData = ScrapeData.Owid();
        }

        public int GetVaccinatedPeople(string location) => owidData.GetLocationData(location).people_vaccinated;

        public int GetFullyVaccinatedPeople(string location) => owidData.GetLocationData(location).people_fully_vaccinated;

        public int GetTotalVaccinations(string location) => owidData.GetLocationData(location).total_vaccinations;
    }
}

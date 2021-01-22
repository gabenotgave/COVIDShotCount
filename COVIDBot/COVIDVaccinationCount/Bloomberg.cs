using COVIDVaccinationCount.ScrapeObjects;

namespace COVIDVaccinationCount
{
    public class Bloomberg
    {
        private BloombergVaccinationData bloombergData;
        public Bloomberg()
        {
            this.bloombergData = ScrapeData.Bloomberg();
        }

        public int GetDosesAdministered() => bloombergData.Doses_Administered;
    }
}

using COVIDVaccinationCount.ScrapeObjects;
using System;

namespace COVIDVaccinationCount
{
    public class CDC
    {
        private CDCVaccinationData[] cdcData;
        public CDC()
        {
            this.cdcData = ScrapeData.CDC();
        }

        public DateTime GetDate(string location) => cdcData.GetLocationData(location).Date;

        public string GetShortName(string location) => cdcData.GetLocationData(location).ShortName;

        public string GetLongName(string location) => cdcData.GetLocationData(location).LongName;

        public int GetDosesDistributed(string location) => cdcData.GetLocationData(location).Doses_Distributed;

        public int GetDosesAdministered(string location) => cdcData.GetLocationData(location).Doses_Administered;

        public int Get1stDosesAdministered(string location) => cdcData.GetLocationData(location).Administered_Dose1_Recip - cdcData.GetLocationData(location).Administered_Janssen;

        public int Get2ndDosesAdministered(string location) => cdcData.GetLocationData(location).Administered_Dose2_Recip;

        public int GetFullyVaccinated(string location) => cdcData.GetLocationData(location).Series_Complete_Yes;

        public int GetFullyVaccinatedMinors(string location) => cdcData.GetLocationData(location).Series_Complete_Yes - cdcData.GetLocationData(location).Series_Complete_18Plus;

        public int GetPfizerDosesAdministered(string location) => cdcData.GetLocationData(location).Administered_Pfizer;

        public int GetModernaDosesAdministered(string location) => cdcData.GetLocationData(location).Administered_Moderna;

        public int GetJanssenDosesAdministered(string location) => cdcData.GetLocationData(location).Administered_Janssen;

        public int Get2019Census(string location) => cdcData.GetLocationData(location).Census2019;
    }
}

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

        public int Get1stDosesAdministered(string location) => cdcData.GetLocationData(location).Administered_Dose1_Recip;

        public int Get2ndDosesAdministered(string location) => cdcData.GetLocationData(location).Administered_Dose2_Recip;

        public int Get2019Census (string location) => cdcData.GetLocationData(location).Census2019;
    }
}

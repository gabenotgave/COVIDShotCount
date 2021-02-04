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

        public int Get1stDosesAdministered(string location) => cdcData.GetLocationData(location).Administered_Dose1;

        public int Get2ndDosesAdministered(string location) => cdcData.GetLocationData(location).Administered_Dose2;

        public int GetPfizerDosesAdministered(string location) => cdcData.GetLocationData(location).Administered_Pfizer;

        public int GetModernaDosesAdministered(string location) => cdcData.GetLocationData(location).Administered_Moderna;

        public int GetUnkManufDosesAdministered(string location) => cdcData.GetLocationData(location).Administered_Unk_Manuf;

        public int GetDistPer100K(string location) => cdcData.GetLocationData(location).Dist_Per_100K;

        public int GetAdminPer100K(string location) => cdcData.GetLocationData(location).Admin_Per_100K;

        public int Get2019Census (string location) => cdcData.GetLocationData(location).Census2019;
    }
}

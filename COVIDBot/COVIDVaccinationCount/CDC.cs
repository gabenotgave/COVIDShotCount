using COVIDVaccinationCount.ScrapeObjects;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

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

        public int GetMMWRWeek(string location) => cdcData.GetLocationData(location).MMWR_week;

        public string GetShortName(string location) => cdcData.GetLocationData(location).ShortName;

        public string GetLongName(string location) => cdcData.GetLocationData(location).LongName;

        public int GetDosesDistributed(string location) => cdcData.GetLocationData(location).Doses_Distributed;

        public int GetDosesAdministered(string location) => cdcData.GetLocationData(location).Doses_Administered;

        public int GetDistPer100K(string location) => cdcData.GetLocationData(location).Dist_Per_100K;

        public int GetAdminPer100K(string location) => cdcData.GetLocationData(location).Admin_Per_100K;

        public int Get2019Census (string location) => cdcData.GetLocationData(location).Census2019;
    }
}

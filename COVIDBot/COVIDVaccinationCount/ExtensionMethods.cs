using COVIDVaccinationCount.ScrapeObjects;
using System;
using System.Linq;

namespace COVIDVaccinationCount
{
    public static class ExtensionMethods
    {
        public static int StrToInt(this string str) => Int32.Parse(str);

        public static string IntToStrWithComma(this int num) => String.Format("{0:n0}", num);

        public static CDCVaccinationData GetLocationData(this CDCVaccinationData[] cdcData, string location)
        {
            return cdcData.Where(x => x.Location == location).FirstOrDefault();
        }

        public static OwidVaccinationData GetLocationData(this OwidRegion[] owidData, string location)
        {
            return owidData.FirstOrDefault(x => x.country == location).data.OrderByDescending(o => o.date).FirstOrDefault();
        }
    }
}

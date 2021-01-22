using COVIDVaccinationCount.ScrapeObjects;
using System;
using System.Linq;

namespace COVIDVaccinationCount
{
    public static class ExtensionMethods
    {
        public static int StrToInt(this string str)
        {
            return Int32.Parse(str);
        }

        public static string IntToStrWithComma(this int num)
        {
            return String.Format("{0:n0}", num);
        }

        public static CDCVaccinationData GetLocationData(this CDCVaccinationData[] cdcData, string location)
        {
            return cdcData.Where(x => x.Location == location).FirstOrDefault();
        }
    }
}

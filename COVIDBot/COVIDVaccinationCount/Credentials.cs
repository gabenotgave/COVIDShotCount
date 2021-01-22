using System.Configuration;

namespace COVIDVaccinationCount
{
    static class Credentials
    {
        public static string GetValue(string key) => ConfigurationManager.AppSettings.Get(key);
    }
}

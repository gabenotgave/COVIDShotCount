using COVIDVaccinationCount.ScrapeObjects;
using System;
using System.Collections.Generic;
using System.Text;

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

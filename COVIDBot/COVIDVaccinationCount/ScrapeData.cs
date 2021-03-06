﻿using COVIDVaccinationCount.ScrapeObjects;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Text;

namespace COVIDVaccinationCount
{
    static class ScrapeData
    {
        public static CDCVaccinationData[] CDC()
        {
            HtmlDocument siteHtml = RetrieveSiteHtml("https://covid.cdc.gov/covid-data-tracker/COVIDData/getAjaxData?id=vaccination_data");

            return JsonConvert.DeserializeObject<CDCAjax>(siteHtml.Text).vaccination_data;
        }

        public static BloombergVaccinationData Bloomberg()
        {
            HtmlDocument siteHtml = RetrieveSiteHtml("https://www.bloomberg.com/graphics/covid-vaccine-tracker-global-distribution/");

            int dosesCount = Int32.Parse(siteHtml.DocumentNode.SelectNodes("//span[@class='string-replacement']")[0].ToString().Replace(",", ""));

            return new BloombergVaccinationData { Doses_Administered = dosesCount };
        }

        public static OwidRegion[] Owid()
        {
            HtmlDocument siteHtml = RetrieveSiteHtml("https://raw.githubusercontent.com/owid/covid-19-data/master/public/data/vaccinations/vaccinations.json");

            return JsonConvert.DeserializeObject<OwidRegion[]>(siteHtml.Text);
        }

        private static HtmlDocument RetrieveSiteHtml(string url)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.OptionReadEncoding = false;
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                using (var stream = response.GetResponseStream())
                {
                    htmlDoc.Load(stream, Encoding.UTF8);
                }
            }
            return htmlDoc;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Overwatcher
{
    class UrlBuilder
    {
        const string _carrer = "career";
        public UrlBuilder() { }

        public static string BuildFullUrl()
        {
            const string baseURL = "https://playoverwatch.com/en-us/";
            string basePage = String.Format("{0}/{1}/{2}/{3}", _carrer, PlayerInformation.Platform, PlayerInformation.Region, PlayerInformation.Btag);

            return String.Format("{0}{1}", baseURL, basePage);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace Overwatcher.Source.Utils
{
    class HtmlScraper
    {
        public static string XPathScraper(HtmlDocument webPage, string xPath)
        {
            try
            {
                return webPage.DocumentNode.SelectSingleNode(xPath).InnerText;
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("XPathScraper: Trouble collecting competitive information " + e.Message);
                return null;
            }
        }

        public static string HtmlElementScraper(HtmlDocument webPage, string xPath, string element)
        {
            try
            {
                return parseUrlFromHtmlStyle(webPage.DocumentNode.SelectSingleNode(xPath).GetAttributeValue(element, null));
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("HtmlElementScraper: Error scraping HTML element " + e.Message);
                return null;
            }
        }

        private static string parseUrlFromHtmlStyle(string fullStyle)
        {
            var pattern = @"background-image:url";
            var regex = new Regex(pattern);
            var result = regex.Replace(fullStyle, "", 1);
            return result.Trim('(', ')');
        }
    }
}

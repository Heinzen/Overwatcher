using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Overwatcher.Source.Utils;
using System.Xml.Linq;
using System.IO;
using System.Reflection;

namespace Overwatcher
{
    class PlayerDataHandler
    {
        private static string currentSr;
        private static string competitiveMatchesWon;
        private static string competitiveMatchesPlayed;
        private static string prestigeBorderUrl;
        private static string nonPrestigeLevel;
        private static string prestigeRankUrl;

        private static void ParseWebPage()
        {
            HtmlWeb web = new HtmlWeb();

            //TODO Find decent way to do this
            string xpathXmlPath = Directory.GetParent(Directory.GetParent(Directory.GetParent(System.AppDomain.CurrentDomain.BaseDirectory).FullName).FullName).FullName + @"\Assets\Data\XPathElements.xml";
            XDocument xpathXml = XDocument.Load(xpathXmlPath);

            try
            {
                HtmlDocument playOverwatchPage = web.Load(PlayerInformation.WebsiteToScrapeFrom);
                Console.WriteLine("Full URL to parse from: " + PlayerInformation.WebsiteToScrapeFrom);

                //Parses website information
                //TODO change the keys to a separate Key->Value file
                currentSr = HtmlScraper.XPathScraper(playOverwatchPage, getXmlValueByElement(xpathXml, "CurrentSr"));
                competitiveMatchesWon = HtmlScraper.XPathScraper(playOverwatchPage, getXmlValueByElement(xpathXml, "CompetitiveMatchesWon"));
                competitiveMatchesPlayed = HtmlScraper.XPathScraper(playOverwatchPage, getXmlValueByElement(xpathXml, "CompetitiveMatchesTotal"));
                nonPrestigeLevel = HtmlScraper.XPathScraper(playOverwatchPage, getXmlValueByElement(xpathXml, "NonPrestigeLevel"));

                prestigeBorderUrl = HtmlScraper.HtmlElementScraper(playOverwatchPage, getXmlValueByElement(xpathXml, "PrestigeBorder"), "style");
                prestigeRankUrl = HtmlScraper.HtmlElementScraper(playOverwatchPage, getXmlValueByElement(xpathXml, "PrestigeRank"), "style");
            }
            catch(System.Net.WebException e)
            {
                Console.WriteLine("Error loading page: " + e.Message);
            }
            catch(NullReferenceException e)
            {
                Console.WriteLine("Error loading competitive information " + e.Message);
            }
        }

        private static void UpdatePlayerInformation()
        {
            if(currentSr != null)
            {
                PlayerInformation.CurrentSkillRating = Int32.Parse(currentSr);
                if (PlayerInformation.CurrentSkillRating < 1500)
                    PlayerInformation.PlayerRank = PlayerInformation.Rank.BRONZE;
                else if (PlayerInformation.CurrentSkillRating >= 1500 && PlayerInformation.CurrentSkillRating < 2000)
                    PlayerInformation.PlayerRank = PlayerInformation.Rank.SILVER;
                else if (PlayerInformation.CurrentSkillRating >= 2000 && PlayerInformation.CurrentSkillRating < 2500)
                    PlayerInformation.PlayerRank = PlayerInformation.Rank.GOLD;
                else if (PlayerInformation.CurrentSkillRating >= 2500 && PlayerInformation.CurrentSkillRating < 3000)
                    PlayerInformation.PlayerRank = PlayerInformation.Rank.PLATINUM;
                else if (PlayerInformation.CurrentSkillRating >= 3000 && PlayerInformation.CurrentSkillRating < 3500)
                    PlayerInformation.PlayerRank = PlayerInformation.Rank.DIAMOND;
                else if (PlayerInformation.CurrentSkillRating >= 3500 && PlayerInformation.CurrentSkillRating < 4000)
                    PlayerInformation.PlayerRank = PlayerInformation.Rank.MASTER;
                else
                    PlayerInformation.PlayerRank = PlayerInformation.Rank.GRANDMASTER;
            }
            if( competitiveMatchesPlayed != null)
                PlayerInformation.CompetitiveMatches = Int32.Parse(competitiveMatchesPlayed);
            if(competitiveMatchesWon != null)
                PlayerInformation.CompetitiveMatchesWon = Int32.Parse(competitiveMatchesWon);
            if (nonPrestigeLevel != null)
                PlayerInformation.CurrentLevel = Int32.Parse(nonPrestigeLevel);
            if (prestigeBorderUrl != null)
                PlayerInformation.PrestigeBorderUrl = prestigeBorderUrl;
            if (prestigeRankUrl != null)
                PlayerInformation.PrestigeRankUrl = prestigeRankUrl;
            if(competitiveMatchesPlayed != null && competitiveMatchesWon != null)
                PlayerInformation.WinLoseRatio = (float)Math.Round((((float)PlayerInformation.CompetitiveMatchesWon / PlayerInformation.CompetitiveMatches) * 100), 2);
        }

        public static void UpdateAll()
        {
            ParseWebPage();
            UpdatePlayerInformation();
        }

        private static string getXmlValueByElement(XDocument xmlFile, string id)
        {
            var element = from c in xmlFile.Descendants("HTMLXpaths")
                               select c.Elements("xpath").Single(p => p.Attribute("id").Value == id).Value;
            return element.ElementAt(0);
        }
    }
}

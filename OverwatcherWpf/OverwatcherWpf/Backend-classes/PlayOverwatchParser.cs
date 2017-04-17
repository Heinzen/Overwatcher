using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace Overwatcher
{
    class PlayOverwatchParser
    {
        //These are XPath, meaning that if anything changes in Play Overwatch website's change, these must be reviewed
        const string _currentSrWebpageXPath = "//*[@id=\"overview-section\"]/div/div[2]/div/div/div[1]/div/div[2]/div";
        const string _competitiveMatchesWebpageXPath = "//*[@id=\"competitive\"]/section[3]/div/div[2]/div[7]/div/table/tbody/tr[1]/td[2]";
        const string _competitiveMatchesWonWebpageXPath = "//*[@id=\"competitive\"]/section[3]/div/div[2]/div[7]/div/table/tbody/tr[2]/td[2]";
        const string _prestigeBorderXPath = "//*[@id=\"overview-section\"]/div/div[2]/div/div/div[1]/div/div[1]";
        const string _prestigeRankXPath = "//*[@id=\"overview-section\"]/div/div[2]/div/div/div[1]/div/div[1]/div[2]";
        const string _nonPrestigeLevelXPath = "//*[@id=\"overview-section\"]/div/div[2]/div/div/div[1]/div/div[1]/div[1]";

        private static string currentSr;
        private static string competitiveMatchesWon;
        private static string competitiveMatchesPlayed;
        private static string prestigeBorderUrl;
        private static string nonPrestigeLevel;
        private static string prestigeRankUrl;

        private static void ParseWebPage()
        {
            HtmlWeb web = new HtmlWeb();

            try
            {
                HtmlDocument playOverwatchPage = web.Load(PlayerInformation.FullPlayOverwatchUrl);
                Console.WriteLine("Full URL to parse from: " + PlayerInformation.FullPlayOverwatchUrl);

                //Parses website information
                currentSr = ParseCurrentSr(playOverwatchPage);
                competitiveMatchesWon = ParseCompetitiveMatchesWon(playOverwatchPage);
                competitiveMatchesPlayed = ParseCompetitiveMatchesPlayed(playOverwatchPage);
                prestigeBorderUrl = ParsePrestigeBorderUrl(playOverwatchPage);
                prestigeRankUrl = ParsePrestigeRankUrl(playOverwatchPage);
                nonPrestigeLevel = ParseNonPrestigeLevel(playOverwatchPage);
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

        private static string ParseCurrentSr(HtmlDocument webPage)
        {
            try
            {
                return webPage.DocumentNode.SelectSingleNode(_currentSrWebpageXPath).InnerText;
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("ParseCurrentSr: Player has no Skill Rating " + e.Message);
                return null;
            }
        }

        private static string ParseCompetitiveMatchesPlayed(HtmlDocument webPage)
        {
            try
            {
                return webPage.DocumentNode.SelectSingleNode(_competitiveMatchesWebpageXPath).InnerText;
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("ParseCompetitiveMatchesPlayed: Error loading competitive information. " + e.Message);
                return null;
            }
        }

        private static string ParseCompetitiveMatchesWon(HtmlDocument webPage)
        {
            try
            {
                return webPage.DocumentNode.SelectSingleNode(_competitiveMatchesWonWebpageXPath).InnerText;
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("ParseCompetitiveMatchesWon: Error loading competitive information. " + e.Message);
                return null;
            }
        }

        private static string ParsePrestigeBorderUrl(HtmlDocument webPage)
        {
            try
            {
                return parseUrlFromHtmlStyle(webPage.DocumentNode.SelectSingleNode(_prestigeBorderXPath).GetAttributeValue("style", null)); ;
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("ParsePrestigeLevelUrl: Error loading competitive information. " + e.Message);
                return null;
            }
        }

        private static string ParsePrestigeRankUrl(HtmlDocument webPage)
        {
            try
            {
                return parseUrlFromHtmlStyle(webPage.DocumentNode.SelectSingleNode(_prestigeRankXPath).GetAttributeValue("style", null));
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("ParsePrestigeRankUrl: Error loading competitive information. " + e.Message);
                return null;
            }
        }

        private static string ParseNonPrestigeLevel(HtmlDocument webPage)
        {
            try
            {
                return webPage.DocumentNode.SelectSingleNode(_nonPrestigeLevelXPath).InnerText;
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("ParseNonPrestigeLevel: Error loading competitive information. " + e.Message);
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
    }
}

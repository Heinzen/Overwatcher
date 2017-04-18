using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Overwatcher
{
    class PlayerInformation
    {
        public PlayerInformation(string platform, string region, string btag)
        {
            Platform = platform.ToLower();

            if (region == "Americas")
                Region = "us";
            else if (region == "Europe")
                Region = "eu";
            else 
                Region = "kr";

            if(btag.Contains("#"))
                btag = btag.Replace("#", "-");

            Btag = btag;
            Nickname = btag.Substring(0, btag.IndexOf("-"));
            WebsiteToScrapeFrom = UrlBuilder.BuildFullUrl();            
        }

        public PlayerInformation(){ }

        public enum Rank
        {
            BRONZE,
            SILVER,
            GOLD,
            PLATINUM,
            DIAMOND,
            MASTER,
            GRANDMASTER,
            TOP500
        }

        static Rank _playerRank;

        internal static Rank PlayerRank
        {
            get { return PlayerInformation._playerRank; }
            set { PlayerInformation._playerRank = value; }
        }

        static string _nickname;

        public static string Nickname
        {
            get { return PlayerInformation._nickname; }
            set { PlayerInformation._nickname = value; }
        }

        static string _prestigeBorderUrl;

        public static string PrestigeBorderUrl
        {
            get { return PlayerInformation._prestigeBorderUrl; }
            set { PlayerInformation._prestigeBorderUrl = value; }
        }

        static string _prestigeRankUrl;

        public static string PrestigeRankUrl
        {
            get { return PlayerInformation._prestigeRankUrl; }
            set { PlayerInformation._prestigeRankUrl = value; }
        }

        static int _currentLevel;

        public static int CurrentLevel
        {
            get { return PlayerInformation._currentLevel; }
            set { PlayerInformation._currentLevel = value; }
        }
        
        static string _platform;

        public static string Platform
        {
            get { return PlayerInformation._platform; }
            set { PlayerInformation._platform = value; }
        }
        static string _region;

        public static string Region
        {
            get { return PlayerInformation._region; }
            set { PlayerInformation._region = value; }
        }
        static string _btag;

        public static string Btag
        {
            get { return PlayerInformation._btag; }
            set { PlayerInformation._btag = value; }
        }
        static int _currentSkillRating;

        public static int CurrentSkillRating
        {
            get { return PlayerInformation._currentSkillRating; }
            set { PlayerInformation._currentSkillRating = value; }
        }

        static int _competitiveMatchesWon;

        public static int CompetitiveMatchesWon
        {
            get { return PlayerInformation._competitiveMatchesWon; }
            set { PlayerInformation._competitiveMatchesWon = value; }
        }

        static int _competitiveMatches;

        public static int CompetitiveMatches
        {
            get { return PlayerInformation._competitiveMatches; }
            set { PlayerInformation._competitiveMatches = value; }
        }

        static string _webSiteToScrapeFrom;

        public static string WebsiteToScrapeFrom
        {
            get { return PlayerInformation._webSiteToScrapeFrom; }
            set { PlayerInformation._webSiteToScrapeFrom = value; }
        }

        static float _winLoseRatio;

        public static float WinLoseRatio
        {
            get { return PlayerInformation._winLoseRatio; }
            set { PlayerInformation._winLoseRatio = value; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Diagnostics;
using HTStats.Models;

namespace HTStats
{
    public class XmlParser
    {
        XElement doc;

        public XmlParser()
        {
            
        }

        //untested
        internal IEnumerable<MatchRating> ParseMatchRatingsData(XElement data)
        {
            List <MatchRatings> mrList = new List<MatchRatings>();
            MatchRatings mrAway = new MatchRatings();
            MatchRatings mrHome = new MatchRatings();

            XElement awayData = data.Element("AwayTeam");
            XElement homeData = data.Element("HomeTeam");

            //parse data for away team
            mrAway.BpFirst = (byte)(int)data.Element("PossessionFirstHalfAway");
            mrAway.BpSecond = (byte)(int)data.Element("PossessionSecondHalfAway");
            mrAway.Formation = (string)awayData.Element("Formation");
            mrAway.Goals = (byte)(int)awayData.Element("AwayGoals");
            mrAway.GoalsAgainst = (byte)(int)homeData.Element("HomeGoals");
            mrAway.MatchId = (int)data.Element("MatchID");
            mrAway.RatingIndirectSpAtt = (byte)(int)awayData.Element("RatingIndirectSpAtt");
            mrAway.RatingIndirectSpDef = (byte)(int)awayData.Element("RatingIndirectSpDef");
            mrAway.RatingLeftAtt = (byte)(int)awayData.Element("RatingLeftAtt");
            mrAway.RatingLeftDef = (byte)(int)awayData.Element("RatingLeftDef");
            mrAway.RatingMidAtt = (byte)(int)awayData.Element("RatingMidLeft");
            mrAway.RatingMidDef = (byte)(int)awayData.Element("RatingMidDef");
            mrAway.RatingMidfield = (byte)(int)awayData.Element("RatingMidfield");
            mrAway.RatingRightAtt = (byte)(int)awayData.Element("RatingRightAtt");
            mrAway.RatingRightDef = (byte)(int)awayData.Element("RatingRightDef");
            mrAway.TacticSkill = (byte)(int)awayData.Element("TacticSkill");
            mrAway.TacticType = (byte)(int)awayData.Element("TacticType");
            //mrAway.TeamAttitude = (byte)(int)awayData.Element("TeamAttitude");  //cannot access unless user approves product
            mrAway.TeamId = (int)awayData.Element("TeamID");

            //parse data for home team
            mrHome.BpFirst = (byte)(int)data.Element("PossessionFirstHalfHome");
            mrHome.BpSecond = (byte)(int)data.Element("PossessionSecondHalfHome");
            mrHome.Formation = (string)homeData.Element("Formation");
            mrHome.Goals = (byte)(int)homeData.Element("HomeGoals");
            mrHome.GoalsAgainst = (byte)(int)awayData.Element("AwayGoals");
            mrHome.MatchId = (int)data.Element("MatchID");
            mrHome.RatingIndirectSpAtt = (byte)(int)homeData.Element("RatingIndirectSpAtt");
            mrHome.RatingIndirectSpDef = (byte)(int)homeData.Element("RatingIndirectSpDef");
            mrHome.RatingLeftAtt = (byte)(int)homeData.Element("RatingLeftAtt");
            mrHome.RatingLeftDef = (byte)(int)homeData.Element("RatingLeftDef");
            mrHome.RatingMidAtt = (byte)(int)homeData.Element("RatingMidLeft");
            mrHome.RatingMidDef = (byte)(int)homeData.Element("RatingMidDef");
            mrHome.RatingMidfield = (byte)(int)homeData.Element("RatingMidfield");
            mrHome.RatingRightAtt = (byte)(int)homeData.Element("RatingRightAtt");
            mrHome.RatingRightDef = (byte)(int)homeData.Element("RatingRightDef");
            mrHome.TacticSkill = (byte)(int)homeData.Element("TacticSkill");
            mrHome.TacticType = (byte)(int)homeData.Element("TacticType");
            //mrHome.TeamAttitude = (byte)(int)homeData.Element("TeamAttitude"); //cannot access unless user approves product
            mrHome.TeamId = (int)homeData.Element("TeamID");


            //parse result of match
            if ((int)awayData.Element("AwayGoals") < (int)homeData.Element("HomeGoals"))
            {   
                //away team lost
                mrAway.Result = 2;
                //home team won
                mrHome.Result = 1;
            }
            else if ((int)awayData.Element("AwayGoals") > (int)homeData.Element("HomeGoals"))
            {
                //away team win
                mrAway.Result = 1;
                //home team lost
                mrHome.Result = 2;
            }
            else
            {
                //draw
                mrAway.Result = 0;
                //draw
                mrHome.Result = 0;
            }

            mrList.Add(mrAway);
            mrList.Add(mrHome);

            return (IEnumerable<MatchRating>)mrList;
        }

        //get team IDs from leagueDetails.xml file
        internal List<int> ParseTeamIDs(string data)
        {
            List<int> teamIDs = new List<int>();
            XElement doc = XElement.Parse(data);
            IEnumerable <XElement> teams = doc.Elements("Team");

            foreach(XElement el in teams)
            {
                teamIDs.Add((int)el.Element("TeamID")); 
            }
            return teamIDs;
        }

        //Parses a Match element from MatchArchive.xml and returns a partially pupolated MatchInfo obj.
        //Missing fields: addedMinutes & weatherID
        internal MatchInfo ParseMatchInfoData(XElement data)
        {
            MatchInfo mi = new MatchInfo();
            
            mi.AwayGoals = (byte)(int)data.Element("AwayGoals");
            mi.AwayTeamId = (int)data.Element("AwayTeam").Element("AwayTeamID");
            mi.CupLevel = (byte)(int)data.Element("CupLevel");
            mi.CupLevelIndex = (byte)(int)data.Element("CupLevelIndex");
            mi.HomeGoals = (byte)(int)data.Element("HomeGoals");
            mi.HomeTeamId = (int)data.Element("HomeTeam").Element("HomeTeamID");
            mi.MatchContextId = (int)data.Element("MatchContextId");
            mi.MatchDate = (DateTime)data.Element("MatchDate");
            mi.MatchId = (int)data.Element("MatchID");
            mi.MatchRuleId = (byte)(int)data.Element("MatchRuleId");
            mi.MatchType = (byte)(int)data.Element("MatchType");
            
            return mi;
        }

        //untested
        internal List<Event> ParseEventData(XElement data, List<short> eventCodes)
        {
            List<Event> eList = new List<Event>();            
            IEnumerable<XElement> events = data.Element("EventList").Elements("Event");

            //traverse all events reported
            foreach (XElement el in events)
            {
                short eCode = (short) el.Element("EventTypeId");
                if (eventCodes.Contains(eCode)) //we track this event type
                {
                    Event e = new Event();
                    e.EventTypeId = (short)eCode;
                    e.MatchId = (int)data.Element("MatchID");
                    e.Matchpart = (byte)(int)el.Element("MatchPart");
                    e.Minute = (byte)(int)el.Element("Minute");
                    e.ObjectPlayerId = (int)el.Element("ObjectPlayerID");
                    e.SubjectPlayerId = (int)el.Element("SubjectPlayerID");
                    e.SubjectTeamId = (int)el.Element("SubjectTeamID");

                    eList.Add(e);
                }
            }
            return eList;
        }

        //untested
        internal List<Goal> ParseGoalData(XElement data)
        {
            List<Goal> gList = new List<Goal>();
            IEnumerable<XElement> goals = data.Element("Scorers").Elements("Goal");

            foreach (XElement el in goals)
            {
                Goal g = new Goal();
                g.AwayGoals = (byte)(int)el.Element("ScorerHomeGoals");
                g.HomeGoals = (byte)(int)el.Element("ScorerAwayGoals");
                g.MatchId = (int)el.Element("MatchID");
                g.MatchPart = (byte)(int)el.Element("matchPart");
                g.Minute = (byte)(int)el.Element("ScorerMinute");
                g.PlayerId = (byte)(int)el.Element("ScorerPlayerID");
                g.HomeTeamId = (int)data.Element("HomeTeam").Element("HomeTeamID");
                g.AwayTeamId = (int)data.Element("AwayTeam").Element("AwayTeamID");

                gList.Add(g);
            }
            return gList;
        }

        //untested
        internal List<Booking> ParseBookingData(XElement data)
        {
            List<Booking> bList = new List<Booking>();
            IEnumerable<XElement> bookings = data.Element("Bookings").Elements("Booking");

            foreach(XElement el in bookings)
            {
                Booking b = new Booking();
                b.MatchId = (int)data.Element("MatchID");
                b.MatchPart = (byte)(int)el.Element("MatchPart");
                b.Minute = (byte)(int)el.Element("BookingMinute");
                b.PlayerId = (int)el.Element("BookingPlayerID");
                b.TeamId = (int)el.Element("BookingTeamID");
                b.Type = (byte)(int)el.Element("BookingType");

                bList.Add(b);
            }
            return bList;
        }

        //untested
        internal List<Injury> ParseInjuryData(XElement data)
        {
            List<Injury> iList = new List<Injury>();
            IEnumerable<XElement> injuries = data.Element("Injuries").Elements("Injury");

            foreach(XElement el in injuries)
            {
                Injury i = new Injury();
                i.InjuryType = (byte)(int)el.Element("InjuryType");
                i.MatchId = (int)data.Element("MatchID");
                i.MatchPart = (byte)(int)el.Element("MatchPart");
                i.Minute = (byte)(int)el.Element("InjuryMinute");
                i.PlayerId = (int)el.Element("InjuryPlayerID");

                iList.Add(i);
            }
            return iList;
        }

       
        //finds a specific <TEAM> element based on TeamID from TeamDetails.xml file and returns a Team object.
        internal Team ParseTeamData(int teamId, string data)
        {
            Team team = new Team();
            try {
                doc = GetTeamXml(teamId, data);
            }
            catch (Exception e) {
                Debug.WriteLine(e.ToString());
            }
            
            try
            {
                XElement rankingInfo = doc.Element("PowerRating");

                team.TeamId = (int)doc.Element("TeamID");
                team.SeriesId = (int)doc.Element("LeagueLevelUnit").Element("LeagueLevelUnitID");
                team.PowerRanking = (int)rankingInfo.Element("PowerRating");
                team.LeagueRanking = (int)rankingInfo.Element("LeagueRanking");
                team.LeagueLevel = (byte)(int)doc.Element("LeagueLevelUnit").Element("LeagueLevel");
                team.LeagueId = (int)doc.Element("League").Element("LeagueID");
                team.GlobalRanking = (int)rankingInfo.Element("GlobalRanking");
                team.CountryId = (int)doc.Element("Country").Element("CountryID");
                team.FoundedDate = (DateTime)doc.Element("FoundedDate");
                team.Trainer = (int)doc.Element("Trainer").Element("PlayerID");
            }
            catch (Exception e)
            {
                Debug.WriteLine("ERROR PARSING TEAM: " + doc);
                Debug.WriteLine("BY ERROR: " + e);
            }
            return team;
        }
        
        // Parsing CHPP file "League Details.xml"
        public League ParseLeagueData(string data)
        {
            League league = new League();
            doc = XElement.Parse(data);

            bool noSuchLeague = (doc.Element("Error") == null ? false : true); //true if no error element found

            if (!noSuchLeague)
            {
                try
                {
                    league.LeagueId = (int)doc.Element("LeagueID");
                    league.LeagueLevel = (byte)(double)doc.Element("LeagueLevel");
                    league.SeriesId = (int)doc.Element("LeagueLevelUnitID");
                }
                catch (Exception e)
                {
                    Debug.WriteLine("ERROR PARSING LEAGUE: "  + data);
                    Debug.WriteLine("NoSuchLeague = " + noSuchLeague);
                }
            }
            return league;
        }

        // Parsing CHPP file "WorldDetails.xml"
        public List<LeagueInfo> ParseLeagueInfoData(string data)
        {
            List<LeagueInfo> leagueList = new List<LeagueInfo>();
            doc = XElement.Parse(data);
            doc = doc.Element("LeagueList");

            IEnumerable<XElement> leagues = doc.Elements("League");
            Debug.WriteLine("League Element Count: " + leagues.Count());
            foreach (XElement el in leagues) {
                Debug.WriteLine("NAME: " + (string)el.Element("EnglishName"));
                LeagueInfo leagueInfo = new LeagueInfo();
                leagueInfo.LeagueId = (int)el.Element("LeagueID");
                leagueInfo.LeagueName = (string)el.Element("EnglishName");
                leagueInfo.MaxLeagueLevel = (byte)(double)el.Element("NumberOfLevels");
                leagueInfo.Season = (int)el.Element("Season");
                leagueList.Add(leagueInfo);
            }
            return leagueList;
        }

        //takes a MatchArchive.xml file and returns all matches
        public IEnumerable<XElement> GetMatchesFromArchive(string data)
        {
            doc = XElement.Parse(data);
            doc = doc.Element("Team").Element("MatchList");

            return doc.Elements("Match");
        }

        internal int GetTrainerID(int teamID, string data)
        {
            XElement el = GetTeamXml(teamID, data);
            return (int)el.Element("Trainer").Element("PlayerID");
        }

        internal DateTime GetFoundedDate(int teamID, string data)
        {
            XElement el = GetTeamXml(teamID, data);
            return (DateTime)el.Element("FoundedDate");
        }

        internal bool IsBotTeam(int teamID, string data)
        {
            XElement el = GetTeamXml(teamID, data);
                        
            string isBot = (string)el.Element("BotStatus").Element("IsBot");
            //Debug.WriteLine("parsing team data for: " + teamID + " which is " + isBot);
            if (isBot.Equals("True"))
                return true;                    
            else            
                return false;
        }

        //returns a single team structure from TeamDetails.xml file
        private XElement GetTeamXml(int teamID, string data)
        {
            doc = XElement.Parse(data);
            IEnumerable<XElement> teams = doc.Element("Teams").Elements("Team");

            foreach (XElement el in teams)
            {
                //data contains up to 3 teams, make sure we have the right one
                if (teamID == (int)el.Element("TeamID"))
                {
                    return el;
                }
            }
            throw new Exception("Did not find teamID " + teamID + " in provided xml: " + data);
        }
    }
}

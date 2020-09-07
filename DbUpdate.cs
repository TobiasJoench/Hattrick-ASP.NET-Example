using HTStats.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HTStats
{
    //Used intermittently and only by changing source code to update the HT data
    public class DbUpdate
    {
        ht_stats_dk_dbContext context;  //Refactor to remove for better performance (see UpdateTeams method).
        XmlParser parser;
        ChppAccess access;
        List<short> eventCodes;  //TODO: Load from DB
        List<int> matchTypes;
        public DbUpdate(int DBUpdateType)
        {
            // create CHPP access, XML parser and Entity framework context
            access = new ChppAccess();
            parser = new XmlParser();
            context = new ht_stats_dk_dbContext(); //Refactor to remove for better performance (see UpdateTeams method).
            eventCodes = GetEventTypes();
            matchTypes = GetMatchTypeList();

            //decide what to update
            switch (DBUpdateType)
            {
                case 1: //update league_info table
                    Debug.WriteLine("Updating league_info table");
                    UpdateLeagueInfo(access.session, parser, context);
                    break;
                case 2: //update leauge table
                    Debug.WriteLine("Updating league table");
                    UpdateLeagues(access.session, parser);
                    break;
                case 3: //update team table
                    Debug.WriteLine("Updating team tables");
                    UpdateTeams(access.session, parser, context);
                    break;
                case 4: //update match_info
                    Debug.WriteLine("Updating tables related to matches");
                    UpdateMatchInfo(access.session, parser, context);
                    break;
                default:
                    Debug.WriteLine("Incorrect update code");
                    break;
            }
        }

       
        //partial update - missing fields: weatherID and added_minutes
        private void UpdateMatchInfo(HttpClient session, XmlParser parser, ht_stats_dk_dbContext context)
        {
            
            int fetchSeason = 0;
            int numGoBackSeasons = 1;
            int numTeamsPerRun = 1;
            int numTeamsTaken = 0;
            int startAtTeamID = 237833;  //usually 0, but handy if process breaks

            //figure out how many teams' matches to fetch
            int teamCount = (from t in context.Team
                             where t.TeamId > startAtTeamID
                             select t).Count();

            while (numTeamsTaken <= teamCount)
            {
                using (ht_stats_dk_dbContext db = new ht_stats_dk_dbContext())
                {
                    // get a number of teams from team table
                    var data = (from p in db.Team
                                where p.LeagueLevel <= 5 && p.TeamId > startAtTeamID
                                orderby p.TeamId ascending
                                select p).Skip(numTeamsTaken).Take(numTeamsPerRun).ToList();                    

                    foreach (Team t in data)
                    {
                        //check that team is still in a league level we care about
                        if (t.LeagueLevel <= 5)
                        {
                            //find the active season of the team's league
                            var seasonResult = from t1 in db.Team
                                               join t2 in db.LeagueInfo
                                               on t1.LeagueId equals t2.LeagueId
                                               where t1.TeamId == t.TeamId
                                               select t2.Season;

                            //figure the lastest valid season
                            int i = seasonResult.First() - numGoBackSeasons;
                            while (i < 1) { i++; }
                            fetchSeason = i;
                            Debug.WriteLine("Getting matches for team: " + t.TeamId + ", season: " + fetchSeason);

                            //TODO: Check that season's matches aren't already fetched (add DB field 'earliest season fetched')

                            //get the matches
                            string matchArchive = GetMatchArchiveData(t.TeamId, fetchSeason, session);
                            IEnumerable<XElement> matches = parser.GetMatchesFromArchive(matchArchive);

                            //parse all the matches for this team in the given season
                            foreach (XElement el in matches)
                            {
                                //check that we are interested in this match
                                if (matchTypes.Contains((int)el.Element("MatchType")))
                                {
                                    //make sure we don't have that match already - slow :-/
                                    if (GetMatchInfo((int)el.Element("MatchID")) == null)
                                    {
                                        //Finally! Add the new match.
                                        db.MatchInfo.Add(parser.ParseMatchInfoData(el));
                                    }
                                }
                            }

                            try //to update the database
                            {
                                db.SaveChanges();
                                //track number of teams processed
                                numTeamsTaken += numTeamsPerRun;
                            }
                            catch (Exception e)
                            {
                                Debug.WriteLine("ERROR SAVING MATCH INFO: " + e.ToString());
                                //TODO: implement proper logging
                            }
                        }
                    }
                    Debug.WriteLine("UPDATED MATCHES FOR " + data.Count() + " TEAMS. " + (teamCount - numTeamsTaken) + " TEAMS LEFT.");
                }
            }
            Debug.WriteLine("Updating match info completed successfully!!");
        }

       
        //Update Team table based on TeamDetails.xml
        private void UpdateTeams(HttpClient session, XmlParser parser, ht_stats_dk_dbContext context)
        {
            List<int> teamIDs = new List<int>();
            int updated = 0;
            int added = 0;

            // get a number of series from League table - move to method?
            using (ht_stats_dk_dbContext db = new ht_stats_dk_dbContext())
            {
                var data = (from s in db.League
                            where s.LeagueLevel <= 5
                            orderby s.SeriesId ascending
                            select s).Skip(0).Take(1); //refactor to approach used in UpdateMatchInfo().

                //for every series, get team IDs
                foreach (League l in data)
                {
                    string leagueData = GetLeagueUnitData(l.SeriesId, session);
                    teamIDs.AddRange(parser.ParseTeamIDs(leagueData));
                }
                Debug.WriteLine("Getting data on " + teamIDs.Count + " teams!");
            }

            using (ht_stats_dk_dbContext db = new ht_stats_dk_dbContext())
            {
                //for every team in league, get team data
                foreach (int id in teamIDs)
                {
                    string teamData = GetTeamData(id, session);
                    if (teamData == null)
                    {
                        Debug.WriteLine("no teamdata at team ID: " + id);
                        break;
                    }
                    bool existingTeam = db.Team.Where(c => c.TeamId == id).Count() == 1;
                    //do not store bot teams
                    if (!parser.IsBotTeam(id, teamData))
                    {
                        //add or update team 
                        if (!existingTeam)
                        {
                            db.Add(parser.ParseTeamData(id, teamData));  //new team
                            added++;
                            //Debug.WriteLine("Added team: " + id);
                        }
                        else
                        {
                            db.Update(parser.ParseTeamData(id, teamData));  //existing team
                            updated++;
                            //Debug.WriteLine("Updated team: " + id);
                        }
                    }
                    else
                    {                        
                        if (existingTeam) 
                        {
                            //we encountered a bot team in our db
                            //it became bot since first fetch, update to maintain data integrity (matches)
                            db.Update(parser.ParseTeamData(id, teamData));
                            updated++;
                            //Debug.WriteLine("Updated BOT team: " + id);
                        }
                    }
                }
                Debug.WriteLine("Updating " + updated + " teams");
                Debug.WriteLine("Adding " + added + " teams");
                Debug.WriteLine("Skipped " + (teamIDs.Count - updated - added) + " bot teams.");
                try
                {
                    db.SaveChanges();
                    Debug.WriteLine("Database changes saved!");
                }
                catch (Exception e)
                {
                    Debug.WriteLine("DB ERROR: " + e);
                }
            }           
        }

        //Updates league table based on LeagueDetails.xml file
        private void UpdateLeagues(HttpClient session, XmlParser parser)
        {
            int fetchCounter = 0;
            int fetchBatch = 0;
            int fetchMax = 300000;
            int teamID = 1;


            //TODO: Add USING context ------ UNTESTED!


            //Iterate through all leagues 
            for (teamID = 1; teamID <= fetchMax; )  //last max ~270.000
            {
                //fetch in batches to keep db context slim
                for (fetchCounter = 0; fetchCounter <= fetchBatch; )
                {
                    
                    using (ht_stats_dk_dbContext db = new ht_stats_dk_dbContext())
                    {
                        string leagueData = GetLeagueUnitData(teamID, session);
                        //break when no new league is fetched (assumed max reached)
                        if (leagueData == null)
                            break;

                        League league = parser.ParseLeagueData(leagueData);

                        //is this league stored already?
                        bool existingLeague = context.League.Where(c => c.SeriesId == league.SeriesId).Count() == 1;


                        if (existingLeague)
                            context.League.Update(league);
                        else
                            context.League.Add(league);

                        if (fetchCounter >= fetchBatch)
                        {
                            try
                            {
                                context.SaveChanges();
                                Debug.WriteLine("Database changes saved!");
                            }
                            catch (Exception e)
                            {
                                Debug.WriteLine("DB ERROR AT TEAMID " + teamID + ": " + e);
                                break;
                            }
                        }
                        fetchCounter++;
                        teamID++;
                    }
                }
            }
            Debug.WriteLine("Total Leagues Fetched: " + teamID);
        }

        //Update LeagueInfo table based on WorldDetails.xml file
        private void UpdateLeagueInfo(HttpClient session, XmlParser parser, ht_stats_dk_dbContext context)
        {
            string leagueData = GetLeagueData(session);

            List<LeagueInfo> leagueList = parser.ParseLeagueInfoData(leagueData);
            Debug.WriteLine("LeagueInfo Count: " + leagueList.Count());

            foreach (LeagueInfo league in leagueList)
            {
                //add or update entity
                if (context.LeagueInfo.Where(c => c.LeagueId == league.LeagueId).Count() == 0) //   Find(league.LeagueId) == null)
                {
                    context.Add(league);
                    Debug.WriteLine("Adding: " + league.LeagueId);
                }
                else
                    context.Update(league);
            }

            try
            {
                context.SaveChanges();
                Debug.WriteLine("Database changes saved!");
            }
            catch (Exception e)
            {
                Debug.WriteLine("DB ERROR: " + e);
            }
        }

        private MatchInfo GetMatchInfo(int matchID)
        {
            var result = from m in context.MatchInfo
                         where m.MatchId == matchID
                         select m;
            return result.FirstOrDefault();
        }

        public List<short> GetEventTypes()
        {
            
           var resultList = from eTypes in context.EventType
                        orderby eTypes.EventTypeId
                        select eTypes.EventTypeId;
                      
           return resultList.ToList();
        }

        //Dammit. Can't make the dates work :-/
        private string GetMatchArchiveData(int teamID, int season, HttpClient session)
        {
            string query = "https://chpp.hattrick.org/chppxml.ashx?file=matchesarchive&version=1.4&teamID=" + teamID + "&season="  + season;
            //string query = Uri.EscapeUriString("https://chpp.hattrick.org/chppxml.ashx?file=matchesarchive&version=1.4&teamID=59&FirstMatchDate=2020-04-01 00:00:00&LastMatchDate=2020-04-07 00:00:00");
            //string query = "https://chpp.hattrick.org/chppxml.ashx?file=matchesarchive&version=1.4&teamID=" + teamID + "&FirstMatchDate=" + fromDate + "&LastMatchDate=" + toDate;
            //Debug.WriteLine("query: " + query);                                                                                                                         //teamID=50596&FirstMatchDate=2020-04-01 00:00:00&LastMatchDate=2020-04-07 00:00:00
            string result = Get(query, session).Result;

            return result;
        }

        private string GetMatchData(int matchID, HttpClient session)
        {
            string query = "https://chpp.hattrick.org/chppxml.ashx?file=matchdetails&version=3.0&matchEvents=true&matchID=" + matchID;

            string result = Get(query, session).Result;

            return result;
        }


        //Fetch team data by TeamID. Returns up to 3 teams!
        private string GetTeamData(int id, HttpClient session)
        {
            string query = "https://chpp.hattrick.org/chppxml.ashx?file=teamdetails&version=3.5&teamID=" + id;

            string result = Get(query, session).Result;

            return result;
        }


        private string GetLeagueUnitData(int seriesID, HttpClient session)
        {
            string query = "https://chpp.hattrick.org/chppxml.ashx?file=leaguedetails&version=1.5&leagueLevelUnitID=" + seriesID;

            string result = Get(query, session).Result;

            return result;
        }

        private string GetLeagueData(HttpClient session)
        {
            string query = "https://chpp.hattrick.org/chppxml.ashx?file=worlddetails&version=1.8&includeRegions=false";

            string result = Get(query, session).Result;

            return result;
        }

        //Leave async scheme
        public static async Task<string> Get(string queryString, HttpClient session)
        {
            // The actual Get method
            using (var result = await session.GetAsync(queryString))
            {
                string content = await result.Content.ReadAsStringAsync();
                return content;
            }
        }

        private List<int> GetMatchTypeList()
        {
            List<int> resultList = new List<int>();
            resultList.Add(1); //   1   League match
            resultList.Add(2); //   2   Qualification match
            resultList.Add(3); //   3   Cup match(standard league match)
            resultList.Add(7); //   7   Hattrick Masters
            resultList.Add(10); // 10   National teams competition match(normal rules)
            resultList.Add(11); // 11   National teams competition match(cup rules)

            return resultList;
            /*  4   Friendly(normal rules)
                5   Friendly(cup rules)
                6   Not currently in use, but reserved for international competition matches with normal rules(may or may not be implemented at some future point).
                8   International friendly(normal rules)
                9   Internation friendly(cup rules)
                12  National teams friendly
                50  Tournament League match
                51  Tournament Playoff match
                61  Single match
                62  Ladder match
                80  Preparation match
                100 Youth league match
                101 Youth friendly match
                102 RESERVED
                103 Youth friendly match(cup rules)
                104 RESERVED
                105 Youth international friendly match
                106 Youth international friendly match(Cup rules)
                107 RESERVED
            */
        }

    }
}

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HTStats.Models
{
    public partial class ht_stats_dk_dbContext : DbContext
    {
        public ht_stats_dk_dbContext()
        {
        }

        public ht_stats_dk_dbContext(DbContextOptions<ht_stats_dk_dbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Booking> Booking { get; set; }
        public virtual DbSet<Event> Event { get; set; }
        public virtual DbSet<EventType> EventType { get; set; }
        public virtual DbSet<Goal> Goal { get; set; }
        public virtual DbSet<Injury> Injury { get; set; }
        public virtual DbSet<League> League { get; set; }
        public virtual DbSet<LeagueInfo> LeagueInfo { get; set; }
        public virtual DbSet<MatchInfo> MatchInfo { get; set; }
        public virtual DbSet<MatchRating> MatchRating { get; set; }
        public virtual DbSet<Player> Player { get; set; }
        public virtual DbSet<Team> Team { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySQL("YOUR CONNECTION STRING. SeeSS appsettings.json file");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("booking");

                entity.HasComment("from matchDetails.xml");

                entity.HasIndex(e => e.PlayerId)
                    .HasName("player_id");

                entity.Property(e => e.MatchId)
                    .HasColumnName("match_id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.MatchPart)
                    .HasColumnName("match_part")
                    .HasColumnType("tinyint(1) unsigned");

                entity.Property(e => e.Minute)
                    .HasColumnName("minute")
                    .HasColumnType("tinyint(3) unsigned");

                entity.Property(e => e.PlayerId)
                    .HasColumnName("player_id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.TeamId)
                    .HasColumnName("team_id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasColumnType("tinyint(1) unsigned");
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("event");

                entity.HasComment("data from 'MatchDetails'");

                entity.HasIndex(e => e.EventTypeId)
                    .HasName("event_type_id");

                entity.Property(e => e.EventTypeId)
                    .HasColumnName("event_type_id")
                    .HasColumnType("smallint(5) unsigned");

                entity.Property(e => e.MatchId)
                    .HasColumnName("match_id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.Matchpart)
                    .HasColumnName("matchpart")
                    .HasColumnType("tinyint(3) unsigned");

                entity.Property(e => e.Minute)
                    .HasColumnName("minute")
                    .HasColumnType("tinyint(3) unsigned");

                entity.Property(e => e.ObjectPlayerId)
                    .HasColumnName("object_player_id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.SubjectPlayerId)
                    .HasColumnName("subject_player_id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.SubjectTeamId)
                    .HasColumnName("subject_team_id")
                    .HasColumnType("int(10) unsigned");
            });

            modelBuilder.Entity<EventType>(entity =>
            {
                entity.ToTable("event_type");

                entity.Property(e => e.EventTypeId)
                    .HasColumnName("event_type_id")
                    .HasColumnType("smallint(4) unsigned");

                entity.Property(e => e.EventName)
                    .IsRequired()
                    .HasColumnName("event_name")
                    .HasMaxLength(120)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Goal>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("goal");

                entity.HasComment("from matchDetails.xml");

                entity.HasIndex(e => e.MatchId)
                    .HasName("match_id");

                entity.Property(e => e.AwayGoals)
                    .HasColumnName("away_goals")
                    .HasColumnType("tinyint(2) unsigned");

                entity.Property(e => e.AwayTeamId)
                    .HasColumnName("away_team_id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.HomeGoals)
                    .HasColumnName("home_goals")
                    .HasColumnType("tinyint(2) unsigned");

                entity.Property(e => e.HomeTeamId)
                    .HasColumnName("home_team_id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.MatchId)
                    .HasColumnName("match_id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.MatchPart)
                    .HasColumnName("match_part")
                    .HasColumnType("tinyint(1) unsigned");

                entity.Property(e => e.Minute)
                    .HasColumnName("minute")
                    .HasColumnType("tinyint(3) unsigned");

                entity.Property(e => e.PlayerId)
                    .HasColumnName("player_id")
                    .HasColumnType("int(10) unsigned");
            });

            modelBuilder.Entity<Injury>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("injury");

                entity.Property(e => e.InjuryType)
                    .HasColumnName("injury_type")
                    .HasColumnType("tinyint(1) unsigned");

                entity.Property(e => e.MatchId)
                    .HasColumnName("match_id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.MatchPart)
                    .HasColumnName("match_part")
                    .HasColumnType("tinyint(1) unsigned");

                entity.Property(e => e.Minute)
                    .HasColumnName("minute")
                    .HasColumnType("tinyint(3) unsigned");

                entity.Property(e => e.PlayerId)
                    .HasColumnName("player_id")
                    .HasColumnType("int(10) unsigned");
            });

            modelBuilder.Entity<League>(entity =>
            {
                entity.HasKey(e => e.SeriesId)
                    .HasName("PRIMARY");

                entity.ToTable("league");

                entity.Property(e => e.SeriesId)
                    .HasColumnName("series_id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.LeagueId)
                    .HasColumnName("league_id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.LeagueLevel)
                    .HasColumnName("league_level")
                    .HasColumnType("tinyint(2) unsigned");
            });

            modelBuilder.Entity<LeagueInfo>(entity =>
            {
                entity.HasKey(e => e.LeagueId)
                    .HasName("PRIMARY");

                entity.ToTable("league_info");

                entity.Property(e => e.LeagueId)
                    .HasColumnName("league_id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.LeagueName)
                    .IsRequired()
                    .HasColumnName("league_name")
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.MaxLeagueLevel)
                    .HasColumnName("max_league_level")
                    .HasColumnType("tinyint(2) unsigned");

                entity.Property(e => e.Season)
                    .HasColumnName("season")
                    .HasColumnType("int(3) unsigned");
            });

            modelBuilder.Entity<MatchInfo>(entity =>
            {
                entity.HasKey(e => e.MatchId)
                    .HasName("PRIMARY");

                entity.ToTable("match_info");

                entity.HasIndex(e => e.CupLevel)
                    .HasName("cup_level");

                entity.HasIndex(e => e.CupLevelIndex)
                    .HasName("cup_level_index");

                entity.HasIndex(e => e.MatchContextId)
                    .HasName("match_context_id");

                entity.HasIndex(e => e.MatchRuleId)
                    .HasName("match_rule_id");

                entity.HasIndex(e => e.MatchType)
                    .HasName("match_type");

                entity.Property(e => e.MatchId)
                    .HasColumnName("match_id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.AddedMinutes)
                    .HasColumnName("added_minutes")
                    .HasColumnType("tinyint(3) unsigned");

                entity.Property(e => e.AwayGoals)
                    .HasColumnName("away_goals")
                    .HasColumnType("tinyint(3) unsigned");

                entity.Property(e => e.AwayTeamId)
                    .HasColumnName("away_team_id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.CupLevel)
                    .HasColumnName("cup_level")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.CupLevelIndex)
                    .HasColumnName("cup_level_index")
                    .HasColumnType("tinyint(3) unsigned");

                entity.Property(e => e.HomeGoals)
                    .HasColumnName("home_goals")
                    .HasColumnType("tinyint(3) unsigned");

                entity.Property(e => e.HomeTeamId)
                    .HasColumnName("home_team_id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.MatchContextId)
                    .HasColumnName("match_context_id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.MatchDate)
                    .HasColumnName("match_date")
                    .HasColumnType("date")
                    .HasComment("finished date");

                entity.Property(e => e.MatchRuleId)
                    .HasColumnName("match_rule_id")
                    .HasColumnType("tinyint(3) unsigned");

                entity.Property(e => e.MatchType)
                    .HasColumnName("match_type")
                    .HasColumnType("tinyint(3) unsigned");

                entity.Property(e => e.WeatherId)
                    .HasColumnName("weather_id")
                    .HasColumnType("tinyint(3) unsigned");
            });

            modelBuilder.Entity<MatchRating>(entity =>
            {
                entity.HasKey(e => new { e.MatchId, e.TeamId })
                    .HasName("PRIMARY");

                entity.ToTable("match_rating");

                entity.HasComment("data from 'MatchDetails'");

                entity.HasIndex(e => e.Result)
                    .HasName("result");

                entity.HasIndex(e => e.TacticType)
                    .HasName("tactic_type");

                entity.HasIndex(e => e.TeamId)
                    .HasName("team_id");

                entity.Property(e => e.MatchId)
                    .HasColumnName("match_id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.TeamId)
                    .HasColumnName("team_id")
                    .HasColumnType("int(11) unsigned");

                entity.Property(e => e.BpFirst)
                    .HasColumnName("bp_first")
                    .HasColumnType("tinyint(3) unsigned");

                entity.Property(e => e.BpSecond)
                    .HasColumnName("bp_second")
                    .HasColumnType("tinyint(3) unsigned");

                entity.Property(e => e.Formation)
                    .IsRequired()
                    .HasColumnName("formation")
                    .HasMaxLength(5)
                    .IsFixedLength();

                entity.Property(e => e.Goals)
                    .HasColumnName("goals")
                    .HasColumnType("tinyint(3) unsigned");

                entity.Property(e => e.GoalsAgainst)
                    .HasColumnName("goals_against")
                    .HasColumnType("tinyint(3) unsigned");

                entity.Property(e => e.RatingIndirectSpAtt)
                    .HasColumnName("rating_indirect_sp_att")
                    .HasColumnType("tinyint(3) unsigned");

                entity.Property(e => e.RatingIndirectSpDef)
                    .HasColumnName("rating_indirect_sp_def")
                    .HasColumnType("tinyint(3) unsigned");

                entity.Property(e => e.RatingLeftAtt)
                    .HasColumnName("rating_left_att")
                    .HasColumnType("tinyint(3) unsigned");

                entity.Property(e => e.RatingLeftDef)
                    .HasColumnName("rating_left_def")
                    .HasColumnType("tinyint(3) unsigned");

                entity.Property(e => e.RatingMidAtt)
                    .HasColumnName("rating_mid_att")
                    .HasColumnType("tinyint(3) unsigned");

                entity.Property(e => e.RatingMidDef)
                    .HasColumnName("rating_mid_def")
                    .HasColumnType("tinyint(3) unsigned");

                entity.Property(e => e.RatingMidfield)
                    .HasColumnName("rating_midfield")
                    .HasColumnType("tinyint(3) unsigned");

                entity.Property(e => e.RatingRightAtt)
                    .HasColumnName("rating_right_att")
                    .HasColumnType("tinyint(3) unsigned");

                entity.Property(e => e.RatingRightDef)
                    .HasColumnName("rating_right_def")
                    .HasColumnType("tinyint(3) unsigned");

                entity.Property(e => e.Result)
                    .HasColumnName("result")
                    .HasColumnType("tinyint(3) unsigned")
                    .HasComment("win 1 / draw 0 / loss 2");

                entity.Property(e => e.TacticSkill)
                    .HasColumnName("tactic_skill")
                    .HasColumnType("tinyint(3) unsigned");

                entity.Property(e => e.TacticType)
                    .HasColumnName("tactic_type")
                    .HasColumnType("tinyint(3) unsigned");

                entity.Property(e => e.TeamAttitude)
                    .HasColumnName("team_attitude")
                    .HasColumnType("tinyint(3) unsigned");
            });

            modelBuilder.Entity<Player>(entity =>
            {
                entity.ToTable("player");

                entity.HasComment("data from 'Player Details'");

                entity.HasIndex(e => e.Age)
                    .HasName("age");

                entity.HasIndex(e => e.Speciality)
                    .HasName("speciality");

                entity.Property(e => e.PlayerId)
                    .HasColumnName("player_id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.Age)
                    .HasColumnName("age")
                    .HasColumnType("smallint(5) unsigned");

                entity.Property(e => e.AgeDays)
                    .HasColumnName("age_days")
                    .HasColumnType("tinyint(3) unsigned");

                entity.Property(e => e.Agreeability)
                    .HasColumnName("agreeability")
                    .HasColumnType("tinyint(3) unsigned");

                entity.Property(e => e.Agressiveness)
                    .HasColumnName("agressiveness")
                    .HasColumnType("tinyint(3) unsigned");

                entity.Property(e => e.Caps)
                    .HasColumnName("caps")
                    .HasColumnType("smallint(5) unsigned");

                entity.Property(e => e.CapsU20)
                    .HasColumnName("caps_u20")
                    .HasColumnType("smallint(5) unsigned");

                entity.Property(e => e.CareerGoals)
                    .HasColumnName("career_goals")
                    .HasColumnType("smallint(5) unsigned");

                entity.Property(e => e.CareerHattricks)
                    .HasColumnName("career_hattricks")
                    .HasColumnType("smallint(5) unsigned");

                entity.Property(e => e.CurrentTeamGoals)
                    .HasColumnName("current_team_goals")
                    .HasColumnType("smallint(5) unsigned");

                entity.Property(e => e.Defender)
                    .HasColumnName("defender")
                    .HasColumnType("tinyint(3) unsigned");

                entity.Property(e => e.FriendliesGoals)
                    .HasColumnName("friendlies_goals")
                    .HasColumnType("smallint(5) unsigned");

                entity.Property(e => e.Honesty)
                    .HasColumnName("honesty")
                    .HasColumnType("tinyint(3) unsigned");

                entity.Property(e => e.Keeper)
                    .HasColumnName("keeper")
                    .HasColumnType("tinyint(3) unsigned");

                entity.Property(e => e.Leadership)
                    .HasColumnName("leadership")
                    .HasColumnType("tinyint(3) unsigned");

                entity.Property(e => e.Loyalty)
                    .HasColumnName("loyalty")
                    .HasColumnType("tinyint(3) unsigned");

                entity.Property(e => e.MatchesCurrentTeam)
                    .HasColumnName("matches_current_team")
                    .HasColumnType("smallint(5) unsigned");

                entity.Property(e => e.MotherclubBonus)
                    .HasColumnName("motherclub_bonus")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Passing)
                    .HasColumnName("passing")
                    .HasColumnType("tinyint(3) unsigned");

                entity.Property(e => e.Playmaker)
                    .HasColumnName("playmaker")
                    .HasColumnType("tinyint(3) unsigned");

                entity.Property(e => e.Salary)
                    .HasColumnName("salary")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Scoring)
                    .HasColumnName("scoring")
                    .HasColumnType("tinyint(3) unsigned");

                entity.Property(e => e.Setpieces)
                    .HasColumnName("setpieces")
                    .HasColumnType("tinyint(3) unsigned");

                entity.Property(e => e.Speciality)
                    .HasColumnName("speciality")
                    .HasColumnType("tinyint(3) unsigned");

                entity.Property(e => e.Stamina)
                    .HasColumnName("stamina")
                    .HasColumnType("tinyint(3) unsigned");

                entity.Property(e => e.TeamId)
                    .HasColumnName("team_id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.TeamLeagueId)
                    .HasColumnName("team_league_id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.TrainerSkill)
                    .HasColumnName("trainer_skill")
                    .HasColumnType("tinyint(4) unsigned");

                entity.Property(e => e.TrainerType)
                    .HasColumnName("trainer_type")
                    .HasColumnType("tinyint(3) unsigned");

                entity.Property(e => e.Tsi)
                    .HasColumnName("tsi")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.Winger)
                    .HasColumnName("winger")
                    .HasColumnType("tinyint(3) unsigned");

                entity.Property(e => e.Xp)
                    .HasColumnName("xp")
                    .HasColumnType("tinyint(3) unsigned");
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.ToTable("team");

                entity.HasIndex(e => e.CountryId)
                    .HasName("CountryID");

                entity.HasIndex(e => e.LeagueId)
                    .HasName("LeagueID");

                entity.HasIndex(e => e.SeriesId)
                    .HasName("SeriesID");

                entity.Property(e => e.TeamId)
                    .HasColumnName("team_id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.CountryId)
                    .HasColumnName("country_id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.FoundedDate).HasColumnName("founded_date");

                entity.Property(e => e.GlobalRanking)
                    .HasColumnName("global_ranking")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.LeagueId)
                    .HasColumnName("league_id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.LeagueLevel)
                    .HasColumnName("league_level")
                    .HasColumnType("tinyint(2) unsigned");

                entity.Property(e => e.LeagueRanking)
                    .HasColumnName("league_ranking")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.PowerRanking)
                    .HasColumnName("power_ranking")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.SeriesId)
                    .HasColumnName("series_id")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("LeaugeLevelUnitID");

                entity.Property(e => e.Trainer)
                    .HasColumnName("trainer")
                    .HasColumnType("int(10)")
                    .HasComment("player id");

                entity.Property(e => e.Updated)
                    .HasColumnName("updated")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

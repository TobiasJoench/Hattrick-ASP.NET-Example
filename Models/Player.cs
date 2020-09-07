using System;
using System.Collections.Generic;

namespace HTStats.Models
{
    public partial class Player
    {
        public int PlayerId { get; set; }
        public int TeamId { get; set; }
        public byte Stamina { get; set; }
        public byte? Keeper { get; set; }
        public byte? Playmaker { get; set; }
        public byte? Scoring { get; set; }
        public byte? Passing { get; set; }
        public byte? Winger { get; set; }
        public byte? Defender { get; set; }
        public byte? Setpieces { get; set; }
        public int TeamLeagueId { get; set; }
        public short Age { get; set; }
        public byte AgeDays { get; set; }
        public byte Xp { get; set; }
        public byte Loyalty { get; set; }
        public byte Leadership { get; set; }
        public byte MotherclubBonus { get; set; }
        public int Tsi { get; set; }
        public int Salary { get; set; }
        public byte Speciality { get; set; }
        public byte Agreeability { get; set; }
        public byte Agressiveness { get; set; }
        public byte Honesty { get; set; }
        public byte? TrainerType { get; set; }
        public byte? TrainerSkill { get; set; }
        public short Caps { get; set; }
        public short CapsU20 { get; set; }
        public short CareerGoals { get; set; }
        public short CareerHattricks { get; set; }
        public short FriendliesGoals { get; set; }
        public short CurrentTeamGoals { get; set; }
        public short MatchesCurrentTeam { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace HTStats.Models
{
    public partial class Event
    {
        public int MatchId { get; set; }
        public byte Minute { get; set; }
        public short EventTypeId { get; set; }
        public int SubjectPlayerId { get; set; }
        public int SubjectTeamId { get; set; }
        public int ObjectPlayerId { get; set; }
        public byte Matchpart { get; set; }
    }
}

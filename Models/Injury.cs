using System;
using System.Collections.Generic;

namespace HTStats.Models
{
    public partial class Injury
    {
        public int MatchId { get; set; }
        public int PlayerId { get; set; }
        public byte InjuryType { get; set; }
        public byte Minute { get; set; }
        public byte MatchPart { get; set; }
    }
}

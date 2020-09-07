using System;
using System.Collections.Generic;

namespace HTStats.Models
{
    public partial class Booking
    {
        public int MatchId { get; set; }
        public int TeamId { get; set; }
        public int PlayerId { get; set; }
        public byte Type { get; set; }
        public byte Minute { get; set; }
        public byte MatchPart { get; set; }
    }
}

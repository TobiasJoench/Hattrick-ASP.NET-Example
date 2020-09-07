using System;
using System.Collections.Generic;

namespace HTStats.Models
{
    public partial class Tester
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public string Result { get; set; }
        public int? TeamId { get; set; }
        public int TesteMigration { get; set; }
    }
}

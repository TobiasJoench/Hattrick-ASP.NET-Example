using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTStats.Data
{
    public class MatchInfoContext : DbContext
    {
        public MatchInfoContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Models.MatchInfo> MatchInfo { get; set; }
    }
}

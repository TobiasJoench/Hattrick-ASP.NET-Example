using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTStats.Data
{
    public class PlayerContext : DbContext
    {

        public DbSet<Models.Player> Player { get; set; }
    }
}

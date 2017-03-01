using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace GameDatabaseProject.Models
{
    public class GameDbEntities : DbContext
    {
        public DbSet<Games> GamesSet { get; set; }
        public DbSet<Distributors> DistributorsSet { get; set; }
        public DbSet<Genre> GenreSet { get; set; }
        public DbSet<Creators> CreatorsSet { get; set; }

    }
    }

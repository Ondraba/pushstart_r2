using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using GameDatabaseProject.Models;
using System.Data.Entity;

namespace GameDatabaseProject.Models
{
    public class GameRepository : IGameRepository
    {
        private Entities currentDbContext;

        public GameRepository(Entities currentDbContext)
        {
            this.currentDbContext = currentDbContext;
        }

        public Entities getCurrentDbContext()
        {
            return this.currentDbContext;
        }

        public IEnumerable<Games> GetGames()
        {
            return getCurrentDbContext().Games.ToList();
        }

        public Games getGameById(int id)
        {
            return getCurrentDbContext().Games.Find(id);
        }

        public void addGame(Games game)
        {
            getCurrentDbContext().Games.Add(game);
        }

        public void removeGame(int id)
        {
            Games gameToRemove = currentDbContext.Games.Find(id);
            getCurrentDbContext().Games.Remove(gameToRemove);
        }

        public void updateGame(Games game)
        {
            getCurrentDbContext().Entry(game).State = EntityState.Modified;
        }

    }
}
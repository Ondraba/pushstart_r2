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

        public IEnumerable<Games> GetGames()
        {
            return currentDbContext.Games.ToList();
        }

        public Games getGameById(int id)
        {
            return currentDbContext.Games.Find(id);
        }

        public void addGame(Games game)
        {
            currentDbContext.Games.Add(game);
        }

        public void removeGame(int id)
        {
            Games gameToRemove = currentDbContext.Games.Find(id);
            currentDbContext.Games.Remove(gameToRemove);
        }

        public void updateGame(Games game)
        {
            currentDbContext.Entry(game).State = EntityState.Modified;
        }

        public void saveGame()
        {
            currentDbContext.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    currentDbContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using GameDatabaseProject.Models;
using System.Data.Entity;

namespace GameDatabaseProject.Models
{
    public class GameRepository : IGameRepository, IPublicGameRepository
    {
        private Entities currentDbContext;
        private List<Games> proposedGames = new List<Games>();

        public GameRepository(Entities currentDbContext)
        {
            this.currentDbContext = currentDbContext;
        }

        private Entities getCurrentDbContext()
        {
            return this.currentDbContext;
        }

        public IEnumerable<Games> GetGames()
        {
            return getCurrentDbContext().Games.ToList();
        }

        public Games getGameById(int? id)
        {
            return getCurrentDbContext().Games.Find(id);
        }

        public void addGame(Games game)
        {
            getCurrentDbContext().Games.Add(game);
        }

        public void proposeNewGame(Games game)
        {
            this.proposedGames.Add(game);
        }

        public IEnumerable<Games> GetProposedGames()
        {
            return proposedGames;
        }

        public void proposedToConfirmedGamesTransform()
        {

        }

        public void removeGameById(int id)
        {
            Games gameToRemove = currentDbContext.Games.Find(id);
            getCurrentDbContext().Games.Remove(gameToRemove);
        }

        public void removeGame(Games game)
        {
            getCurrentDbContext().Games.Remove(game);
        }


        public void updateGame(Games game)
        {
            getCurrentDbContext().Entry(game).State = EntityState.Modified;
        }

    }
}
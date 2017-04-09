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
        private ObjectRepository objectRepository;
      

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

        public ProposedGames getProposedGameById(int? id)
        {
            return getCurrentDbContext().ProposedGames.Find(id);
        }

        public void addGame(Games game)
        {
            getCurrentDbContext().Games.Add(game);
            
        }

        public void proposeNewGame(ProposedGames proposedGame)
        {
             getCurrentDbContext().ProposedGames.Add(proposedGame);
        }

        public IEnumerable<ProposedGames> GetProposedGames()
        {
            return getCurrentDbContext().ProposedGames.ToList();
        }


        public void removeProposedGameById(int id)
        {
            ProposedGames proposedGameToRemove = currentDbContext.ProposedGames.Find(id);
            getCurrentDbContext().ProposedGames.Remove(proposedGameToRemove);
            getCurrentDbContext().SaveChanges();
        }

        public void removeProposedGame(ProposedGames proposedGame)
        {
            getCurrentDbContext().ProposedGames.Remove(proposedGame);
        }


        public void removeGameById(int id)
        {
            Games gameToRemove = currentDbContext.Games.Find(id);
            getCurrentDbContext().Games.Remove(gameToRemove);
            getCurrentDbContext().SaveChanges();
        }

        public void removeGame(Games game)
        {
            getCurrentDbContext().Games.Remove(game);
        }


        public void updateGame(Games game)
        {
            getCurrentDbContext().Entry(game).State = EntityState.Modified;
        }
    
        public void updateProposedGame(ProposedGames proposedGame)
        {
            getCurrentDbContext().Entry(proposedGame).State = EntityState.Modified;
        }

    }
}
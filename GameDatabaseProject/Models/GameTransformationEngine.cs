using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GameDatabaseProject.Models;
using System.Data.Entity;

namespace GameDatabaseProject.Models
{
    public class GameTransformationEngine
    {
        private Entities currentDbContext;
        private DIC dic;

        public  GameTransformationEngine(Entities currentDbContext)
        {
            this.currentDbContext = currentDbContext;
            this.dic = new DIC();
        }

        private Entities getCurrentDbContext()
        {
            return this.currentDbContext;
        }

        public void gameSecuriteStateCheck(ProposedGames proposedGame)
        {
            var state = getCurrentDbContext().ProposedStates.Find(proposedGame.State_Id);
            if (state.State == "Confirmed")
            {
                tableTransfer(proposedGame);
                dic.getGameRepository().removeProposedGame(proposedGame);
            }
        }

        public void tableTransfer(ProposedGames proposedGame)
        {
            Games beforeConvert = new Games();
            beforeConvert.Name = proposedGame.Name;
            beforeConvert.Picture = proposedGame.Picture;
            beforeConvert.ProposedBy = proposedGame.ProposedBy;
            beforeConvert.RankCount = proposedGame.RankCount;
            beforeConvert.RankOveral = proposedGame.RankOveral;
            beforeConvert.DeviceId = proposedGame.DeviceId;
            beforeConvert.Distributor_Id = proposedGame.Distributor_Id;
            beforeConvert.Creator_Id = proposedGame.Creator_Id;
            beforeConvert.Genre_Id = proposedGame.Genre_Id;
            beforeConvert.Year = proposedGame.Year;
            dic.getGameRepository().addGame(beforeConvert);
            dic.getObjectRepository().save();
        }

       
    }
}
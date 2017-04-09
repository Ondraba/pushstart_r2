using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using GameDatabaseProject.Models;
using System.Data.Entity;

namespace GameDatabaseProject.Models
{
    public interface IGameTransformationEngine
    {
        void gameSecuriteStateCheck(ProposedGames proposedGame);
        void tableTransfer(ProposedGames proposedGame);
    }
}

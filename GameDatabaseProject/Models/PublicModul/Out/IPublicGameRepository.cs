using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDatabaseProject.Models
{
    public interface IPublicGameRepository
    {
        void proposeNewGame(ProposedGames proposedGame);
        IEnumerable<Games> GetGames();
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameDatabaseProject.Models;
using System.Data.Entity;


namespace GameDatabaseProject.Models
{
    public interface IGameRepository
    {
        IEnumerable<Games> GetGames();
        Games getGameById(int? id);
        void addGame(Games game);
        void removeGameById(int id);
        void removeGame(Games game);
        void updateGame(Games game);
        IEnumerable<Games> GetProposedGames();
    }
}

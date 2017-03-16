using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GameDatabaseProject.Models;
using System.Data.Entity;

namespace GameDatabaseProject.Models
{
    public class DIC
    {
       private Entities dicContext;

        public  DIC(){
            this.loadConnDic();

        }

        private void loadConnDic()
        {
            ConnDIC connDIC = new ConnDIC();
            this.dicContext = connDIC.returnCurrentConnection();

        }

        Entities getCurrentConnection()
        {
            return this.dicContext;
        }

        public IGameRepository getGameRepository()
        {
            IGameRepository gameRepository = new GameRepository(this.getCurrentConnection());
            return gameRepository;
        }
    }
}
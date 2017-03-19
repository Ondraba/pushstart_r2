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

        public IObjectRepository getObjectRepository()
        {
            IObjectRepository objectRepository = new ObjectRepository(this.getCurrentConnection());
            return objectRepository;
        }


        public IGameRepository getGameRepository()
        {
            IGameRepository gameRepository = new GameRepository(this.getCurrentConnection());
            return gameRepository;
        }

        public IDeviceRepository getDeviceRepository()
        {
            IDeviceRepository deviceRepository = new DeviceRepository(this.getCurrentConnection());
            return deviceRepository;
        }

        public IGenreRepository getGenreRepository()
        {
            IGenreRepository genreRepository = new GenreRepository(this.getCurrentConnection());
            return genreRepository;
        }

    }
}
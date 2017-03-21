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
            ConnDIC connDIC = ConnDIC.returnInstance();
            this.dicContext = connDIC.returnCurrentConnection();
        }

        private Entities getCurrentConnection()
        {
            return this.dicContext;
        }

        public Entities returnCurrentPublicConnection()
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

        public IPublicUser getPublicUserRepository()
        {
            IPublicUser genreRepository = new UserRepository(this.getCurrentConnection());
            return genreRepository;
        }

        public IServerUser getServerUserRepository()
        {
            IServerUser genreRepository = new UserRepository(this.getCurrentConnection());
            return genreRepository;
        }

    }
}
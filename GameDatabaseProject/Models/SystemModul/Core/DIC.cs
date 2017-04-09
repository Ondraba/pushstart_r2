using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GameDatabaseProject.Models;
using System.Data.Entity;

namespace GameDatabaseProject.Models
{
    public class DIC : AdminAcess, PublicAcess
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

        public IPublicGameRepository getPublicGameRepository()
        {
            IPublicGameRepository publicGameRepository = new GameRepository(this.getCurrentConnection());
            return publicGameRepository;
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

        public IMultimedia getMultimedia()
        {
            IMultimedia multimedia = new Multimedia(this.getCurrentConnection());
            return multimedia;
        }

        public ISystemModul getSystemModul()
        {
            ISystemModul systemModul = new SystemModul(this.getCurrentConnection());
            return systemModul;
        }

        public IGameTransformationEngine getTransformationEngine()
        {
            IGameTransformationEngine gameTransformationEngine = new GameTransformationEngine(this.getCurrentConnection());
            return gameTransformationEngine;
        }

    }
}
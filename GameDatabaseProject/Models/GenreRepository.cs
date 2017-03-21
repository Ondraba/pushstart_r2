using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GameDatabaseProject.Models;
using System.Data.Entity;

namespace GameDatabaseProject.Models
{
    public class GenreRepository : IGenreRepository
    {
        private Entities currentDbContext;

        public GenreRepository(Entities currentDbContext)
        {
            this.currentDbContext = currentDbContext;
        }

        private Entities getCurrentDbContext()
        {
            return this.currentDbContext;
        }

        public IEnumerable<Genre> getGenres()
        {
            return getCurrentDbContext().Genre.ToList();
        }

        public void addGenre(Genre genre)
        {
            getCurrentDbContext().Genre.Add(genre);
        }

        public Genre getGenreById(int id)
        {
            return getCurrentDbContext().Genre.Find(id);
        }

        public void removeGenre(int id)
        {
            Device deviceToremove = getCurrentDbContext().Device.Find(id);
            getCurrentDbContext().Device.Remove(deviceToremove);
        }

        public void updateGenre(Genre genre)
        {
            getCurrentDbContext().Entry(genre).State = EntityState.Modified;
        }

    }
}
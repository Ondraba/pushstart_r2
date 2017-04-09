using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDatabaseProject.Models
{
    public interface IGenreRepository
    {
        IEnumerable<Genre> getGenres();
        Genre getGenreById(int id);
        void addGenre(Genre device);
        void removeGenre(int id);
        void updateGenre(Genre genre);
    }
}

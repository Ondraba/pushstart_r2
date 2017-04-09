using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations; //pro restrikce
using System.ComponentModel;

namespace GameDatabaseProject.Models
{
    class Game
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public string Picture { get; set; }

        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

        public DateTime Year { get; set; }

        public virtual Creators CreatorId //foreign key
        {
            get; set;
        }

        public virtual Distributors DistributorId //foreign key
        {
            get; set;
        }


        public virtual Genre GenreId //foreign key
        {
            get; set;
        }


        public int RankCount { get; set; }

        public int Rank { get; set; }

        public List<Reviews> ReviewList {get; set;} 
     
    }
}

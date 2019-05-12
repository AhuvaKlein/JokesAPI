using JokesApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JokesApi.Web.Models
{
    public class IndexViewModel
    {
        public Joke Joke { get; set; }
        public bool Verified { get; set; }
        public User User { get; set; }
        public bool AlreadyLiked { get; set; }
        public bool AlreadyDisliked { get; set; }
        public bool CanStillLike { get; set; }
    }
}

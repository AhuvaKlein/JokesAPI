using System;

namespace JokesApi.Data
{
    public class Like
    {
        public int UserId { get; set; }
        public int JokeId { get; set; }
        public bool Liked { get; set; }
        public DateTime DateLiked { get; set; } 
        public User User { get; set; }
        public Joke Joke { get; set; }
    }

}

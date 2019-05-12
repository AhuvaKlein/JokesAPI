using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;

namespace JokesApi.Data
{
    public class JokeRepository
    {
        private string _connectionString;

        public JokeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Joke GetAJoke()
        {
            var client = new HttpClient();
            string url = $"https://official-joke-api.appspot.com/jokes/programming/random";
            string json = client.GetStringAsync(url).Result;
            var result = JsonConvert.DeserializeObject<IEnumerable<Joke>>(json).First();
            AddJokeToDB(result);
            //returning specific joke for test purposes
            //using (var x = new JokeContext(_connectionString))
            //{
            //    return x.Jokes.FirstOrDefault(j => j.Id == 2);
            //}
                return result;
        }

        private void AddJokeToDB(Joke joke)
        {
            using (var ctx = new JokeContext(_connectionString))
            {
                ctx.Jokes.Add(joke);
                ctx.SaveChanges();
            }
        }

        public IEnumerable<Joke> GetAllJokes()
        {
            using (var ctx = new JokeContext(_connectionString))
            {
                return ctx.Jokes.Include(j => j.Likes).ToList();
            }
        }

        public void ChangeToDislikeJoke(int jokeId, int userId)
        {
            using (var context = new JokeContext(_connectionString))
            {
                context.Database.ExecuteSqlCommand(
                    "UPDATE Likes SET Liked = 'false' WHERE UserId = @userid AND JokeId = @jokeid",
                    new SqlParameter("@userid", userId), new SqlParameter("@jokeid", jokeId));
            }
        }

        public void ChangeToLikeJoke(int jokeId, int userId)
        {
            using (var context = new JokeContext(_connectionString))
            {
                context.Database.ExecuteSqlCommand(
                    "UPDATE Likes SET Liked = 'true' WHERE UserId = @userid AND JokeId = @jokeid",
                    new SqlParameter("@userid", userId), new SqlParameter("@jokeid", jokeId));
            }
        }

        public void LikeDislikeJoke(Like like)
        {
            using (var ctx = new JokeContext(_connectionString))
            {
                if (!ctx.Likes.Any(l => l.JokeId == like.JokeId && l.UserId == like.UserId))
                {
                    ctx.Likes.Add(new Like
                    {
                        UserId = like.UserId,
                        JokeId = like.JokeId,
                        DateLiked = DateTime.Now,
                        Liked = like.Liked
                    });
                }

                UpdateLike(like);
                ctx.SaveChanges();
            }
        }

        public bool AlreadyLiked(Like like)
        {
            using (var ctx = new JokeContext(_connectionString))
            {
                return ctx.Likes.Any(l => l.UserId == like.UserId && l.JokeId == like.JokeId && l.Liked == true);
            }
        }

        public bool AlreadyDisliked(Like like)
        {
            using (var ctx = new JokeContext(_connectionString))
            {
                return ctx.Likes.Any(l => l.UserId == like.UserId && l.JokeId == like.JokeId && l.Liked == false);
            }
        }

        public bool CanStillLike(Like like)
        {
            using (var context = new JokeContext(_connectionString))
            {
                Like li = context.Likes.FirstOrDefault(l => l.UserId == like.UserId && l.JokeId == like.JokeId);
                if (li != null)
                {
                    return li.DateLiked.AddMinutes(5) > DateTime.Now;
                }
                else
                {
                    return true;
                }
            }
        }

        public Like GetLikeInfo(Like like)
        {
            using (var ctx = new JokeContext(_connectionString))
            {
                return ctx.Likes.FirstOrDefault(l => l.UserId == like.UserId && l.JokeId == like.JokeId);
            }
        }

        public void UpdateLike(Like like)
        {
            using (var ctx = new JokeContext(_connectionString))
            {
                ctx.Database.ExecuteSqlCommand(
                    "UPDATE Likes SET Liked = @liked WHERE UserId = @id AND JokeId = @jokeid",
                    new SqlParameter("@id", like.UserId), new SqlParameter("@jokeid", like.JokeId), new SqlParameter("@liked", like.Liked));
            }
        }

    }

}

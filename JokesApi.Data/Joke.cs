using Newtonsoft.Json;
using System.Collections.Generic;

namespace JokesApi.Data
{
    public class Joke
    {
        public int Id { get; set; }
        [JsonProperty("id")]
        public int JokeId { get; set; }
        public string Setup { get; set; }
        public string Punchline { get; set; }
        public List<Like> Likes { get; set; }
    }

}

using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace JokesApi.Data
{
    public class JokeContextFactory : IDesignTimeDbContextFactory<JokeContext>
    {
        public JokeContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), $"..{Path.DirectorySeparatorChar}JokesApi.Web"))
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true).Build();

            return new JokeContext(config.GetConnectionString("ConStr"));
        }
    }

}

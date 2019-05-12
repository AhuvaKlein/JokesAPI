using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using JokesApi.Web.Models;
using JokesApi.Data;
using Microsoft.Extensions.Configuration;

namespace JokesApi.Web.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString;
        private JokeRepository _repo;
        private UserRepository _urepo;

        public HomeController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
            _repo = new JokeRepository(_connectionString);
            _urepo = new UserRepository(_connectionString);
        }

        public IActionResult Index()
        {
            IndexViewModel vm = new IndexViewModel
            {
                Joke = _repo.GetAJoke(),
                Verified = User.Identity.IsAuthenticated,
                User = _urepo.GetUserByEmail(User.Identity.Name)
            };
            if (vm.Verified)
            {
                vm.AlreadyLiked = _repo.AlreadyLiked(new Like { JokeId = vm.Joke.Id, UserId = vm.User.Id });
                vm.AlreadyDisliked = _repo.AlreadyDisliked(new Like { JokeId = vm.Joke.Id, UserId = vm.User.Id });
                vm.CanStillLike = _repo.CanStillLike(new Like { JokeId = vm.Joke.Id, UserId = vm.User.Id });
            }

            return View(vm);
        }

        public IActionResult SeeAllJokes()
        {
            return View(_repo.GetAllJokes());
        }

        [HttpPost]
        public void LikeDislikeJoke(Like like)
        {
            _repo.LikeDislikeJoke(like);
        }

        public IActionResult CanStillLike(Like like)
        {
            bool csl = _repo.CanStillLike(like);
            return Json(csl);
        }
    }
}

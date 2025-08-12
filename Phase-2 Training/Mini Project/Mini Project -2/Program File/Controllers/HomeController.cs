using Microsoft.AspNetCore.Mvc;
using MovieTicket.context;
using MovieTicket.Models;
using System.Diagnostics;
using System.Linq;

namespace MovieTicket.Controllers
{
    public class HomeController : Controller
    {
        public AppDBContext context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger,AppDBContext context)
        {
            _logger = logger;
            this.context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult BookTicket()
        {
            return View();
        }

        public IActionResult Filter(string gender)
        {
            var movies = context.GetMovieList().ToList();

            if (!string.IsNullOrEmpty(gender))
            {
                movies = movies
                    .Where(m => m.gender == gender).ToList();
            }

            ViewBag.TotalMovies = movies.Count();
            ViewBag.AverageAge = Math.Round(movies.Average(m => m.age), 2);

            return View("MovieDetail", movies);
        }



        public IActionResult DeleteDetail(string name)
        {
            var movie = context.Movies.FirstOrDefault(m => m.name == name);
            context.Movies.Remove(movie);
            context.SaveChanges();
            return RedirectToAction("MovieDetail");
        }

        public IActionResult AddDB(movie _movie)
        {
            context.Movies.Add(_movie);
            context.SaveChanges();
            return RedirectToAction("MovieDetail");
        }

        public IActionResult MovieDetail()
        {
            var movies = context.Movies.ToList();
            ViewBag.TotalMovies = movies.Count;
            ViewBag.AverageAge = Math.Round(movies.Average(m => m.age),2);
            return View(movies);
        }

        public IActionResult ViewDetail(string name)
        {
            var movie = context.Movies.FirstOrDefault(m => m.name == name);
            return View(movie);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

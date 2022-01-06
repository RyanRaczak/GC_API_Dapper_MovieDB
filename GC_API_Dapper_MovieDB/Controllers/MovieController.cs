using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GC_API_Dapper_MovieDB.Controllers
{
    //Generally each table gets its own API Controller
    //Unless 2 tables have a forgein key or the whole database
    //is small/simple. In which case you would put multiple into
    //the same controller

    [ApiController]
    [Route("[controller]")]
    public class MovieController : ControllerBase
    {
        readonly MovieDAL db = new MovieDAL();

        //It is common for an API at it's base URL to return
        //the full listing it's pulling from the database

        [HttpGet]
        public List<Movie> GetMovies()
        {
            return db.GetAllMovies(); ;
        }

        [HttpGet("MoviesByGenre/{genre}")]
        public List<Movie> GetByGenre(string genre)
        {
            List<Movie> movies = db.GetAllMovies();
            List<Movie> filteredMovies = new List<Movie>();
            foreach (var movie in movies)
            {
                if (movie.Genre.Trim().ToLower() == genre.Trim().ToLower())
                {
                    filteredMovies.Add(movie);
                }
            }
            return filteredMovies;
        }

        [HttpGet("RandomMovie")]
        public Movie GetRandomMovie()
        {
            List<Movie> movies = db.GetAllMovies();
            Random rng = new Random();
            int pick = rng.Next(0, movies.Count);
            Movie m = movies[pick];
            return m;
        }

        [HttpGet("RandomByGenre/{genre}")]
        public Movie GetRandomMovieByGenre(string genre)
        {
            List<Movie> movies = GetByGenre(genre);
            Random rng = new Random();
            int pick = rng.Next(0, movies.Count);
            Movie m = movies[pick];
            return m;
        }

        [HttpGet("RandomNumber/{total}")]
        public List<Movie> GetRandomMovieTotal(int total)
        {
            try
            {
                List<Movie> movies = db.GetAllMovies();
                List<Movie> totalMovies = new List<Movie>();
                Random rng = new Random();
                while (totalMovies.Count < total)
                {
                    if (total > movies.Count)
                    {
                        break;
                    }
                    int pick = rng.Next(0, movies.Count);
                    if (!totalMovies.Contains(movies[pick]))
                    {
                        totalMovies.Add(movies[pick]);
                    }
                }
                return totalMovies;
            }
            catch (Exception)
            {
                Movie m = new Movie();
                m.Title = "ERROR MUST BE INT";
                List<Movie> Error = new List<Movie>() { m };
                return Error;
            }
        }

        [HttpGet("ListGenres")]
        public List<string> GetGenres()
        {
            return db.GetGenres();
        }

        [HttpGet("FindByTitle/{title}")]
        public Movie GetMovieByTitle(string title)
        {
            return db.GetMovieByTitle(title);
        }

        [HttpGet("FindAllByTitle/{title}")]
        public List<Movie> GetAllMoviesByTitle(string title)
        {
            return db.GetAllMoviesByTitle(title);
        }

        [HttpPost("AddMovie")]
        public string AddMovie(Movie m)
        {
            db.AddMovie(m);
            return $"{m.Title} has been added to the DB";
        }

        //curly brackets in the route tells the API accept parameters
        //via the URL. In this case, it accepts for ID which is specified
        //in the parameter of the method signature.
        [HttpDelete("Delete/{id}")]
        public string RemoveMovie(int id)
        {
            db.RemoveMovie(id);
            return $"Movie has been removed";
        }

        [HttpGet("FindById/{id}")]
        public Movie GetMovie(int id)
        {
            return db.GetMovie(id);
        }

        [HttpPut("UpdateMovie/{id}")]
        public string UpdateMovie(int id, Movie updatedMovie)
        {
            //We want our user to be able to select which properties
            //they wish to change and leave the rest alone. This will
            //make it so that for properties we wish to leave alone,
            //we don't have to re-enter their values. So will have to
            //compare what values have changed.
            
            Movie oldMovie = GetMovie(id);

            //Check updated movie for changed properties
            if (updatedMovie.Title == null)
            {
                updatedMovie.Title = oldMovie.Title;
            }
            if (updatedMovie.Genre == null)
            {
                updatedMovie.Genre = oldMovie.Genre;
            }
            if (updatedMovie.Year == 0)
            {
                updatedMovie.Year = oldMovie.Year;
            }
            if (updatedMovie.Runtime == 0)
            {
                updatedMovie.Runtime = oldMovie.Runtime;
            }

            db.UpdateMovie(id, updatedMovie);

            return $"{updatedMovie.Title} at id {id} has been updated";
        }
    }
}

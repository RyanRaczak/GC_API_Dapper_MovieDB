using Dapper;
using MySqlConnector;
using System.Collections.Generic;
using System.Linq;

namespace GC_API_Dapper_MovieDB
{
    public class MovieDAL
    {
        string connection = Secret.Connection;

        public List<Movie> GetAllMovies()
        {
            using (var connect = new MySqlConnection(connection))
            {
                var sql = "select * from movies";
                connect.Open();
                List<Movie> movies = connect.Query<Movie>(sql).ToList();
                connect.Close();

                return movies;
            }
        }

        public Movie GetMovie(int id)
        {
            if (ValidID(id))
            {
                using (var connect = new MySqlConnection(connection))
                {
                    var sql = $"select * from movies " +
                        $"where id = {id}";
                    connect.Open();
                    Movie m = connect.Query<Movie>(sql).ToList().First();
                    connect.Close();
                    return m;
                }
            }
            else
            {
                Movie m = new Movie();
                m.Id = id;
                m.Title = "ERROR: No movie found";
                return m;
            }

        }

        public void AddMovie(Movie m)
        {
            using (var connect = new MySqlConnection(connection))
            {
                var sql = $"insert into movies " +
                    $"values(0, '{m.Title}', '{m.Genre}', {m.Year}, {m.Runtime})";
                connect.Open();
                connect.Query<Movie>(sql);
                connect.Close();
            }
        }

        public void RemoveMovie(int id)
        {
            if (ValidID(id))
            {
                using (var connect = new MySqlConnection(connection))
                {
                    var sql = $"delete from movies " +
                        $"where id = {id}";
                    connect.Open();
                    connect.Query<Movie>(sql);
                    connect.Close();
                }
            }
        }

        public void UpdateMovie(int id, Movie m)
        {
            if (ValidID(id))
            {
                using (var connect = new MySqlConnection(Secret.Connection))
                {
                    string sql = $"update movies " +
                        $"set Title = '{m.Title}', Genre = '{m.Genre}', `Year` = {m.Year}, Runtime = {m.Runtime} " +
                        $"where ID = {id}";
                    connect.Open();
                    connect.Query<Movie>(sql);
                    connect.Close();
                }
            }
        }

        public List<string> GetGenres()
        {
            using (var connect = new MySqlConnection(Secret.Connection))
            {
                List<string> genres = new List<string>();
                string sql = $"select distinct Genre from movies";
                connect.Open();
                List<Movie> movies = connect.Query<Movie>(sql).ToList();
                connect.Close();
                foreach (var movie in movies)
                {
                    genres.Add(movie.Genre);
                }
                return genres;
            }
        }

        public Movie GetMovieByTitle(string title)
        {
            using (var connect = new MySqlConnection(connection))
            {
                var sql = $"select * from movies " +
                    $"where title like '{title}%'";
                connect.Open();
                Movie m = connect.Query<Movie>(sql).ToList().First();
                connect.Close();
                return m;
            }
        }

        public List<Movie> GetAllMoviesByTitle(string title)
        {
            using (var connect = new MySqlConnection(connection))
            {
                var sql = $"select * from movies " +
                    $"where title like '%{title}%'";
                connect.Open();
                List<Movie> m = connect.Query<Movie>(sql).ToList();
                connect.Close();
                return m;
            }
        }

        public bool ValidID(int id)
        {
            List<Movie> movies = GetAllMovies();
            foreach (var movie in movies)
            {
                if (movie.Id == id)
                {
                    return true;
                }
            }
            return false;
        }
    }
}

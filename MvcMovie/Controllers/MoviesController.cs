using Microsoft.AspNetCore.Mvc;
using MvcMovie.Models;
using Microsoft.Data.SqlClient;

namespace MvcMovie.Controllers
{
    public class MoviesController : Controller
    {
        private string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DBMovies;Integrated Security=True;Connect Timeout=30;Encrypt=False;";
        public static List<Movie> movies = new List<Movie>();
        // GET: Movies
        public IActionResult Index()
        {
            try
            {
                movies.Clear();
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();

                string queryString = "SELECT * FROM Movies";
                SqlCommand command = new SqlCommand(queryString, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Movie movie = new Movie()
                    {
                        Id = int.Parse(reader[0].ToString()),
                        Genre = reader[1].ToString(),
                        Price = (decimal)Double.Parse(reader[2].ToString()),
                        ReleaseDate = DateTime.Parse(reader[3].ToString()),
                        Title = reader[4].ToString()
                    };
                    movies.Add(movie);
                }
                connection.Close();
                return View(movies);
            }
            catch (Exception ex)
            {
                throw;
            }

           }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound(); // TODO: Agregar una vista
            }

            //Simulación de creación de un objeto (model)
            //Mas adelante vamos a ver como usar una base de datos
            var movie = new Movie
            {
                Genre = "Terror",
                Id = 1,
                Price = 1,
                ReleaseDate = DateTime.Now,
                Title = "La noche del terror"
            };


            return View(movie);
        }


        // GET: Movies/Details/5
        public IActionResult CreateMovie()
        {

            return View();
        }

        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                string queryString = "SELECT * FROM Movies where id=@Id;";
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = command.ExecuteReader();
                Movie movie = null;
                while (reader.Read())
                {
                    movie = new Movie()
                    {
                        Id = (int)reader[0],
                        Genre = reader[1].ToString(),
                        Price = (decimal)Double.Parse(reader[2].ToString()),
                        ReleaseDate = (DateTime)reader[3],
                        Title = reader[4].ToString()
                    };
                }
                connection.Close();
                return View(movie);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ActionResult HandleDelete(int? id)
        {
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                string queryString = "DELETE FROM Movies where id=@Id;";
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@Id", id);

                SqlDataReader reader = command.ExecuteReader();
                connection.Close();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}

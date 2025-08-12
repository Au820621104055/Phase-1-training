using Microsoft.EntityFrameworkCore;
using MovieTicket.Models;
using System.Collections.Generic;

namespace MovieTicket.context
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }

        public DbSet<movie> GetMovieList()
        {
            return Movies;
        }
        public DbSet<movie> Movies { get; set; }
    }
}

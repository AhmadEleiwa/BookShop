using Microsoft.EntityFrameworkCore;
namespace book_project.Models
{
    public class BookContext : DbContext
    {
        public BookContext(DbContextOptions<BookContext> options) : base(options) { }
        public DbSet<Genre> Generes { get; set; }
        public DbSet<Author> Author{ get; set; }

        public DbSet<Book> Books { get; set; }
  
    }
}

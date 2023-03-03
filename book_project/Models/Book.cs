using System.ComponentModel.DataAnnotations;
namespace book_project.Models
{
    public class Book
    {
        
        public int BookId { get; set; }
        [Required()]
        public string Title { get; set; }
        [Required()]

        public string Description { get; set; }
        [Required()]

        public int AuthorId { get; set; }
        public Author Author { get; set; }
        [Required()]


        public string Image { get; set; }
        [Required()]

        public string File { get; set; }
        [Required()]

        public int GenreId{ get; set; }
        public Genre Genre { get; set; }
    }
}

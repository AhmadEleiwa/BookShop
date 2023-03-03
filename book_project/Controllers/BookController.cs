using Microsoft.AspNetCore.Mvc;
using book_project.Models;
using Microsoft.Extensions.Hosting;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using Microsoft.EntityFrameworkCore;

namespace book_project.Controllers
{
    public class BookController : Controller
    {
        private readonly BookContext context;

        private readonly IHostingEnvironment hostingEnvironment;
  
        public BookController(BookContext context, IHostingEnvironment environment)
        {
            this.context = context;
            hostingEnvironment = environment;

        }
        public IActionResult Upload()
        {
            var author = HttpContext.Session.GetString("author");
            if(author == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Genres = context.Generes.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Upload(BookForm model)
        {
            var author = HttpContext.Session.GetString("author");
            if (author == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var author_db = context.Author.Where(m => m.AuthorName == author).FirstOrDefault();
            var uniqueFileName = GetUniqueFileName(model.Image.FileName);
            var uploads = Path.Combine(hostingEnvironment.WebRootPath, "uploads");
            var filePath = Path.Combine(uploads, uniqueFileName);
            model.Image.CopyTo(new FileStream(filePath, FileMode.Create));

            var uniqueFileName2 = GetUniqueFileName(model.File.FileName);
   
            var filePath2 = Path.Combine(uploads, uniqueFileName2);
            model.File.CopyTo(new FileStream(filePath2, FileMode.Create));

            context.Books.Add(new Book {AuthorId= author_db.AuthorId, Title=model.Title, Description=model.Description, GenreId=model.GenreId, Image= uniqueFileName, File= uniqueFileName2 });
            context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }

        public IActionResult Delete(int id) 
        {
            var author = HttpContext.Session.GetInt32("authorId");
            if (author == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var author_db = context.Author.Find(author);
            var book = context.Books.Find(id);
            if(book.AuthorId == author_db.AuthorId)
            {
                context.Books.Remove(book);
                context.SaveChanges();
            }
            
            return RedirectToAction("Index", "Home");
        }

        public IActionResult BookView(int id)
        {
            var book = context.Books.Include(mbox => mbox.Genre).Include(mbox => mbox.Author).Where(m => m.BookId == id).FirstOrDefault();
            if(book == null)
            {
                return Content("Error");
            }
            return View("BookView",book);
        }
    }
}

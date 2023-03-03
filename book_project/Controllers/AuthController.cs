using book_project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace book_project.Controllers
{
    public class AuthController : Controller
    {
        private readonly BookContext  context;
        public AuthController(BookContext context)
        {
            this.context = context;
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            var author = context.Author.Where(m => m.Email == model.Email).FirstOrDefault();
            if(author != null && author.Password == model.Password)
            {
                HttpContext.Session.SetString("author", author.AuthorName);
                HttpContext.Session.SetInt32("authorId", author.AuthorId);

                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterModel model)
        {
            var author = context.Author.Where(m => m.Email == model.Email).FirstOrDefault();
            if (author == null)
            {
                context.Author.Add(new Author { AuthorName = model.Name, Email = model.Email, Password = model.Password });
                context.SaveChanges();
            }
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}

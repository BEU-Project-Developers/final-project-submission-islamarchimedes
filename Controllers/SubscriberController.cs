using Microsoft.AspNetCore.Mvc;
using PharmaPulseApp.Data;
using PharmaPulseApp.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PharmaPulseApp.Controllers
{
    public class SubscriberController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public SubscriberController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Subscribe(string email)
        {
            if (string.IsNullOrEmpty(email) || !new EmailAddressAttribute().IsValid(email))
            {
                ViewBag.ErrorMessage = "Invalid email address.";
                return View("Index");
            }

            if (_appDbContext.Subscribers.Any(s => s.Email == email))
            {
                ViewBag.ErrorMessage = "This email is already subscribed.";
                return View("Index");
            }
            
            var subscriber = new Subscriber
            {
                Email = email
            };

            _appDbContext.Subscribers.Add(subscriber);
            _appDbContext.SaveChanges();

            ViewBag.SuccessMessage = "You have successfully subscribed to the newsletter.";
            return View("Index");
        }
    }
}

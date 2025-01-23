using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmaPulseApp.Data;
using PharmaPulseApp.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
namespace PharmaPulseApp.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public UserController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (ModelState.IsValid)
            {
                var user = await _appDbContext.Users
                    .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

                if (user != null)
                {
                    HttpContext.Session.SetInt32("UserId", user.Id);
                    HttpContext.Session.SetString("UserName", user.FirstName);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View();
        }

        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Email already exists.");
                    return View();
                }

                _appDbContext.Users.Add(user);
                await _appDbContext.SaveChangesAsync();
                return RedirectToAction("Login");
            }
            return View();
        }



        public IActionResult ResetPassword()
        {
            return View();
        }

        public IActionResult Reset()
        {
            return View();
        }


        [HttpGet("/User/Card")]
        public IActionResult Card()
        {
            return View();
        }

        [HttpGet("/User/Card/{medicineId}")]
        public IActionResult Card(int medicineId)
        {

            if (!HttpContext.Session.TryGetValue("UserId", out var userIdBytes))
            {
                return RedirectToAction("Login", "User");
            }


            Medicine medicine = _appDbContext.Medicines.FirstOrDefault(m => m.Id == medicineId);

            if (medicine == null)
            {
                return NotFound();
            }

            return View(medicine);
        }


        [HttpPost]
        public async Task<IActionResult> CreateOrder(int medicineId)
        {
            if (!HttpContext.Session.TryGetValue("UserId", out var userIdBytes))
            {
                return RedirectToAction("Login", "User");
            }

            int userId = BitConverter.ToInt32(userIdBytes, 0);


            if (userId == 0)
            {
                return RedirectToAction("Login", "User");
            }

            Medicine medicine = await _appDbContext.Medicines.FindAsync(medicineId);

            if (medicine == null)
            {
                return NotFound();
            }

            Order newOrder = new Order()
            {
                MedicineId = medicineId,
                UserId = 1006,
                MedicinePrice = medicine.Price,
                Amount = 1,
                OrderDate = DateTime.Now
            };

            _appDbContext.Orders.Add(newOrder);
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("Thanks");
        }

        public IActionResult Thanks()
        {
            return View();
        }
    }
}
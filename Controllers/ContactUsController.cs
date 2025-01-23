using Microsoft.AspNetCore.Mvc;
using PharmaPulseApp.Data;
using PharmaPulseApp.Models;

namespace PharmaPulseApp.Controllers
{
    public class ContactUsController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public ContactUsController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IActionResult Index()
        {
            var medicines = _appDbContext.Medicines.ToList();
            ViewBag.Medicines = medicines;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Request request)
        {
            if (ModelState.IsValid)
            {
                _appDbContext.Requests.Add(request);
                await _appDbContext.SaveChangesAsync();
                return RedirectToAction("Index"); 
            }

            var medicines = _appDbContext.Medicines.ToList();
            ViewBag.Medicines = medicines;
            return View(request);
        }
    }
}

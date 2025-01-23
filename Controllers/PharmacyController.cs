using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmaPulseApp.Data;

namespace PharmaPulseApp.Controllers
{
    public class PharmacyController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public PharmacyController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var pharmacies = await _appDbContext.Pharmacies.ToListAsync();
            return View(pharmacies);
        }

        public async Task<IActionResult> Details(int id)
        {
            var pharmacy = await _appDbContext.Pharmacies
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pharmacy == null)
            {
                return View("PharmacyNotFound");
            }

            return View(pharmacy);
        }

        public async Task<IActionResult> DetailsByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return View("PharmacyNotFound");
            }

            var pharmacy = await _appDbContext.Pharmacies
                .FirstOrDefaultAsync(p => p.Name == name);

            if (pharmacy == null)
            {
                return View("PharmacyNotFound");
            }

            return View("Details", pharmacy);
        }
    }
}

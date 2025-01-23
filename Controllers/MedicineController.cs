using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PharmaPulseApp.Data;
using PharmaPulseApp.Models;

namespace PharmaPulseApp.Controllers
{
    public class MedicineController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public MedicineController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IActionResult Index()
        {
            var categoriesWithMedicines = _appDbContext.Categories
                .Include(c => c.Medicines)
                .ToList();

            return View(categoriesWithMedicines);
        }

       
        [HttpGet("Store/{page?}")]
        public IActionResult Store(int? page)
        {
            int pageSize = 9; 
            int pageNumber = page ?? 1; 

            int totalMedicines = _appDbContext.Medicines.Count();

            int medicinesToSkip = (pageNumber - 1) * pageSize;

            var paginatedMedicines = _appDbContext.Medicines
                                                .Skip(medicinesToSkip)
                                                .Take(pageSize)
                                                .ToList();  

            ViewBag.Medicines = paginatedMedicines;
            ViewBag.PageNumber = pageNumber; 
            ViewBag.TotalMedicines = totalMedicines;
            ViewBag.PageSize = pageSize;

            return View();
        }



        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return View("MedicineNotFound");
            }

            var medicine = await _appDbContext.Medicines
                .Include(m => m.AgeRange)
                .Include(m => m.Category)
                .Include(m => m.Pharmacy_Medicines)
                .ThenInclude(pm => pm.Pharmacy)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (medicine == null)
            {
                return View("MedicineNotFound");
            }

            return View(medicine);
        }


        public async Task<IActionResult> DetailsByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return View("MedicineNotFound");
            }

            var medicine = await _appDbContext.Medicines
                .Include(m => m.AgeRange)
                .Include(m => m.Category)
                .Include(m => m.Pharmacy_Medicines) 
                    .ThenInclude(pm => pm.Pharmacy) 
                .FirstOrDefaultAsync(m => m.Name == name);

            if (medicine == null)
            {
                return View("MedicineNotFound");
            }

            return View("Details", medicine);
        }


        public async Task<IActionResult> MedicinesByPharmacy(int pharmacyId)
        {
            var pharmacy = await _appDbContext.Pharmacies
                .Include(p => p.Pharmacy_Medicines)
                .ThenInclude(pm => pm.Medicine)
                .FirstOrDefaultAsync(p => p.Id == pharmacyId);

            if (pharmacy == null)
            {
                return View("MedicineNotFound");
            }

            var medicines = pharmacy.Pharmacy_Medicines
                .Select(pm => pm.Medicine)
                .ToList();

            return View(medicines);
        }
    }
}

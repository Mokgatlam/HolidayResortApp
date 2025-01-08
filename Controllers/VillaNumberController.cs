using HolidayResortApp.Domain.Entities;
using HolidayResortApp.Infrastructure.Data;
using HolidayResortApp.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HolidayResortApp.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly ApplicationDbContext _db;

        public VillaNumberController(ApplicationDbContext db)
        {
            _db = db;
            
        }
        public IActionResult Index()
        {   // inner join
            var villaNumbers = _db.VillaNumbers.Include(u=>u.Villa).ToList();
            return View(villaNumbers);
        }

        [HttpGet]
        public IActionResult Create() 
        {
            VillaNumberVM villaNumberVM = new()
            {

                VillaList = _db.Villas.ToList().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
            };
            return View(villaNumberVM);

            //IEnumerable<SelectListItem> list= _db.Villas.ToList().Select(u=>new SelectListItem
            //{ 
            //    Text= u.Name,
            //    Value= u.Id.ToString()
            //});

            //ViewData["VillaList"] = list;
            //ViewBag.VillaList = list;


        }

        [HttpPost]
        public IActionResult Create(VillaNumberVM obj) 
        {
            //ModelState.Remove("Villa");
            bool roomNumberExists = _db.VillaNumbers.Any(u => u.Villa_Number == obj.VillaNumber.Villa_Number);

            if (ModelState.IsValid && !roomNumberExists)
            {
                //create the Villa Number object in database with a helper method
                _db.VillaNumbers.Add(obj.VillaNumber);
                _db.SaveChanges();
                TempData["success"] = "The villa Number has been created Successfully.";

                return RedirectToAction("Index", "VillaNumber");

            }

            if (roomNumberExists)
            {
                TempData["error"] = "The villa Number already Exists.";
            }
            

            obj.VillaList = _db.Villas.ToList().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            
            return View(obj);
            
        }


        [HttpGet]
        public IActionResult Update(int villaNumberId)
        {
            // find the correct villa by Id
            //Villa? obj= _db.Villas.FirstOrDefault(u=>u.Id==villaId);

            VillaNumberVM villaNumberVM = new()
            {

                VillaList = _db.Villas.ToList().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VillaNumber= _db.VillaNumbers.FirstOrDefault(u=>u.Villa_Number==villaNumberId)
            };
            


            if (villaNumberVM.VillaNumber == null)
            { 
                return RedirectToAction("Error","Home");
            }
            return View(villaNumberVM);
        
        }


        [HttpPost]
        public IActionResult Update(VillaNumberVM villaNumberVM)
        {

            //ModelState.Remove("Villa");
           // bool roomNumberExists = _db.VillaNumbers.Any(u => u.Villa_Number == obj.VillaNumber.Villa_Number);

            if (ModelState.IsValid)
            {
                //update the Villa Number object in database with a helper method
                _db.VillaNumbers.Update(villaNumberVM.VillaNumber);
                _db.SaveChanges();
                TempData["success"] = "The villa Number has been updated Successfully.";

                return RedirectToAction("Index", "VillaNumber");

            }

           


            villaNumberVM.VillaList = _db.Villas.ToList().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });

            return View(villaNumberVM);

           

        }


        [HttpGet]
        public IActionResult Delete(int villaId)
        {
            // find the correct villa by Id
            Villa? obj = _db.Villas.FirstOrDefault(u => u.Id == villaId);




            if (obj is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);

        }


        [HttpPost]
        public IActionResult Delete(Villa obj)
        {

            Villa? objFromDb = _db.Villas.FirstOrDefault(u => u.Id == obj.Id);

            if (objFromDb is not null)
            {
                //update the Villa object in database with a helper method
                _db.Villas.Remove(objFromDb);
                _db.SaveChanges();
                TempData["success"] = "The villa has been deleted Successfully.";

                return RedirectToAction("Index", "Villa");

            }
            TempData["error"] = "The villa could not be deleted.";
            return View();

        }



    }
}

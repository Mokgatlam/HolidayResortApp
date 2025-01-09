using HolidayResortApp.Application.Common.Interfaces;
using HolidayResortApp.Domain.Entities;
using HolidayResortApp.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace HolidayResortApp.Controllers
{
    public class VillaController : Controller
    {
        // private readonly ApplicationDbContext _db;

        // private readonly IVillaRepository _villaRepo;

        private readonly IUnitofWork _unitofWork;

        public VillaController(IUnitofWork unitofWork)
        {
            _unitofWork= unitofWork;
            
        }
        public IActionResult Index()
        {
            // var villas = _villaRepo.GetAll();

            var villas = _unitofWork.Villa.GetAll();
            return View(villas);
        }

        [HttpGet]
        public IActionResult Create() 
        { 

            return View();

        }

        [HttpPost]
        public IActionResult Create(Villa obj) 
        {
            if (obj.Name == obj.Description)
            {
                ModelState.AddModelError("name", "The description cannot exacty match the Name");
            }
            if (ModelState.IsValid)
            {
                //create the Villa object in database with a helper method
                // _villaRepo.Add(obj);
                _unitofWork.Villa.Add(obj);
                //_villaRepo.Save();
                _unitofWork.Save();
                TempData["success"] = "The villa has been created Successfully.";

                return RedirectToAction("Index", "Villa");

            }
            TempData["error"] = "The villa could not be created.";
            return View();
            
        }


        [HttpGet]
        public IActionResult Update(int villaId)
        {
            // find the correct villa by Id
            // Villa? obj= _db.Villas.FirstOrDefault(u=>u.Id==villaId);

            Villa? obj = _unitofWork.Villa.Get(u => u.Id == villaId);   //_villaRepo.Get(u => u.Id == villaId);




            if (obj is null)
            { 
                return RedirectToAction("Error","Home");
            }
            return View(obj);
        
        }


        [HttpPost]
        public IActionResult Update(Villa obj)
        {
            
            if (ModelState.IsValid && obj.Id>0)
            {
                //update the Villa object in database with a helper method
                //_db.Villas.Update(obj);

                _unitofWork.Villa.Update(obj);                //_villaRepo.Update(obj);
                                                              // _db.SaveChanges();
                _unitofWork.Save();                                              //_villaRepo.Save();
                TempData["success"] = "The villa has been updated Successfully.";

                return RedirectToAction("Index", "Villa");

            }
            TempData["error"] = "The villa could not be updated.";
            return View();

        }


        [HttpGet]
        public IActionResult Delete(int villaId)
        {
            // find the correct villa by Id
            //Villa? obj = _db.Villas.FirstOrDefault(u => u.Id == villaId);
            
            Villa? obj = _unitofWork.Villa.Get(u => u.Id == villaId);




            if (obj is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);

        }


        [HttpPost]
        public IActionResult Delete(Villa obj)
        {

            //Villa? objFromDb = _db.Villas.FirstOrDefault(u => u.Id == obj.Id);

            Villa? objFromDb = _unitofWork.Villa.Get(u => u.Id == obj.Id);

            if (objFromDb is not null)
            {
                //update the Villa object in database with a helper method
                _unitofWork.Villa.Remove(objFromDb);
                _unitofWork.Save();
                TempData["success"] = "The villa has been deleted Successfully.";

                return RedirectToAction("Index", "Villa");

            }
            TempData["error"] = "The villa could not be deleted.";
            return View();

        }



    }
}

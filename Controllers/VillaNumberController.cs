using HolidayResortApp.Application.Common.Interfaces;
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
        private readonly IUnitofWork _unitofWork;

        public VillaNumberController(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;

        }
        public IActionResult Index()
        {   // inner join
            var villaNumbers = _unitofWork.VillaNumber.GetAll(includeProperties: "Villa");   //_db.VillaNumbers.Include(u=>u.Villa).ToList();
            return View(villaNumbers);
        }

        [HttpGet]
        public IActionResult Create() 
        {
            VillaNumberVM villaNumberVM = new()
            {

                VillaList = _unitofWork.Villa.GetAll().Select(u => new SelectListItem
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
            bool roomNumberExists = _unitofWork.VillaNumber.Any(u => u.Villa_Number == obj.VillaNumber.Villa_Number);

            if (ModelState.IsValid && !roomNumberExists)
            {
                //create the Villa Number object in database with a helper method
                _unitofWork.VillaNumber.Add(obj.VillaNumber);
                _unitofWork.Save();
                TempData["success"] = "The villa Number has been created Successfully.";

                return RedirectToAction(nameof(Index));

            }

            if (roomNumberExists)
            {
                TempData["error"] = "The villa Number already Exists.";
            }
            

            obj.VillaList = _unitofWork.Villa.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            
            return View(obj);
            
        }


        [HttpGet]
        public IActionResult Update(int villaNumberId)
        {
            
            VillaNumberVM villaNumberVM = new()
            {

                VillaList = _unitofWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VillaNumber= _unitofWork.VillaNumber.Get(u=>u.Villa_Number==villaNumberId)
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
                _unitofWork.VillaNumber.Update(villaNumberVM.VillaNumber);
                _unitofWork.Save();
                TempData["success"] = "The villa Number has been updated Successfully.";

                return RedirectToAction(nameof(Index));

            }

           


            villaNumberVM.VillaList = _unitofWork.Villa.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });

            return View(villaNumberVM);

           

        }


        [HttpGet]
        public IActionResult Delete(int villaNumberId)
        {

            VillaNumberVM villaNumberVM = new()
            {

                VillaList = _unitofWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VillaNumber = _unitofWork.VillaNumber.Get(u => u.Villa_Number == villaNumberId)
            };



            if (villaNumberVM.VillaNumber == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(villaNumberVM);

        }


        [HttpPost]
        
        public IActionResult Delete(VillaNumberVM villaNumberVM)
        {

            //ModelState.Remove("Villa");
            VillaNumber? objFromDb = _unitofWork.VillaNumber.Get(u => u.Villa_Number == villaNumberVM.VillaNumber.Villa_Number);

            if (objFromDb is not null)
            {
                //update the Villa Number object in database with a helper method
                _unitofWork.VillaNumber.Remove(objFromDb);
                _unitofWork.Save();
                TempData["success"] = "The villa Number has been deleted Successfully.";

                return RedirectToAction(nameof(Index));

            }
            TempData["error"] = "the villa number could not be deleted";
            return View();




           

        }



    }
}

﻿using HolidayResortApp.Domain.Entities;
using HolidayResortApp.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace HolidayResortApp.Controllers
{
    public class VillaController : Controller
    {
        private readonly ApplicationDbContext _db;

        public VillaController(ApplicationDbContext db)
        {
            _db = db;
            
        }
        public IActionResult Index()
        {   
            var villas = _db.Villas.ToList();
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
                _db.Villas.Add(obj);
                _db.SaveChanges();
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
            Villa? obj= _db.Villas.FirstOrDefault(u=>u.Id==villaId);

           


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
                _db.Villas.Update(obj);
                _db.SaveChanges();
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

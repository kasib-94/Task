using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task.Models;

namespace Task.Controllers
{
   
    [Authorize]
    public class BookController : Controller
    {

        private readonly ApplicationDbContext _db;

        public BookController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Reservations(int? id)
        {
            List<Reservation> reservations = new List<Reservation>();
            if (id!=null)
            {
                reservations = _db.Reservations.Where(x => x.BookId == id).ToList(); 
            }
            else
            {
                reservations = _db.Reservations.ToList(); 
            }

            foreach (var res in reservations)
            {
                res.ApplicationUser = _db.Users.First(x => x.Id == res.ApplicationUserId);
            }
            return View(reservations);
   
        }
        

        public IActionResult Upsert(int? id)
        {
            Book book = new();
    


            if (id == null || id == 0)
            {
                book.ReleaseDate = DateTime.Now;
                return View(book);
            }
            else
            {
                
                book = _db.Books.First(x => x.Id == id);
                return View(book);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Book obj)
        {
            if (ModelState.IsValid  )
            {
                if (obj.Id == 0)
                {
                    obj.ReleaseDateString = obj.ReleaseDate.ToString("d");
                    _db.Books.Add(obj);
                }
                else
                {
                    obj.ReleaseDateString = obj.ReleaseDate.ToString("d");
                    _db.Books.Update(obj);
                }
                TempData["success"] = "Book updated successfully";
                _db.SaveChanges();
              
                return RedirectToAction("Index");
            }

           
            return View(obj);
        }


        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var bookList = _db.Books.ToList();
            return Json(new { data = bookList });
        }


        [HttpDelete]
 
        public IActionResult Delete(int? id)
        {
            var obj = _db.Books.First(u => u.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }


            _db.Books.Remove(obj);
            _db.SaveChanges();
            return Json(new { success = true, message = "Deleted successfully " });
        }
        [HttpPost]
        public IActionResult AddReservation(int id)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            Reservation reservation = new()
            {
                ApplicationUserId = claim.Value,
                BookId = id,
                ReservationDate = DateTime.Now,
            };
            _db.Reservations.Add(reservation);
            _db.SaveChanges();
         
            return Json(new { success = true, message = "Reservation Added ! " });
        }
        #endregion
    }
}
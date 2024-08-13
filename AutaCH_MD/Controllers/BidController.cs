using AutaCH_MD.Contexts;
using Microsoft.AspNetCore.Mvc;
using AutaCH_MD.Models;
using AutaCH_MD.DTOs;
using Microsoft.EntityFrameworkCore;

namespace AutaCH_MD.Controllers
{
    public class BidController : Controller
    {
        private readonly AppDataContext _ctx;

        public BidController(AppDataContext ctx)
        {
            _ctx = ctx;
        }

        public IActionResult BidsList()
        {
            var bids = this._ctx.Bids
                .Include(b => b.User)
                .Include(b => b.Car)
                .ToList();
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            return View("BidsList", bids);
        }

        [HttpPost]
        public IActionResult SubmitBid(Guid carId, Guid userId, decimal bidAmount)
        {
            if(bidAmount <= 10)
            {
                TempData["ErrorMessage"] = "Bid amount must be greater than 10.";
                return RedirectToAction("CarDetails", "Car", new { id = carId });
            }


            var bid = new Bid
            {
                BidId = Guid.NewGuid(),
                CarId = carId,
                UserId = userId,
                BidAmount = bidAmount,
                DateTime = DateTime.Now,
                IsActive = true
            };
            this._ctx.Bids.Add(bid);
            this._ctx.SaveChanges();

            return RedirectToAction("CarDetails", "Car", new { id = carId });
        }

        [HttpGet]
        public IActionResult EditBid(Guid bidId, bool isDelete = false)
        {
            var bid = this._ctx.Bids
                .Include (b => b.User)
                .Include(b => b.Car)
                .SingleOrDefault(bid => bid.BidId == bidId);
   

            if (bid != null)
            {
                var dto = new BidDTO
                {
                    BidId = bid.BidId,
                    UserId= bid.UserId,
                    FirstName = bid.User.FirstName,
                    LastName = bid.User.LastName,
                    CarId = bid.CarId,
                    Make = bid.Car.Make,
                    Model = bid.Car.Model,
                    Year = bid.Car.Year,
                    Color = bid.Car.Color,
                    BidAmount = bid.BidAmount,
                    IsActive = bid.IsActive
                };
                ViewBag.IsDelete = isDelete;
                return View("EditBidPage", dto);
            }
            return RedirectToAction("BidsList");
        }

        [HttpPost]
        public IActionResult EditBid(BidDTO dto)
        {
            if (ModelState.IsValid)
            {
                var bid = this._ctx.Bids.FirstOrDefault(bid => bid.BidId == dto.BidId);

                if (bid != null)
                {
                    bid.UserId = dto.UserId;
                    bid.CarId = dto.CarId;
                    bid.BidAmount = dto.BidAmount;
                    bid.IsActive = dto.IsActive;

                    try
                    {
                        this._ctx.SaveChanges();
                        TempData["SuccesMessage"] = "Bid updated succesfully.";
                        return RedirectToAction("BidsList");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, "An error occurred while saving data: " + ex.Message);
                    }
                }
                return RedirectToAction("BidsList");
            }
            return View("EditBid", dto);
        }


        [HttpGet]
        public IActionResult DeleteBid(Guid bidId)
        {
            var bid = this._ctx.Bids
                .Include(b => b.User)
                .Include(b => b.Car)
                .SingleOrDefault(bid => bid.BidId == bidId);
            if (bid != null)
            {
                var dto = new BidDTO
                {
                    BidId = bid.BidId,
                    UserId = bid.UserId,
                    FirstName = bid.User.FirstName,
                    LastName = bid.User.LastName,
                    CarId = bid.CarId,
                    Make = bid.Car.Make,
                    Model = bid.Car.Model,
                    Year = bid.Car.Year,
                    Color = bid.Car.Color,
                    BidAmount = bid.BidAmount,
                    DateTime = bid.DateTime,
                    IsActive = bid.IsActive
                };
                return View("DeleteBidPage", dto);
            }
            return RedirectToAction("AdminPage", "Admin");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteBidConfirm(Guid bidId)
        {
            var bid = this._ctx.Users.Find(bidId);
            if (bid != null)
            {
                try
                {
                    this._ctx.Remove(bid);
                    this._ctx.SaveChanges();
                    TempData["SuccesMessage"] = "Bid deleted succesfully.";
                    return RedirectToAction("BidsList");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while deleting data." + ex.Message);
                }
            }
            return View("AdminPage", "Admin");
        }
    }
}

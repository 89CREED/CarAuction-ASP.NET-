using AutaCH_MD.Contexts;
using AutaCH_MD.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Collections.Generic;
using AutaCH_MD.DTOs;

namespace AutaCH_MD.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDataContext _ctx;

        public HomeController(ILogger<HomeController> logger, AppDataContext ctx)
        {
            _logger = logger;
            _ctx = ctx;
        }

        public IActionResult Index()
        {
            var cars = this._ctx.Cars.ToList();

            var model = new CarListViewModel
            {
                Cars = cars
            };

            return View(model);
        }

        public IActionResult Search(string? make, string? model, int? yearFrom, int? yearTo, int? mileageFrom, int? mileageTo)
        {
            var carsQuery = this._ctx.Cars.AsQueryable();

            if(!string.IsNullOrEmpty(make))
            {
                carsQuery = carsQuery.Where(c => c.Make.Contains(make));
            }

            if(!string.IsNullOrEmpty(model))
            {
                carsQuery = carsQuery.Where(c => c.Model.Contains(model));
            }

            if(yearFrom.HasValue)
            {
                carsQuery = carsQuery.Where(c => c.Year >= yearFrom.Value);
            }

            if(yearTo.HasValue)
            {
                carsQuery = carsQuery.Where(c => c.Year <= yearTo.Value);
            }

            if(mileageFrom.HasValue)
            {
                carsQuery = carsQuery.Where(c => c.Mileage >= mileageFrom.Value);
            }

            if(mileageTo.HasValue)
            {
                carsQuery = carsQuery.Where(c => c.Mileage <= mileageTo.Value);
            }

            var cars = carsQuery.ToList();

            var view = new CarListViewModel
            {
                Cars = cars,
                Make = make,
                Model = model,
                YearFrom = yearFrom,
                YearTo = yearTo,
                MileageFrom = mileageFrom,
                MileageTo = mileageTo
            };

            return View("Index", view);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

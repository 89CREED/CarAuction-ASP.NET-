using AutaCH_MD.Contexts;
using Microsoft.AspNetCore.Mvc;
using AutaCH_MD.Models;
using AutaCH_MD.DTOs;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;

namespace AutaCH_MD.Controllers
{
    public class CarController : Controller
    {
        private readonly AppDataContext _ctx;

        public CarController(AppDataContext ctx)
        {
            _ctx = ctx;
        }

        public IActionResult AddCar()
        {
            return View();
        }

        public IActionResult CarsList()
        {
            var cars = this._ctx.Cars.ToList();
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            return View("CarsList", cars);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCar(CarDTO carDto, List<IFormFile> images)
        {
            if (ModelState.IsValid)
            {
                var car = new Car
                {
                    StartAuction = carDto.StartAuction ?? DateTime.Now,
                    EndAuction = carDto.EndAuction ?? DateTime.Now,
                    Make = carDto.Make ?? string.Empty,
                    Model = carDto.Model ?? string.Empty,
                    Body = carDto.Body ?? string.Empty,
                    Color = carDto.Color ?? string.Empty,
                    Engine = carDto.Engine ?? 0,
                    Fuel = carDto.Fuel ?? string.Empty,
                    Transmission = carDto.Transmission ?? string.Empty,
                    Gears = carDto.Gears ?? 0,
                    Doors = carDto.Doors ?? 0,
                    Seats = carDto.Seats ?? 0,
                    Tires = carDto.Tires ?? string.Empty,
                    Key = carDto.Key ?? 0,
                    FirstRegister = carDto.FirstRegister ?? string.Empty,
                    Year = carDto.Year ?? 0,
                    Mileage = carDto.Mileage ?? 0,
                    ReferenceNumber = carDto.ReferenceNumber ?? string.Empty,
                    Images = new List<string>(),
                    NewPrice = carDto.NewPrice ?? 0.0m,
                    Value = carDto.Value ?? 0.0m,
                    RepairCosts = carDto.RepairCosts ?? 0.0m,
                    MoreInfo = carDto.MoreInfo ?? string.Empty,
                    SerialEquipment = carDto.SerialEquipment ?? string.Empty,
                    RegistrationCertificate = carDto.RegistrationCertificate ?? false,
                    Drive = carDto.Drive ?? false,
                    Damaged = carDto.Damaged ?? false,
                    IsActive = true,
                    DateTime = DateTime.Now
                };

                if (images != null && images.Count > 0)
                {
                    foreach (var image in images)
                    {
                        if (image.Length > 0)
                        {
                            var fileName = Path.GetFileName(image.FileName);
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                await image.CopyToAsync(fileStream);
                            }
                            car.Images.Add($"/images/{fileName}");
                        }
                    }
                }

                try
                {
                    Console.WriteLine("Adding car to database");

                    this._ctx.Cars.Add(car);
                    await this._ctx.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Car added succesfully.";
                    return RedirectToAction("CarsList");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while saving data: " + ex.Message);
                }
            }
            return View(carDto);
        }

        [HttpGet]
        public IActionResult EditCar(Guid carId)
        {
            var car = this._ctx.Cars.FirstOrDefault(car => car.CarId == carId);
            if (car != null)
            {
                var dto = new CarDTO
                {
                    CarId = car.CarId,
                    StartAuction = car.StartAuction,
                    EndAuction = car.EndAuction,
                    Make = car.Make,
                    Model = car.Model,
                    Body = car.Body,
                    Color = car.Color,
                    Engine = car.Engine,
                    Fuel = car.Fuel,
                    Transmission = car.Transmission,
                    Gears = car.Gears,
                    Doors = car.Doors,
                    Seats = car.Seats,
                    Tires = car.Tires,
                    Key = car.Key,
                    FirstRegister = car.FirstRegister,
                    Year = car.Year,
                    Mileage = car.Mileage,
                    ReferenceNumber = car.ReferenceNumber,
                    Images = car.Images,
                    NewPrice = car.NewPrice,
                    Value = car.Value,
                    RepairCosts = car.RepairCosts,
                    MoreInfo = car.MoreInfo,
                    SerialEquipment = car.SerialEquipment,
                    RegistrationCertificate = car.RegistrationCertificate,
                    Drive = car.Drive,
                    Damaged = car.Damaged,
                    IsActive = car.IsActive
                };
                return View("EditCarPage", dto);
            }
            return RedirectToAction("CarsList");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCar(CarDTO carDto, List<IFormFile> imgs, List<string> imgToRemove)
        {
            var car = this._ctx.Cars.SingleOrDefault(car => car.CarId == carDto.CarId);
            if (car != null)
            {
                if (ModelState.IsValid)
                {

                    

                    car.CarId = carDto.CarId;
                    car.StartAuction = carDto.StartAuction ?? car.StartAuction;
                    car.EndAuction = carDto.EndAuction ?? car.EndAuction;
                    car.Make = carDto.Make ?? string.Empty;
                    car.Model = carDto.Model ?? string.Empty;
                    car.Body = carDto.Body ?? string.Empty;
                    car.Color = carDto.Color ?? string.Empty;
                    car.Engine = carDto.Engine ?? 0;
                    car.Fuel = carDto.Fuel ?? string.Empty;
                    car.Transmission = carDto.Transmission ?? string.Empty;
                    car.Gears = carDto.Gears ?? 0;
                    car.Doors = carDto.Doors ?? 0;
                    car.Seats = carDto.Seats ?? 0;
                    car.Tires = carDto.Tires ?? string.Empty;
                    car.Key = carDto.Key ?? 0;
                    car.FirstRegister = carDto.FirstRegister ?? string.Empty;
                    car.Year = carDto.Year ?? 0;
                    car.Mileage = carDto.Mileage ?? 0;
                    car.ReferenceNumber = carDto.ReferenceNumber ?? string.Empty;
                    car.NewPrice = carDto.NewPrice ?? 0.0m;
                    car.Value = carDto.Value ?? 0.0m;
                    car.RepairCosts = carDto.RepairCosts ?? 0.0m;
                    car.MoreInfo = carDto.MoreInfo ?? string.Empty;
                    car.SerialEquipment = carDto.SerialEquipment ?? string.Empty;
                    car.RegistrationCertificate = carDto.RegistrationCertificate ?? car.RegistrationCertificate;
                    car.Drive = carDto.Drive ?? car.Drive;
                    car.Damaged = carDto.Damaged ?? car.Damaged;
                    car.IsActive = carDto.IsActive;

                    var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    
                    foreach (var image in imgs)
                    {
                        if (image.Length > 0)
                        {
                            var fileName = Path.GetFileName(image.FileName);
                            var filePath = Path.Combine(directoryPath, fileName);

                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                await image.CopyToAsync(fileStream);
                            }
                            var imagePath = $"/images/{fileName}";
                            if (!car.Images.Contains(imagePath))
                            {
                                car.Images.Add(imagePath);
                            }
                        }
                    }

                    Console.WriteLine("Images after adding new ones: " + string.Join(", ", car.Images));

                    if (imgToRemove != null && imgToRemove.Any())
                    {
                        foreach (var img in imgToRemove)
                        {
                            var imgPath = Path.Combine(directoryPath, Path.GetFileName(img));
                            if (System.IO.File.Exists(imgPath))
                            {
                                System.IO.File.Delete(imgPath);
                            }
                            car.Images.Remove(img);
                        }
                    }

                    try
                    {
                        this._ctx.SaveChanges();
                        TempData["SuccesMessage"] = "Car updated succesfully.";
                        return RedirectToAction("CarsList");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, "An error occurred while saving data: " + ex.Message);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "The data entered is not valid.");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "The user cannot be found.");
            }
            return View("EditCarPage", carDto);
        }

        public IActionResult CarDetails(Guid id)
        {
            var car = this._ctx.Cars.SingleOrDefault(car => car.CarId == id);
            if (car == null)
            {
                return NotFound();
            }

            ViewBag.SuccesMessage = TempData["SuccessMessage"];
            return View("CarDetails", car);
        }

        [HttpGet]
        public IActionResult DeleteCar(Guid carId)
        {
            var car = this._ctx.Cars.SingleOrDefault(car => car.CarId == carId);
            if (car != null)
            {
                var dto = new CarDTO
                {
                    CarId = car.CarId,
                    StartAuction = car.StartAuction,
                    EndAuction = car.EndAuction,
                    Make = car.Make,
                    Model = car.Model,
                    Body = car.Body,
                    Color = car.Color,
                    Engine = car.Engine,
                    Fuel = car.Fuel,
                    Transmission = car.Transmission,
                    Gears = car.Gears,
                    Doors = car.Doors,
                    Seats = car.Seats,
                    Tires = car.Tires,
                    Key = car.Key,
                    FirstRegister = car.FirstRegister,
                    Year = car.Year,
                    Mileage = car.Mileage,
                    ReferenceNumber = car.ReferenceNumber,
                    Images = car.Images,
                    NewPrice = car.NewPrice,
                    Value = car.Value,
                    RepairCosts = car.RepairCosts,
                    MoreInfo = car.MoreInfo,
                    SerialEquipment = car.SerialEquipment,
                    RegistrationCertificate = car.RegistrationCertificate,
                    Drive = car.Drive,
                    Damaged = car.Damaged,
                    DateTime = car.DateTime,
                    IsActive = car.IsActive
                };
                return View("DeleteCarPage", dto);
            }
            return RedirectToAction("AdminPage", "Admin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCarConfirm(Guid carId)
        {
            var car = this._ctx.Cars.Find(carId);
            if (car != null)
            {
                try
                {
                    this._ctx.Remove(car);
                    this._ctx.SaveChanges();
                    TempData["SuccesMessage"] = "Car deleted succesfully.";
                    return RedirectToAction("CarsList");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while deleting data." + ex.Message);
                }
            }
            return View("DeleteCarPage", car);
        }

    }
}

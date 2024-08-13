using AutaCH_MD.Contexts;
using AutaCH_MD.DTOs;
using AutaCH_MD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Diagnostics.CodeAnalysis;

namespace AutaCH_MD.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDataContext _ctx;
        private readonly IPasswordHasher<User> _passHasher;

        public AccountController(AppDataContext ctx, IPasswordHasher<User> passHasher)
        {
            _ctx = ctx;
            _passHasher = passHasher;
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(UserDTO userDTO)
        {
            if (!userDTO.TermsAccepted || !userDTO.AknowledgeAccepted)
            {
                if (!userDTO.TermsAccepted)
                {
                    ModelState.AddModelError("TermsAccepted", "You must agree to the terms of use and data policy.");
                }

                if (!userDTO.AknowledgeAccepted)
                {
                    ModelState.AddModelError("AknowledgeAccepted", "You must understand that all offers are binding and cannot be withdrawn.");
                }

                return View(userDTO);
            }

            if (ModelState.IsValid)
            {
                if (_ctx.Users.Any(user => user.Email == userDTO.Email || user.Login == userDTO.Login))
                {
                    ModelState.AddModelError(string.Empty, "User with the same email or login already exists.");
                    return View(userDTO);
                }

                var user = new User
                {
                    FirstName = userDTO.FirstName,
                    LastName = userDTO.LastName,
                    Country = userDTO.Country,
                    City = userDTO.City,
                    PostalCode = userDTO.PostalCode,
                    Street = userDTO.Street,
                    HouseNumber = userDTO.HouseNumber,
                    ApartmentNumber = userDTO.ApartmentNumber,
                    PhoneNumber = userDTO.PhoneNumber,
                    Login = userDTO.Login,
                    Email = userDTO.Email,
                    TermsAccepted = userDTO.TermsAccepted,
                    AknowledgeAccepted = userDTO.AknowledgeAccepted,
                    IsActive = true,
                    Type = "user"
                };
                user.Password = _passHasher.HashPassword(user, userDTO.Password);

                try
                {
                    _ctx.Users.Add(user);
                    _ctx.SaveChanges();
                    TempData["SuccessMessage"] = "Registered successfully.";
                    return RedirectToAction("Login");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while saving data: " + ex.Message);
                }
            }  
            return View(userDTO);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(UserDTO userDTO)
        {
            // Verifică dacă loginul este pentru un admin
            var adminUser = this._ctx.Users
                .FirstOrDefault(user => user.Login == userDTO.Login && user.Type == "admin" && user.IsActive);

            if (adminUser != null)
            {
                var result = _passHasher.VerifyHashedPassword(adminUser, adminUser.Password, userDTO.Password);

                if (result == PasswordVerificationResult.Success)
                {
                    HttpContext.Session.SetString("Type", adminUser.Type);// Salvează "admin" în sesiune
                    HttpContext.Session.SetString("Login", adminUser.Login);
                    var cookie = new CookieOptions
                    {
                        Expires = DateTime.Now.AddYears(1),
                    };

                    Response.Cookies.Append("UserId", adminUser.UserId.ToString(), cookie);
                    Response.Cookies.Append("Type", adminUser.Type.ToString(), cookie);
                    return RedirectToAction("AdminPage", "Admin");
                }
            }

            // Verifică dacă loginul este pentru un utilizator obișnuit
            var existingUser = this._ctx.Users
                .FirstOrDefault(user => user.Login == userDTO.Login && user.IsActive && user.Type == "user" );

            if (existingUser != null)
            {
                var result = _passHasher.VerifyHashedPassword(existingUser, existingUser.Password, userDTO.Password);

                if (result == PasswordVerificationResult.Success)
                {
                    HttpContext.Session.SetString("Login", existingUser.Login);

                    var cookie = new CookieOptions
                    {
                        Expires = DateTime.Now.AddYears(1),
                    };

                    Response.Cookies.Append("UserId", existingUser.UserId.ToString(), cookie);
                    return RedirectToAction("AccountPage", "Account");
                }
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(userDTO);
        }


        public IActionResult AccountPage()
        {
            var userType = HttpContext.Session.GetString("Type");
            var login = HttpContext.Session.GetString("Login");

            if (login != null)
            {
                var user = this._ctx.Users.FirstOrDefault(user => user.Login == login);
                return View(user);
            }
            return RedirectToAction("Login");
        }


        public IActionResult MyProfilePage()
        {
            var userId = Request.Cookies["UserId"];
            if (userId != null)
            {
                var user = this._ctx.Users.SingleOrDefault(user => user.UserId.ToString() == userId.ToString());
                if (user != null)
                {
                    return View(user);
                }
            }
            return RedirectToAction("Login");

        }

        [HttpGet]
        public IActionResult EditProfile(Guid? userId)
        {
            if(!userId.HasValue)
            {
                var currentUser = Request.Cookies["UserId"];
                if(currentUser != null)
                {
                    userId = Guid.Parse(currentUser);
                }
            }

            var existingUser = this._ctx.Users.SingleOrDefault(user => user.UserId == userId);
            if(existingUser != null)
            {
                var dto = new UpdateUserDTO
                {
                    UserId = existingUser.UserId,
                    FirstName = existingUser.FirstName,
                    LastName = existingUser.LastName,
                    Country = existingUser.Country,
                    City = existingUser.City,
                    PostalCode = existingUser.PostalCode,
                    Street = existingUser.Street,
                    HouseNumber = existingUser.HouseNumber,
                    ApartmentNumber = existingUser.ApartmentNumber,
                    PhoneNumber = existingUser.PhoneNumber,
                };
                return View(dto);
            }
            return RedirectToAction("Login");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditProfile(UpdateUserDTO updateUserDto)
        {
            var existingUser = this._ctx.Users.SingleOrDefault(user => user.UserId == updateUserDto.UserId);
            if (existingUser != null)
            {
                if (ModelState.IsValid)
                {
                    existingUser.UserId = updateUserDto.UserId;
                    existingUser.FirstName = updateUserDto.FirstName;
                    existingUser.LastName = updateUserDto.LastName;
                    existingUser.Country = updateUserDto.Country;
                    existingUser.City = updateUserDto.City;
                    existingUser.PostalCode = updateUserDto.PostalCode;
                    existingUser.Street = updateUserDto.Street;
                    existingUser.HouseNumber = updateUserDto.HouseNumber;
                    existingUser.ApartmentNumber = updateUserDto.ApartmentNumber;
                    existingUser.PhoneNumber = updateUserDto.PhoneNumber;
                    try
                    {
                        this._ctx.SaveChanges();
                        TempData["SuccessMessage"] = "Profile updated successfully.";
                        return RedirectToAction("MyProfilePage");
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
                Console.WriteLine("UserId:" + existingUser);
                ModelState.AddModelError(string.Empty, "The user cannot be found.");
            }
            return View(updateUserDto);
        }
        
 
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(ChangePasswordDTO newPassDto)
        {
            // Verifică dacă modelul este valid
            if (!ModelState.IsValid)
            {
                return View(newPassDto);
            }

            var userId = Request.Cookies["UserId"];
            if (userId != null)
            {
                var user = _ctx.Users.SingleOrDefault(u => u.UserId.ToString() == userId);
                if (user != null)
                {
                    // Verifică parola curentă
                    var result = _passHasher.VerifyHashedPassword(user, user.Password, newPassDto.CurrentPass);

                    if (result == PasswordVerificationResult.Success)
                    {
                        // Verifică dacă noua parolă și confirmarea corespund
                        if (newPassDto.NewPass == newPassDto.ConfirmNewPass)
                        {
                            // Hash noua parolă și salvează în baza de date
                            user.Password = _passHasher.HashPassword(user, newPassDto.NewPass);

                            try
                            {
                                _ctx.SaveChanges();
                                TempData["SuccessMessage"] = "Password updated successfully.";
                                return RedirectToAction("MyProfilePage");
                            }
                            catch (Exception ex)
                            {
                                ModelState.AddModelError(string.Empty, "An error occurred while saving data: " + ex.Message);
                            }
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "New password does not match.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "The current password does not match.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "The user cannot be found.");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "User not authenticated.");
            }

            return View(newPassDto);
        }

        public IActionResult MyCars()
        {
            var userIdCookie = Request.Cookies["UserId"];

            if(!Guid.TryParse(userIdCookie, out Guid userId))
            {
                return RedirectToAction("Home");
            }

            var cars = this._ctx.Cars
                .Where(car => car.Bids.Any(bid => bid.UserId == userId))
                .Include(car => car.Bids)
                .Select(car => new CarDTO
                {
                    CarId = car.CarId,
                    Make = car.Make,
                    Model = car.Model,
                    Year = car.Year,
                    Mileage = car.Mileage,
                    ReferenceNumber = car.ReferenceNumber,
                    Images = car.Images,
                    EndAuction = car.EndAuction,
                    UserBid = car.Bids
                        .Where(bid => bid.UserId == userId)
                        .Select(bid => bid.BidAmount)
                        .FirstOrDefault()

                })
                .ToList();

            return View(cars);
        }



        public IActionResult WatchList()
        {

            var cars = this._ctx.Cars.ToList();

            var model = new CarListViewModel
            {
                Cars = cars
            };

            return View("WatchList", model);
        }

        [HttpPost]
        public IActionResult GetWatchList([FromBody] List<Guid> watchListCarIds)
        {
            var cars = _ctx.Cars
                .Where(car => watchListCarIds.Contains(car.CarId))
                .ToList();


            var model = new CarListViewModel
            {
              Cars = cars
            };

            return View("WatchList", model);
        }



        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            Response.Cookies.Append("UserId", "", new CookieOptions
            {
                Expires = DateTime.Now.AddDays(-1),
            });

            Response.Cookies.Append("UserType", "", new CookieOptions
            {
                Expires = DateTime.Now.AddDays(-1),
            });
            return RedirectToAction("Login");
        }


    }
}

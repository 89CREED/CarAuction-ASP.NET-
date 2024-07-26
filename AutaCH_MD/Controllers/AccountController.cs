using AutaCH_MD.Contexts;
using AutaCH_MD.DTOs;
using AutaCH_MD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
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
            if(!userDTO.TermsAccepted || !userDTO.AknowledgeAccepted)
            {
                if(!userDTO.TermsAccepted)
                {
                    ModelState.AddModelError("TermsAccepted", "You must agree to the terms of use and data policy.");
                }

                if(!userDTO.AknowledgeAccepted)
                {
                    ModelState.AddModelError("AknoledgeAccepted", "You must understand that all offers are binding and cannot be withdrawn.");
                }

                return View(userDTO);
            }

            if (ModelState.IsValid)
            {
                if(_ctx.Users.Any(user => user.Email == userDTO.Email || user.Login == userDTO.Login))
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
                };

                user.Password = _passHasher.HashPassword(user, userDTO.Password);
                
                if(user.IsActive == false)
                {
                    user.IsActive = true;
                    this._ctx.Users.Add(user);
                    this._ctx.SaveChanges();
                }
                return RedirectToAction("Login");
            }

            return View(userDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(UserDTO userDTO)
        {
            var existingUser = this._ctx.Users.FirstOrDefault(user => user.Login == userDTO.Login && user.IsActive == true);
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

                    return RedirectToAction("AccountPage");
                }
            }
            
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(userDTO);

        }

        public IActionResult AccountPage()
        {
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditProfile(UpdateUserDTO updateUserDto)
        {
            var userId = Request.Cookies["UserId"];
            if (userId != null)
            {
                var existingUser = _ctx.Users.SingleOrDefault(user => user.UserId.ToString() == userId);
                if (existingUser != null)
                {
                    if (ModelState.IsValid)
                    {
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
                            _ctx.SaveChanges();
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
                    ModelState.AddModelError(string.Empty, "The user cannot be found..");
                }
            }
            return View(updateUserDto);
        }


        public IActionResult EditProfile()
        {
            var userId = Request.Cookies["UserId"];
            if (userId != null)
            {
                var user = _ctx.Users.SingleOrDefault(user => user.UserId.ToString() == userId);
                if (user != null)
                {
                    var dto = new UpdateUserDTO
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Country = user.Country,
                        City = user.City,
                        PostalCode = user.PostalCode,
                        Street = user.Street,
                        HouseNumber = user.HouseNumber,
                        ApartmentNumber = user.ApartmentNumber,
                        PhoneNumber = user.PhoneNumber,
                    };

                    return View(dto);
                }
            }
            return RedirectToAction("Login");
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



        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            Response.Cookies.Append("UserId", "", new CookieOptions
            {
                Expires= DateTime.Now.AddDays(-1),
            });
            return RedirectToAction("Index", "Home");
        }


    }
}

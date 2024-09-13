using AutaCH_MD.Contexts;
using AutaCH_MD.DTOs;
using AutaCH_MD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace AutaCH_MD.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDataContext _ctx;

        public AdminController(AppDataContext ctx)
        {
            _ctx = ctx;
        }

        public IActionResult AdminPage()
        {
            var userType = HttpContext.Session.GetString("Type");
            var adminUser = HttpContext.Session.GetString("Login");

            if (adminUser != null)
            {
                var admin = this._ctx.Users.FirstOrDefault(user => user.Type == userType);
                return View(admin);
            }
            return RedirectToAction("Login", "Account");
        }

        public IActionResult UsersList()
        {
            var users = this._ctx.Users.ToList();
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            return View("UsersList", users);
        }


        [HttpGet]
        public IActionResult EditUser(Guid userId)
        {
            var existingUser = this._ctx.Users.SingleOrDefault(user => user.UserId == userId);
            if (existingUser != null)
            {
                var dto = new AdminUpdateUserDTO
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
                    Login = existingUser.Login,
                    Password = existingUser.Password,
                    Email = existingUser.Email,
                    IsActive = existingUser.IsActive,
                    DateTime = existingUser.DateTime,
                    Type = existingUser.Type
                };
                return View("EditUserPage", dto);
            }
            return RedirectToAction("AdminPage");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditUser(AdminUpdateUserDTO updateUserDto)
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
                    existingUser.Login = updateUserDto.Login;
                    existingUser.Password = updateUserDto.Password;
                    existingUser.Email = updateUserDto.Email;
                    existingUser.IsActive = updateUserDto.IsActive;
                    existingUser.DateTime = updateUserDto.DateTime;
                    existingUser.Type = updateUserDto.Type;

                    try
                    {
                        this._ctx.SaveChanges();
                        TempData["SuccessMessage"] = "User updated successfully.";
                        return RedirectToAction("UsersList");
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
            return View("EditUserPage", updateUserDto);
        }

        [HttpGet]
        public IActionResult DeleteUser(Guid userId)
        {
            var user = this._ctx.Users.SingleOrDefault(user => user.UserId == userId);
            if (user != null)
            {
                var dto = new AdminUpdateUserDTO
                {
                    UserId = user.UserId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Country = user.Country,
                    City = user.City,
                    PostalCode = user.PostalCode,
                    Street = user.Street,
                    HouseNumber = user.HouseNumber,
                    ApartmentNumber = user.ApartmentNumber,
                    PhoneNumber = user.PhoneNumber,
                    Login = user.Login,
                    Password = user.Password,
                    Email = user.Email,
                    IsActive = user.IsActive,
                    DateTime = user.DateTime,
                    Type = user.Type
                };
                return View("DeleteUserPage", dto);
            }
            return RedirectToAction("AdminPage");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteUserConfirm(Guid userId)
        {
            var user = this._ctx.Users.Find(userId);
            if (user != null)
            {
                try
                {
                    this._ctx.Remove(user);
                    this._ctx.SaveChanges();
                    TempData["SuccesMessage"] = "User deleted succesfully.";
                    return RedirectToAction("UsersList");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while deleting data." + ex.Message);
                }
            }
            return View("AdminPage");
        }
    }
}

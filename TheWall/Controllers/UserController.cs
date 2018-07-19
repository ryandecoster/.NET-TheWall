using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TheWall.Models;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace TheWall.Controllers
{
    
    public class UserController : Controller
    {
        private YourContext _context;
        public UserController(YourContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("login/user")]
        public IActionResult LoginUser()
        {
            return View("Login");
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(User userReg)
        {
            User check = _context.Users.FirstOrDefault(u => u.Email == userReg.Email);
            if (check != null) {
                ModelState.AddModelError("email", "Email already exists.");
                return View("Index", userReg);
            }

            if (ModelState.IsValid){
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                userReg.Password = Hasher.HashPassword(userReg, userReg.Password);
                _context.Add(userReg);
                _context.SaveChanges();
                ViewBag.User = userReg;
                HttpContext.Session.SetInt32("id", userReg.Id);
                int? user_id = userReg.Id;
                return RedirectToAction("Wall", "Home");
            }
            else {
                return View("Index", userReg);
            }
            
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(User userLog)
        {
            User check = _context.Users.FirstOrDefault(u => u.Email == userLog.Email);
            if(check != null && userLog.Password != null)
            {
                var Hasher = new PasswordHasher<User>();
                if(0 != Hasher.VerifyHashedPassword(check, check.Password, userLog.Password))
                {
                    ViewBag.User = check.Id;
                    HttpContext.Session.SetInt32("id", check.Id);
                    int? user_id = check.Id;
                    return RedirectToAction("Wall", "Home");
                }
                ModelState.AddModelError("password", "Incorrect email or password.");
                return View("Login", userLog);
                
            }
            ModelState.AddModelError("email", "Incorrect email or password.");
            return View("Login", userLog);
        }

        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["success"] = "You have successfully logged out.";
            return Redirect("/");
        }
    }
}


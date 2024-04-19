using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Lamborghini.Models;

namespace Lamborghini.Controllers;

public class UserController : Controller
{
    private readonly ILogger<UserController> _logger;

    private MyContext _context;

    public UserController(ILogger<UserController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet("")]
    public IActionResult Index()
    {
        List<string> Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
        Errors.ForEach(Console.WriteLine);

        return View();
    }

    [HttpPost("users/create")]
    public IActionResult RegisterUser(User newUser)
    {
        List<string> Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
        Errors.ForEach(Console.WriteLine);

        if (!ModelState.IsValid)
        {
            return View("Index");
        }
        PasswordHasher<User> hasher =new();
        newUser.Password = hasher.HashPassword(newUser, newUser.Password);
        _context.Add(newUser);
        _context.SaveChanges();
        HttpContext.Session.SetInt32("UserId", newUser.UserId);
        return RedirectToAction ("AllWeddings" , "Wedding");
    }

    [HttpPost("users/login")]
    public IActionResult LoginUser(LogUser logAttempt)
    {
        List<string> Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
        Errors.ForEach(Console.WriteLine);


        if (!ModelState.IsValid)
        {
            return View("Index");
        }
        User? dbUser = _context.Users.FirstOrDefault(u => u.Email == logAttempt.LogEmail);
        if (dbUser == null)
        {
            ModelState.AddModelError("LogPassword", "Invalid Credentials (e)");
            return View("Index");
        }
        PasswordHasher<LogUser> hasher = new();
        PasswordVerificationResult pwCompareResult = hasher.VerifyHashedPassword(logAttempt, dbUser.Password, logAttempt.LogPassword);
        if (pwCompareResult == 0)
        {
            ModelState.AddModelError("LogPassword", "Invalid Credentials (p)");
            return View("Index");
        }

        HttpContext.Session.SetInt32("UserId", dbUser.UserId);
        ViewBag.LoggedUserName = dbUser.FirstName;
        return RedirectToAction("AllWeddings", "Wedding");
    }

    [HttpPost("user/logout")]
    public RedirectToActionResult Logout()
    {
        List<string> Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
        Errors.ForEach(Console.WriteLine);

        HttpContext.Session.Remove("UserId");
        return RedirectToAction("Index");
    }

}

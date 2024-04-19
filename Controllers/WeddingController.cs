using Microsoft.AspNetCore.Mvc;
using Lamborghini.Models;
using Microsoft.EntityFrameworkCore;
namespace Lamborghini.Controllers;


[SessionCheck]
public class WeddingController : Controller
{
    private readonly ILogger<WeddingController> _logger;
    private MyContext _context;

    public WeddingController(ILogger<WeddingController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }


    [HttpGet("weddings")]
    public ViewResult AllWeddings()
    {


        List<string> Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
        Errors.ForEach(Console.WriteLine);

        List<Wedding> Weddings = _context.Weddings.OrderByDescending(p => p.CreatedAt).Take(100).ToList();
        return View(Weddings);
    }

    [HttpGet("weddings/new")]
    public ViewResult NewWedding() => View();

    [HttpPost("weddings/create")]
    public IActionResult CreateWedding(Wedding newWedding)
    {
        List<string> Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
        Errors.ForEach(Console.WriteLine);

        if (!ModelState.IsValid)
        {
            return View("newWedding");
        }
        _context.Add(newWedding);
        _context.SaveChanges();
        return RedirectToAction("AllWeddings");
    }


    [HttpGet("weddings/{weddingId}")]
    public IActionResult ViewWedding(int weddingId)
    {
        List<string> Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
        Errors.ForEach(Console.WriteLine);

           Wedding? Wedding = _context.Weddings.Include(w => w.Rsvps).FirstOrDefault(w => w.WeddingId == weddingId);

            ViewBag.NotRsvpYet = _context.Users.Include(u => u.Rsvps)
                                                        .ThenInclude(r => r.Wedding)
                                                        .Where(r => !r.Rsvps
                                                        .Any(u => u.WeddingId == weddingId));

        Wedding? wedding = _context.Weddings.FirstOrDefault(p => p.WeddingId == weddingId);
        if (wedding == null)
        {
            return RedirectToAction("AllWeddings");
        }
        return View(wedding);
    }


    [HttpPost("wedding/{weddingId}/rsvp")]
    public IActionResult AddingRsvp(Rsvp newRsvp)
    {
        int? userId = HttpContext.Session.GetInt32("UserId");
         if (userId == null)
    {
        return RedirectToAction("Login");
    }


        _context.Add(newRsvp);
        _context.SaveChanges();
        return RedirectToAction("AllWeddings");
    }

    [SessionCheck]
    [HttpGet("weddings/{weddingId}/edit")]
    public IActionResult EditWedding(int weddingId)
    {
        List<string> Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
        Errors.ForEach(Console.WriteLine);

        Wedding? toBeEdited = _context.Weddings.FirstOrDefault(w => w.WeddingId == weddingId);
        if (toBeEdited == null)
        {
            return RedirectToAction("AllWeddings");
        }
        return View(toBeEdited);
    }

    [HttpPost("weddings/{postId}/update")]
    public IActionResult UpdateWedding(int weddingId, Wedding editedWedding)
    {
        List<string> Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
        Errors.ForEach(Console.WriteLine);

        Wedding? OldWedding = _context.Weddings.FirstOrDefault(w => w.WeddingId == weddingId);
        if (!ModelState.IsValid || OldWedding == null)
        {
            if (OldWedding == null)
            {
                ModelState.AddModelError("WedderOne", "Wedding not found in DB");
            }
             return View("EditedWedding", OldWedding);
        }
        OldWedding.WedderOne = editedWedding.WedderOne;
        OldWedding.WedderTwo = editedWedding.WedderTwo;
        OldWedding.Date = editedWedding.Date;
        OldWedding.Address = editedWedding.Address;
        OldWedding.UpdateAt = DateTime.Now;
        _context.SaveChanges();
        return RedirectToAction("ViewWedding", new {weddingId = weddingId});
    }

    [HttpPost("weddings/{deleteId}/delete")]
    public RedirectToActionResult DeleteWedding(int deleteId)
    {
        List<string> Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
        Errors.ForEach(Console.WriteLine);

         Wedding? DeleteMe = _context.Weddings.SingleOrDefault(w => w.WeddingId == deleteId);
        if (DeleteMe != null)
        {
            _context.Remove(DeleteMe);
            _context.SaveChanges();
        }
         return RedirectToAction("AllWeddings");
    }





}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using ManagerforSubjects.Data; 
using ManagerforSubjects.Models;
namespace ManagerforSubjects.Controllers
{
    public class SubjectController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubjectController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Subject()
        {
            var subjects = await _context.Subjects.ToListAsync();  // Ensure you're using _context.Subjects
            return View(subjects);
        }

        public IActionResult Create()
        {
            return View(new Subject()); // Ensure you are passing a new Subject object
        }


        // POST: Subject/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description")] Subject subject)
        {
            if (ModelState.IsValid)
            {
                // Add the new subject to the database
                _context.Add(subject);
                await _context.SaveChangesAsync();

                // Redirect to the index or any other page
                return RedirectToAction(nameof(Subject)); // Or whatever action you want to redirect to
            }

            return View(subject); // If the model state is invalid, return the view with the model
        }

    }
}

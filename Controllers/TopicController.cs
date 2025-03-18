using ManagerforSubjects.Data;
using ManagerforSubjects.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ManagerforSubjects.Controllers
{
    public class TopicController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TopicController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Topic
        public async Task<IActionResult> Index()
        {
            // Retrieve all topics from the database
            var topics = await _context.Topics.Include(t => t.Subject).ToListAsync();
            return View(topics); // Pass the list of topics to the view
        }


        public async Task<IActionResult> Create()
        {
            // Fetch the list of subjects from the database
            var subjects = await _context.Subjects.ToListAsync();

            // Debugging: Check if subjects are being fetched correctly
            Console.WriteLine("Subjects fetched: " + subjects.Count);

            ViewBag.Subjects = subjects;
            return View();
        }






        // POST: Topic/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,SubjectId")] Topic topic)
        {


            Console.WriteLine("POST method reached");
            Console.WriteLine($"Name: {topic.Name}, Description: {topic.Description}, SubjectId: {topic.SubjectId}");

            
                var subject = await _context.Subjects.FindAsync(topic.SubjectId);
                if (subject != null)
                {
                    _context.Topics.Add(topic);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Subject not found.");
            
            return View(topic);
        }







    }
}

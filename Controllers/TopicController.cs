using ManagerforSubjects.Data;
using ManagerforSubjects.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public IActionResult GetBySubject(int subjectId)
        {
            var topics = _context.Topics.Where(t => t.SubjectId == subjectId).ToList();
            return PartialView("_TopicsTable", topics);
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




        // GET: Topic/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            var topic = await _context.Topics.FindAsync(id);
            if (topic == null)
            {
                return NotFound();
            }

            ViewBag.Subjects = new SelectList(_context.Subjects, "Id", "Name", topic.SubjectId);
            return View(topic);
        }

        // POST: Topic/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Topic topic)
        {
            if (id != topic.Id)
            {
                return BadRequest();
            }

            
                try
                {
                    _context.Update(topic);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Topics.Any(t => t.Id == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            

            ViewBag.Subjects = new SelectList(_context.Subjects, "Id", "Name", topic.SubjectId);
            return View(topic);
        }

        // POST: Topic/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var topic = await _context.Topics.FindAsync(id);
            if (topic == null)
            {
                return NotFound();
            }

            _context.Topics.Remove(topic);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }



}

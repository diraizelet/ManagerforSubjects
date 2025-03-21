using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ManagerforSubjects.Models;
using ManagerforSubjects.Data;

namespace ManagerforSubjects.Controllers
{
    public class SubTopicController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubTopicController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult GetByTopic(int topicId)
        {
            var subTopics = _context.SubTopics.Where(st => st.TopicId == topicId).ToList();
            return PartialView("~/Views/SubTopic/_SubTopicsTable.cshtml", subTopics);
        }

        // GET: SubTopics

        public IActionResult Edit(int id)
        {
            var subTopic = _context.SubTopics.Include(s => s.Topic)
                                              .FirstOrDefault(s => s.Id == id);

            if (subTopic == null)
            {
                return NotFound();
            }

            ViewBag.Topics = _context.Topics.ToList(); // Ensure topics are passed

            return View(subTopic);
        }


        public async Task<IActionResult> Index()
        {
            // Retrieve all subtopics along with their associated topics and subjects
            var subTopics = await _context.SubTopics
                                          .Include(st => st.Topic)        // Include Topic to get topic data
                                          .ThenInclude(t => t.Subject)    // Include Subject to get subject data
                                          .ToListAsync();                // Fetch all subtopics with associated topics and subjects

            return View(subTopics); // Pass the subtopics to the view
        }

        // GET: SubTopic/Create
        [HttpGet]
        public IActionResult Create(int topicId)
        {
            var topic = _context.Topics.Include(t => t.Subject).FirstOrDefault(t => t.Id == topicId);
            if (topic == null)
            {
                return NotFound(); // Handle topic not found
            }

            ViewBag.TopicName = topic.Name;

            // Pass an empty SubTopic object with TopicId set to the correct TopicId
            return View(new SubTopic { TopicId = topic.Id });
        }

        // POST: SubTopic/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubTopic subTopic)
        {
            // Check if the model state is valid
            
                // Log the received data
                Console.WriteLine($"SubTopic Name: {subTopic.Name}, Description: {subTopic.Description}, TopicId: {subTopic.TopicId}");

                // Fetch the topic to ensure it's valid
                var topic = await _context.Topics.FindAsync(subTopic.TopicId);
                if (topic == null)
                {
                    ModelState.AddModelError("", "Topic not found.");
                    return View(subTopic); // Return view if the topic is not found
                }

                _context.SubTopics.Add(subTopic);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "SubTopic"); // Redirect after saving
            

            // If ModelState is not valid, return to the view
            return View(subTopic);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SubTopic subTopic)
        {
            // Check if the ID matches the SubTopic's ID
            if (id != subTopic.Id)
            {
                return NotFound(); // If ID doesn't match, return NotFound
            }

            
                try
                {
                    // Update the SubTopic entity in the database
                    _context.Update(subTopic);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Handle concurrency issues if any
                    if (!_context.SubTopics.Any(s => s.Id == subTopic.Id))
                    {
                        return NotFound(); // If SubTopic does not exist anymore
                    }
                    else
                    {
                        throw; // Re-throw if a different exception occurred
                    }
                }

                return RedirectToAction(nameof(Index)); // Redirect to the Index page after successful edit
            

            // If the model is not valid, return the view with the current data
            ViewBag.Topics = _context.Topics.ToList(); // Ensure Topics are still passed in case of validation errors
            return View(subTopic); // Return to the Edit page with errors
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var subTopic = await _context.SubTopics.FindAsync(id);

            if (subTopic == null)
            {
                return NotFound();
            }

            _context.SubTopics.Remove(subTopic);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}

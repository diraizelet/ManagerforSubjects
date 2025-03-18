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
        // GET: SubTopics
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

    }
}

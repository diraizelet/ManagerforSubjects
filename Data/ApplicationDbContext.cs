using Microsoft.EntityFrameworkCore;
using ManagerforSubjects.Models;
using System.Linq;

namespace ManagerforSubjects.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets for the models
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<SubTopic> SubTopics { get; set; }

        // Fluent API to define relationships and constraints if needed
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // One-to-many relationship: Subject -> Topic
            modelBuilder.Entity<Subject>()
                .HasMany(s => s.Topics)
                .WithOne(t => t.Subject)
                .HasForeignKey(t => t.SubjectId)
                .OnDelete(DeleteBehavior.Cascade); // Ensures cascading delete

            // One-to-many relationship: Topic -> SubTopic
            modelBuilder.Entity<Topic>()
                .HasMany(t => t.SubTopics)
                .WithOne(st => st.Topic)
                .HasForeignKey(st => st.TopicId)
                .OnDelete(DeleteBehavior.Cascade); // Ensures cascading delete
        }

        // Methods to add Topic to Subject and SubTopic to Topic
        public void AddTopicToSubject(int subjectId, string topicName)
        {
            // Retrieve the Subject with its Topics
            var subject = Subjects.Include(s => s.Topics).FirstOrDefault(s => s.Id == subjectId);

            if (subject != null)
            {
                // Create a new Topic instance
                var topic = new Topic
                {
                    Name = topicName,
                    SubjectId = subject.Id // Set the foreign key to the Subject
                };

                // Add the Topic to the Subject's Topics list
                subject.Topics.Add(topic);

                // Save changes to persist the new Topic and relationship
                SaveChanges();
            }
        }

        public void AddSubTopicToTopic(int topicId, string subTopicName, string description)
        {
            // Retrieve the Topic with its SubTopics
            var topic = Topics.Include(t => t.SubTopics).FirstOrDefault(t => t.Id == topicId);

            if (topic != null)
            {
                // Create a new SubTopic instance
                var subTopic = new SubTopic
                {
                    Name = subTopicName,
                    Description = description,
                    TopicId = topic.Id // Set the foreign key to the Topic
                };

                // Add the SubTopic to the Topic's SubTopics list
                topic.SubTopics.Add(subTopic);

                // Save changes to persist the new SubTopic and relationship
                SaveChanges();
            }
        }
    }
}

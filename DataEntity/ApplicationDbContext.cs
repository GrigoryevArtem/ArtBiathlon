
using ArtBiathlon.DataEntity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace ArtBiathlon.DataEntity
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
           //  Database.EnsureDeleted();
            // Database.EnsureCreated();        
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Help> Helps { get; set; }
        public DbSet<CampPeriod> CampsPeriods { get; set; }
        public DbSet<HrvIndicator> HrvIndicators { get; set; }
        public DbSet<Training> Trainings { get; set; }
        public DbSet<TrainingSchedule> TrainingsSchedules { get;set; }
        public DbSet<TrainingType> TrainingTypes { get; set; }
        public DbSet<Mailing> Mailings { get; set; }
        public DbSet<MailingTopic> MailingTopics { get; set; }
        public DbSet<MailingTopicSubscriber> MailingTopicSubscribers { get; set;}
        public DbSet<ForumMessage> ForumMessages { get; set; }   
        public DbSet<ForumReaction> ForumReactions { get; set; }



    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Mailing_list_app.Models
{
    public class dbContext : DbContext
    {
        public dbContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<SMTP> SMTPs { get; set; }
        public DbSet<Recipient> Recipients { get; set; }
        public DbSet<SendDBEmail> SendDBEmails { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Recipient>()
                        .HasMany<UserProfile>(s => s.UserProfiles)
                        .WithMany(c => c.Recipients)
                        .Map(cs => 
                        {
                            cs.MapLeftKey("RecipientsRefId");
                            cs.MapRightKey("UserProfilesRefId");
                            cs.ToTable("RecipientsUserProfiles");
                        });

            modelBuilder.Entity<Recipient>()
            .HasMany<SendDBEmail>(s => s.SendDBEmails)
            .WithMany(c => c.Recipients)
            .Map(cs => 
            {
                cs.MapLeftKey("RecipientsRefId");
                cs.MapRightKey("SendDBEmailsRefId");
                cs.ToTable("RecipientsSendDBEmails");
            });

        }
    }
}
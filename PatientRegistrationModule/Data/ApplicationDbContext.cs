using Microsoft.EntityFrameworkCore;
using PatientRegistrationModule.Models;

namespace PatientRegistrationModule.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        public DbSet<Patient>Patients { get; set; }
        public DbSet<OTPVerification>OTPVerifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Patient>().Property(p=>p.Gender).HasConversion<string>();
            modelBuilder.Entity<Patient>().HasIndex(p => p.Mobile).IsUnique();
            modelBuilder.Entity<Patient>().HasIndex(p => p.Email).IsUnique();
            modelBuilder.Entity<Patient>().HasIndex(p=> p.PatientCode).IsUnique();
        }
    }
}

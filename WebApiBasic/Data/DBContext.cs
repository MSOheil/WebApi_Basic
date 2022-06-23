using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiBasic.Models;
using WebApplication1.Models;

namespace WebApiBasic.Data
{
    public class DBContext : IdentityDbContext<ApplicationUser>
    {
        public DBContext(DbContextOptions<DBContext> options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Visit>().HasOne(a => a.Patient)
                .WithMany(a => a.Visits).HasForeignKey(a => a.PatientId);   
            modelBuilder.Entity<Patient>().HasMany(a => a.Visits)
                .WithOne(s => s.Patient);

            modelBuilder.Entity<Visit>().HasOne(a => a.ApplicationUser)
                .WithMany(a => a.Visit).HasForeignKey(s => s.UserId);

            modelBuilder.Entity<Visit>().HasOne(a => a.ApplicationUser)
                 .WithMany(a => a.Visit).HasForeignKey(s => s.DoctorId);

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Visit> Visits { get; set; }

    }
}

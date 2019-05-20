using System;
using System.Collections.Generic;
using System.Text;
using CarnivorousPlants.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarnivorousPlants.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<MyProject> MyProjects { get; set; }
        public DbSet<MyTag> MyTags { get; set; }
        public DbSet<DefaultProjectHistory> DefaultProjectHistories { get; set; }
        public DbSet<ImageWaitingToConfirm> ImagesWaitingToConfirm { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<MyProject>()
                .HasMany(x => x.MyTags)
                .WithOne(x => x.MyProject)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

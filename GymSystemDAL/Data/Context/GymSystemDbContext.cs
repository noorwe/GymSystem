using GymSystemDAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Data.Context
{
    public class GymSystemDbContext : IdentityDbContext<ApplicationUser>
    {

        public GymSystemDbContext(DbContextOptions<GymSystemDbContext> options) : base(options)
        {
        }

      

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<ApplicationUser>(AU =>
            {
                AU.Property(X => X.FirstName).HasColumnType("varchar")
                .HasMaxLength(50);
                AU.Property(X => X.LastName).HasColumnType("varchar")
                .HasMaxLength(50);
            });
        }


        #region Tables

        public DbSet<Member> Members { get; set; }
        public DbSet<HealthRecord> HealthRecords { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Membership> Memberships { get; set; }

        public DbSet<MemberSession> MemberSessions { get; set; }

        #endregion
    }

}

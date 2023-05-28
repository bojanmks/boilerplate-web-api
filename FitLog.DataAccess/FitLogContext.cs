using FitLog.DataAccess.Entities;
using FitLog.DataAccess.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.DataAccess
{
    public class FitLogContext : DbContext
    {
        public FitLogContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
            modelBuilder.SetupInitialData();
            modelBuilder.AddQueryFilterForSoftDeletableEntities();

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.OnAddedBehaviour();
                        break;
                    case EntityState.Deleted:
                        entry.OnDeletedBehaviour();
                        break;
                }
            }

            return base.SaveChanges();
        }

        public DbSet<Translation> Translations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<JwtTokenRecord> JwtTokenRecords { get; set; }
    }
}

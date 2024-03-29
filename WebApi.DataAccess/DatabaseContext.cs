﻿using Microsoft.EntityFrameworkCore;
using WebApi.DataAccess.Entities;
using WebApi.DataAccess.Extensions;

namespace WebApi.DataAccess
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options) { }

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
                }
            }

            return base.SaveChanges();
        }

        public DbSet<Translation> Translations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<JwtTokenRecord> JwtTokenRecords { get; set; }
    }
}

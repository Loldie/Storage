using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Storage.Models
{
    public class StorageContext : DbContext
    {
        public DbSet<StorageFile> StorageFiles { get; set; }
        public DbSet<UserStorageFileEntry> UserStorageFileEntries { get; set; }

        public StorageContext(DbContextOptions<StorageContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserStorageFileEntry>()
                .Property(u => u.UploadDate)
                .HasDefaultValueSql("GETDATE()");

        }
    }
}

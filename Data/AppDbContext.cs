using Microsoft.EntityFrameworkCore;
using PharmaPulseApp.Models;

namespace PharmaPulseApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pharmacy_Medicine>().HasKey(pm => new
            {
                pm.PharmacyId,
                pm.MedicineId
            });

            modelBuilder.Entity<Pharmacy_Medicine>()
                .HasOne(p => p.Pharmacy)
                .WithMany(pm => pm.Pharmacy_Medicines)
                .HasForeignKey(p => p.PharmacyId);

            modelBuilder.Entity<Pharmacy_Medicine>()
                .HasOne(m => m.Medicine)
                .WithMany(pm => pm.Pharmacy_Medicines)
                .HasForeignKey(m => m.MedicineId);


            base.OnModelCreating(modelBuilder);
        }

        public DbSet<AgeRange> AgeRanges { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Pharmacy> Pharmacies { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<Pharmacy_Medicine> Pharmacy_Medicines { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Subscriber> Subscribers { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}

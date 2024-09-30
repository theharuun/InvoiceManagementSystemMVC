using InvoiceManagementSystemMVC.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace InvoiceManagementSystemMVC.DataContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Apartment> Apartments { get; set; }
        public DbSet<ApartmentType> ApartmentTypes { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<BillType> BillTypes { get; set; }
        public DbSet<UserAdmin> UserAdmins { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Primary Keys
            modelBuilder.Entity<Apartment>().HasKey(a => a.ApartmentId);
            modelBuilder.Entity<ApartmentType>().HasKey(at => at.ATypeId);
            modelBuilder.Entity<Bill>().HasKey(b => b.BillId);
            modelBuilder.Entity<BillType>().HasKey(bt => bt.BTypeId);
            modelBuilder.Entity<UserAdmin>().HasKey(u => u.UserId);

   

            // Apartment ile ApartmentType ilişkisi (Güncellenmiş)
        modelBuilder.Entity<Apartment>()
                .HasOne(a => a.ApartmentTypeNavigation)
                .WithMany(at => at.Apartments)
                .HasForeignKey(a => a.ApartmentTypeId)
               .HasConstraintName("FK_Apartment_ApartmentType");



         modelBuilder.Entity<Apartment>()
             .HasOne(a => a.UserAdmin)
             .WithOne(u => u.Apartment)
             .HasForeignKey<Apartment>(a => a.UserId)
             .OnDelete(DeleteBehavior.SetNull)  // Kullanıcı silindiğinde UserId null yapılır, apartman silinmez
             .IsRequired(false)  // Bu satır opsiyonel olduğunu belirtiyor
             .HasConstraintName("FK_Apartment_UserAdmin");



            // Apartment ile Bill ilişkisi (One-to-Many)
            modelBuilder.Entity<Bill>()
                .HasOne(b => b.Apartment)  // Bill bir Apartment'a ait
                .WithMany(a => a.Bills)    // Bir Apartment birden fazla Bill'e sahip olabilir
                .HasForeignKey(b => b.ApartmentId)
                .HasConstraintName("FK_Bill_Apartment");

            modelBuilder.Entity<Bill>()
                .HasOne(b => b.BillTypeN)
                .WithMany(bt => bt.Bills)
                .HasForeignKey(b => b.BillType)
                .HasConstraintName("FK_Bill_BillType");

            modelBuilder.Entity<Message>()
              .HasOne(m => m.Sender)
              .WithMany(u => u.MessagesSent)
              .HasForeignKey(m => m.SenderId)
              .OnDelete(DeleteBehavior.Restrict); // Silme davranışını belirtin

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany(u => u.MessagesReceived)
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict); // Silme davranışını belirtin


        }
    }

}

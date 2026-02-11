using Microsoft.EntityFrameworkCore;
using Api.Models;

namespace Api.Data
{
    public class LotteryDbContext : DbContext
    {
        public LotteryDbContext(DbContextOptions<LotteryDbContext> options) : base(options)
        {
        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Present> Presents { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<Donor> Donors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketItem> BasketItem { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. נטרול מחיקה משורשרת עבור הזוכה במתנה
            modelBuilder.Entity<Present>()
                .HasOne(p => p.Winner)
                .WithMany()
                .HasForeignKey(p => p.WinnerId)
                .OnDelete(DeleteBehavior.NoAction); // קריטי

            // 2. נטרול מחיקה משורשרת עבור רכישות (הגורם הסביר לכפל הנתיבים)
            modelBuilder.Entity<Purchase>()
                .HasOne(p => p.Person)
                .WithMany()
                .HasForeignKey(p => p.PersonId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Purchase>()
                .HasOne(p => p.Present)
                .WithMany()
                .HasForeignKey(p => p.PresentId)
                .OnDelete(DeleteBehavior.NoAction);

            // 3. אם יש לך BasketItem, גם שם עלולה להיות בעיה
            modelBuilder.Entity<BasketItem>()
                .HasOne(bi => bi.Present)
                .WithMany()
                .HasForeignKey(bi => bi.PresentId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
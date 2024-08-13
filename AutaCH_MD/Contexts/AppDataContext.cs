using Microsoft.EntityFrameworkCore;
using AutaCH_MD.Models;

namespace AutaCH_MD.Contexts
{
    public class AppDataContext : DbContext
    {
        public AppDataContext(DbContextOptions<AppDataContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Bid> Bids { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                        .Property(user => user.IsActive)
                        .HasDefaultValue(true);
            modelBuilder.Entity<User>()
                        .Property(user => user.UserId)
                        .HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<User>()
                        .Property(user => user.DateTime)
                        .HasDefaultValueSql("GETDATE()");


            modelBuilder.Entity<Car>()
                        .Property(car => car.IsActive)
                        .HasDefaultValue(true);
            modelBuilder.Entity<Car>()
                        .Property(car => car.CarId)
                        .HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Car>()
                        .Property(user => user.DateTime)
                        .HasDefaultValueSql("GETDATE()");


            modelBuilder.Entity<Bid>()
                        .Property(bid => bid.IsActive)
                        .HasDefaultValue(true);
            modelBuilder.Entity<Bid>()
                        .Property(bid => bid.BidId)
                        .HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Bid>()
                        .Property(user => user.DateTime)
                        .HasDefaultValueSql("GETDATE()");


            //Configurarea relatiilor User-Bids
            //Un utilizator poate avea mai multe Bids, dar o licitatie apartine unui singur User.
            modelBuilder.Entity<User>()
                        .HasMany(user => user.Bids)
                        .WithOne(bid => bid.User)
                        .HasForeignKey(bid => bid.UserId);

            //Configurarea relatiilor Car-Bids
            //O masina poate avea mai multe Bids, dar o licitatie apartine unei singure masini
            modelBuilder.Entity<Car>()
                        .HasMany(car => car.Bids)
                        .WithOne(bid => bid.Car)
                        .HasForeignKey(bid => bid.CarId);

            modelBuilder.Entity<Bid>()
                .HasOne(bid => bid.User)
                .WithMany(user => user.Bids)
                .HasForeignKey(bid => bid.UserId);

            modelBuilder.Entity<Bid>()
            .HasOne(b => b.Car)
            .WithMany(c => c.Bids)
            .HasForeignKey(b => b.CarId);



        }
    }
}

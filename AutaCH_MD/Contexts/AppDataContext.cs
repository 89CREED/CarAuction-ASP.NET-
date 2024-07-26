using Microsoft.EntityFrameworkCore;
using AutaCH_MD.Models;

namespace AutaCH_MD.Contexts
{
    public class AppDataContext : DbContext
    {
        public AppDataContext(DbContextOptions<AppDataContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<CarInformation> CarInformation { get; set; }
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


            modelBuilder.Entity<CarInformation>()
                        .Property(carInfo => carInfo.IsActive)
                        .HasDefaultValue(true);
            modelBuilder.Entity<CarInformation>()
                        .Property(carInfo => carInfo.InfoId)
                        .HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<CarInformation>()
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

            //Configurarea relatiilor Car-CarInformation
            //O masina are o singura CarInformation, iar o informatie apratine doar unei singure masini
            modelBuilder.Entity<Car>()
                        .HasOne(car => car.CarInformation)
                        .WithOne(carInfo => carInfo.Car)
                        .HasForeignKey<CarInformation>(carInfo => carInfo.CarId);


        }
    }
}

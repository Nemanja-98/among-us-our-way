using Microsoft.EntityFrameworkCore;

namespace AmongUs_OurWay.Models {
    public class AmongUsContext : DbContext {
        public AmongUsContext() { }
        public AmongUsContext(DbContextOptions<AmongUsContext> options) : base (options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\APSProjekat;Initial Catalog=model;Integrated Security=True");
            optionsBuilder.UseLazyLoadingProxies();
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Game> Games { get; set; }
        
        public DbSet<PlayerAction> PlayerActions { get; set; }

        public DbSet<GameHistory> GameHistorys { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //User
            modelBuilder.Entity<User>().HasMany(u => u.Games).WithOne(gH => gH.User).HasForeignKey(gH => gH.UserId).OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<User>().HasMany(u => u.Friends).WithOne(f => f.User1).HasForeignKey(f => f.User1Ref).OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<User>().HasMany(u => u.SentRequests).WithOne(pR => pR.UserSent).HasForeignKey(pR => pR.UserSentRef).OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<User>().HasMany(u => u.PendingRequests).WithOne(pR => pR.UserReceived).HasForeignKey(pR => pR.UserReceivedRef).OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<User>().HasMany(u => u.Actions).WithOne(pA => pA.User).HasForeignKey(pA => pA.UserId).OnDelete(DeleteBehavior.ClientCascade);
            //Game
            modelBuilder.Entity<Game>().HasMany(g => g.Actions).WithOne(pA => pA.Game).HasForeignKey(pA => pA.GameId).OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<Game>().HasMany(g => g.Players).WithOne(gH => gH.Game).HasForeignKey(gH => gH.GameId).OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
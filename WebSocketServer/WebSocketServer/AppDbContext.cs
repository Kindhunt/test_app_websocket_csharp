using Microsoft.EntityFrameworkCore;

namespace WebSocketServer
{
    class NotificationDB : Notification {
        public int Id { get; set; }
        public NotificationDB() {}
        public NotificationDB(Notification notification)
        {
            Event = notification.Event;
            UserId = notification.UserId;
            MessageType = notification.MessageType;
        }
    }

    class AppDbContext : DbContext
    {
        public DbSet<NotificationDB> Notifications { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Также добавить названия в конфиг файл
            modelBuilder.Entity<NotificationDB>()
                .ToTable("notifications");
            modelBuilder.Entity<NotificationDB>()
                .Property(n => n.Id).HasColumnName("id");
            modelBuilder.Entity<NotificationDB>()
                .Property(n => n.Event).HasColumnName("event");
            modelBuilder.Entity<NotificationDB>()
                .Property(n => n.MessageType).HasColumnName("message_type");
            modelBuilder.Entity<NotificationDB>()
                .Property(n => n.UserId).HasColumnName("user_id");
        }

    }
}

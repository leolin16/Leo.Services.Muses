using System;
using Leo.Services.Muses.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace Leo.Services.Muses.Data
{
    public class MusesDbContext : DbContext
    {
        public static readonly ILoggerFactory MyConsoleLoggerFactory
            =  LoggerFactory.Create(builder =>
            {
                builder.AddFilter((category, level) =>
                    category == DbLoggerCategory.Database.Command.Name
                    && level == LogLevel.Error)
                       .AddConsole();
            });
        public DbSet<Singer> Singers { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<Criticism> Criticisms { get; set; }
        public MusesDbContext(DbContextOptions<MusesDbContext> options) : base(options)
        {
            Database.Migrate(); // instead of EnsureCreated(which can only build from non-exist to exist), migrate can build both from non-exist to exist, also from exist v1 to exist v2
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(MyConsoleLoggerFactory)
                            .EnableSensitiveDataLogging(true);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<SingerSong>()
                .HasKey(ss => new { ss.SingerId, ss.SongId });

            modelBuilder.Entity<SingerSong>()
                .HasOne(ss => ss.Singer)
                .WithMany(singer => singer.SingerSongs)
                .HasForeignKey(ss => ss.SingerId);

            modelBuilder.Entity<SingerSong>()
                .HasOne(ss => ss.Song)
                .WithMany(song => song.SingerSongs)
                .HasForeignKey(ss => ss.SongId);
            
            base.OnModelCreating(modelBuilder);
        }
    }
    public class ApplicationContextDbFactory : IDesignTimeDbContextFactory<MusesDbContext>
    {
        MusesDbContext IDesignTimeDbContextFactory<MusesDbContext>.CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MusesDbContext>();
            optionsBuilder.UseSqlite<MusesDbContext>("Data Source=muses.db;");
            return new MusesDbContext(optionsBuilder.Options);
        }
    }
}

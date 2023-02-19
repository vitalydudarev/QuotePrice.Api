using Microsoft.EntityFrameworkCore;
using QuotePrice.Infrastructure.Database.Entities;

namespace QuotePrice.Infrastructure.Database;

public class QuoteDbContext : DbContext
{
    public DbSet<DbQuote> Quotes { get; set; }

    // public string DbPath { get; }

    public QuoteDbContext(DbContextOptions<QuoteDbContext> options) : base(options)
    {
        // var folder = Environment.SpecialFolder.LocalApplicationData;
        // var path = Environment.GetFolderPath(folder);
        // DbPath = System.IO.Path.Join(path, "blogging.db");
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // for postgres
        modelBuilder.Entity<DbQuote>().HasKey(a => a.Id);
        modelBuilder.Entity<DbQuote>().Property(a => a.Pair).IsRequired();
        modelBuilder.Entity<DbQuote>().Property(a => a.Source).IsRequired();
        modelBuilder.Entity<DbQuote>().ToTable("quotes");
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    // protected override void OnConfiguring(DbContextOptionsBuilder options)
    // {
        // options.UseSqlite($"Data Source={DbPath}");
    // }
}
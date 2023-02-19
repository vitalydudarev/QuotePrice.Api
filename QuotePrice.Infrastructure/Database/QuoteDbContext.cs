using Microsoft.EntityFrameworkCore;
using QuotePrice.Infrastructure.Database.Entities;

namespace QuotePrice.Infrastructure.Database;

public class QuoteDbContext : DbContext
{
    public DbSet<DbQuote> Quotes { get; set; }

    public QuoteDbContext(DbContextOptions<QuoteDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DbQuote>().HasKey(a => a.Id);
        modelBuilder.Entity<DbQuote>().Property(a => a.Pair).IsRequired();
        modelBuilder.Entity<DbQuote>().Property(a => a.Source).IsRequired();
        modelBuilder.Entity<DbQuote>().ToTable("quotes");
    }
}
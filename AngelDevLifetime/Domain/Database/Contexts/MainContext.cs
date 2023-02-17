using AngelDevLifetime.Domain.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace AngelDevLifetime.Domain.Database.Contexts;

public sealed class MainContext : DbContext
{
	public DbSet<ComicsModel> Comics { get; init; }
	public DbSet<ComicsProviderModel> ComicsProviders { get; init; }

	public MainContext(DbContextOptions options) : base(options) => Database.EnsureCreated();
}

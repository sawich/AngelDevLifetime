using AngelDevLifetime.Domain.Database.Contexts;
using AngelDevLifetime.Services;
using Microsoft.EntityFrameworkCore;

await Host
	.CreateDefaultBuilder(args)
	.ConfigureServices(services => services
		.AddPooledDbContextFactory<MainContext>(v => v.UseInMemoryDatabase("AngelDevLifetime").EnableSensitiveDataLogging())
		// .AddPooledDbContextFactory<MainContext>(v => v.UseSqlite("Data Source=file::memory:?cache=shared").EnableSensitiveDataLogging())
		.AddHttpClient()
		.AddHostedService<MangaChanComicsObserver>())
	.Build()
	.RunAsync();

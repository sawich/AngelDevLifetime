using AngelDevLifetime.Domain;
using AngelDevLifetime.Domain.Database.Contexts;
using AngelDevLifetime.Domain.Database.Models;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace AngelDevLifetime.Services;

public partial class MangaChanComicsObserver : BackgroundService
{
	public MangaChanComicsObserver(IDbContextFactory<MainContext> factory, ILogger<MangaChanComicsObserver> logger)
	{
		_factory = factory;
		_logger = logger;
	}

	protected override async Task ExecuteAsync(CancellationToken ct)
	{
		var provider = await GetComicsProvider(ct);

		// while (!ct.IsCancellationRequested)
		// {
			var document = new HtmlDocument();
			document.Load("C:\\Users\\sawic\\Downloads\\Манга от AngelDev.html");

			var nodes = document.DocumentNode.SelectNodes("//div[@class='manga_row1']/div/h2/a");
			var values = nodes.Select(v => new { Title = v.GetAttributeValue("title", ""), Url = v.GetAttributeValue("href", "0") }).Select(v => new { v.Url, v.Title, Id = int.Parse(CaptureId().Match(v.Url).Groups[1].Value) });

			await using var context = await _factory.CreateDbContextAsync(ct);

			var missing = values.Where(x => !context.Comics.Any(z => z.ExternalId == x.Id)).Select(v => new ComicsModel
			{
				ExternalId = v.Id,
				Provider = provider
			}).ToArray();

			foreach (var v in missing)
			{
				await context.Comics.AddAsync(v, ct);
				break;
			}

			// await context.Comics.AddRangeAsync(missing, ct);
			await context.SaveChangesAsync(ct);

			// await Task.Delay(3000, ct);
		// }
	}

	private async Task<ComicsProviderModel> GetComicsProvider(CancellationToken ct)
	{
		await using var context = await _factory.CreateDbContextAsync(ct);

		var provider = await context.ComicsProviders.FirstOrDefaultAsync(v => v.Type == ComicsProvider.MangaChan, ct);
		if (provider is not null) return provider;

		provider = new ComicsProviderModel
		{
			Type = ComicsProvider.MangaChan
		};

		await context.ComicsProviders.AddAsync(provider, ct);
		await context.SaveChangesAsync(ct);

		return provider;
	}

	[GeneratedRegex("/([\\d]+)-")]
	private static partial Regex CaptureId();

	private readonly IDbContextFactory<MainContext> _factory;
	private readonly ILogger<MangaChanComicsObserver> _logger;
}

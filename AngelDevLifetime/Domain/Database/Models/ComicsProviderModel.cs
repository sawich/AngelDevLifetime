namespace AngelDevLifetime.Domain.Database.Models;

public class ComicsProviderModel
{
    public int Id { get; init; }

    public ComicsProvider Type { get; init; }

    public virtual ICollection<ComicsModel> Comics { get; set; }

    public static ComicsProviderModel Empty { get; } = new();
}

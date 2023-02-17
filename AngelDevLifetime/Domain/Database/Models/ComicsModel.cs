namespace AngelDevLifetime.Domain.Database.Models;

public class ComicsModel
{
    public int Id { get; init; }
	public int ExternalId { get; init; }

	public virtual ComicsProviderModel Provider { get; set; }// = ComicsProviderModel.Empty;
}

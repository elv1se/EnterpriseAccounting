namespace ConsoleApp.Models;

public partial class Category
{
	public Guid CategoryId { get; set; }

	public string Name { get; set; } = null!;

	public string? Description { get; set; }

	public virtual ICollection<Operation> Operations { get; set; } = [];
}

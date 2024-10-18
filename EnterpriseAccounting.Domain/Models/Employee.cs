namespace EnterpriseAccounting.Domain.Models;

public partial class Employee
{
	public Guid EmployeeId { get; set; }

	public string Surname { get; set; } = null!;

	public string Name { get; set; } = null!;

	public string? Midname { get; set; }

	public string Position { get; set; } = null!;

	public Guid DepartmentId { get; set; }

	public virtual Department Department { get; set; } = null!;
}

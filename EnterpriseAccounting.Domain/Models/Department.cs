namespace EnterpriseAccounting.Domain.Models;

public class Department
{
	public Guid DepartmentId { get; set; }

	public string Name { get; set; } = null!;

	public virtual ICollection<Account> Accounts { get; set; } = [];

	public virtual ICollection<Employee> Employees { get; set; } = [];

	public virtual ICollection<Transaction> Transactions { get; set; } = [];
}

namespace EnterpriseAccounting.Domain.Models;

public class Transaction
{
	public Guid TransactionId { get; set; }

	public string Type { get; set; } = null!;

	public Guid OperationId { get; set; }

	public Guid DepartmentId { get; set; }

	public virtual Department Department { get; set; } = null!;

	public virtual Operation Operation { get; set; } = null!;
}

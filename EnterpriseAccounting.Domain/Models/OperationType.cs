﻿namespace EnterpriseAccounting.Domain.Models;

public class OperationType
{
	public Guid OperationTypeId { get; set; }

	public string Name { get; set; } = null!;

	public virtual ICollection<Operation> Operations { get; set; } = [];
}

using System;
using System.Collections.Generic;

namespace ConsoleApp;

public partial class OperationType
{
    public Guid OperationTypeId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Operation> Operations { get; set; } = new List<Operation>();
}

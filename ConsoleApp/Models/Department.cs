using System;
using System.Collections.Generic;

namespace ConsoleApp;

public partial class Department
{
    public Guid DepartmentId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}

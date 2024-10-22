using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnterpriseAccounting.Domain.Models;

namespace Contracts.Services;

public interface IEmployeeService
{
	Task<IEnumerable<Employee>> GetAllEmployeesAsync(bool trackChanges);
	Task<Employee?> GetEmployeeAsync(Guid id, bool trackChanges);
}

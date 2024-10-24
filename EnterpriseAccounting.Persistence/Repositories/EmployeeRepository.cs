using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.Repositories;
using EnterpriseAccounting.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseAccounting.Persistence.Repositories;

internal class EmployeeRepository(EnterpriseAccountingContext EnterpriseAccountingContext) :
	RepositoryBase<Employee>(EnterpriseAccountingContext), IEmployeeRepository
{
	public async Task<IEnumerable<Employee>> GetAllEmployeesAsync(bool trackChanges = false) =>
		await FindAll(trackChanges)
			.OrderBy(c => c.Name)
			.ToListAsync();

	public IEnumerable<Employee> GetEmployeesTop(int rows) =>
		 [.. FindAll().Include(x => x.Department).Take(rows)];
}

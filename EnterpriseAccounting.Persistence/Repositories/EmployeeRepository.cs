using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.Repositories;
using EnterpriseAccounting.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseAccounting.Persistence.Repositories;

internal class EmployeeRepository(EnterpriseAccountingContext appDbContext) :
	RepositoryBase<Employee>(appDbContext), IEmployeeRepository
{
	public void CreateEmployee(Employee Employee) => Create(Employee);
	public void UpdateEmployee(Employee Employee) => Update(Employee);

	public void DeleteEmployee(Employee Employee) => Delete(Employee);

	public async Task<IEnumerable<Employee>> GetAllEmployeesAsync(bool trackChanges = false) =>
		await FindAll(trackChanges)
			.OrderBy(c => c.Name)
			.ToListAsync();
	public async Task<Employee?> GetEmployeeAsync(Guid employeeId, bool trackChanges = false) =>
		await FindByCondition(c => c.EmployeeId.Equals(employeeId), trackChanges)
			.SingleOrDefaultAsync();

	public async Task<IEnumerable<Employee>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges = false) =>
		await FindByCondition(x => ids.Contains(x.EmployeeId), trackChanges)
			.ToListAsync();

	public IEnumerable<Employee> GetEmployeesTop(int rows) =>
		 [.. FindAll().Take(rows)];
}

using Microsoft.EntityFrameworkCore;
using Contracts.Repositories;
using EnterpriseAccounting.Domain.Models;

namespace EnterpriseAccounting.Persistence.Repositories;

internal class DepartmentRepository(EnterpriseAccountingContext dbContext) : RepositoryBase<Department>(dbContext), IDepartmentRepository
{
	private readonly EnterpriseAccountingContext _dbContext = dbContext;

	public async Task<IEnumerable<Department>> GetAllDepartmentsAsync(bool trackChanges = false) =>
		await FindAll(trackChanges)
			.OrderBy(c => c.Name)
			.ToListAsync();

	public IEnumerable<Department> GetDepartmentsTop(int rows) =>
		 [.. FindAll().Include(x => x.Transactions).Include(x => x.Employees).Include(x => x.Accounts).Take(rows)];
}


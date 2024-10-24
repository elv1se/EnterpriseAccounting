using Microsoft.EntityFrameworkCore;
using Contracts.Repositories;
using EnterpriseAccounting.Domain.Models;

namespace EnterpriseAccounting.Persistence.Repositories;

internal class OperationRepository(EnterpriseAccountingContext dbContext) : RepositoryBase<Operation>(dbContext), IOperationRepository
{
	private readonly EnterpriseAccountingContext _dbContext = dbContext;

	public async Task<IEnumerable<Operation>> GetAllOperationsAsync(bool trackChanges = false) =>
		await FindAll(trackChanges)
			.OrderBy(c => c.Name)
			.ToListAsync();

	public IEnumerable<Operation> GetOperationsTop(int rows) =>
		 [.. FindAll().Take(rows)];
}


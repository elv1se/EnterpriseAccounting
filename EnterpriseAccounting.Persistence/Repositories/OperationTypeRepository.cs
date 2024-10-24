using Microsoft.EntityFrameworkCore;
using Contracts.Repositories;
using EnterpriseAccounting.Domain.Models;

namespace EnterpriseAccounting.Persistence.Repositories;

internal class OperationTypeRepository(EnterpriseAccountingContext dbContext) : RepositoryBase<OperationType>(dbContext), IOperationTypeRepository
{
	private readonly EnterpriseAccountingContext _dbContext = dbContext;

	public async Task<IEnumerable<OperationType>> GetAllOperationTypesAsync(bool trackChanges = false) =>
		await FindAll(trackChanges)
			.OrderBy(c => c.Name)
			.ToListAsync();

	public IEnumerable<OperationType> GetOperationTypesTop(int rows) =>
		 [.. FindAll().Include(x => x.Operations).Take(rows)];
}


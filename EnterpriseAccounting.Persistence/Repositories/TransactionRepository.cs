using Microsoft.EntityFrameworkCore;
using Contracts.Repositories;
using EnterpriseAccounting.Domain.Models;

namespace EnterpriseAccounting.Persistence.Repositories;

internal class TransactionRepository(EnterpriseAccountingContext dbContext) : RepositoryBase<Transaction>(dbContext), ITransactionRepository
{
	private readonly EnterpriseAccountingContext _dbContext = dbContext;

	public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync(bool trackChanges = false) =>
		await FindAll(trackChanges)
			.OrderBy(c => c.Type)
			.ToListAsync();

	public IEnumerable<Transaction> GetTransactionsTop(int rows) =>
		 [.. FindAll().Include(x => x.Operation).Include(x => x.Department).Take(rows)];
}


using Contracts.Repositories;
using EnterpriseAccounting.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseAccounting.Persistence.Repositories;

internal class AccountRepository(EnterpriseAccountingContext dbContext) : 
	RepositoryBase<Account>(dbContext), IAccountRepository
{
	public async Task<IEnumerable<Account>> GetAllAccountsAsync(bool trackChanges = false) =>
		await FindAll(trackChanges)
			.OrderBy(c => c.Number)
			.ToListAsync();

	public IEnumerable<Account> GetAccountsTop(int rows) =>
		 [.. FindAll().Include(x => x.Department).Take(rows)];
}


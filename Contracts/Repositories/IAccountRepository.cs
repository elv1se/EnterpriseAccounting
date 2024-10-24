using System.Linq.Expressions;
using EnterpriseAccounting.Domain.Models;

namespace Contracts.Repositories;

public interface IAccountRepository
{
	Task<IEnumerable<Account>> GetAllAccountsAsync(bool trackChanges);
	IEnumerable<Account> GetAccountsTop(int rows);

	IQueryable<Account> FindByCondition(Expression<Func<Account, bool>> expression, bool trackChanges = false);

}


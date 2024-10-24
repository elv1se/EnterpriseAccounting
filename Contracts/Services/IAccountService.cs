using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EnterpriseAccounting.Domain.Models;

namespace Contracts.Services;

public interface IAccountService
{
	public IEnumerable<Account> GetAccounts();

	public void AddAccounts(string cacheKey);

	public IEnumerable<Account>? GetAccounts(string cacheKey);

	public void AddAccountsByCondition(string cacheKey, Expression<Func<Account, bool>> expression);
}

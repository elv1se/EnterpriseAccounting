using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.Services;
using Contracts;
using EnterpriseAccounting.Domain.Models;
using System.Linq.Expressions;
using Microsoft.Extensions.Caching.Memory;

namespace EnterpriseAccounting.Application.Services;

internal sealed class AccountService(IRepositoryManager rep, IMemoryCache memoryCache) : IAccountService
{
	private readonly IRepositoryManager _rep = rep;
	private readonly IMemoryCache _cache = memoryCache;
	private int _rowsNumber = 20;

	public IEnumerable<Account> GetAccounts()
	{
		return _rep.Accounts.GetAccountsTop(_rowsNumber);
	}

	public void AddAccounts(string cacheKey)
	{
		IEnumerable<Account> Accounts = _rep.Accounts.GetAccountsTop(_rowsNumber);

		_cache.Set(cacheKey, Accounts, new MemoryCacheEntryOptions
		{
			AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(248)
		});
	}

	public void AddAccountsByCondition(string cacheKey, Expression<Func<Account, bool>> expression)
	{
		IEnumerable<Account> Accounts = _rep.Accounts.FindByCondition(expression).Take(_rowsNumber);

		_cache.Set(cacheKey, Accounts, new MemoryCacheEntryOptions
		{
			AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(248)
		});
	}

	public IEnumerable<Account>? GetAccounts(string cacheKey)
	{
		if (!_cache.TryGetValue(cacheKey, out IEnumerable<Account>? Accounts))
		{
			Accounts = _rep.Accounts.GetAccountsTop(_rowsNumber);
			if (Accounts != null)
			{
				_cache.Set(cacheKey, Accounts,
				new MemoryCacheEntryOptions()
					.SetAbsoluteExpiration(TimeSpan.FromSeconds(248)));
			}
		}
		return Accounts;
	}
}

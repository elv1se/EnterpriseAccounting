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

internal sealed class TransactionService(IRepositoryManager rep, IMemoryCache memoryCache) : ITransactionService
{
	private readonly IRepositoryManager _rep = rep;
	private readonly IMemoryCache _cache = memoryCache;
	private int _rowsNumber = 20;

	public IEnumerable<Transaction> GetTransactions()
	{
		return _rep.Transactions.GetTransactionsTop(_rowsNumber);
	}

	public void AddTransactions(string cacheKey)
	{
		IEnumerable<Transaction> Transactions = _rep.Transactions.GetTransactionsTop(_rowsNumber);

		_cache.Set(cacheKey, Transactions, new MemoryCacheEntryOptions
		{
			AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(298)
		});
	}

	public void AddTransactionsByCondition(string cacheKey, Expression<Func<Transaction, bool>> expression)
	{
		IEnumerable<Transaction> Transactions = _rep.Transactions.FindByCondition(expression).Take(_rowsNumber);

		_cache.Set(cacheKey, Transactions, new MemoryCacheEntryOptions
		{
			AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(298)
		});
	}

	public IEnumerable<Transaction>? GetTransactions(string cacheKey)
	{
		if (!_cache.TryGetValue(cacheKey, out IEnumerable<Transaction>? Transactions))
		{
			Transactions = _rep.Transactions.GetTransactionsTop(_rowsNumber);
			if (Transactions != null)
			{
				_cache.Set(cacheKey, Transactions,
				new MemoryCacheEntryOptions()
					.SetAbsoluteExpiration(TimeSpan.FromSeconds(298)));
			}
		}
		return Transactions;
	}
}

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

internal sealed class OperationService(IRepositoryManager rep, IMemoryCache memoryCache) : IOperationService
{
	private readonly IRepositoryManager _rep = rep;
	private readonly IMemoryCache _cache = memoryCache;
	private int _rowsNumber = 20;

	public IEnumerable<Operation> GetOperations()
	{
		return _rep.Operations.GetOperationsTop(_rowsNumber);
	}

	public void AddOperations(string cacheKey)
	{
		IEnumerable<Operation> Operations = _rep.Operations.GetOperationsTop(_rowsNumber);

		_cache.Set(cacheKey, Operations, new MemoryCacheEntryOptions
		{
			AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(298)
		});
	}

	public void AddOperationsByCondition(string cacheKey, Expression<Func<Operation, bool>> expression)
	{
		IEnumerable<Operation> Operations = _rep.Operations.FindByCondition(expression).Take(_rowsNumber);

		_cache.Set(cacheKey, Operations, new MemoryCacheEntryOptions
		{
			AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(298)
		});
	}

	public IEnumerable<Operation>? GetOperations(string cacheKey)
	{
		if (!_cache.TryGetValue(cacheKey, out IEnumerable<Operation>? Operations))
		{
			Operations = _rep.Operations.GetOperationsTop(_rowsNumber);
			if (Operations != null)
			{
				_cache.Set(cacheKey, Operations,
				new MemoryCacheEntryOptions()
					.SetAbsoluteExpiration(TimeSpan.FromSeconds(298)));
			}
		}
		return Operations;
	}
}

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

internal sealed class OperationTypeService(IRepositoryManager rep, IMemoryCache memoryCache) : IOperationTypeService
{
	private readonly IRepositoryManager _rep = rep;
	private readonly IMemoryCache _cache = memoryCache;
	private int _rowsNumber = 20;

	public IEnumerable<OperationType> GetOperationTypes()
	{
		return _rep.OperationTypes.GetOperationTypesTop(_rowsNumber);
	}

	public void AddOperationTypes(string cacheKey)
	{
		IEnumerable<OperationType> OperationTypes = _rep.OperationTypes.GetOperationTypesTop(_rowsNumber);

		_cache.Set(cacheKey, OperationTypes, new MemoryCacheEntryOptions
		{
			AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(248)
		});
	}

	public void AddOperationTypesByCondition(string cacheKey, Expression<Func<OperationType, bool>> expression)
	{
		IEnumerable<OperationType> OperationTypes = _rep.OperationTypes.FindByCondition(expression).Take(_rowsNumber);

		_cache.Set(cacheKey, OperationTypes, new MemoryCacheEntryOptions
		{
			AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(248)
		});
	}

	public IEnumerable<OperationType>? GetOperationTypes(string cacheKey)
	{
		if (!_cache.TryGetValue(cacheKey, out IEnumerable<OperationType>? OperationTypes))
		{
			OperationTypes = _rep.OperationTypes.GetOperationTypesTop(_rowsNumber);
			if (OperationTypes != null)
			{
				_cache.Set(cacheKey, OperationTypes,
				new MemoryCacheEntryOptions()
					.SetAbsoluteExpiration(TimeSpan.FromSeconds(248)));
			}
		}
		return OperationTypes;
	}
}

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

internal sealed class DepartmentService(IRepositoryManager rep, IMemoryCache memoryCache) : IDepartmentService
{
	private readonly IRepositoryManager _rep = rep;
	private readonly IMemoryCache _cache = memoryCache;
	private int _rowsNumber = 20;

	public IEnumerable<Department> GetDepartments()
	{
		return _rep.Departments.GetDepartmentsTop(_rowsNumber);
	}

	public void AddDepartments(string cacheKey)
	{
		IEnumerable<Department> Departments = _rep.Departments.GetDepartmentsTop(_rowsNumber);

		_cache.Set(cacheKey, Departments, new MemoryCacheEntryOptions
		{
			AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(298)
		});
	}

	public void AddDepartmentsByCondition(string cacheKey, Expression<Func<Department, bool>> expression)
	{
		IEnumerable<Department> Departments = _rep.Departments.FindByCondition(expression).Take(_rowsNumber);

		_cache.Set(cacheKey, Departments, new MemoryCacheEntryOptions
		{
			AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(298)
		});
	}

	public IEnumerable<Department>? GetDepartments(string cacheKey)
	{
		if (!_cache.TryGetValue(cacheKey, out IEnumerable<Department>? Departments))
		{
			Departments = _rep.Departments.GetDepartmentsTop(_rowsNumber);
			if (Departments != null)
			{
				_cache.Set(cacheKey, Departments,
				new MemoryCacheEntryOptions()
					.SetAbsoluteExpiration(TimeSpan.FromSeconds(298)));
			}
		}
		return Departments;
	}
}

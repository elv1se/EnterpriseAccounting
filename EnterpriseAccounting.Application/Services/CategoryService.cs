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

internal sealed class CategoryService(IRepositoryManager rep, IMemoryCache memoryCache) : ICategoryService
{
	private readonly IRepositoryManager _rep = rep;
	private readonly IMemoryCache _cache = memoryCache;
	private int _rowsNumber = 20;

	public IEnumerable<Category> GetCategories()
	{
		return _rep.Categories.GetCategoriesTop(_rowsNumber);
	}

	public void AddCategories(string cacheKey)
	{
		IEnumerable<Category> categories = _rep.Categories.GetCategoriesTop(_rowsNumber);

		_cache.Set(cacheKey, categories, new MemoryCacheEntryOptions
		{
			AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(298)
		});
	}

	public void AddCategoriesByCondition(string cacheKey, Expression<Func<Category, bool>> expression)
	{
		IEnumerable<Category> categories = _rep.Categories.FindByCondition(expression).Take(_rowsNumber);

		_cache.Set(cacheKey, categories, new MemoryCacheEntryOptions
		{
			AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(298)
		});
	}

	public IEnumerable<Category>? GetCategories(string cacheKey)
	{
		if (!_cache.TryGetValue(cacheKey, out IEnumerable<Category>? categories))
		{
			categories = _rep.Categories.GetCategoriesTop(_rowsNumber);
			if (categories != null)
			{
				_cache.Set(cacheKey, categories,
				new MemoryCacheEntryOptions()
					.SetAbsoluteExpiration(TimeSpan.FromSeconds(298)));
			}
		}
		return categories;
	}
}

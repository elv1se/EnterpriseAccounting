using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EnterpriseAccounting.Domain.Models;

namespace Contracts.Services;

public interface ICategoryService
{
	public IEnumerable<Category> GetCategories();

	public void AddCategories(string cacheKey);

	public IEnumerable<Category>? GetCategories(string cacheKey);

	public void AddCategoriesByCondition(string cacheKey, Expression<Func<Category, bool>> expression);
}

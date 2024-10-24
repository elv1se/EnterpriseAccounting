using System.Linq.Expressions;
using EnterpriseAccounting.Domain.Models;


namespace Contracts.Repositories;

public interface ICategoryRepository 
{
	Task<IEnumerable<Category>> GetAllCategoriesAsync(bool trackChanges);
	IEnumerable<Category> GetCategoriesTop(int rows);

	IQueryable<Category> FindByCondition(Expression<Func<Category, bool>> expression, bool trackChanges = false);

}


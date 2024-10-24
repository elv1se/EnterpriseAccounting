using Microsoft.EntityFrameworkCore;
using Contracts.Repositories;
using EnterpriseAccounting.Domain.Models;

namespace EnterpriseAccounting.Persistence.Repositories;

internal class CategoryRepository(EnterpriseAccountingContext dbContext) :
	RepositoryBase<Category>(dbContext), ICategoryRepository
{
	private readonly EnterpriseAccountingContext _dbContext = dbContext;

	public async Task<IEnumerable<Category>> GetAllCategoriesAsync(bool trackChanges = false) =>
		await FindAll(trackChanges)
			.OrderBy(c => c.Name)
			.ToListAsync();

	public IEnumerable<Category> GetCategoriesTop(int rows) =>
		 [.. FindAll().Take(rows)];
}


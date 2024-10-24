using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Contracts.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseAccounting.Persistence.Repositories;

internal abstract class RepositoryBase<T>(EnterpriseAccountingContext EnterpriseAccountingContext) : IRepositoryBase<T> where T : class
{
	protected EnterpriseAccountingContext EnterpriseAccountingContext = EnterpriseAccountingContext;

	public IQueryable<T> FindAll(bool trackChanges = false) =>
		!trackChanges ? EnterpriseAccountingContext.Set<T>()
		.AsNoTracking() : EnterpriseAccountingContext.Set<T>();

	public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false) =>
		!trackChanges ? EnterpriseAccountingContext.Set<T>().Where(expression)
		.AsNoTracking() : EnterpriseAccountingContext.Set<T>().Where(expression);

	public void Create(T entity) => EnterpriseAccountingContext.Set<T>().Add(entity);
	public void Update(T entity) => EnterpriseAccountingContext.Set<T>().Update(entity);
	public void Delete(T entity) => EnterpriseAccountingContext.Set<T>().Remove(entity);
}

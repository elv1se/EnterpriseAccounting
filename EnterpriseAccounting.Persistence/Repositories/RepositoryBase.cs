﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Contracts.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseAccounting.Persistence.Repositories;

internal abstract class RepositoryBase<T>(EnterpriseAccountingContext appDbContext) : IRepositoryBase<T> where T : class
{
	protected EnterpriseAccountingContext AppDbContext = appDbContext;

	public IQueryable<T> FindAll(bool trackChanges = false) =>
		!trackChanges ? AppDbContext.Set<T>()
		.AsNoTracking() : AppDbContext.Set<T>();

	public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false) =>
		!trackChanges ? AppDbContext.Set<T>().Where(expression)
		.AsNoTracking() : AppDbContext.Set<T>().Where(expression);

	public void Create(T entity) => AppDbContext.Set<T>().Add(entity);
	public void Update(T entity) => AppDbContext.Set<T>().Update(entity);
	public void Delete(T entity) => AppDbContext.Set<T>().Remove(entity);
}

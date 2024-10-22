using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using Contracts.Repositories;
using EnterpriseAccounting.Persistence.Repositories;

namespace EnterpriseAccounting.Persistence;

public class RepositoryManager : IRepositoryManager
{
	private readonly EnterpriseAccountingContext _dbContext;
	private readonly Lazy<IEmployeeRepository> _empRep;

	public RepositoryManager(EnterpriseAccountingContext dbContext)
	{
		_dbContext = dbContext;
		_empRep = new Lazy<IEmployeeRepository>(() => new EmployeeRepository(_dbContext));
	}

	public IEmployeeRepository Employees => _empRep.Value;

	public async Task SaveAsync() => await _dbContext.SaveChangesAsync();
	public void SaveChanges() => _dbContext.SaveChanges();
}

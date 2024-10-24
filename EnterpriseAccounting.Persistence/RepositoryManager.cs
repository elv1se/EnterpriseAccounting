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
	private readonly Lazy<IAccountRepository> _accRep;
	private readonly Lazy<ICategoryRepository> _catRep;
	private readonly Lazy<IDepartmentRepository> _depRep;
	private readonly Lazy<IOperationRepository> _opRep;
	private readonly Lazy<IOperationTypeRepository> _optypeRep;
	private readonly Lazy<ITransactionRepository> _tranRep;

	public RepositoryManager(EnterpriseAccountingContext dbContext)
	{
		_dbContext = dbContext;
		_empRep = new Lazy<IEmployeeRepository>(() => new EmployeeRepository(_dbContext));
		_accRep = new Lazy<IAccountRepository>(() => new AccountRepository(_dbContext));
		_catRep = new Lazy<ICategoryRepository>(() => new CategoryRepository(_dbContext));
		_depRep = new Lazy<IDepartmentRepository>(() => new DepartmentRepository(_dbContext));
		_opRep = new Lazy<IOperationRepository>(() => new OperationRepository(_dbContext));
		_optypeRep = new Lazy<IOperationTypeRepository>(() => new OperationTypeRepository(_dbContext));
		_tranRep = new Lazy<ITransactionRepository>(() => new TransactionRepository(_dbContext));
	}

	public IEmployeeRepository Employees => _empRep.Value;
	public IAccountRepository Accounts => _accRep.Value;
	public ICategoryRepository Categories => _catRep.Value;
	public IDepartmentRepository Departments => _depRep.Value;
	public IOperationRepository Operations => _opRep.Value;
	public IOperationTypeRepository OperationTypes => _optypeRep.Value;
	public ITransactionRepository Transactions => _tranRep.Value;

	public async Task SaveAsync() => await _dbContext.SaveChangesAsync();
	public void SaveChanges() => _dbContext.SaveChanges();
}

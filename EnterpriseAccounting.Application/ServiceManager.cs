using Contracts;
using Contracts.Repositories;
using Contracts.Services;
using EnterpriseAccounting.Application.Services;
using Microsoft.Extensions.Caching.Memory;

namespace EnterpriseAccounting.Application;

public class ServiceManager(IRepositoryManager repManager, IMemoryCache cache) : IServiceManager
{
	private readonly Lazy<IEmployeeService> _empService = new(() =>
		new EmployeeService(repManager, cache));
	private readonly Lazy<IDepartmentService> _depService = new(() =>
		new DepartmentService(repManager, cache));
	private readonly Lazy<IAccountService> _accService = new(() =>
		new AccountService(repManager, cache));
	private readonly Lazy<ICategoryService> _catService = new(() =>
		new CategoryService(repManager, cache));
	private readonly Lazy<ITransactionService> _tranService = new(() =>
		new TransactionService(repManager, cache));
	private readonly Lazy<IOperationService> _opService = new(() =>
		new OperationService(repManager, cache));
	private readonly Lazy<IOperationTypeService> _optypeService = new(() =>
		new OperationTypeService(repManager, cache));

	public IEmployeeService EmployeeService => _empService.Value;
	public IDepartmentService DepartmentService => _depService.Value;
	public IAccountService AccountService => _accService.Value;
	public ICategoryService CategoryService => _catService.Value;
	public IOperationService OperationService => _opService.Value;
	public IOperationTypeService OperationTypeService => _optypeService.Value;
	public ITransactionService TransactionService => _tranService.Value;
}

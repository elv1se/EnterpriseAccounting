using Contracts.Repositories;
using Contracts.Services;

namespace Contracts;

public interface IServiceManager
{
	IEmployeeService EmployeeService { get; }
	IAccountService AccountService { get; }
	ICategoryService CategoryService { get; }
	IDepartmentService DepartmentService { get; }
	IOperationService OperationService { get; }
	IOperationTypeService OperationTypeService { get; }
	ITransactionService TransactionService { get; }
}

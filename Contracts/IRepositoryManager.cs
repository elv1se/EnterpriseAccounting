using Contracts.Repositories;

namespace Contracts;

public interface IRepositoryManager
{
	IEmployeeRepository Employees { get; }
	IAccountRepository Accounts { get; }
	ICategoryRepository Categories { get; }
	IDepartmentRepository Departments { get; }
	IOperationRepository Operations { get; }
	IOperationTypeRepository OperationTypes { get; }
	ITransactionRepository Transactions { get; }
	Task SaveAsync();
	void SaveChanges();
}

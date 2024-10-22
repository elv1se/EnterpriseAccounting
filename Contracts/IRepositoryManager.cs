using Contracts.Repositories;

namespace Contracts;

public interface IRepositoryManager
{
	IEmployeeRepository Employees { get; }
	Task SaveAsync();
	void SaveChanges();
}

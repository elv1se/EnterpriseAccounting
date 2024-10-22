using Contracts;
using Contracts.Services;
using EnterpriseAccounting.Application.Services;

namespace EnterpriseAccounting.Application;

public class ServiceManager(IRepositoryManager repManager) : IServiceManager
{
	private readonly Lazy<IEmployeeService> _empService = new(() =>
		new EmployeeService(repManager));



	public IEmployeeService EmployeeService => _empService.Value;
}

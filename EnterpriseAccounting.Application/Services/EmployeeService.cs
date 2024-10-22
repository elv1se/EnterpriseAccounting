using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.Services;
using Contracts;
using EnterpriseAccounting.Domain.Models;

namespace EnterpriseAccounting.Application.Services;

internal sealed class EmployeeService(IRepositoryManager rep) : IEmployeeService
{
	private readonly IRepositoryManager _rep = rep;
	public async Task<IEnumerable<Employee>> GetAllEmployeesAsync(bool trackChanges)
	{

		var employees = await _rep.Employees.GetAllEmployeesAsync(trackChanges);
		return employees;

	}

	public async Task<Employee?> GetEmployeeAsync(Guid id, bool trackChanges)
	{

		var employee = await _rep.Employees.GetEmployeeAsync(id, trackChanges);
		return employee;

	}
}

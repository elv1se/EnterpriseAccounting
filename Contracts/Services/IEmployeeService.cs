using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EnterpriseAccounting.Domain.Models;

namespace Contracts.Services;

public interface IEmployeeService
{
	public IEnumerable<Employee> GetEmployees();

	public void AddEmployees(string cacheKey);

	public IEnumerable<Employee>? GetEmployees(string cacheKey);

	public void AddEmployeesByCondition(string cacheKey, Expression<Func<Employee, bool>> expression);
}

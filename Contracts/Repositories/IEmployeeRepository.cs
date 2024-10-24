using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EnterpriseAccounting.Domain.Models;

namespace Contracts.Repositories;

public interface IEmployeeRepository
{
	Task<IEnumerable<Employee>> GetAllEmployeesAsync(bool trackChanges);
	IEnumerable<Employee> GetEmployeesTop(int rows);

	IQueryable<Employee> FindByCondition(Expression<Func<Employee, bool>> expression, bool trackChanges = false);

}

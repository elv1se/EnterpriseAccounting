using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EnterpriseAccounting.Domain.Models;

namespace Contracts.Services;

public interface IDepartmentService
{
	public IEnumerable<Department> GetDepartments();

	public void AddDepartments(string cacheKey);

	public IEnumerable<Department>? GetDepartments(string cacheKey);

	public void AddDepartmentsByCondition(string cacheKey, Expression<Func<Department, bool>> expression);
}

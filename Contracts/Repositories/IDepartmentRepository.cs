using System.Linq.Expressions;
using EnterpriseAccounting.Domain.Models;

namespace Contracts.Repositories;

public interface IDepartmentRepository
{
	Task<IEnumerable<Department>> GetAllDepartmentsAsync(bool trackChanges);
	IEnumerable<Department> GetDepartmentsTop(int rows);

	IQueryable<Department> FindByCondition(Expression<Func<Department, bool>> expression, bool trackChanges = false);

}


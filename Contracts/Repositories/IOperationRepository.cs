using System.Linq.Expressions;
using EnterpriseAccounting.Domain.Models;

namespace Contracts.Repositories;

public interface IOperationRepository
{
	Task<IEnumerable<Operation>> GetAllOperationsAsync(bool trackChanges);
	IEnumerable<Operation> GetOperationsTop(int rows);

	IQueryable<Operation> FindByCondition(Expression<Func<Operation, bool>> expression, bool trackChanges = false);

}


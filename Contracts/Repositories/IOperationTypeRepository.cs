using System.Linq.Expressions;
using EnterpriseAccounting.Domain.Models;

namespace Contracts.Repositories;

public interface IOperationTypeRepository
{
	Task<IEnumerable<OperationType>> GetAllOperationTypesAsync(bool trackChanges);
	IEnumerable<OperationType> GetOperationTypesTop(int rows);

	IQueryable<OperationType> FindByCondition(Expression<Func<OperationType, bool>> expression, bool trackChanges = false);

}


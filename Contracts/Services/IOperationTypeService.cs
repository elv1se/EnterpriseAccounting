using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EnterpriseAccounting.Domain.Models;

namespace Contracts.Services;

public interface IOperationTypeService
{
	public IEnumerable<OperationType> GetOperationTypes();

	public void AddOperationTypes(string cacheKey);

	public IEnumerable<OperationType>? GetOperationTypes(string cacheKey);

	public void AddOperationTypesByCondition(string cacheKey, Expression<Func<OperationType, bool>> expression);
}

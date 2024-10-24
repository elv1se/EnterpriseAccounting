using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EnterpriseAccounting.Domain.Models;

namespace Contracts.Services;

public interface IOperationService
{
	public IEnumerable<Operation> GetOperations();

	public void AddOperations(string cacheKey);

	public IEnumerable<Operation>? GetOperations(string cacheKey);

	public void AddOperationsByCondition(string cacheKey, Expression<Func<Operation, bool>> expression);
}

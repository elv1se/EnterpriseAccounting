using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EnterpriseAccounting.Domain.Models;

namespace Contracts.Services;

public interface ITransactionService
{
	public IEnumerable<Transaction> GetTransactions();

	public void AddTransactions(string cacheKey);

	public IEnumerable<Transaction>? GetTransactions(string cacheKey);

	public void AddTransactionsByCondition(string cacheKey, Expression<Func<Transaction, bool>> expression);
}

using System.Linq.Expressions;
using EnterpriseAccounting.Domain.Models;

namespace Contracts.Repositories;

public interface ITransactionRepository
{
	Task<IEnumerable<Transaction>> GetAllTransactionsAsync(bool trackChanges);
	IEnumerable<Transaction> GetTransactionsTop(int rows);

	IQueryable<Transaction> FindByCondition(Expression<Func<Transaction, bool>> expression, bool trackChanges = false);

}


using ConsoleApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ConsoleApp;

public class Program
{
	static void Print<T>(string sqlText, IEnumerable<T>? items)
	{
		Console.WriteLine(sqlText);
		Console.WriteLine("Записи: ");
		if (!items.IsNullOrEmpty())
		{
			foreach (var item in items!)
			{
				Console.WriteLine(item?.ToString());
			}
		}
		else
		{
			Console.WriteLine("Пусто");
		}
		Console.WriteLine();
		Console.WriteLine("Нажмите любую клавишу");
		Console.ReadKey();
	}

	static void Select(EnterpriseAccountingContext db)
	{
		var queryLINQ1 = from g in db.Categories
						 select new
						 {
							 Название_Категории = g.Name,
							 Описание_Категории = g.Description,
						 };
		string comment = "1. Результат выполнения запроса на выборку всех данных из таблицы, стоящей в схеме базы данных на стороне отношения 'Один'";
		Print(comment, queryLINQ1.Take(10).ToList());

		var queryLINQ2 = from emp in db.Employees
						 where emp.Name == "Евгений"
						 select new
						 {
							 Имя_Работника = emp.Name,
							 Должность_Работника = emp.Position
						 };
		comment = "2. Результат выполнения запроса на выборку данных из таблицы, стоящей в схеме базы данных на стороне отношения 'Один', отфильтрованные по определенному условию";
		Print(comment, queryLINQ2.ToList());

		var queryLINQ3 = db.Operations.
						 Join(db.Categories, m => m.CategoryId, g => g.CategoryId, (m, g) => new { m.CategoryId, m.Amount, g.Name }).
						 GroupBy(m => new { m.CategoryId, m.Name }).
						 Select(gr => new
						 {
							 Название_Категории = gr.Key.Name,
							 Средняя_сумма = gr.Average(m => m.Amount),
						 });
		comment = "3. Результат выполнения запроса на выборку данных из таблицы, стоящей в схеме базы данных на стороне отношения 'Многие', сгрупированных по любому из полей данных с выводом итогового результата";
		Print(comment, queryLINQ3.Take(10).ToList());

		var queryLINQ4 = db.Employees.
						 Join(db.Accounts, e => e.DepartmentId, w => w.DepartmentId, (e, w) => new { e.Name, w.Number}).
						 Select(gr => new
						 {
							 Название_отдела = gr.Name,
							 Номер_счета = gr.Number,
						 });
		comment = "4. Результат выполнения запроса на выборку данных двух полей из двух таблиц, связанных между собой отношением 'один-ко-многим'";
		Print(comment, queryLINQ4.Take(10).ToList());

		var queryLINQ5 = from e in db.Operations
						 join w in db.OperationTypes
						 on e.OperationTypeId equals w.OperationTypeId
						 where e.Amount < 3000
						 select new
						 {
							 Название_операции = e.Name,
							 Тип_операции = w.Name,
							 Сумма_операции = e.Amount
						 };

		comment = "5. Результат выполнения запроса на выборку данных из двух таблиц, связанных между собой отношением 'один-ко-многим' и отфильтрованным по некоторому условию";
		Print(comment, queryLINQ5.ToList());
	}

	static void Insert(EnterpriseAccountingContext db)
	{
		Category category = new Category
		{
			Name = "New category 1",
			Description = "New description"
		};
		db.Categories.Add(category);
		db.SaveChanges();
		string comment = "Выборка категорий после вставки новой категории";
		var queryLINQ1 = from g in db.Categories
						 where g.Name == "New category 1"
						 select new
						 {
							 Нзавание_Категории = g.Name,
							 Описание_Категории = g.Description
						 };

		var queryLINQ = from g in db.OperationTypes
						where g.Name.Length == 5
						select g.OperationTypeId;

		Guid opType = queryLINQ.Take(1).First();

		Print(comment, queryLINQ1.ToList());

		Operation op = new Operation
		{
			Name = "New operation 1",
			Date = new DateOnly(2024, 10, 5),
			Amount = 5000,
			CategoryId = category.CategoryId,
			OperationTypeId = opType,
		};
		db.Operations.Add(op);
		db.SaveChanges();
		comment = "Выборка операций после вставки новой операции";
		var queryLINQ2 = from m in db.Operations
						 where m.Name == "New operation 1"
						 select new
						 {
							 Название_Операции = m.Name,
							 Дата_Операции = m.Date,
							 Сумма_Операции = m.Amount,
							 Нзавание_Категории = m.Category.Name,
						 };
		Print(comment, queryLINQ2.ToList());
	}

	static void Delete(EnterpriseAccountingContext db)
	{
		string genreName = "New category 1";
		var genre = db.Categories.Where(g => g.Name == genreName);

		if (genre != null)
		{
			db.Categories.RemoveRange(genre);
			db.SaveChanges();
		}
		string comment = "Выборка категорий после удаления жанра";
		var queryLINQ1 = from g in db.Categories
						 where g.Name == "New genre 1"
						 select new
						 {
							 Нзавание_Жанра = g.Name,
							 Описание_Жанра = g.Description
						 };
		Print(comment, queryLINQ1.ToList());

		string movieTitle = "New category 1";
		var Operations = db.Operations.Where(m => m.Name == movieTitle);

		if (Operations != null)
		{
			db.Operations.RemoveRange(Operations);
			db.SaveChanges();
		}
		comment = "Выборка операций после удаления категорий";
		var queryLINQ2 = from m in db.Operations
						 where m.Name == "New category 1"
						 select new
						 {
							 Название_Операции = m.Name,
							 Дата_Операции = m.Date,
							 Сумма_Операции = m.Amount,
							 Нзавание_Категории = m.Category.Name,
						 };
		Print(comment, queryLINQ2.ToList());
	}

	static void Update(EnterpriseAccountingContext db)
	{
		int amount = 2300;
		var ops = db.Operations.Where(w => w.Amount <= amount);
		if (!ops.IsNullOrEmpty())
		{
			foreach (var o in ops)
			{
				o.Amount = 2499;
			}
			db.SaveChanges();
		}

		string comment = "Выберка после обновления";
		var queryLINQ1 = from w in db.Operations
						 where w.Amount == 2499
						 select new
						 {
							 Название_операции = w.Name,
							 Дата_операции = w.Date,
							 Сумма_операции = w.Amount
						 };
		Print(comment, queryLINQ1.ToList());
	}

	static void Main()
	{
		var builder = new ConfigurationBuilder();
		builder.SetBasePath(Directory.GetCurrentDirectory());
		builder.AddJsonFile("appsettings.json");
		var config = builder.Build();
		string? connectionString = config.GetConnectionString("DefaultConnection");

		var optionsBuilder = new DbContextOptionsBuilder<EnterpriseAccountingContext>();
		var options = optionsBuilder
			.UseSqlServer(connectionString)
			.Options;

		using (EnterpriseAccountingContext db = new(options))
		{
			Console.WriteLine("Будет выполнена выборка данных (нажмите любую клавишу) ============");
			Console.ReadKey();
			Select(db);

			Console.WriteLine("Будет выполнена вставка данных (нажмите любую клавишу) ============");
			Console.ReadKey();
			Insert(db);

			Console.WriteLine("Будет выполнено удаление данных (нажмите любую клавишу) ============");
			Console.ReadKey();
			Delete(db);

			Console.WriteLine("Будет выполнено обновление данных (нажмите любую клавишу) ============");
			Console.ReadKey();
			Update(db);
		}
	}
}
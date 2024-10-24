using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.HttpOverrides;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Contracts.Repositories;
using Contracts.Services;
using EnterpriseAccounting.WebMiddleware.Extensions;
using EnterpriseAccounting.Domain.Models;
using Contracts;

namespace EnterpriseAccounting.WebMiddleware;

public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		ConfigureServices(builder.Services, builder.Configuration);

		var app = builder.Build();
		if (app.Environment.IsProduction())
			app.UseHsts();

		ConfigureApp(app);

		app.Map("/info", Info);
		app.Map("/accounts", Accounts);
		app.Map("/employees", Employees);
		app.Map("/transactions", Transactions);
		app.Map("/operations", Operations);
		app.Map("/departments", Departments);
		app.Map("/searchform1", SearchForm1);
		app.Map("/searchform2", SearchForm2);

		app.Run(async (context) =>
		{
			IEmployeeService cachedAccountsService = context.RequestServices.GetService<IEmployeeService>();
			cachedAccountsService?.AddEmployees("Accounts20");

			string HtmlString = "<HTML><HEAD><TITLE>Главная</TITLE></HEAD>" +
			"<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
			"<BODY><H1>Главная</H1>";
			HtmlString += "<H2>Данные записаны в кэш сервера</H2>";
			HtmlString += "<BR><A href='/'>Главная</A>";
			HtmlString += "<BR><A href='/departments'>Отделы</A>";
			HtmlString += "<BR><A href='/employees'>Работники</A>";
			HtmlString += "<BR><A href='/transactions'>Транзакции</A>";
			HtmlString += "<BR><A href='/operations'>Операции</A>";
			HtmlString += "<BR><A href='/accounts'>Счета</A>";
			HtmlString += "<BR><A href='/info'>Информация о клиенте</A>";
			HtmlString += "<BR><A href='/searchform1'>searchform1</A>";
			HtmlString += "<BR><A href='/searchform2'>searchform2</A>";
			HtmlString += "</BODY></HTML>";

			await context.Response.WriteAsync(HtmlString);
		});

		app.Run();
	}

	public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
	{
		services.ConfigureCors();

		services.ConfigureSqlContext(configuration);

		services.AddMemoryCache();
		services.AddDistributedMemoryCache();
		services.AddSession();

		services.ConfigureRepositoryManager();
		services.ConfigureServiceManager();
	}

	public static void ConfigureApp(IApplicationBuilder app)
	{
		app.UseHttpsRedirection();

		app.UseForwardedHeaders(new ForwardedHeadersOptions
		{
			ForwardedHeaders = ForwardedHeaders.All
		});

		app.UseCors("CorsPolicy");
		app.UseSession();
		app.UseCookiePolicy();
	}

	private static void Info(IApplicationBuilder app)
	{
		app.Run(async (context) =>
		{
			string strResponse = "<HTML><HEAD><TITLE>Информация</TITLE></HEAD>" +
			"<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
			"<BODY><H1>Информация:</H1>";
			strResponse += "<BR> Сервер: " + context.Request.Host;
			strResponse += "<BR> Путь: " + context.Request.PathBase;
			strResponse += "<BR> Протокол: " + context.Request.Protocol;
			strResponse += "<BR><A href='/'>Главная</A></BODY></HTML>";
			await context.Response.WriteAsync(strResponse);
		});
	}

	private static void Accounts(IApplicationBuilder app)
	{
		app.Run(async context =>
		{
			IAccountService? cachedAccountsService = context.RequestServices.GetService<IServiceManager>()?.AccountService;
			IEnumerable<Account>? Accounts = cachedAccountsService?.GetAccounts("Accounts20");

			string HtmlString = "<HTML><HEAD>" +
				"<TITLE>Счета</TITLE>" +
				"<META http-equiv='Content-Type' content='text/html; charset=utf-8 >'" +
				"</HEAD><BODY><H1>Список счетов</H1>" +
				"<TABLE BORDER=1 cellspacing=0>";
			HtmlString += "<TH>";
			HtmlString += "<TD>Тип счета</TD>";
			HtmlString += "<TD>Номер счета</TD>";
			HtmlString += "<TD>Название отдела</TD>";
			HtmlString += "<TD>Название банка</TD>";
			HtmlString += "</TH>";
			foreach (Account Account in Accounts ?? [])
			{
				HtmlString += "<TR>";
				HtmlString += "<TD>" + Account.AccountId + "</TD>";
				HtmlString += "<TD>" + Account.Type + "</TD>";
				HtmlString += "<TD>" + Account.Number + "</TD>";
				HtmlString += "<TD>" + Account.Department.Name + "</TD>";
				HtmlString += "<TD>" + Account.BankName + "</TD>";
				HtmlString += "</TR>";
			}
			HtmlString += "</table></BODY></HTML>";

			await context.Response.WriteAsync(HtmlString);
		});
	}

	private static void Employees(IApplicationBuilder app)
	{
		app.Run(async context =>
		{
			IEmployeeService? cachedEmployeesService = context.RequestServices.GetService<IServiceManager>()?.EmployeeService;
			IEnumerable<Employee>? employees = cachedEmployeesService?.GetEmployees("Employees20");

			string HtmlString = "<HTML><HEAD>" +
				"<TITLE>Сотрудники</TITLE></HEAD>" +
				"<META http-equiv='Content-Type' content='text/html; charset=utf-8 />'" +
				"<BODY><H1>Список сотрудников</H1>" +
				"<TABLE BORDER=1 cellspacing=0>";
			HtmlString += "<TH>";
			HtmlString += "<TD>Название отдела</TD>";
			HtmlString += "<TD>Фамилия</TD>";
			HtmlString += "<TD>Имя</TD>";
			HtmlString += "<TD>Должность</TD>";
			HtmlString += "</TH>";
			foreach (Employee employee in employees)
			{
				HtmlString += "<TR>";
				HtmlString += "<TD>" + employee.EmployeeId + "</TD>";
				HtmlString += "<TD>" + employee.Department.Name + "</TD>";
				HtmlString += "<TD>" + employee.Surname + "</TD>";
				HtmlString += "<TD>" + employee.Name + "</TD>";
				HtmlString += "<TD>" + employee.Position + "</TD>";
				HtmlString += "</TR>";
			}
			HtmlString += "</table></BODY></HTML>";

			await context.Response.WriteAsync(HtmlString);
		});
	}

	private static void Transactions(IApplicationBuilder app)
	{
		app.Run(async context =>
		{
			ITransactionService? cachedTransactionsService = context.RequestServices.GetService<IServiceManager>()?.TransactionService;
			IEnumerable<Transaction>? Transactions = cachedTransactionsService?.GetTransactions("Transactions20");

			string HtmlString = "<HTML><HEAD>" +
				"<TITLE>Транзакции</TITLE></HEAD>" +
				"<META http-equiv='Content-Type' content='text/html; charset=utf-8 />'" +
				"<BODY><H1>Список транзакций</H1>" +
				"<TABLE BORDER=1 cellspacing=0>";
			HtmlString += "<TH>";
			HtmlString += "<TD>Тип транзакции</TD>";
			HtmlString += "<TD>Название отдела</TD>";
			HtmlString += "<TD>Название операции</TD>";
			HtmlString += "</TH>";
			foreach (Transaction Transaction in Transactions)
			{
				HtmlString += "<TR>";
				HtmlString += "<TD>" + Transaction.TransactionId + "</TD>";
				HtmlString += "<TD>" + Transaction.Type + "</TD>";
				HtmlString += "<TD>" + Transaction.Department.Name + "</TD>";
				HtmlString += "<TD>" + Transaction.Operation.Name + "</TD>";
				HtmlString += "</TR>";
			}
			HtmlString += "</table></BODY></HTML>";

			await context.Response.WriteAsync(HtmlString);
		});
	}

	private static void Operations(IApplicationBuilder app)
	{
		app.Run(async context =>
		{
			IOperationService? cachedOperationsService = context.RequestServices.GetService<IServiceManager>()?.OperationService;
			IEnumerable<Operation>? Operations = cachedOperationsService?.GetOperations("Operations20");

			string HtmlString = "<HTML><HEAD>" +
				"<TITLE>Операции</TITLE></HEAD>" +
				"<META http-equiv='Content-Type' content='text/html; charset=utf-8 />'" +
				"<BODY><H1>Список операций</H1>" +
				"<TABLE BORDER=1 cellspacing=0>";
			HtmlString += "<TH>";
			HtmlString += "<TD>Название операции</TD>";
			HtmlString += "<TD>Название категории</TD>";
			HtmlString += "<TD>Дата</TD>";
			HtmlString += "<TD>Сумма</TD>";
			HtmlString += "<TD>Типы транзакций</TD>";
			HtmlString += "</TH>";
			foreach (Operation Operation in Operations)
			{
				HtmlString += "<TR>";
				HtmlString += "<TD>" + Operation.OperationId + "</TD>";
				HtmlString += "<TD>" + Operation.Name + "</TD>";
				HtmlString += "<TD>" + Operation.Category.Name + "</TD>";
				HtmlString += "<TD>" + Operation.Date + "</TD>";
				HtmlString += "<TD>" + Operation.Amount + "</TD>";
				HtmlString += "<TD>";
				foreach (string empl in Operation.Transactions.Select(x => x.Type))
					HtmlString += empl + "<BR>";
				HtmlString += "</TD>";
				HtmlString += "</TR>";
			}
			HtmlString += "</table></BODY></HTML>";

			await context.Response.WriteAsync(HtmlString);
		});
	}

	private static void Departments(IApplicationBuilder app)
	{
		app.Run(async context =>
		{
			IDepartmentService? cachedDepartmentsService = context.RequestServices.GetService<IServiceManager>()?.DepartmentService;
			IEnumerable<Department>? Departments = cachedDepartmentsService?.GetDepartments("Departments20");

			string HtmlString = "<HTML><HEAD>" +
				"<TITLE>Отделы</TITLE></HEAD>" +
				"<META http-equiv='Content-Type' content='text/html; charset=utf-8 />'" +
				"<BODY><H1>Список отделов</H1>" +
				"<TABLE BORDER=1 cellspacing=0>";
			HtmlString += "<TH>";
			HtmlString += "<TD>Название отдела</TD>";
			HtmlString += "<TD>Сотрудники</TD>";
			HtmlString += "</TH>";
			foreach (Department Department in Departments)
			{
				HtmlString += "<TR>";
				HtmlString += "<TD>" + Department.DepartmentId + "</TD>";
				HtmlString += "<TD>" + Department.Name + "</TD>";
				HtmlString += "<TD>";
				foreach (string empl in Department.Employees.Select(x => x.Surname + " " + x.Name + " (" + x.Position + ")"))
					HtmlString += empl + "<BR>";
				HtmlString += "</TD>";


				HtmlString += "</TR>";
			}
			HtmlString += "</table></BODY></HTML>";

			await context.Response.WriteAsync(HtmlString);
		});
	}

	private static void SearchForm1(IApplicationBuilder app) =>
		app.Run(HandleSearchForm1);

	private static async Task HandleSearchForm1(HttpContext context)
	{
		var userJson = context.Request.Cookies["searchData"];
		var searchData = string.IsNullOrEmpty(userJson) ? new SearchData() : JsonSerializer.Deserialize<SearchData>(userJson);

		ArgumentNullException.ThrowIfNull(searchData);

		if (context.Request.Query.ContainsKey("number"))
		{
			searchData.Number = context.Request.Query["number"];
		}
		if (context.Request.Query.ContainsKey("bank"))
		{
			searchData.BankName = context.Request.Query["bank"];
		}

		IAccountService? cachedAccountsService = context.RequestServices.GetService<IServiceManager>()?.AccountService;

		cachedAccountsService?.AddAccountsByCondition(
			"Account20",
			x => x.BankName == searchData.BankName &&
				  x.Number.Contains(searchData.Number));
		var Accounts = cachedAccountsService?.GetAccounts("Account20");

		context.Response.Cookies.Append("searchData", JsonSerializer.Serialize(searchData), new CookieOptions
		{
			Expires = DateTimeOffset.UtcNow.AddDays(30)
		});

		string tableHtml = "<TABLE BORDER=1 cellspacing=0>";
		tableHtml += "<TH><TD>Тип счета</TD><TD>Номер счета</TD><TD>Название банка</TD></TH>";

		foreach (Account Account in Accounts ?? [])
		{
			tableHtml += "<TR>";
			tableHtml += $"<TD>{Account.AccountId}</TD>";
			tableHtml += $"<TD>{Account.Type}</TD>";
			tableHtml += $"<TD>{Account.Number}</TD>";
			tableHtml += $"<TD>{Account.BankName}</TD>";
			tableHtml += "</TR>";
		}
		tableHtml += "</TABLE>";

		string selectedBank = searchData.BankName ?? string.Empty;

		string formHtml = "<HTML><HEAD><TITLE>Форма поиска 2</TITLE></HEAD>" +
			"<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
			"<BODY>" +
			"<FORM method='get' action='/searchform2'>" +
			"Поиск по номеру счета:<BR><INPUT type='text' name='number' value='" + searchData.Number + "'>" +
			"<BR>Выберите банк:<BR><SELECT name='bank'>" +
			"<OPTION value='Белагропромбанк ОАО'" + (selectedBank == "Белагропромбанк ОАО" ? " selected" : "") + ">Белагропромбанк ОАО</OPTION>" +
			"<OPTION value='АСБ Беларусбанк ОАО'" + (selectedBank == "АСБ Беларусбанк ОАО" ? " selected" : "") + ">АСБ Беларусбанк ОАО</OPTION>" +
			"</SELECT><BR><BR><INPUT type='submit' value='Искать'>" +
			"<INPUT type='button' value='Показать' onclick='alert(\"" + searchData.Number + " " + searchData.BankName + "\");'></FORM>" +
			"<BR><A href='/'>Главная</A>" +
			"<H2>Результаты поиска:</H2>" +
			tableHtml +
			"</BODY></HTML>";

		await context.Response.WriteAsync(formHtml);
	}

	private static void SearchForm2(IApplicationBuilder app) =>
		app.Run(HandleSearchForm2);
	private static async Task HandleSearchForm2(HttpContext context)
	{
		var userJson = context.Session.GetString("searchData");
		var searchData = string.IsNullOrEmpty(userJson) ? new SearchData() : JsonSerializer.Deserialize<SearchData>(userJson);

		ArgumentNullException.ThrowIfNull(searchData);

		if (context.Request.Query.ContainsKey("number"))
		{
			searchData.Number = context.Request.Query["number"];
		}
		if (context.Request.Query.ContainsKey("bank"))
		{
			searchData.BankName = context.Request.Query["bank"];
		}

		IAccountService? cachedAccountsService = context.RequestServices.GetService<IServiceManager>()?.AccountService;

		cachedAccountsService?.AddAccountsByCondition(
			"Account20",
			x => x.BankName == (searchData.BankName) &&
					x.Number.Contains(searchData.Number));
		var Accounts = cachedAccountsService?.GetAccounts("Account20");

		context.Session.SetString("searchData", JsonSerializer.Serialize(searchData));

		string tableHtml = "<TABLE BORDER=1 cellspacing=0>";
		tableHtml += "<TH><TD>Тип счета</TD><TD>Номер счета</TD><TD>Название банка</TD></TH>";

		foreach (Account Account in Accounts ?? [])
		{
			tableHtml += "<TR>";
			tableHtml += $"<TD>{Account.AccountId}</TD>";
			tableHtml += $"<TD>{Account.Type}</TD>";
			tableHtml += $"<TD>{Account.Number}</TD>";
			tableHtml += $"<TD>{Account.BankName}</TD>";
			tableHtml += "</TR>";
		}
		tableHtml += "</TABLE>";

		string selectedBank = searchData.BankName ?? string.Empty;

		string formHtml = "<HTML><HEAD><TITLE>Форма поиска 2</TITLE></HEAD>" +
		"<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
		"<BODY>" +
		"<FORM method='get' action='/searchform2'>" +
		"Поиск по номеру счета:<BR><INPUT type='text' name='number' value='" + searchData.Number + "'>" +
		"<BR>Выберите банк:<BR><SELECT name='bank'>" +
		"<OPTION value='Белагропромбанк ОАО'" + (selectedBank == "Белагропромбанк ОАО" ? " selected" : "") + ">Белагропромбанк ОАО</OPTION>" +
		"<OPTION value='АСБ Беларусбанк ОАО'" + (selectedBank == "АСБ Беларусбанк ОАО" ? " selected" : "") + ">АСБ Беларусбанк ОАО</OPTION>" +
		"</SELECT><BR><BR><INPUT type='submit' value='Искать'>" +
		"<INPUT type='button' value='Показать' onclick='alert(\"" + searchData.Number + " " + searchData.BankName + "\");'></FORM>" +
		"<BR><A href='/'>Главная</A>" +
		"<H2>Результаты поиска:</H2>" +
		tableHtml +
		"</BODY></HTML>";

		await context.Response.WriteAsync(formHtml);
	}
}

public class SearchData
{
	public string Number { get; set; }
	public string BankName { get; set; }
}

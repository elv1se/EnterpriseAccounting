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
		app.Map("/students", Students);
		app.Map("/employees", Employees);
		app.Map("/courses", Courses);
		app.Map("/payments", Payments);
		app.Map("/jobtitles", JobTitles);
		app.Map("/searchform1", SearchForm1);
		app.Map("/searchform2", SearchForm2);

		app.Run(async (context) =>
		{
			IEmployeeService cachedStudentsService = context.RequestServices.GetService<IEmployeeService>();
			cachedStudentsService?.AddEmployees("Employees20");

			string HtmlString = "<HTML><HEAD><TITLE>�������</TITLE></HEAD>" +
			"<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
			"<BODY><H1>�������</H1>";
			HtmlString += "<H2>������ �������� � ��� �������</H2>";
			HtmlString += "<BR><A href='/'>�������</A>";
			HtmlString += "<BR><A href='/students'>���������</A>";
			HtmlString += "<BR><A href='/employees'>���������</A>";
			HtmlString += "<BR><A href='/courses'>�����</A>";
			HtmlString += "<BR><A href='/payments'>�������</A>";
			HtmlString += "<BR><A href='/jobtitles'>���������</A>";
			HtmlString += "<BR><A href='/info'>���������� � �������</A>";
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
			string strResponse = "<HTML><HEAD><TITLE>����������</TITLE></HEAD>" +
			"<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
			"<BODY><H1>����������:</H1>";
			strResponse += "<BR> ������: " + context.Request.Host;
			strResponse += "<BR> ����: " + context.Request.PathBase;
			strResponse += "<BR> ��������: " + context.Request.Protocol;
			strResponse += "<BR><A href='/'>�������</A></BODY></HTML>";
			await context.Response.WriteAsync(strResponse);
		});
	}

	private static void Students(IApplicationBuilder app)
	{
		app.Run(async context =>
		{
			IStudentService? cachedStudentsService = context.RequestServices.GetService<IServiceManager>()?.StudentService;
			IEnumerable<Student>? students = cachedStudentsService?.GetStudents("Students20");

			string HtmlString = "<HTML><HEAD>" +
				"<TITLE>���������</TITLE>" +
				"<META http-equiv='Content-Type' content='text/html; charset=utf-8 >'" +
				"</HEAD><BODY><H1>������ ����������</H1>" +
				"<TABLE BORDER=1 cellspacing=0>";
			HtmlString += "<TH>";
			HtmlString += "<TD>�������</TD>";
			HtmlString += "<TD>���</TD>";
			HtmlString += "<TD>�����</TD>";
			HtmlString += "<TD>���� ��������</TD>";
			HtmlString += "<TD>����� ���������</TD>";
			HtmlString += "<TD>����� ��������</TD>";
			HtmlString += "<TD>�������� ������</TD>";
			HtmlString += "<TD>������</TD>";
			HtmlString += "</TH>";
			foreach (Student student in students)
			{
				HtmlString += "<TR>";
				HtmlString += "<TD>" + student.StudentId + "</TD>";
				HtmlString += "<TD>" + student.Surname + "</TD>";
				HtmlString += "<TD>" + student.Name + "</TD>";
				HtmlString += "<TD>" + student.Address + "</TD>";
				HtmlString += "<TD>" + student.BirthDate + "</TD>";
				HtmlString += "<TD>" + student.PassportNumber + "</TD>";
				HtmlString += "<TD>" + student.Phone + "</TD>";
				HtmlString += "<TD>";
				foreach (string courseName in student.Courses.Select(x => x.Name))
					HtmlString += courseName + "<BR>";
				HtmlString += "</TD>";
				HtmlString += "<TD>";
				foreach (string pay in student.Payments.Select(x => x.Purpose + ": " + x.Amount))
					HtmlString += pay + "<BR>";
				HtmlString += "</TD>";
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
				"<TITLE>����������</TITLE></HEAD>" +
				"<META http-equiv='Content-Type' content='text/html; charset=utf-8 />'" +
				"<BODY><H1>������ �����������</H1>" +
				"<TABLE BORDER=1 cellspacing=0>";
			HtmlString += "<TH>";
			HtmlString += "<TD>���������</TD>";
			HtmlString += "<TD>�������</TD>";
			HtmlString += "<TD>���</TD>";
			HtmlString += "<TD>�����</TD>";
			HtmlString += "<TD>���� ��������</TD>";
			HtmlString += "<TD>����� ���������</TD>";
			HtmlString += "<TD>����� ��������</TD>";
			HtmlString += "<TD>�����������</TD>";
			HtmlString += "<TD>�������� ������</TD>";
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

	private static void Courses(IApplicationBuilder app)
	{
		app.Run(async context =>
		{
			ICourseService? cachedCoursesService = context.RequestServices.GetService<IServiceManager>()?.CourseService;
			IEnumerable<Course>? courses = cachedCoursesService?.GetCourses("Courses20");

			string HtmlString = "<HTML><HEAD>" +
				"<TITLE>�����</TITLE></HEAD>" +
				"<META http-equiv='Content-Type' content='text/html; charset=utf-8 />'" +
				"<BODY><H1>������ ������</H1>" +
				"<TABLE BORDER=1 cellspacing=0>";
			HtmlString += "<TH>";
			HtmlString += "<TD>�������� ������</TD>";
			HtmlString += "<TD>��������� ��������</TD>";
			HtmlString += "<TD>��������� �����</TD>";
			HtmlString += "<TD>����</TD>";
			HtmlString += "<TD>�������������</TD>";
			HtmlString += "<TD>���������</TD>";
			HtmlString += "<TD>�������������</TD>";
			HtmlString += "<TD>���������</TD>";
			HtmlString += "</TH>";
			foreach (Course course in courses)
			{
				HtmlString += "<TR>";
				HtmlString += "<TD>" + course.CourseId + "</TD>";
				HtmlString += "<TD>" + course.Name + "</TD>";
				HtmlString += "<TD>" + course.TuitionFee + "</TD>";
				HtmlString += "<TD>" + course.AvailableSeats + "</TD>";
				HtmlString += "<TD>" + course.Hours + "</TD>";
				HtmlString += "<TD>" + course.Intensity + "</TD>";
				HtmlString += "<TD>" + course.TrainingProgram + "</TD>";
				HtmlString += "<TD>" + course.Employee.Surname + " " + course.Employee.Name + "</TD>";
				HtmlString += "<TD>";
				foreach (string courseName in course.Students.Select(x => x.Surname + " " + x.Name + " " + x.Address))
					HtmlString += courseName + "<BR>";
				HtmlString += "</TD>";
				HtmlString += "</TR>";
			}
			HtmlString += "</table></BODY></HTML>";

			await context.Response.WriteAsync(HtmlString);
		});
	}

	private static void Payments(IApplicationBuilder app)
	{
		app.Run(async context =>
		{
			IPaymentService? cachedPaymentsService = context.RequestServices.GetService<IServiceManager>()?.PaymentService;
			IEnumerable<Payment>? payments = cachedPaymentsService?.GetPayments("Payments20");

			string HtmlString = "<HTML><HEAD>" +
				"<TITLE>�������</TITLE></HEAD>" +
				"<META http-equiv='Content-Type' content='text/html; charset=utf-8 />'" +
				"<BODY><H1>������ ��������</H1>" +
				"<TABLE BORDER=1 cellspacing=0>";
			HtmlString += "<TH>";
			HtmlString += "<TD>�������� �������</TD>";
			HtmlString += "<TD>����</TD>";
			HtmlString += "<TD>�����</TD>";
			HtmlString += "<TD>���������</TD>";
			HtmlString += "</TH>";
			foreach (Payment payment in payments)
			{
				HtmlString += "<TR>";
				HtmlString += "<TD>" + payment.PaymentId + "</TD>";
				HtmlString += "<TD>" + payment.Purpose + "</TD>";
				HtmlString += "<TD>" + payment.Date + "</TD>";
				HtmlString += "<TD>" + payment.Amount + "</TD>";
				HtmlString += "<TD>" + payment.Student.Surname + " " + payment.Student.Name + "</TD>";
				HtmlString += "</TR>";
			}
			HtmlString += "</table></BODY></HTML>";

			await context.Response.WriteAsync(HtmlString);
		});
	}

	private static void JobTitles(IApplicationBuilder app)
	{
		app.Run(async context =>
		{
			IJobTitleService? cachedJobTitlesService = context.RequestServices.GetService<IServiceManager>()?.JobTitleService;
			IEnumerable<JobTitle>? jobTitles = cachedJobTitlesService?.GetJobTitles("JobTitles20");

			string HtmlString = "<HTML><HEAD>" +
				"<TITLE>���������</TITLE></HEAD>" +
				"<META http-equiv='Content-Type' content='text/html; charset=utf-8 />'" +
				"<BODY><H1>������ ����������</H1>" +
				"<TABLE BORDER=1 cellspacing=0>";
			HtmlString += "<TH>";
			HtmlString += "<TD>�������� ���������</TD>";
			HtmlString += "<TD>��������</TD>";
			HtmlString += "<TD>���������</TD>";
			HtmlString += "</TH>";
			foreach (JobTitle jobTitle in jobTitles)
			{
				HtmlString += "<TR>";
				HtmlString += "<TD>" + jobTitle.JobTitleId + "</TD>";
				HtmlString += "<TD>" + jobTitle.Name + "</TD>";
				HtmlString += "<TD>" + jobTitle.Salary + "</TD>";
				HtmlString += "<TD>";
				foreach (string empl in jobTitle.Employees.Select(x => x.Surname + " " + x.Name + " " + x.Address))
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

		if (context.Request.Query.ContainsKey("name"))
		{
			searchData.Name = context.Request.Query["name"];
		}
		if (context.Request.Query.ContainsKey("city"))
		{
			searchData.City = context.Request.Query["city"];
		}

		IStudentService? cachedStudentsService = context.RequestServices.GetService<IServiceManager>()?.StudentService;

		cachedStudentsService?.AddStudentsByCondition(
			"Students20",
			x => x.Address.Contains(searchData.City) &&
				  x.Name.Contains(searchData.Name));
		var students = cachedStudentsService?.GetStudents("Students20");

		context.Response.Cookies.Append("searchData", JsonSerializer.Serialize(searchData), new CookieOptions
		{
			Expires = DateTimeOffset.UtcNow.AddDays(30)
		});

		string tableHtml = "<TABLE BORDER=1 cellspacing=0>";
		tableHtml += "<TH><TD>�������</TD><TD>���</TD><TD>�����</TD><TD>���� ��������</TD><TD>����� ��������</TD><TD>����� ��������</TD></TH>";

		foreach (Student student in students ?? Enumerable.Empty<Student>())
		{
			tableHtml += "<TR>";
			tableHtml += $"<TD>{student.StudentId}</TD>";
			tableHtml += $"<TD>{student.Surname}</TD>";
			tableHtml += $"<TD>{student.Name}</TD>";
			tableHtml += $"<TD>{student.Address}</TD>";
			tableHtml += $"<TD>{student.BirthDate:dd-MM-yyyy}</TD>";
			tableHtml += $"<TD>{student.PassportNumber}</TD>";
			tableHtml += $"<TD>{student.Phone}</TD>";
			tableHtml += "</TR>";
		}
		tableHtml += "</TABLE>";

		string selectedCity = searchData.City ?? string.Empty;

		string formHtml = "<HTML><HEAD><TITLE>����� ������ 2</TITLE></HEAD>" +
			"<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
			"<BODY>" +
			"<FORM method='get' action='/searchform2'>" +
			"����� �� �����:<BR><INPUT type='text' name='name' value='" + searchData.Name + "'>" +
			"<BR>�������� �����:<BR><SELECT name='city'>" +
			"<OPTION value='������'" + (selectedCity == "������" ? " selected" : "") + ">������</OPTION>" +
			"<OPTION value='�����'" + (selectedCity == "�����" ? " selected" : "") + ">�����</OPTION>" +
			"</SELECT><BR><BR><INPUT type='submit' value='������'>" +
			"<INPUT type='button' value='��������' onclick='alert(\"" + searchData.Name + " " + searchData.City + "\");'></FORM>" +
			"<BR><A href='/'>�������</A>" +
			"<H2>���������� ������:</H2>" +
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

		if (context.Request.Query.ContainsKey("name"))
		{
			searchData.Name = context.Request.Query["name"];
		}
		if (context.Request.Query.ContainsKey("city"))
		{
			searchData.City = context.Request.Query["city"];
		}

		IStudentService? cachedStudentsService = context.RequestServices.GetService<IServiceManager>()?.StudentService;

		cachedStudentsService?.AddStudentsByCondition(
			"Students20",
			x => x.Address.Contains(searchData.City) &&
					x.Name.Contains(searchData.Name));
		var students = cachedStudentsService?.GetStudents("Students20");

		context.Session.SetString("searchData", JsonSerializer.Serialize(searchData));

		string tableHtml = "<TABLE BORDER=1 cellspacing=0>";
		tableHtml += "<TH><TD>�������</TD><TD>���</TD><TD>�����</TD><TD>���� ��������</TD><TD>����� ��������</TD><TD>����� ��������</TD></TH>";

		foreach (Student student in students ?? [])
		{
			tableHtml += "<TR>";
			tableHtml += $"<TD>{student.StudentId}</TD>";
			tableHtml += $"<TD>{student.Surname}</TD>";
			tableHtml += $"<TD>{student.Name}</TD>";
			tableHtml += $"<TD>{student.Address}</TD>";
			tableHtml += $"<TD>{student.BirthDate:dd-MM-yyyy}</TD>";
			tableHtml += $"<TD>{student.PassportNumber}</TD>";
			tableHtml += $"<TD>{student.Phone}</TD>";
			tableHtml += "</TR>";
		}
		tableHtml += "</TABLE>";

		string selectedCity = searchData.City ?? string.Empty;

		string formHtml = "<HTML><HEAD><TITLE>����� ������ 2</TITLE></HEAD>" +
		"<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
		"<BODY>" +
		"<FORM method='get' action='/searchform2'>" +
		"����� �� �����:<BR><INPUT type='text' name='name' value='" + searchData.Name + "'>" +
		"<BR>�������� �����:<BR><SELECT name='city'>" +
		"<OPTION value='������'" + (selectedCity == "������" ? " selected" : "") + ">������</OPTION>" +
		"<OPTION value='�����'" + (selectedCity == "�����" ? " selected" : "") + ">�����</OPTION>" +
		"</SELECT><BR><BR><INPUT type='submit' value='������'>" +
		"<INPUT type='button' value='��������' onclick='alert(\"" + searchData.Name + " " + searchData.City + "\");'></FORM>" +
		"<BR><A href='/'>�������</A>" +
		"<H2>���������� ������:</H2>" +
		tableHtml +
		"</BODY></HTML>";

		await context.Response.WriteAsync(formHtml);
	}
}

public class SearchData
{
	public string Name { get; set; }
	public string City { get; set; }
}
}

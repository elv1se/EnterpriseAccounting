using Microsoft.AspNetCore.Mvc;

namespace EnterpriseAccounting.WebMVC.Controllers;

public class EmployeeController : Controller
{
	
	public IActionResult Index()
	{
		return View();
	}
}

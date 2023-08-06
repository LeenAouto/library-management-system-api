using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
	public class ReservationsController : Controller
	{
		private readonly Uri baseAddress = new Uri("https://localhost:7172/api");
		private readonly string _controllerName = "/Reservations";
		private readonly HttpClient _client;

		public ReservationsController()
		{
			_client = new HttpClient();
			_client.BaseAddress = baseAddress;
		}

		[HttpGet]
		public IActionResult Index()
		{
			return View();
		}
	}
}

using Entities.AuthModels;
using Entities.DTO;
using Humanizer;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class UsersController : Controller
    {
		private readonly Uri baseAddress = new Uri("https://localhost:7172/api");
		private readonly string _controllerName = "/UserAuth";
		private readonly HttpClient _client;

		public UsersController()
		{
			_client = new HttpClient();
			_client.BaseAddress = baseAddress;
		}

		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Login(TokenRequestModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            HttpResponseMessage response = _client.PostAsJsonAsync<TokenRequestModel>(_client.BaseAddress + _controllerName + "/login", model).Result;

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            return RedirectToAction("Index", "Home");
        }


		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public IActionResult Signup()
		//{

		//}


	}
}

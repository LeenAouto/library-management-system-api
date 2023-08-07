using Entities;
using Entities.DTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using static System.Reflection.Metadata.BlobBuilder;

namespace MVC.Controllers
{
	public class BooksController : Controller
	{
		private readonly Uri baseAddress = new Uri("https://localhost:7172/api");
		private readonly string _controllerName = "/Books";
		private readonly HttpClient _client;

		public BooksController()
		{
			_client = new HttpClient();
			_client.BaseAddress = baseAddress;
		}


		public IActionResult Index()
		{
			var books = new List<Book>();

			HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + _controllerName).Result;

			if(!response.IsSuccessStatusCode)
			{
				return BadRequest();
			}

            string data = response.Content.ReadAsStringAsync().Result;
            books = JsonConvert.DeserializeObject<List<Book>>(data);

            return View(books);
		}

		public IActionResult Details(int? id)
		{
			if(id == null)
				return BadRequest();

			var book = new Book();

			HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + _controllerName + $"/{id}").Result;

			if (!response.IsSuccessStatusCode)
			{
				return BadRequest();
			}

            string data = response.Content.ReadAsStringAsync().Result;
            book = JsonConvert.DeserializeObject<Book>(data);

            return View(book);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(RecievedBookDTO dto)
		{
			if(!ModelState.IsValid)
				return BadRequest();

			HttpResponseMessage response = _client.PostAsJsonAsync<RecievedBookDTO>(_client.BaseAddress + _controllerName, dto).Result;

			if (!response.IsSuccessStatusCode)
			{
				return BadRequest();
			}

			return RedirectToAction(nameof(Index));
		}


        public IActionResult Edit(int? id)
        {
            if (id == null)
                return BadRequest();

            var book = new Book();

            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + _controllerName + $"/{id}").Result;

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            string data = response.Content.ReadAsStringAsync().Result;
            book = JsonConvert.DeserializeObject<Book>(data);

            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Book book)
        {
            if (!ModelState.IsValid)
				return BadRequest();

            string data = JsonConvert.SerializeObject(book);
			StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = _client.PutAsync(_client.BaseAddress + _controllerName + $"/{book.Id}", content).Result;

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
                return BadRequest();

            var book = new Book();

            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + _controllerName + $"/{id}").Result;

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            string data = response.Content.ReadAsStringAsync().Result;
            book = JsonConvert.DeserializeObject<Book>(data);

            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            HttpResponseMessage response = _client.DeleteAsync(_client.BaseAddress + _controllerName + $"/{id}").Result;

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(Index));
        }

    }
}

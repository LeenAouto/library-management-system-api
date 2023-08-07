using Entities;
using Entities.DTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using static System.Reflection.Metadata.BlobBuilder;

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


		public IActionResult Index()
		{
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + _controllerName).Result;

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            string data = response.Content.ReadAsStringAsync().Result;
            var reservations = JsonConvert.DeserializeObject<List<ReturnedReservationDTO>>(data);

            return View(reservations);
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
                return BadRequest();

            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + _controllerName + $"/{id}").Result;

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            string data = response.Content.ReadAsStringAsync().Result;
            var reservation = JsonConvert.DeserializeObject<ReturnedReservationDTO>(data);

            return View(reservation);
        }

        public IActionResult Create()
        {
            HttpResponseMessage booksResponse = _client.GetAsync(_client.BaseAddress + "/Books").Result;

            if (!booksResponse.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            string booksData = booksResponse.Content.ReadAsStringAsync().Result;
            var books = JsonConvert.DeserializeObject<List<Book>>(booksData);

            HttpResponseMessage UsersrResponse = _client.GetAsync(_client.BaseAddress + "/UserAuth").Result;

            if (!UsersrResponse.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            string usersData = UsersrResponse.Content.ReadAsStringAsync().Result;
            var users = JsonConvert.DeserializeObject<List<AppUser>>(usersData);


            var dto = new RecievedReservationDTO
            {
                Books = books,
                AppUsers = users
            };

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RecievedReservationDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            HttpResponseMessage response = _client.PostAsJsonAsync<RecievedReservationDTO>(_client.BaseAddress + _controllerName, dto).Result;

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

            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + _controllerName + $"/{id}").Result;

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            string data = response.Content.ReadAsStringAsync().Result;
            var reservation = JsonConvert.DeserializeObject<ReturnedReservationDTO>(data);

            //==============================================================================
            HttpResponseMessage booksResponse = _client.GetAsync(_client.BaseAddress + "/Books").Result;

            if (!booksResponse.IsSuccessStatusCode)
                return BadRequest();

            string booksData = booksResponse.Content.ReadAsStringAsync().Result;
            var books = JsonConvert.DeserializeObject<List<Book>>(booksData);
            //==============================================================================
            HttpResponseMessage UsersrResponse = _client.GetAsync(_client.BaseAddress + "/UserAuth").Result;

            if (!UsersrResponse.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            string usersData = UsersrResponse.Content.ReadAsStringAsync().Result;
            var users = JsonConvert.DeserializeObject<List<AppUser>>(usersData);

            if(reservation != null)
            {
                reservation.Books = books;
                reservation.AppUsers = users;
            }
            

            return View(reservation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ReturnedReservationDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var reservation = new Reservation { 
                Id = dto.Id,
                AppUserId = dto.AppUserId,
                BookId = dto.BookId,
                StartDate = dto.StartDate,
                IsReturned = dto.IsReturned
            };

            string data = JsonConvert.SerializeObject(reservation);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = _client.PutAsync(_client.BaseAddress + _controllerName + $"/{reservation.Id}", content).Result;

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

            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + _controllerName + $"/{id}").Result;

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            string data = response.Content.ReadAsStringAsync().Result;
            var reservation = JsonConvert.DeserializeObject<Reservation>(data);

            return View(reservation);
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

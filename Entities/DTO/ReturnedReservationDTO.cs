
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Entities.DTO
{
    public class ReturnedReservationDTO : RecievedReservationDTO
    {
        [Display(Name = "Reservation ID")]
        public int Id { get; set; }

        [Display(Name = "Username")]
        public string? AppUserUserName { get; set; }

        [Display(Name = "Book Title")]
        public string? BookTitle { get; set; }
    }
}

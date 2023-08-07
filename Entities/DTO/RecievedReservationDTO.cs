
using System.ComponentModel.DataAnnotations;

namespace Entities.DTO
{
    public class RecievedReservationDTO
    {
        [Display(Name = "Date")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Is Returned?")]
        public bool IsReturned { get; set; }

        [Display(Name = "User ID")]
        public string AppUserId { get; set; }
        public IList<AppUser>? AppUsers { get; set; }

        [Display(Name = "Book ID")]
        public int BookId { get; set; }
        public IList<Book>? Books { get; set; }
    }
}

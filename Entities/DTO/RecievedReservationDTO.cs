
namespace Entities.DTO
{
    public class RecievedReservationDTO
    {
        public DateTime StartDate { get; set; }
        public bool IsReturned { get; set; }
        public string AppUserId { get; set; }
        public int BookId { get; set; }
    }
}

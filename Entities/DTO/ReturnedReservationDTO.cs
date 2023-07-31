
namespace Entities.DTO
{
    public class ReturnedReservationDTO : RecievedReservationDTO
    {
        public int Id { get; set; }
        public string AppUserUserName { get; set; }
        public string BookTitle { get; set; }
    }
}

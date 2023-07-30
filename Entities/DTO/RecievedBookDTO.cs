
using System.ComponentModel.DataAnnotations;

namespace Entities.DTO
{
    public class RecievedBookDTO
    {
        [MaxLength(250)]
        public string Title { get; set; }

        [MaxLength(250)]
        public string Author { get; set; }

        [MaxLength(250)]
        public string Publisher { get; set; }

        public int? PublishYear { get; set; }

        public bool IsAvailable { get; set; }

    }
}
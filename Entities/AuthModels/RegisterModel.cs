using System.ComponentModel.DataAnnotations;

namespace Entities.AuthModels
{
    public class RegisterModel
    {
        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string Username { get; set; }

        [StringLength(128)]
        public string Email { get; set; }

        public string Password { get; set; }
    }
}

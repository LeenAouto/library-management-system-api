
namespace Entities.AuthModels
{
    public class AuthModel
    {
        public string Message { get; set; }

        //by default is false
        public bool IsAuthenticated { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresOn { get; set; }
    }
}

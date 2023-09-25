namespace CA.Application.DTOs.Identity.Requests
{
    public class TokenRequest
    {
        //[Required]
        public string Email { get; set; }

        //[Required]
        public string Password { get; set; }
    }
}
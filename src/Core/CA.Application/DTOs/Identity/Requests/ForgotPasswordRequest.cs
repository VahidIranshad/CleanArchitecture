namespace CA.Application.DTOs.Identity.Requests
{
    public class ForgotPasswordRequest
    {
        //[Required]
        //[EmailAddress]
        public string Email { get; set; }
    }
}
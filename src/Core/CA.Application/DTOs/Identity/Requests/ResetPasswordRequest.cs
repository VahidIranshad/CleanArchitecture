namespace CA.Application.DTOs.Identity.Requests
{
    public class ResetPasswordRequest
    {
        //[Required]
        //[EmailAddress]
        public string ID { get; set; }
        public string Email { get; set; }
        //[Required]
        public string Password { get; set; }
        //[Required]
        //[Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
        //[Required]
        public string Token { get; set; }
    }
}
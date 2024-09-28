namespace DIGIFNB_BackEnd_API.Services.Partner_Shoppe
{
    public class LoginShoppeRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    // Model chứa mã OTP
    public class OtpRequest
    {
        public string OtpCode { get; set; }
    }
}

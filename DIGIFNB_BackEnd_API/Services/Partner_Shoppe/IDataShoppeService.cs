namespace DIGIFNB_BackEnd_API.Services.Partner_Shoppe
{
    public interface IDataShoppeService
    {
        Task<string> StartLoginAsync(string username, string password);
        Task<string> VerifyOtpAndCrawlDataAsync(string otpCode);
    }
}

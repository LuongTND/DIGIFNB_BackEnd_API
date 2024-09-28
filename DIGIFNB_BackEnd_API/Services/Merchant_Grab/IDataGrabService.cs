using DIGIFNB_BackEnd_API.Models.Grab.Order;

namespace DIGIFNB_BackEnd_API.Services.Merchant_Grab
{
    public interface IDataGrabService
    {
        Task<string> CrawlDataFeedBackGrabAsync(string username, string password);
        Task<string> CrawlDataOrderGrabAsync(string username, string password);
        Task<List<History>> GetOrdersWithoutSavingAsync(string username, string password); // Hàm mới
    }
}

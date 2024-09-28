using DIGIFNB_BackEnd_API.Services.Partner_Shoppe;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace DIGIFNB_BackEnd_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataShoppeController : ControllerBase
    {
        private readonly IDataShoppeService _DataShoppeService;

        public DataShoppeController(IDataShoppeService DataShoppeService)
        {
            _DataShoppeService = DataShoppeService;
        }

        // POST: api/crawl/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginShoppeRequest request)
        {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Username and password are required.");
            }

            // Gọi CrawlService để thực hiện đăng nhập
            var result = await _DataShoppeService.StartLoginAsync(request.Username, request.Password);

            // Trả về kết quả
            return Ok(new { message = "Login initiated. Please provide OTP." });
        }

        // POST: api/crawl/verify-otp
        [HttpPost("verify-otp-and-get-data")]
        public async Task<IActionResult> VerifyOtp([FromBody] OtpRequest request)
        {
            if (string.IsNullOrEmpty(request.OtpCode))
            {
                return BadRequest("OTP code is required.");
            }

            // Gọi CrawlService để xác thực OTP và lấy dữ liệu
            var result = await _DataShoppeService.VerifyOtpAndCrawlDataAsync(request.OtpCode);

            // Trả về kết quả
            return Ok(new { data = result });
        }
    }
}

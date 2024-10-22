using DIGIFNB_BackEnd_API.Data;
using DIGIFNB_BackEnd_API.Models.Grab.Order;
using DIGIFNB_BackEnd_API.Services.Merchant_Grab;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace DIGIFNB_BackEnd_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataGrabController : ControllerBase
    {
        private readonly IDataGrabService _dataGrabService;
        private readonly ApplicationDBContext _context;
        public DataGrabController(IDataGrabService DataGrabService ,ApplicationDBContext context)
        {
            _dataGrabService = DataGrabService;
            _context = context;
        }

        [HttpPost("Data-FeedBack-Test")]
        public async Task<IActionResult> dataData([FromBody] LoginGrabRequest request)
        {
            var data = await _dataGrabService.CrawlDataFeedBackTestGrabAsync(request.Username, request.Password);

            if (data.Contains("Error"))
                return BadRequest(data);

            return Ok(new { Data = data });
        }


        // API để lấy dữ liệu Feedback từ Grab
        [HttpPost("crawl-feedbacks")]
        public async Task<IActionResult> GetFeedbackData([FromBody] LoginGrabRequest loginRequest)
        {
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Username) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest("Invalid login request");
            }

            var result = await _dataGrabService.CrawlDataFeedBackGrabAsync(loginRequest.Username, loginRequest.Password);

            if (result.Contains("Error"))
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        // API để lấy dữ liệu Order từ Grab
        [HttpPost("crawl-orders")]
        public async Task<IActionResult> CrawlOrders([FromBody] LoginGrabRequest credentials)
        {
            if (string.IsNullOrEmpty(credentials.Username) || string.IsNullOrEmpty(credentials.Password))
            {
                return BadRequest("Username và Password không được bỏ trống");
            }

            // Gọi phương thức CrawlDataOrderGrabAsync
            var result = await _dataGrabService.CrawlDataOrderGrabAsync(credentials.Username, credentials.Password);

            return Ok(result);
        }

        // API để lấy dữ liệu order mà không cần lưu vào database
        [HttpGet("crawl-orders-nosave")]
        public async Task<IActionResult> GetOrders([FromQuery] string username, [FromQuery] string password)
        {
            var orders = await _dataGrabService.GetOrdersWithoutSavingAsync(username, password);
            if (orders == null)
            {
                return NotFound("Không tìm thấy dữ liệu orders");
            }

            return Ok(orders);
        }

        // API để lấy tất cả dữ liệu từ bảng Histories
        [HttpGet("get-data-oderhistory")]
        public async Task<ActionResult<IEnumerable<History>>> GetHistories()
        {
            var histories = await _context.Historys.OrderByDescending(h => h.CreatedAt).ToListAsync();
            return Ok(histories);
        }

        // API để lấy chi tiết một record từ bảng Histories bằng ID
        [HttpGet("get-data-oderhistory/{id}")]
        public async Task<ActionResult<History>> GetHistoryById(int id)
        {
            var history = await _context.Historys.FindAsync(id);

            if (history == null)
            {
                return NotFound();
            }

            return Ok(history);
        }

    }
}

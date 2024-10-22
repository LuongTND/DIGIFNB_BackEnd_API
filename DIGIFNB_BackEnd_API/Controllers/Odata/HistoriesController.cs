using DIGIFNB_BackEnd_API.Data;
using DIGIFNB_BackEnd_API.Models.Grab.Order;
using DIGIFNB_BackEnd_API.Services.Merchant_Grab;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;

namespace DIGIFNB_BackEnd_API.Controllers.Odata
{
    public class HistoriesController : ODataController
    {
        private readonly IDataGrabService _dataGrabService;
        private readonly ApplicationDBContext _context;
        public HistoriesController(IDataGrabService DataGrabService, ApplicationDBContext context)
        {
            _dataGrabService = DataGrabService;
            _context = context;
        }
        // API để lấy tất cả dữ liệu từ bảng Histories
        [EnableQuery]
        public async Task<ActionResult> Get(ODataQueryOptions<History> queryOptions)
        {
            var histories = await _context.Historys.ToListAsync();
            return Ok(histories);
        }
    }
}

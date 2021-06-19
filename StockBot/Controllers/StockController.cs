using Microsoft.AspNetCore.Mvc;
using StockBot.Service;
using System;

namespace StockBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockService _stockService;
        public StockController(IStockService stockService)
        {
            _stockService = stockService;
        }
        [HttpGet]
        public ActionResult<FinanceCommon.Models.StockModel> Get(string stock_code )
        {
            try
            {
                if (!string.IsNullOrEmpty(stock_code))
                {
                    var result = _stockService.GetStock(stock_code);
                    return Ok(result);
                }
                else
                {
                    return BadRequest("You must send stock code.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

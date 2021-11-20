using System;
using System.Threading.Tasks;
using dotnet_cloud_run_hello_world.Models;
using dotnet_cloud_run_hello_world.Services;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_cloud_run_hello_world.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BnrInput input)
        {
            if (input?.Currency == null || input.Currency.Length==0) return StatusCode(400, "input invalid");
            var client = new BnrClient();
            try
            {
                var bnrOutput = await client.Work(input.Currency.ToUpper());
                return Ok(bnrOutput);
            }
            catch (MyException ex)
            {
                return StatusCode(400, ex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.ToString());
            }
        }
    }
}
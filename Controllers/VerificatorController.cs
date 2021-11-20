using System;
using System.Threading.Tasks;
using dotnet_cloud_run_hello_world.Models;
using dotnet_cloud_run_hello_world.Services;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_cloud_run_hello_world.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VerificatorController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EbmInput input)
        {
            if (input?.Signature == null) return StatusCode(400, "input invalid");
            var client = new EbmClient();
            try
            {
                var output = await client.Work(input.Signature);
                return Ok(output);
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
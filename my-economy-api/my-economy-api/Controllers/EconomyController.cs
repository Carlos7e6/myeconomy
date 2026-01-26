using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using my_economy_api.Models;
using my_economy_api.Services;

namespace my_economy_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EconomyController : ControllerBase
    {
        private readonly DbProvider dbProvider;
        public EconomyController(DbProvider dbProvider)
        {
            this.dbProvider = dbProvider;
        }

        [HttpGet("GetFixedsCost")]
        public async Task<IActionResult> GetFixedsCost()
        {
            var fixedCost = await dbProvider.GetFixedCost();
            return Ok(fixedCost);
        }

        [HttpGet("GetFixedsCostByMonth")]
        public async Task<IActionResult> GetFixedsCostByMonth()
        {
            float fixedTotalCost = 0;
            var fixedCost = await dbProvider.GetFixedCost();

            return Ok(fixedTotalCost);
        }

        [HttpGet("GetFixedsCostByYear")]
        public async Task<IActionResult> GetFixedsCostByYear()
        {
            float fixedTotalCost = 0;
            var fixedCost = await dbProvider.GetFixedCost();

            return Ok(fixedTotalCost);
        }

        [HttpPost("SaveFixedCost")]

        public async Task<IActionResult> SaveFixedCost(FixedCost fx)
        {
            var fc = await dbProvider.SaveFixedCost(fx);
            if (fc == null) return BadRequest();
            return Ok(fx);
        }


    }
}

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using my_economy_api.Models;
using RepositoryPatern;
using RepositoryPatern.Interfaces;
using RepositoryPatern.Repositories;

namespace my_economy_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FixedCostsController : ControllerBase
    {
        private readonly IRepository<FixedCost> _fixedCostRepository;
        public FixedCostsController(IRepository<FixedCost> fixedCostRepository)
        {
            _fixedCostRepository = fixedCostRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var fixedCost = await _fixedCostRepository.GetAllAsync();

            if (fixedCost == null || !fixedCost.Any())
                return NotFound(); 

            return Ok(fixedCost);
        }

        //[HttpPost]
        //public async Task<IActionResult> Edit(FixedCost model)
        //{
        //    try
        //    {
        //        _fixedCostRepository.Update(model);
        //        var success = await _fixedCostRepository.SaveAsync();

        //        if (!success)
        //            return BadRequest("No se realizaron cambios en la base de datos.");

        //        return Ok(model);
        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        return StatusCode(500, $"Error al guardar en la DB: {ex.InnerException?.Message ?? ex.Message}");
        //    }
        //}

        //[HttpPut]
        //public async Task<IActionResult> Add(FixedCost fixedCostEdited)
        //{
        //    await _fixedCostRepository.AddAsync(fixedCostEdited);
        //    await _fixedCostRepository.SaveAsync();

        //    return Ok(fixedCostEdited);
        //}

    }
}

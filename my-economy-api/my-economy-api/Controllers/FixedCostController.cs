using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using my_economy_api.Models;
using RepositoryPatern;
using RepositoryPatern.Interfaces;

namespace my_economy_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FixedCostController : ControllerBase
    {
        private readonly IRepository<FixedCost> _fixedCostRepository;
        public FixedCostController(IRepository<FixedCost> fixedCostRepository)
        {
            _fixedCostRepository = fixedCostRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var fixedCost = await _fixedCostRepository.GetAllAsync();

            if (fixedCost == null || !fixedCost.Any())
                return NotFound(); 

            return Ok(fixedCost);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] FixedCost entity)
        {
            if(entity.Id > 0) return BadRequest("El Id debe ser 0 para crear un nuevo registro.");

            await _fixedCostRepository.AddAsync(entity);
            return CreatedAtAction(nameof(Post), new { id = entity.Id }, entity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] FixedCost entity)
        {
            // Esta es la validación clave
            if (id != entity.Id)
            {
                return BadRequest("El ID de la URL no coincide con el ID del objeto enviado.");
            }

            if(id == 0 || entity.Id == 0) return BadRequest("El ID no puede ser 0 en ninguno de los casos");
            _fixedCostRepository.Update(entity);
            return null;

            // ... lógica para actualizar ...
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

using Assessment3.API.Models;
using Assessment3.API.Repository;
using Microsoft.AspNetCore.Mvc;
using static Assessment3.API.Enums.RegisterEnums;
using static Dapper.SqlMapper;

namespace Assessment3.API.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class EventController(IEventRepository repository, ILocationRepository locationRepository) : ControllerBase
    {
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] Event entity)
        {
            try
            {
                var result = await repository.CreateAsync(entity);
                if (result > 0)
                {
                    entity.Location.EventId = result;
                    if (await locationRepository.CreateAsync(entity.Location) <= 0)
                    {
                        throw new Exception();
                    }
                }
                return Ok(result > 0);
            }
            catch (Exception)
            {
                return BadRequest(false);
            }
        }
        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] Event entity)
        {
            try
            {
                if (!await repository.UpdateAsync(entity))
                {
                    throw new Exception();
                }
                var result = await locationRepository.UpdateAsync(entity.Location);
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest(false);
            }
        }

        [HttpGet("DeleteById")]
        public async Task<IActionResult> DeleteById(int Id)
        {
            try
            {
                if (await repository.DeleteByIdAsync(Id))
                {
                    throw new Exception();  
                }
                var result = await locationRepository.DeleteByIdAsync(Id);
                return Ok(result);

            }
            catch (Exception)
            {
                return BadRequest(false);
            }
        }

        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await repository.GetAllAsync();
                foreach (var entity in result)
                {
                    entity.Location = await locationRepository.GetById(entity.Id);
                }
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest(new List<Event>());
            }
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int Id)
        {
            try
            {
                var result = await repository.GetById(Id);
                result.Location = await locationRepository.GetById(Id);
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest(new Event());
            }
        }
    }
}

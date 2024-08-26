using Assessment3.API.Models;
using Assessment3.API.Repository;
using Microsoft.AspNetCore.Mvc;
using static Assessment3.API.Enums.RegisterEnums;

namespace Assessment3.API.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class RegisterController(IRegisterRepository repository) : ControllerBase
    {
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] Register entity)
        {
            try
            {
                var result = await repository.CreateAsync(entity);
                return Ok(result > 0);
            }
            catch (Exception e)
            {
                return BadRequest(false);
            }
        }
        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] Register entity)
        {
            try
            {
                var result = await repository.UpdateAsync(entity);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(false);
            }
        }

        [HttpGet("DeleteById")]
        public async Task<IActionResult> DeleteById(int Id)
        {
            try
            {
                var result = await repository.DeleteByIdAsync(Id);
                return Ok(result);
            }
            catch (Exception e)
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
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(new List<Register>());
            }
        }
        [HttpPost("GetAllById")]
        public async Task<IActionResult> GetAll(int Id, RegisterIdType Type)
        {
            try
            {
                var result = await repository.GetAllAsync(Id, Type);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(new List<Register>());
            }
        }
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int Id)
        {
            try
            {
                var result = await repository.GetById(Id);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(new Register());
            }
        }
    }
}

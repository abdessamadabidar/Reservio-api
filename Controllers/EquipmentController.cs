using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reservio.Dto;
using Reservio.Interfaces;
using Reservio.Models;

namespace Reservio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EquipmentController : Controller
    {

        private readonly IEquipmentService _equipmentService;

        public EquipmentController(IEquipmentService equipmentService)
        {
            _equipmentService = equipmentService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<EquipmentDto>))]
        [ProducesResponseType(400)]
        [Authorize(Roles = "USER")]
        public async Task<IActionResult> GetAllEquipments()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var equipments = await _equipmentService.GetAllEquipments();
            return Ok(equipments);
        }


        [HttpPost]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400)]
        [Authorize(Roles = "ADMIN")]
        public IActionResult AddNewEquipment([FromBody] EquipmentDto equipmentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            if (equipmentDto == null)
            {
                return BadRequest("Name is required");
            }

            var result = _equipmentService.AddEquipment(equipmentDto);

            if (!result.Result)
            {
                ModelState.AddModelError("Equipment", "Failed to add equipment");
                return StatusCode(500, ModelState);
            }


            return Ok("Equipment has been added successfully");
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(200, Type = typeof(EquipmentDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [Authorize(Roles = "USER")]
        public async Task<IActionResult> GetEquipmentById(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var equipment = await _equipmentService.GetEquipmentById(id);

            if (equipment == null)
            {
                return NotFound("Equipment not found");
            }

            return Ok(equipment);
        }

    }
}

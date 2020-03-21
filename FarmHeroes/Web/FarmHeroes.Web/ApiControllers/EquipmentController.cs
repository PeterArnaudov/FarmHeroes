namespace FarmHeroes.Web.ApiControllers
{
    using FarmHeroes.Services.Data.Contracts;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    public class EquipmentController : ApiController
    {
        private readonly IEquipmentService equipmentService;

        public EquipmentController(IEquipmentService equipmentService)
        {
            this.equipmentService = equipmentService;
        }

        [HttpGet("EquipAmulet/{id}")]
        public async Task<ActionResult<object>> EquipAmulet(int id)
        {
            object result = await this.equipmentService.EquipAmulet(id);

            return result;
        }
    }
}

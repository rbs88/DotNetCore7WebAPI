using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Model;
using MagicVilla_VillaAPI.Model.Dto;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/VillaAPI")]
   //or [Route("api/[controller]")] //this is automatic call VillaAPI controller
    [ApiController]
    public class VillaAPIController: ControllerBase
    {
        [HttpGet] //Http Type EndPoint not Expect any parameter
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            //    return new List<VillaDTO>
            //    {
            //        new VillaDTO {Id=1, Name="Pool View"},
            //        new VillaDTO {Id=2, Name="Beach View"}
            //    };

            return Ok(VillaStore.VillaList);
        }

        [HttpGet("{id:int}",Name ="GetVilla")] //Http Type EndPoint expect parameter
        //Retrive individual Villa
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(200, Type = typeof(VillaDTO))]
        public ActionResult<VillaDTO> GetVilla(int id) //Get Only One Id
        {
            if (id == 0)
            {
                return BadRequest();
            }
            //Using LINQ Operation
            // return Ok(VillaStore.VillaList.FirstOrDefault(u=>u.Id==id));

            var villa = VillaStore.VillaList.FirstOrDefault(u => u.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            return Ok(villa);   

        }

        [HttpPost]//Creating new Villa
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDTO> CreateVilla([FromBody] VillaDTO villaDTO) 
        {
            //add Custom Validation
            if (VillaStore.VillaList.FirstOrDefault(u => u.Name.ToLower() == villaDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Villa Already Exists");
                return BadRequest(ModelState);
            }
            //Custom Validation
            //



            if (villaDTO == null)
            {
                return BadRequest(villaDTO);
            }
            if (villaDTO.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            villaDTO.Id = VillaStore.VillaList.OrderByDescending(u => u.Id).FirstOrDefault().Id+1;
            VillaStore.VillaList.Add(villaDTO);

            return CreatedAtRoute("GetVilla", new { id = villaDTO.Id}, villaDTO); //Invoke Created a Route
        }    
    }
}

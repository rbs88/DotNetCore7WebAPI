using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Logging;
using MagicVilla_VillaAPI.Model;
using MagicVilla_VillaAPI.Model.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/VillaAPI")]
   //or [Route("api/[controller]")] //this is automatic call VillaAPI controller
    [ApiController]
    public class VillaAPIController: ControllerBase
    {


        //Logger
        //private readonly ILogger<VillaAPIController> _logger;

        //public VillaAPIController(ILogger<VillaAPIController> logger)
        //{
        //    _logger = logger;
        //}
        //Logger upper part

       // private readonly Ilogging _logger;
        public VillaAPIController() 
        {
           // _logger= logger;
        }

        [HttpGet] //Http Type EndPoint not Expect any parameter
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            //    return new List<VillaDTO>
            //    {
            //        new VillaDTO {Id=1, Name="Pool View"},
            //        new VillaDTO {Id=2, Name="Beach View"}
            //    };


            //Set Logging something
           // _logger.Log("Getting all villas", "");
            //Set Logging

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
               // _logger.Log("Get Villa Error with Id " + id, "Error");
                return BadRequest();
            }
            //Using LINQ Operation
            //return Ok(VillaStore.VillaList.FirstOrDefault(u=>u.Id==id));

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
            //Custom Validation UpperCode



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


        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        public IActionResult DeleteVilla(int id) 
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var villa = VillaStore.VillaList.FirstOrDefault(u =>u.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            VillaStore.VillaList.Remove(villa);
            return NoContent();
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("{id:int}", Name = "UpdateVilla")]
        public IActionResult UpdateVilla(int id, [FromBody] VillaDTO villaDTO) 
        { 
            if (villaDTO == null || id != villaDTO.Id) 
            {
                return BadRequest();
            }
            
            var villa = VillaStore.VillaList.FirstOrDefault(u => u.Id == id);
            villa.Name = villaDTO.Name;
            villa.Sqft = villaDTO.Sqft;
            villa.Occupancy= villaDTO.Occupancy;

            return NoContent();
        }

        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            } 
            //Using LINQ Operation
            var villa = VillaStore.VillaList.FirstOrDefault(u => u.Id == id);
            if (villa == null)
            {
                return NotFound();
            }

            patchDTO.ApplyTo(villa, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }  

            return NoContent();
        }
    }
}

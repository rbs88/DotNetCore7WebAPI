using System.ComponentModel.DataAnnotations;

namespace MagicVilla_VillaAPI.Model.Dto
{
    public class VillaDTO
    {
        public int Id { get; set; }
        
        [Required] //Data Annotation  for Name
        [MaxLength(200)] // Data Annotation for Name
        public string Name { get; set; }
    }
}

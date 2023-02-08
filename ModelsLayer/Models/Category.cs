using System.ComponentModel.DataAnnotations;

namespace ModelsLayer.Models
{
    public class Category
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage ="Name Field Must Have A Value.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "This Field is Required.")]
        [Range(1,100)]
        public int DisplayOrder { get; set; }

        public DateTime CreatedDateTime { get; set; } = DateTime.Now;

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLayer.Models
{
    public class Cover
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [Display(Name ="CoverType")]
        public string Type { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLayer.Models
{
    public class Product
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        public string  Description  { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public double ListPrice { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        [Display(Name ="Price 10~50")]
        public double Price { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Price 50~100")]
        public double Price50 { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Price +100")]
        public double Price100 { get; set; }

        [Display(Name = "Image")]
        [ValidateNever]
        [Required]
        public string ImageUrl { get; set; }
        [Display(Name = "Category")]
        public int CategoryID { get; set; }

        [ForeignKey("CategoryID")]
        [ValidateNever]
        public Category Category { get; set; }

        [Display(Name ="CoverType")]
        public int CoverID { get; set; }

        [ForeignKey("CoverID")]
        [ValidateNever]
        public Cover Cover { get; set; }

    }
}

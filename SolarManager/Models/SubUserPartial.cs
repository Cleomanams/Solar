using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarManager.Models
{
    [MetadataType(typeof(SubUserPartial))]
    public partial class SubUser
    {
    }

    public partial class SubUserPartial
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string SubUserID { get; set; }

        [Display(Name="Name")]
        [Required(ErrorMessage = "Name is Required.")]
        public string SubUserName { get; set; }

        //[StringLength(4, MinimumLength = 4)]
        [Display(Name ="PIN")]
        [Required(ErrorMessage = "Pin is required")]
        [RegularExpression("^[0-9]{4,4}$", ErrorMessage = "Pin is 4 character numeric")]
        public string SubUserPin { get; set; }

        public string UserID { get; set; }

        public bool Admin { get; set; }
        public bool? Status { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SolarManager.Models
{
    [MetadataType(typeof(SuperUserEndOfShiftPartial))]
    public partial class SuperUserEndOfShift
    {

    }
    public partial class SuperUserEndOfShiftPartial
    {
        public long id { get; set; }

        [Display(Name = "Super UserID")]

        public string superUserId { get; set; }

        [Display(Name = "Print Number")]

        public Nullable<int> EOSPrintNumber { get; set; }

        [Display(Name = "Last Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}", ApplyFormatInEditMode = true)]

        public Nullable<System.DateTime> lastEOSDate { get; set; }
        public Nullable<int> subUserID { get; set; }
        public string EOSPrint { get; set; }

        public virtual SubUser SubUser { get; set; }
    }
}
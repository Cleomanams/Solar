//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SolarManager.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SubUserTransaction
    {
        public long txRef { get; set; }
        public Nullable<int> subUserId { get; set; }
        public string txProduct { get; set; }
        public Nullable<int> txAmount { get; set; }
        public Nullable<int> txStatus { get; set; }
        public Nullable<System.DateTime> txDate { get; set; }
        public Nullable<int> txEoSNumber { get; set; }
        public string txGUID { get; set; }
        public string txSubcode { get; set; }
    
        public virtual SubUser SubUser { get; set; }
    }
}

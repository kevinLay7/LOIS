//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LOIS.DAL.Lois
{
    using System;
    using System.Collections.Generic;
    
    public partial class RequisitionDocument
    {
        public int Id { get; set; }
        public Nullable<int> RequisitionId { get; set; }
        public byte[] Document { get; set; }
        public bool MarkedDeleted { get; set; }
        public string MarkedDeletedUser { get; set; }
        public Nullable<System.DateTime> MarkedDeletedDate { get; set; }
        public string DocumentHash { get; set; }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LOIS.DAL.Prolis
{
    using System;
    using System.Collections.Generic;
    
    public partial class Order
    {
        public long ID { get; set; }
        public System.DateTime OrderDate { get; set; }
        public bool Active { get; set; }
        public bool InfiniteTimed { get; set; }
        public string TestDays { get; set; }
        public Nullable<long> Agency_ID { get; set; }
        public Nullable<long> OrderingProvider_ID { get; set; }
        public Nullable<long> AttendingProvider_ID { get; set; }
        public Nullable<byte> BillingType_ID { get; set; }
        public Nullable<long> Patient_ID { get; set; }
        public Nullable<byte> EntrySource_ID { get; set; }
        public byte Phleb_Loc { get; set; }
        public string WorkCmnt { get; set; }
        public bool Fasting { get; set; }
        public Nullable<long> Created_By { get; set; }
        public Nullable<System.DateTime> CompleteDate { get; set; }
        public Nullable<System.DateTime> LastEdited_On { get; set; }
        public Nullable<long> Edited_By { get; set; }
    }
}

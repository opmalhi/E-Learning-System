//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace E_Learning_System.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Student
    {
        public int Student_Id { get; set; }
        public Nullable<int> Class_Id { get; set; }
        public string Student_Name { get; set; }
        public Nullable<int> Roll_No { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string Gender { get; set; }
        public Nullable<int> Contact_No { get; set; }
        public string Address { get; set; }
        public Nullable<bool> IsActive { get; set; }
    
        public virtual Class Class { get; set; }
    }
}

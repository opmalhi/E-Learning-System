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
    
    public partial class StudentAttendance
    {
        public int Attendance_Id { get; set; }
        public Nullable<int> Class_Id { get; set; }
        public Nullable<int> Subject_Id { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<int> Roll_No { get; set; }
    
        public virtual Class Class { get; set; }
        public virtual Subject Subject { get; set; }
    }
}

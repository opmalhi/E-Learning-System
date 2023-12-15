using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Learning_System.Models
{
    public class StudentModel
    {
        public int Student_Id { get; set; }
        public int Class_Id { get; set; }
        public string Student_Name { get; set; }
        public int Roll_No { get; set; }
        public DateTime DOB { get; set; }
        public string Gender { get; set; }
        public int Contact_No { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
    }
}
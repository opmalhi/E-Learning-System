using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_Learning_System.Models
{
    public class SAttendance
    {
        public string Class_Id { get; set; }
        public string Subject_Id { get; set; }
        public string Roll_No { get; set; }
        public string TS_Id { get; set; }
        public string IsActive { get; set; }
        public string User_Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
    }
}
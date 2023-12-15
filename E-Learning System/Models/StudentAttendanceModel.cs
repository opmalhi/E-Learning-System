using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_Learning_System.Models
{
    public class StudentAttendanceModel
    {

        public int Attendance_Id { get; set; }
        public int Class_Id { get; set; }
        public int Subject_Id { get; set; }
        public int Roll_No { get; set; }
        public bool Status { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

    }
}
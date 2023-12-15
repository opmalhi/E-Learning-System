using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Learning_System.Models
{
    public class TeacherSubjectModel
    {
        public int TS_ID { get; set; }
        public int Class_Id { get; set; }
        public int Subject_Id { get; set; }
        public int User_Id { get; set; }
        public bool IsActive { get; set; }
    }
}
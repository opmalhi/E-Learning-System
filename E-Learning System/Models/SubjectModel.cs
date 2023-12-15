using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Learning_System.Models
{
    public class SubjectModel
    {
        public int Subject_Id { get; set; }
        public int Class_Id { get; set; }
        public string Subject_Name { get; set; }
        public bool IsActive { get; set; }
    }
}
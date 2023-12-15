using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Learning_System.Models
{
    public class ExamModel
    {
        public int Exam_Id { get; set; }
        public int Class_Id { get; set; }
        public int Subject_Id { get; set; }
        public int Roll_No { get; set; }
        public int TotalMarks { get; set; }
        public int OutOfMarks { get; set; }
        public int TS_ID { get; set; }
    }
}
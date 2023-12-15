using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Learning_System.Models
{
    public class DashboardModel
    {
        public string List { get; set; }
        public int total_users { get; set; }
        public int total_admins { get; set; }
        public int total_students { get; set; }
        public int total_teachers { get; set; }
        public int total_classes { get; set; }
        public int total_subjects { get; set; }
    }
}
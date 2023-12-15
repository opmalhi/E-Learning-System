﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_Learning_System.Models
{
    public class TeacherModel
    {
        public int Teacher_Id { get; set; }

        public string Teacher_Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }

        public string Gender { get; set; }

        public int Contact_No { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public bool IsActive { get; set; }
    }
}
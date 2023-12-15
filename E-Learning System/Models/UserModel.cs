using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_Learning_System.Models
{
    public class UserModel
    {
        public int User_Id { get; set; }
        public string User_Name { get; set; }
        public int Roll_No { get; set; }

        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }
        public string Gender { get; set; }
        public int Contact_No { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public int Class_Id { get; set; }
        public int Role_Id { get; set; }
    }
}
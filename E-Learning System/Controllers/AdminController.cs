using E_Learning_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace E_Learning_System.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        ELearningDBContext db = new ELearningDBContext();
        // GET: Admin
        [Authorize(Roles = "Admin")]
        public ActionResult Index(DashboardModel dashboardModel)
        {
            dashboardModel.total_users = db.Users.Count();
            dashboardModel.total_students = db.Users.Where(x=>x.Role_Id == 1).Count();
            dashboardModel.total_teachers = db.Users.Where(x=>x.Role_Id == 2).Count();
            dashboardModel.total_admins = db.Users.Where(x => x.Role_Id == 3).Count();
            dashboardModel.total_classes = db.Classes.Count();
            dashboardModel.total_subjects = db.Subjects.Count();
            return View(dashboardModel);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AddClass()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult AddClass([Bind(Include ="Class_Name")] Class cl)
        {
            if (!ModelState.IsValid)
            {
                return HttpNotFound();
            }

            bool ifExists = db.Classes.Any(c => c.Class_Name.ToLower() == cl.Class_Name.ToLower());

            if (ifExists)
            {
                ModelState.AddModelError("", "Course already Exists!");
                return View(cl);
            }
            cl.IsActive = true;
            db.Classes.Add(cl);
            db.SaveChanges();
            return RedirectToAction("ViewClass");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult ViewClasses()
        {
            var classes = db.Classes.Select(x => x);
            return View(classes);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult EditClass(int? id)
        {
            if (id == null) return HttpNotFound();

            var clas = db.Classes.Where(x => x.Class_Id == id).FirstOrDefault();

            if (clas == null) return HttpNotFound();

            return View(clas);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult EditClass([Bind(Include ="Class_Id, Class_Name, IsActive")] Class cas)
        {
            if (!ModelState.IsValid) return View(cas);

            db.Entry(cas).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("ViewClasses");
        }

        //[HttpGet]
        //public ActionResult DeleteClass(int? id)
        //{
        //    if (id == null) return HttpNotFound();

        //    var clas = db.Classes.Where(x => x.Class_Id == id).FirstOrDefault();

        //    if (clas == null) return HttpNotFound();

        //    return View(clas);
        //}

        //[HttpPost]
        //public ActionResult DeleteClass(int id)
        //{
        //    var clas = db.Classes.Where(x => x.Class_Id == id).FirstOrDefault();
        //    db.Classes.Remove(clas);
        //    db.SaveChanges();
        //    return RedirectToAction("ViewClass");
        //}

        [Authorize(Roles = "Admin")]
        public ActionResult AddSubject()
        {
            ViewBag.Class_Id = new SelectList(db.Classes.Where(x => x.IsActive == true).Select(x=>x), "Class_Id", "Class_Name");
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult AddSubject(Subject subject)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Class_Id = new SelectList(db.Classes.Where(x => x.IsActive == true).Select(x => x), "Class_Id", "Class_Name");
                return View(subject);
            }

            bool ifExists = db.Subjects.Any(s => s.Subject_Name.ToLower() == subject.Subject_Name.ToLower() && s.Class_Id == subject.Class_Id);

            if (ifExists)
            {
                ViewBag.Class_Id = new SelectList(db.Classes.Where(x => x.IsActive == true).Select(x => x), "Class_Id", "Class_Name");

                ModelState.AddModelError("", "Course already Exists!");
                return View(subject);
            }
            subject.IsActive = true;
            db.Subjects.Add(subject);
            db.SaveChanges();
            return RedirectToAction("ViewSubjects");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult ViewSubjects()
        {
            IEnumerable<Subject> Subject = db.Subjects
                 .Include("Class")
                 .Select(x => x);

            return View(Subject);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult EditSubject(int? id)
        {
            if (id == null) return HttpNotFound();

            var sbj = db.Subjects.Where(x => x.Subject_Id == id).FirstOrDefault();

            ViewBag.Class_Id = new SelectList(db.Classes.Where(x => x.Class_Id == sbj.Class_Id).Select(x => x), "Class_Id", "Class_Name");

            if (sbj == null) return HttpNotFound();

            return View(sbj);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult EditSubject(Subject subject)
        {
            if (!ModelState.IsValid) return View(subject);

            db.Entry(subject).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("ViewSubjects");
        }

        public ActionResult AddUser()
        {
            var roles = from c in db.UserRoles
                        select new
                        {
                            RoleId = c.Role_Id,
                            Role = c.Role
                        };
            SelectList rolesList = new SelectList(roles, "RoleId", "Role");
            ViewBag.Roles = rolesList;

            ViewBag.Class_Id = new SelectList(db.Classes.Where(x => x.IsActive == true).Select(x => x), "Class_Id", "Class_Name");

            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult AddUser(User user, int RoleId)
        {
            if (!ModelState.IsValid)
            {
                var roles = from c in db.UserRoles
                            select new
                            {
                                RoleId = c.Role_Id,
                                Role = c.Role
                            };
                SelectList rolesList = new SelectList(roles, "RoleId", "Role");
                ViewBag.Roles = rolesList;
                ViewBag.Class_Id = new SelectList(db.Classes.Where(x => x.IsActive == true).Select(x => x), "Class_Id", "Class_Name");

                return View(user);
            }

            bool ifExists = db.Users.Any(u => u.Email.ToLower() == user.Email.ToLower());
            if (ifExists)
            {
                var roles = from c in db.UserRoles
                            select new
                            {
                                RoleId = c.Role_Id,
                                Role = c.Role
                            };
                SelectList rolesList = new SelectList(roles, "RoleId", "Role");
                ViewBag.Roles = rolesList;
                ViewBag.Class_Id = new SelectList(db.Classes.Where(x => x.IsActive == true).Select(x => x), "Class_Id", "Class_Name");

                ModelState.AddModelError("", "Email already Exists!");
                return View(user);
            }
            else
            {
                if (RoleId == 1 && (user.Roll_No == null || user.Class_Id == null))
                {
                    var roles = from c in db.UserRoles
                                select new
                                {
                                    RoleId = c.Role_Id,
                                    Role = c.Role
                                };
                    SelectList rolesList = new SelectList(roles, "RoleId", "Role");
                    ViewBag.Roles = rolesList;
                    ViewBag.Class_Id = new SelectList(db.Classes.Where(x => x.IsActive == true).Select(x => x), "Class_Id", "Class_Name");

                    ModelState.AddModelError("", "Please complete all fields");
                    return View(user);
                }
                user.Role_Id = RoleId;
                user.IsActive = true;
                user.Password = HashPassword(user.Password);
                db.Users.Add(user);
                db.SaveChanges();

                //Models.UserRole role = db.UserRoles.FirstOrDefault(r => r.Role_Id == RoleId);

                //UserRoleMapping userRoleMapping = new UserRoleMapping
                //{
                //    User = user,
                //    UserRole = role
                //};

                //db.UserRoleMappings.Add(userRoleMapping);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult ViewTeachers()
        {
            IEnumerable<User> teachers = db.Users
                 .Where(x => x.Role_Id == 2)
                 .Select(x => x);
            return View(teachers);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult TeacherDetails(int? id)
        {
            if (id == null) return HttpNotFound();

            var teacher = db.Users.Where(x => x.User_Id == id).FirstOrDefault();

            List<SelectListItem> items = new List<SelectListItem>();

            if (teacher.Gender == "Male")
            {
                items.Add(new SelectListItem { Text = teacher.Gender, Value = teacher.Gender });
                items.Add(new SelectListItem { Text = "Female", Value = "Female" });
            }
            else {
                items.Add(new SelectListItem { Text = "Male", Value = "Male" });
                items.Add(new SelectListItem { Text = teacher.Gender, Value = teacher.Gender });
            }
            ViewBag.Gender = items;

            return View(teacher);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult EditTeacher(int? id)
        {
            if (id == null) return HttpNotFound();

            var teacher = db.Users.Where(x => x.User_Id == id).FirstOrDefault();

            List<SelectListItem> items = new List<SelectListItem>();

            if (teacher.Gender == "Male")
            {
                items.Add(new SelectListItem { Text = teacher.Gender, Value = teacher.Gender });
                items.Add(new SelectListItem { Text = "Female", Value = "Female" });
            }
            else {
                items.Add(new SelectListItem { Text = "Male", Value = "Male" });
                items.Add(new SelectListItem { Text = teacher.Gender, Value = teacher.Gender });
            }
            ViewBag.Gender = items;

            return View(teacher);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult EditTeacher(User teacher)
        {
            if (!ModelState.IsValid) return View(teacher);

            db.Entry(teacher).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("ViewTeachers");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult TeacherSubject()
        {
            ViewBag.Class_Id = new SelectList(db.Classes.Where(x => x.IsActive == true).Select(x => x), "Class_Id", "Class_Name");
            ViewBag.Subject_Id = new SelectList(db.Subjects.Where(x => x.IsActive == true).Select(x => x), "Subject_Id", "Subject_Name");

            ViewBag.Teacher_Id = new SelectList(db.Users
                 .Where(x => x.Role_Id == 2 && x.IsActive == true)
                 .Select(x => x),"User_Id", "User_Name");

            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult TeacherSubject(TeacherSubject teacherSubject )
        {
            if (!ModelState.IsValid)
            {
                return View(teacherSubject);
            }

            bool ifExists = db.TeacherSubjects.Any(t => t.User_Id == teacherSubject.User_Id && t.Subject_Id == teacherSubject.Subject_Id && t.Class_Id == teacherSubject.Class_Id || t.Subject_Id == teacherSubject.Subject_Id && t.Class_Id == teacherSubject.Class_Id);
            
            if (ifExists)
            {
                ModelState.AddModelError("", "Teacher Already Assign!");
                return View(teacherSubject);
            }
            teacherSubject.IsActive = true;
            db.TeacherSubjects.Add(teacherSubject);
            db.SaveChanges();
            return RedirectToAction("ViewTeacherSubject");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult ViewTeacherSubject()
        {
            IEnumerable<TeacherSubject> TeacherSubject = db.TeacherSubjects
                 .Include("Class")
                 .Include("Subject")
                 .Include("User")
                 .Where(x=>x.User.Role_Id == 2)
                 .Select(x => x);

            return View(TeacherSubject);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult TeacherSubjectDeatils(int? id)
        {
            if (id == null) return HttpNotFound();

            var ts = db.TeacherSubjects.Where(x => x.TS_ID == id).FirstOrDefault();

            ViewBag.Class_Id = new SelectList(db.Classes.Where(x => x.Class_Id == ts.Class_Id).Select(x => x), "Class_Id", "Class_Name");
            ViewBag.Subject_Id = new SelectList(db.Subjects.Where(x => x.Subject_Id == ts.Subject_Id).Select(x => x), "Subject_Id", "Subject_Name");

            ViewBag.Teacher_Id = db.Users
                 .Include("UserRole")
                 .Where(x => x.UserRole.Role == "Teacher" && x.IsActive == true)
                 .Select(x => x);

            if (ts == null) return HttpNotFound();

            return View(ts);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult EditTeacherSubject(int? id)
        {
            if (id == null) return HttpNotFound();

            var ts = db.TeacherSubjects.Where(x => x.TS_ID == id).FirstOrDefault();

            ViewBag.Class_Id = new SelectList(db.Classes.Where(x => x.Class_Id == ts.Class_Id), "Class_Id", "Class_Name");
            ViewBag.Subject_Id = new SelectList(db.Subjects.Where(x => x.Subject_Id == ts.Subject_Id), "Subject_Id", "Subject_Name");

            ViewBag.Teacher_Id = db.Users
                 .Include("UserRole")
                 .Where(x => x.UserRole.Role == "Teacher" && x.IsActive == true)
                 .Select(x => x);

            if (ts == null) return HttpNotFound();

            return View(ts);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult EditTeacherSubject(TeacherSubject teacherSubject)
        {
            if (!ModelState.IsValid) return View(teacherSubject);

            bool ifExists = db.TeacherSubjects.Any(t => t.User_Id == teacherSubject.User_Id && t.Subject_Id == teacherSubject.Subject_Id && t.Class_Id == teacherSubject.Class_Id || t.Subject_Id == teacherSubject.Subject_Id && t.Class_Id == teacherSubject.Class_Id);

            if (ifExists)
            {
                ModelState.AddModelError("", "Teacher Already Assign!");
                return View(teacherSubject);
            }

            db.Entry(teacherSubject).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("ViewTeacherSubjects");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult ViewStudents()
        {
            IEnumerable<User> Student = db.Users
                 .Include("UserRole")
                 .Where(x => x.UserRole.Role == "Student")
                 .Select(x => x);

            return View(Student);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult StudentDetails(int? id)
        {

            if (id == null) return HttpNotFound();

            var student = db.Users.Where(x => x.User_Id == id).FirstOrDefault();


            List<SelectListItem> items = new List<SelectListItem>();

            if (student.Gender == "Male")
            {
                items.Add(new SelectListItem { Text = student.Gender, Value = student.Gender });
                items.Add(new SelectListItem { Text = "Female", Value = "Female" });
            }
            else
            {
                items.Add(new SelectListItem { Text = "Male", Value = "Male" });
                items.Add(new SelectListItem { Text = student.Gender, Value = student.Gender });
            }
            ViewBag.Gender = items;
            ViewBag.Class_Id = new SelectList(db.Classes.Where(x => x.Class_Id == student.Class_Id).Select(x => x), "Class_Id", "Class_Name");

            return View(student);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult EditStudent(int? id)
        {

            if (id == null) return HttpNotFound();

            var student = db.Users.Where(x => x.User_Id == id).FirstOrDefault();


            List<SelectListItem> items = new List<SelectListItem>();

            if (student.Gender == "Male")
            {
                items.Add(new SelectListItem { Text = student.Gender, Value = student.Gender });
                items.Add(new SelectListItem { Text = "Female", Value = "Female" });
            }
            else
            {
                items.Add(new SelectListItem { Text = "Male", Value = "Male" });
                items.Add(new SelectListItem { Text = student.Gender, Value = student.Gender });
            }
            ViewBag.Gender = items;
            ViewBag.Class_Id = new SelectList(db.Classes.Where(x => x.Class_Id == student.Class_Id).Select(x => x), "Class_Id", "Class_Name");

            return View(student);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult EditStudent(User student)
        {
            if (!ModelState.IsValid) return View(student);

            db.Entry(student).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("ViewStudents");
        }

        [Authorize(Roles = "Teacher")]
        public ActionResult AddMarks()
        {
            ViewBag.Class_Id = new SelectList(db.Classes.Where(x => x.IsActive == true).Select(x => x), "Class_Id", "Class_Name");
            ViewBag.Subject_Id = new SelectList(db.Subjects.Where(x => x.IsActive == true).Select(x => x), "Subject_Id", "Subject_Name");
            return View();
        }

        [Authorize(Roles = "Teacher")]
        [HttpPost]
        public ActionResult AddMarks(Exam exam)
        {
            string userEmail = User.Identity.Name;
            var id = db.Users.Where(x => x.Email == userEmail).Select(x => x.User_Id).Single();
            var ts = db.TeacherSubjects.Where(x => x.User_Id == id).Select(x => x.TS_ID).Single();
            if (!ModelState.IsValid)
            {

                ViewBag.Class_Id = new SelectList(db.Classes.Where(x => x.IsActive == true).Select(x => x), "Class_Id", "Class_Name");
                ViewBag.Subject_Id = new SelectList(db.Subjects.Where(x => x.IsActive == true).Select(x => x), "Subject_Id", "Subject_Name");

                return View(exam);
            }

            bool ifExists = db.Exams.Any(s => s.Roll_No == exam.Roll_No && s.Class_Id == exam.Class_Id && s.Subject_Id == exam.Subject_Id);

            if (ifExists)
            {
                ViewBag.Class_Id = new SelectList(db.Classes.Where(x => x.IsActive == true).Select(x => x), "Class_Id", "Class_Name");
                ViewBag.Subject_Id = new SelectList(db.Subjects.Where(x => x.IsActive == true).Select(x => x), "Subject_Id", "Subject_Name");

                ModelState.AddModelError("", "Marks already Exists!");
                return View(exam);
            }
            exam.TS_ID = ts;
            db.Exams.Add(exam);
            db.SaveChanges();
            return RedirectToAction("ViewMark");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult ViewMarks()
        {
            IEnumerable<Exam> Exam = db.Exams
                .Include("Class")
                .Include("Subject")
                .Select(x => x);

            return View(Exam);
        }

        [Authorize(Roles = "Teacher")]
        public ActionResult ViewMark()
        {
            string userEmail = User.Identity.Name;
            var id = db.Users.Where(x => x.Email == userEmail).Select(x => x.User_Id).Single();
            var ts = db.TeacherSubjects.Where(x => x.User_Id == id).Select(x => x.TS_ID).Single();
            IEnumerable<Exam> Exam = db.Exams
                .Include("Class")
                .Include("Subject")
                .Include("TeacherSubject")
                .Where( x => x.TS_ID == ts)
                .Select(x => x);

            return View(Exam);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult EditMarks(int? id)
        {
            if (id == null) return HttpNotFound();

            var exam = db.Exams.Where(x => x.Exam_Id == id).FirstOrDefault();

            ViewBag.Class_Id = new SelectList(db.Classes.Where(x => x.Class_Id == exam.Class_Id).Select(x => x), "Class_Id", "Class_Name");
            ViewBag.Subject_Id = new SelectList(db.Subjects.Where(x => x.Subject_Id == exam.Subject_Id).Select(x => x), "Subject_Id", "Subject_Name");

            if (exam == null) return HttpNotFound();

            return View(exam);

        }

        [Authorize(Roles = "Admin")]
        public ActionResult ViewMarkDetails(int? id)
        {
            if (id == null) return HttpNotFound();

            var exam = db.Exams.Where(x => x.Exam_Id == id).FirstOrDefault();

            ViewBag.Class_Id = new SelectList(db.Classes.Where(x => x.Class_Id == exam.Class_Id).Select(x => x), "Class_Id", "Class_Name");
            ViewBag.Subject_Id = new SelectList(db.Subjects.Where(x => x.Subject_Id == exam.Subject_Id).Select(x => x), "Subject_Id", "Subject_Name");

            if (exam == null) return HttpNotFound();

            return View(exam);

        }

        [Authorize(Roles = "Teacher")]
        [HttpPost]
        public ActionResult EditMarks(Exam exam)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Class_Id = new SelectList(db.Classes.Where(x => x.Class_Id == exam.Class_Id).Select(x => x), "Class_Id", "Class_Name");
                ViewBag.Subject_Id = new SelectList(db.Subjects.Where(x => x.Subject_Id == exam.Subject_Id).Select(x => x), "Subject_Id", "Subject_Name");

                return View(exam);
            }
            
            bool roll = db.Users.Any(s => s.Roll_No == exam.Roll_No);

            if (!roll)
            {
                ViewBag.Class_Id = new SelectList(db.Classes.Where(x => x.Class_Id == exam.Class_Id).Select(x => x), "Class_Id", "Class_Name");
                ViewBag.Subject_Id = new SelectList(db.Subjects.Where(x => x.Subject_Id == exam.Subject_Id).Select(x => x), "Subject_Id", "Subject_Name");

                ModelState.AddModelError("", "Student Not Exist!");
                return View(exam);
            }
            db.Entry(exam).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("ViewMarks");
            
        }

        private string HashPassword(string password)
        {
            byte[] encData_byte = new byte[password.Length];
            encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
            return Convert.ToBase64String(encData_byte);
        }


        [Authorize(Roles = "Teacher")]
        public ActionResult AddAttendance(int? id)
        {
            
            var classId = db.TeacherSubjects.Where(x => x.TS_ID == id).Select(x => x.Class_Id).SingleOrDefault();
            var sbjId = db.TeacherSubjects.Where(x => x.TS_ID == id).Select(x => x.Subject_Id).SingleOrDefault();

            var student = db.Users.Where(x => x.Class_Id == classId && x.Role_Id == 1).Select(x => x);

            return View(student);
        }

        //[Authorize(Roles = "Teacher")]
        //public JsonResult AddAttendance()
        //{
        //    int id = Convert.ToInt32(Request.QueryString["TS_ID"]);

        //    var classId = db.TeacherSubjects.Where(x => x.TS_ID == id).Select(x => x.Class_Id).SingleOrDefault();
        //    var sbjId = db.TeacherSubjects.Where(x => x.TS_ID == id).Select(x => x.Subject_Id).SingleOrDefault();

        //    var student = db.Users.Where(x => x.Class_Id == classId && x.Role_Id == 1).Select(x => x);
        //    return Json(student, JsonRequestBehavior.AllowGet);
        //}



        [Authorize(Roles = "Teacher")]
        [HttpPost]
        public ActionResult AddAttendance(List<SAttendance> attendance)
        {
            try
            {
                List<StudentAttendance> students = new List<StudentAttendance>();

                var classlist = db.TeacherSubjects.ToList();
                var sbjlist = db.TeacherSubjects.ToList();

                foreach (var data in attendance)
                {
                    StudentAttendance student = new StudentAttendance();
                    student.Class_Id = classlist.Where(x => x.TS_ID == Convert.ToInt32(data.TS_Id)).FirstOrDefault().Class_Id;
                    student.Subject_Id = sbjlist.Where(x => x.TS_ID == Convert.ToInt32(data.TS_Id)).FirstOrDefault().Subject_Id;
                    student.Roll_No = Convert.ToInt32(data.Roll_No);
                    student.Status = data.IsActive == "1" ? true: false;
                    student.Date = data.Date;

                    students.Add(student);
                }

                db.StudentAttendances.AddRange(students);
                int isSuccess = db.SaveChanges();

                if (isSuccess > 0) return Json(true);

                return Json(false);

            }
            catch (Exception ex)
            {
                return Json(false);
            }

        }

        [Authorize(Roles = "Teacher")]
        public ActionResult ShowTeacherSubject()
        {
            string userEmail = User.Identity.Name;
            var id = db.Users.Where(x => x.Email == userEmail).Select(x => x.User_Id).Single();
            IEnumerable<TeacherSubject> TeacherSubject = db.TeacherSubjects
                 .Include("Class")
                 .Include("Subject")
                 .Include("User")
                 .Where(x => x.User.Role_Id == 2 && x.User_Id == id)
                 .Select(x => x);

            return View(TeacherSubject);
        }
    }
}
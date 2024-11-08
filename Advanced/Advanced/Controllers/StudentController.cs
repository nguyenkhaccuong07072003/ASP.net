using Advanced.Models;
using Advanced;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Advanced.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Student
        public ActionResult Dashboard()
        {
            return View();
        }
        [HttpGet]
        public ActionResult ListClass(string search = "", string id = "")
        {
            var classes = db.Lophocs.ToList();
            if (!String.IsNullOrEmpty(search))
            {
                classes = classes.Where(s => s.class_name.Contains(search)).ToList();
            }
            if (!String.IsNullOrEmpty(id))
            {
                classes = classes.Where(s => s.UserId.Contains(id)).ToList();
            }
            ApplicationUserManager UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            ApplicationRoleManager RoleManager = HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            var teachers = (from user in UserManager.Users
                            from userRole in user.Roles
                            join role in RoleManager.Roles on userRole.RoleId equals role.Id
                            where role.Name == "Teacher"
                            select new UserList()
                            {
                                UserId = user.Id,
                                UserName = user.UserName,
                                Email = user.Email,
                                Fullname = user.Fullname,
                                Phonenumber = user.PhoneNumber,
                                Age = user.Age,
                                Address = user.Address,
                                ShortDesc = user.ShortDesc,
                                RoleName = role.Name
                            }).ToList();
            ViewBag.Teacher = teachers;
            return View(classes);
        }
        [HttpGet]
        public ActionResult ListCourses(string search = "")
        {
            var khoahocs = db.Khoahocs.ToList();
            if (!String.IsNullOrEmpty(search))
            {
                khoahocs = khoahocs.Where(s => s.name.Contains(search)).ToList();
            }
            ApplicationUserManager UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            ApplicationRoleManager RoleManager = HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            var teachers = (from user in UserManager.Users
                            from userRole in user.Roles
                            join role in RoleManager.Roles on userRole.RoleId equals role.Id
                            where role.Name == "Teacher"
                            select new UserList()
                            {
                                UserId = user.Id,
                                UserName = user.UserName,
                                Email = user.Email,
                                Fullname = user.Fullname,
                                Phonenumber = user.PhoneNumber,
                                Age = user.Age,
                                Address = user.Address,
                                ShortDesc = user.ShortDesc,
                                RoleName = role.Name
                            }).ToList();
            ViewBag.Teacher = teachers;
            return View(khoahocs);
        }
        [HttpPost]
        public ActionResult JoinClass(int lophoc_id)
        {
            try
            {
                var id = User.Identity.GetUserId();
                if (!string.IsNullOrEmpty(id))
                {
                    ClassMember cm = new ClassMember();
                    cm.UserId = id;
                    cm.lophoc_id = lophoc_id;
                    var find = db.ClassMembers.FirstOrDefault(c => c.UserId == id && c.lophoc_id == lophoc_id);
                    if (find == null)
                    {
                        db.ClassMembers.Add(cm);
                        db.SaveChanges();
                        return Json(new { success = true, message = "Joined class successfully" });
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("You joined this class");
                        return Json(new { success = false, message = "You joined this class" });
                    }
                }
                else
                {
                    return Json(new { success = false, message = "User ID is not valid" });
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Error Add: " + e.ToString());
                return Json(new { success = false, message = "An error occurred while joining the class" });
            }
        }
        [HttpGet]
        public ActionResult TotalAbsent(string id)
        {
            var rollOuts = db.RollOuts
                        .Where(r => r.UserId == id && r.excuted == true)
                        .Include(r => r.Lophoc)
                        .ToList();
            return View(rollOuts);
        }
        [HttpGet]
        public ActionResult MyCourse()
        {
            var UserId = User.Identity.GetUserId();

            var result = from dh in db.DatHang
                         join kh in db.Khoahocs on dh.kh_id equals kh.kh_id
                         where dh.UserId == UserId
                         select new MyCourseViewModel
                         {
                             NgayMua = dh.NgayMua,
                             TongTien = dh.TongTien,
                             KhoaHocName = kh.name
                         };

            var courses = result.ToList();
            return View(courses);
        }
    }
}
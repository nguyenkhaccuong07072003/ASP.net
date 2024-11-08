using Advanced.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Advanced.Controllers
{
    public class AdminController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }
        // GET: Admin
        [HttpGet]
        public ActionResult List()
        {
            ApplicationRoleManager RoleManager = HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            ApplicationUserManager UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var users = (from user in UserManager.Users
                         from userRole in user.Roles
                         join role in RoleManager.Roles on userRole.RoleId equals role.Id
                         //where role.Name == "Teacher"
                         select new UserList()
                         {
                             UserId = user.Id,
                             UserName = user.UserName,
                             Email = user.Email,
                             Fullname = user.Fullname,
                             Phonenumber = user.PhoneNumber,
                             Age = user.Age,
                             Address = user.Address,
                             RoleName = role.Name
                         }).ToList();
            return View(users);
        }
        [HttpGet]
        public ActionResult RoleList()
        {
            ApplicationRoleManager RoleManager = HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            var roles = RoleManager.Roles.Select(x => new RoleViewDto() { RoleId = x.Id, Name = x.Name }).ToList();
            return View(roles);
        }
        public ActionResult AddRole()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddRole(RegisterRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole { Name = model.RoleName };
                IdentityResult result = RoleManager.Create(role);
                if (result.Succeeded)
                {
                    string RoleId = role.Id;
                    return RedirectToAction("RoleList", "Admin", new { Id = RoleId });
                }
                foreach (string error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult UpdateRole(EditRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole roleToEdit = RoleManager.FindById(model.RoleId);
                if (roleToEdit == null)
                {
                    return HttpNotFound();
                }
                if (roleToEdit.Name != model.RoleName)
                {
                    roleToEdit.Name = model.RoleName;
                }

                IdentityResult result = RoleManager.Update(roleToEdit);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home", new { Id = model.RoleId });
                }
                foreach (string error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult DeleteRole(DeleteRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole roleToDelete = RoleManager.FindById(model.RoleId);
                if (roleToDelete == null)
                {
                    return HttpNotFound();
                }
                IdentityResult result = RoleManager.Delete(roleToDelete);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                foreach (string error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult DeleteUser(string UserId)
        {
            ApplicationUserManager UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            ApplicationUser UserToDelete = UserManager.FindById(UserId);
            UserList model = new UserList();
            if (UserToDelete != null)
            {
                model.UserId = UserToDelete.Id;
                model.UserName = UserToDelete.UserName;
                model.Age = UserToDelete.Age;
                model.Email = UserToDelete.Email;
                model.Address = UserToDelete.Address;
                model.Phonenumber = UserToDelete.PhoneNumber;
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult DeleteUser(UserList model)
        {
            ApplicationUserManager UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            ApplicationUser UserToDelete = UserManager.FindById(model.UserId);
            if (UserToDelete != null)
            {
                IdentityResult result = UserManager.Delete(UserToDelete);
                if (result.Succeeded)
                {
                    return RedirectToAction("List");
                }
                foreach (string error in result.Errors)
                    ModelState.AddModelError("", error);
                return View(model);
            }
            return HttpNotFound();
        }
        public ActionResult Blog()
        {
            List<Post_Post> post_Posts = db.Post_Posts.ToList();
            return View(post_Posts);
        }
        [HttpGet]
        public ActionResult Blog_dt(int id)
        {
            Post_Post post = db.Post_Posts.Where(row => row.Id == id).FirstOrDefault();
            List<Post_Post> post_Posts = db.Post_Posts.Where(row => row.Id != id && row.Status == Post_Post.PostStatus.Pending).ToList();
            ViewBag.Pending = post_Posts;
            ViewBag.Category = db.Categories.ToList();
            return View(post);
        }
        [HttpPost]
        public ActionResult Public(int id)
        {
            var post = db.Post_Posts.Find(id);
            if (post != null)
            {
                post.Status = Post_Post.PostStatus.Approved;
                db.SaveChanges();
            }
            return RedirectToAction("Blog");
        }
        [HttpPost]
        public ActionResult Deny(int id)
        {
            var post = db.Post_Posts.Find(id);
            if (post != null)
            {
                post.Status = Post_Post.PostStatus.Denied;
                db.SaveChanges();
            }
            return RedirectToAction("Blog");
        }
        [HttpGet]
        public ActionResult HistoryPurchase()
        {
            var result = from dh in db.DatHang
                         join kh in db.Khoahocs on dh.kh_id equals kh.kh_id
                         join user in db.Users on dh.UserId equals user.Id
                         select new MyCourseViewModel
                         {
                             NgayMua = dh.NgayMua,
                             TongTien = dh.TongTien,
                             KhoaHocName = kh.name,
                             User = user.UserName,
                             TrangThai = dh.TrangThai
                         };
            var courses = result.ToList();
            return View(courses);
        }
    }
}
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.IO;
using Advanced.Models;

namespace Advanced.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationRoleManager _roleManager;
        private ApplicationUserManager _userManager;
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
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        //User areas
        public ActionResult Index()
        {
            ViewBag.Course = db.Khoahocs.ToList();
            ViewBag.Category = db.Categories.ToList();
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
                                RoleName = role.Name,
                                Image = user.Image
                            }).ToList();
            ViewBag.Teacher = teachers;
            ViewBag.Post = db.Post_Posts.Where(row => row.Status == Post_Post.PostStatus.Approved).ToList();
            ViewBag.Class = db.Lophocs.ToList();
            return View();
        }
        public ActionResult Courses()
        {
            List<Khoahoc> kh = db.Khoahocs.ToList();
            return View(kh);
        }
        public ActionResult Teachers()
        {
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
                                RoleName = role.Name,
                                Image = user.Image,
                            }).ToList();
            ViewBag.Class = db.Lophocs.ToList();
            return View(teachers);
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult AddUser()
        {
            ApplicationRoleManager RoleManager = HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            var roles = RoleManager.Roles
                .Where(x => !x.Name.Equals("Admin"))
                .Select(x => new RoleViewDto() { RoleId = x.Id, Name = x.Name })
                .ToList();

            ViewBag.Roles = roles;
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> AddUser(RegisterViewModel model)
        {
            ApplicationRoleManager RoleManager = HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            var roles = RoleManager.Roles
                .Where(x => !x.Name.Equals("Admin"))
                .Select(x => new RoleViewDto() { RoleId = x.Id, Name = x.Name })
                .ToList();

            ViewBag.Roles = roles;

            if (ModelState.IsValid)
            {
                var selectedRole = (await RoleManager.FindByIdAsync(model.RoleId)).Name;
                ApplicationUser user = new ApplicationUser
                {
                    Email = model.Email,
                    UserName = model.UserName,
                };
                var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();


                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user.Id, selectedRole);
                    var SignInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    return RedirectToAction("Index", "Home");
                }
                foreach (string error in result.Errors)
                    ModelState.AddModelError("", error);
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult UpdateUser(string UserId)
        {
            EditUserViewModel model = new EditUserViewModel();
            ApplicationUserManager UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            ApplicationUser UserToEdit = UserManager.FindById(UserId);
            if (UserToEdit != null)
            {
                model.UserId = UserToEdit.Id;
                model.UserName = UserToEdit.UserName;
                model.FullName = UserToEdit.Fullname;
                model.Age = UserToEdit.Age;
                model.Birthdate = UserToEdit.BirtDate;
                model.Main_subject = UserToEdit.Main_subject;
                model.address = UserToEdit.Address;
                model.Phone_number = UserToEdit.PhoneNumber;
                model.ShortDesc = UserToEdit.ShortDesc;
                model.Image = UserToEdit.Image;
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult UpdateUser(EditUserViewModel model)
        {
            /*if ( postedFile != null)
            {
                string path = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                postedFile.SaveAs(path + Path.GetFileName(postedFile.FileName));
            }*/
            if (ModelState.IsValid)
            {
                ApplicationUserManager UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                ApplicationUser UserToEdit = UserManager.FindById(model.UserId);
                var f = Request.Files["fAvatar"];
                if (UserToEdit.UserName != model.UserName)
                    UserToEdit.UserName = model.UserName;
                if (UserToEdit.Fullname != model.FullName)
                    UserToEdit.Fullname = model.FullName;
                if (UserToEdit.Age != model.Age)
                    UserToEdit.Age = model.Age;
                if (UserToEdit.Main_subject != model.Main_subject)
                    UserToEdit.Main_subject = model.Main_subject;
                if (UserToEdit.BirtDate != model.Birthdate)
                    UserToEdit.BirtDate = model.Birthdate;
                if (UserToEdit.Address != model.address)
                    UserToEdit.Address = model.address;
                if (UserToEdit.PhoneNumber != model.Phone_number)
                    UserToEdit.PhoneNumber = model.Phone_number;
                if (UserToEdit.ShortDesc != model.ShortDesc)
                    UserToEdit.ShortDesc = model.ShortDesc;
                if (f != null && f.ContentLength > 0)
                {
                    string folderName = Server.MapPath("~/Uploads");
                    string fileName = f.FileName;
                    f.SaveAs(folderName + "/" + fileName);
                    model.Image = "/Uploads/" + fileName;
                    UserToEdit.Image = model.Image;
                }
                IdentityResult result = UserManager.Update(UserToEdit);
                if (result.Succeeded)
                {
                    return RedirectToAction("UpdateUser", "Home", new { UserId = User.Identity.GetUserId() });
                }
                foreach (string error in result.Errors)
                    ModelState.AddModelError("", error);
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult Security(string UserId)
        {
            ChangeEmail model = new ChangeEmail();
            ApplicationUserManager UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            ApplicationUser UserToEdit = UserManager.FindById(UserId);
            if (UserToEdit != null)
            {
                model.UserId = UserToEdit.Id;
                model.Email = UserToEdit.Email;
                ViewBag.Email = UserToEdit.Email;
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult Security(ChangeEmail model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUserManager UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                ApplicationUser UserToEdit = UserManager.FindById(model.UserId);
                if (UserToEdit.Email != model.Email)
                    UserToEdit.Email = model.Email;
                IdentityResult result = UserManager.Update(UserToEdit);
                if (result.Succeeded)
                {
                    return RedirectToAction("Security", "Home", new { UserId = model.UserId });
                }
                foreach (string error in result.Errors)
                    ModelState.AddModelError("", error);
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult ChangePassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUserManager UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());
                if (user != null)
                {
                    IdentityResult result = UserManager.ChangePassword(User.Identity.GetUserId(), model.OldPassword, model.Password);
                    if (result.Succeeded)
                    {
                        user = UserManager.FindById(User.Identity.GetUserId());
                        if (user != null)
                        {
                            ApplicationSignInManager SignInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
                            SignInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
                        }
                        return RedirectToAction("ChangePassword", "Acount");
                    }
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                    return View(model);
                }
                return HttpNotFound();
            }
            return View(model);
        }
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUserManager UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = await UserManager.FindByEmailAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                //await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                bool IsSendEmail = SendEmail.EmailSend(model.Email, "Reset Your Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>", true);
                if (IsSendEmail)
                {
                    return RedirectToAction("ForgotPasswordConfirmation", "Account");
                }
            }
            return View(model);
        }
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //Role areas
        [HttpPost]
        public ActionResult AssignUserToRole(string userId, string roleName)
        {
            ApplicationUserManager UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            IdentityResult result = UserManager.AddToRole(userId, roleName);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            foreach (string error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
            return View();
        }
        public ActionResult HomeBlog()
        {
            List<Post_Post> post_Posts = db.Post_Posts.Where(row => row.Status == Post_Post.PostStatus.Approved).ToList();
            ViewBag.post = post_Posts;
            ViewBag.Category = db.Categories.ToList();
            return View(post_Posts);
        }
        [Authorize]
        [HttpGet]
        public ActionResult Blog_dt(int id)
        {
            var posts = db.Post_Posts.FirstOrDefault(row => row.Id == id);
            var main_comment = db.Main_Comments.Where(x => x.post_id == id).ToList();
            var mainCommentIds = main_comment.Select(z => z.Id).ToList();
            var sub_comment = db.Sub_Comments
            .Where(y => mainCommentIds.Contains(y.main_id))
            .ToList();
            List<Post_Post> post_Posts = db.Post_Posts.Where(row => row.Id != id && row.Status == Post_Post.PostStatus.Approved).ToList();

            var viewModel = new TrueBlog
            {
                Post_s = posts,
                main_Comments = main_comment,
                sub_Comments = sub_comment,
            };
            ViewBag.Recent = post_Posts;
            ViewBag.Category = db.Categories.ToList();
            return View(viewModel);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Comment(TrueBlog model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                Main_Comment mainComment = new Main_Comment
                {
                    post_id = model.Post_s.Id,
                    UserId = userId,
                    comments = model.NewComment,
                    DateComment = DateTime.Now
                };
                db.Main_Comments.Add(mainComment);
                db.SaveChanges();
                return RedirectToAction("Blog_dt", new { id = model.Post_s.Id });
            }
            return View(model);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reply(TrueBlog model, int main_id, int Id)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                Sub_Comment subComment = new Sub_Comment
                {
                    main_id = main_id,
                    UserId = userId,
                    comments = model.NewComment,
                    DateComment = DateTime.Now
                };
                db.Sub_Comments.Add(subComment);
                db.SaveChanges();
                return RedirectToAction("Blog_dt", new { id = Id });
            }
            return View(model);
        }

    }
}
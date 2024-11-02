﻿using CKFinder.Connector;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext db = new ApplicationDbContext();

        public AccountController() { }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
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

        // GET: Admin/Account
        // public ActionResult Index()
        // {
        //     var items = db.Users.ToList();
        //     return View(items);
        // }
        public ActionResult Index(string searchText, int page = 1)
        {
            int pageSize = 10;
            var users = db.Users.OrderByDescending(x => x.CreatedDate).AsQueryable();
            if (!string.IsNullOrEmpty(searchText))
            {
                users = users.Where(x => x.UserName.Contains(searchText) || x.FullName.Contains(searchText));
            }
            var pagedItems = users.ToPagedList(page, pageSize);
            var userRoles = new Dictionary<string, string>(); // Dictionary to hold user roles

            foreach (var user in users)
            {
                var roles = UserManager.GetRoles(user.Id); // Get roles for each user
                userRoles[user.Id] = roles.FirstOrDefault() ?? "No Role"; // Assuming each user has only one role
            }

            ViewBag.UserRoles = userRoles; // Pass the dictionary to the view
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            ViewBag.SearchText = searchText;
            return View(pagedItems); // Return users as usual
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated && User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }

            // Ngăn không cho trang login lưu vào cache
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await UserManager.FindByNameAsync(model.UserName);

            if (user == null)
            {
                // Case sai username
                return Json(new { success = false, errors = new List<string> { "Tên đăng nhập hoặc mật khẩu của bạn không đúng! Vui lòng thử lại!" } });
            }

            if (!user.IsActive)
            {
                return Json(new { success = false, errors = new List<string> { "Tài khoản của bạn bị khóa!" } });
            }

            var roles = await UserManager.GetRolesAsync(user.Id);
            if (roles.Contains("Customer"))
            {
                return Json(new { success = false, errors = new List<string> { "Bạn không có quyền truy cập!" } });
            }

            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: true);
            switch (result)
            {
                case SignInStatus.Success:
                    return Json(new { success = true, redirectUrl = returnUrl ?? Url.Action("Index", "Home") });                
                case SignInStatus.LockedOut: // Muốn dùng case này thì chỉnh shouldLockout: true
                    return Json(new { success = false, errors = new List<string> { "Bạn đã đăng nhập thất bại 5 lần! Vui lòng thử lại sau 5 phút!" } });
                case SignInStatus.RequiresVerification:
                    return Json(new { success = false, errors = new List<string> { "Xác minh tài khoản của bạn!" } });
                case SignInStatus.Failure: // Case sai password
                    return Json(new { success = false, errors = new List<string> { "Tên đăng nhập hoặc mật khẩu của bạn không đúng! Vui lòng thử lại!" } });
                default:
                    return Json(new { success = false, errors = ReturnErrors() });
            }
        }

        //
        // POST: /Account/Logoff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logoff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Account");
        }

        public ActionResult Lockout()
        {
            return View();
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Create()
        {
            ViewBag.Role = new SelectList(new List<string> { "Admin" }, "Name");
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FullName = model.FullName,
                    Phone = model.Phone,
                    Address = model.Address,
                    CreatedDate = DateTime.Now,
                    IsActive = model.IsActive
                };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "Admin");
                    db.Carts.Add(new Cart
                    {
                        UserId = user.Id
                    });
                    db.SaveChanges();
                    // Redirect sau khi tạo tài khoản thành công
                    return Json(new { success = true });
                }
                AddErrors(result);
            }
            ViewBag.Role = new SelectList(db.Roles.ToList(), "Name", "Name");
            return Json(new { success = false, errors = ReturnErrors() });
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = UserManager.FindById(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var model = new EditAccountViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                Address = user.Address,
                Role = UserManager.GetRoles(user.Id).FirstOrDefault(),
                IsActive = user.IsActive
            };
            if (model.Role == "Customer")
            {
                ViewBag.Role = new SelectList(new List<string> { "Customer" }, model.Role);
            }
            else ViewBag.Role = new SelectList(db.Roles.ToList(), "Name", "Name", model.Role);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(model.Id);
                if (user == null)
                {
                    return HttpNotFound();
                }

                if (UserManager.GetRoles(user.Id).Contains("Customer"))
                {
                    return Json(new { success = false, errors = new List<string> { "Không được sửa thông tin tài khoản khách hàng!" }});
                }

                user.UserName = model.UserName;
                user.FullName = model.FullName;
                user.Email = model.Email;
                user.Phone = model.Phone;
                user.Address = model.Address;
                user.IsActive = model.IsActive;

                var userRoles = await UserManager.GetRolesAsync(user.Id);
                var selectedRole = model.Role;

                if (!userRoles.Contains(selectedRole))
                {
                    await UserManager.RemoveFromRolesAsync(user.Id, userRoles.ToArray());
                    await UserManager.AddToRoleAsync(user.Id, selectedRole);
                }

                var result = await UserManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return Json(new { success = true });
                }
                AddErrors(result);
            }
            ViewBag.Role = new SelectList(db.Roles.ToList(), "Name", "Name", model.Role);
            return Json(new { success = false, errors = ReturnErrors() });
        }

        [HttpPost]
        public async Task<ActionResult> IsActive(string id)
        {
            var user = UserManager.FindById(id);
            if (user != null)
            {
                user.IsActive = !user.IsActive;
                var result = await UserManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return Json(new { success = true, isActive = user.IsActive });
                }
            }
            return Json(new { success = false });
        }

        // POST: /Admin/Account/Delete
        [HttpPost]
        public JsonResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Json(new { success = false, message = "ID không hợp lệ" });
            }

            var user = UserManager.FindById(id);
            if (user == null)
            {
                return Json(new { success = false, message = "Không có người dùng này." });
            }
            if (UserManager.GetRoles(user.Id).Contains("Customer"))
            {
                return Json(new { success = false, message = "Không được xóa tài khoản khách hàng!" });
            }

            var result = UserManager.Delete(user);
            db.Carts.Remove(db.Carts.SingleOrDefault(x => x.UserId == id));
            if (result.Succeeded)
            {
                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Có lỗi xảy ra khi xóa dữ liệu." });
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private List<string> ReturnErrors()
        {
            return ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
        }
    }
}
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
        public ActionResult Index(int page = 1)
        {
            int pageSize = 10;
            var users = db.Users.OrderByDescending(x => x.CreatedDate).ToList();
            var pagedItems = users.ToPagedList(page, pageSize);
            var userRoles = new Dictionary<string, string>(); // Dictionary to hold user roles

            foreach (var user in users)
            {
                var roles = UserManager.GetRoles(user.Id); // Get roles for each user
                userRoles[user.Id] = roles.FirstOrDefault() ?? "No Role"; // Assuming each user has only one role
            }

            ViewBag.UserRoles = userRoles; // Pass the dictionary to the view
            if (Request.IsAjaxRequest())
            {
                return PartialView(pagedItems);
            }
            return View(pagedItems); // Return users as usual
        }


        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
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
            if (user != null && !user.IsActive)
            {
                return Json(new { success = false, errors = new List<string> { "Tài khoản của bạn bị khóa!" } });
            }

            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: true);
            switch (result)
            {
                case SignInStatus.Success:
                    // Trả về thành công và đường dẫn để redirect
                    return Json(new { success = true, redirectUrl = returnUrl ?? Url.Action("Index", "Home") });
                case SignInStatus.LockedOut:
                    return Json(new { success = false, errors = new List<string> { "Tên đăng nhập hoặc mật khẩu của bạn không đúng! Vui lòng thử lại!" } });
                case SignInStatus.RequiresVerification:
                    return Json(new { success = false, errors = new List<string> { "Xác minh tài khoản của bạn!" } });
                case SignInStatus.Failure:
                    return Json(new { success = false, errors = new List<string> { "Tên đăng nhập hoặc mật khẩu của bạn không đúng! Vui lòng thử lại!" } });
                default:
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    return Json(new { success = false, errors });
            }
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
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
                    IsActive = true
                };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "Admin");

                    // Tạo giỏ hàng cho người dùng mới
                    var cart = new ShoppingCart(user.Id);
                    cart.SaveCart(db); // Lưu giỏ hàng vào cơ sở dữ liệu

                    // Redirect sau khi tạo tài khoản thành công
                    return RedirectToAction("Index", "Account");
                }
                AddErrors(result);
            }
            ViewBag.Role = new SelectList(db.Roles.ToList(), "Name", "Name");
            return View(model);
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
                    return RedirectToAction("Index");
                }
                AddErrors(result);
            }

            ViewBag.Role = new SelectList(db.Roles.ToList(), "Name", "Name", model.Role);
            return View(model);
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
                return Json(new { success = false, message = "Invalid ID" });
            }

            var user = UserManager.FindById(id);
            if (user == null)
            {
                return Json(new { success = false, message = "User not found" });
            }

            var result = UserManager.Delete(user);
            if (result.Succeeded)
            {
                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Failed to delete user" });
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
    }
}
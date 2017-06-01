using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebUI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace WebUI.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationUserManager UserManager { get { return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();}    }
        private IAuthenticationManager AuthenticationManager  { get  { return HttpContext.GetOwinContext().Authentication; } }

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            try
            {
                var users = UserManager.Users;
                return View(users);
            }
            catch (Exception e)
            {

                return RedirectToAction("Index", "Error", new {error = e.Message});
            }
          
        }


        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {



            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationUser user = new ApplicationUser { UserName = model.Email, Email = model.Email, Group = model.Group, Name = model.Name };
                    IdentityResult createUser = UserManager.Create(user, model.Password);
                    if (createUser.Succeeded)
                    {
                        UserManager.AddToRole(user.Id, "Student");
                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {
                        foreach (string error in createUser.Errors)
                        {
                            ModelState.AddModelError("", error);
                        }
                    }
                }
                return View(model);
            }
            catch (Exception e)
            {

                return RedirectToAction("Index", "Error", new { error = e.Message });
            }
        }

        public ActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationUser user = UserManager.Find(model.Email, model.Password);
                    if (user == null)
                    {
                        ModelState.AddModelError("", "Неверный логин или пароль.");
                    }
                    else
                    {
                        Login(user);
                        if (string.IsNullOrEmpty(returnUrl))
                            return RedirectToAction("Index", "Home");
                        return Redirect(returnUrl);
                    }
                }
                ViewBag.returnUrl = returnUrl;
                return View(model);
            }
            catch (Exception e)
            {

                return RedirectToAction("Index", "Error", new { error = e.Message });
            }
        }

        private void Login(ApplicationUser user)
        {
            ClaimsIdentity identity = UserManager.CreateIdentity(user,
                                                      DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignOut();
            AuthenticationManager.SignIn(new AuthenticationProperties
            {
                IsPersistent = true
            }, identity);
        }

        [Authorize(Roles = "Teacher, Admin, Student")]
        public ActionResult Logout()
        {
            try
            {
                AuthenticationManager.SignOut();
                return RedirectToAction("Login");
            }
            catch (Exception e)
            {

                return RedirectToAction("Index", "Error", new { error = e.Message });
            }
        }


        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string id)
        {

            try
            {
                ApplicationUser user = UserManager.FindById(id);
                if (user != null)
                {
                    UserManager.Delete(user);
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {

                return RedirectToAction("Index", "Error", new { error = e.Message });
            }



      
        }


        [Authorize(Roles = "Teacher, Admin, Student")]
        public ActionResult Edit()
        {
            try
            {
                var user = UserManager.FindByEmail(User.Identity.Name);

                if (user != null)
                {
                    var model = new EditUserModel() { Email = user.Email, Group = user.Group, Name = user.Name };
                    return View(model);
                }
                return RedirectToAction("Login", "Account");
            }
            catch (Exception e)
            {

                return RedirectToAction("Index", "Error", new { error = e.Message });
            }
        }

        [Authorize(Roles = "Teacher, Admin, Student")]
        [HttpPost]
        public ActionResult Edit(EditUserModel model)
        {

            try
            {
                ApplicationUser user = UserManager.FindByEmail(User.Identity.Name);
                ApplicationUser userWithNewEmail = UserManager.FindByEmail(model.Email);
                if (user != null)
                {
                    if (userWithNewEmail == null || user.Email == model.Email)
                    {

                        user.Email = model.Email;
                        user.UserName = model.Email;
                        user.Name = model.Name;
                        user.Group = model.Group;
                        IdentityResult result = UserManager.Update(user);
                        if (result.Succeeded)
                        {

                            Login(user);
                            return RedirectToAction("Index", "Home");


                        }
                        else
                        {
                            ModelState.AddModelError("", "Что-то пошло не так!");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Пользователь с таким именем уже существует!");
                    }


                }
                else
                {
                    ModelState.AddModelError("", "Пользователь не найден");
                }

                return View(model);

            }
            catch (Exception e)
            {

                return RedirectToAction("Index", "Error", new { error = e.Message });
            }
        }
    }
}
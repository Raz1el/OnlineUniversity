using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class RolesController : Controller
    {
        private ApplicationRoleManager RoleManager { get { return HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>(); } }
        private ApplicationUserManager UserManager { get { return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); } }

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            try
            {
                return View(RoleManager.Roles);
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Error", new { error = e.Message });
            }
        
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult AddUserInRole(string id)
        {

            try
            {
                ApplicationRole role = RoleManager.FindById(id);
                if (role != null)
                {

                    return View(new AddUserInRoleModel() { Role = role.Name });

                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Error", new { error = e.Message });
            }

           
      
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult AddUserInRole(AddUserInRoleModel model)
        {


            try
            {
                if (ModelState.IsValid)
                {
                    var user = UserManager.FindByEmail(model.Email);
                    if (user != null)
                    {
                        IdentityResult result = UserManager.AddToRole(user.Id, model.Role);
                        if (result.Succeeded)
                            return RedirectToAction("Index");
                        else
                        {
                            ModelState.AddModelError("", "Что-то пошло не так");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Такого пользователя не существует");
                    }

                }
                return View(model);
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Error", new { error = e.Message });
            }
          

        }
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Create(CreateRoleModel model)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = RoleManager.Create(new ApplicationRole
                    {
                        Name = model.Name,
                        Description = model.Description
                    });
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Что-то пошло не так");
                    }
                }
                return View(model);
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Error", new { error = e.Message });
            }

         
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string id)
        {

            try
            {
                ApplicationRole role = RoleManager.FindById(id);
                if (role != null)
                {
                    return View(new EditRoleModel { Id = role.Id, Name = role.Name, Description = role.Description });
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Error", new { error = e.Message });
            }

           
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Edit(EditRoleModel model)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationRole role = RoleManager.FindById(model.Id);
                    if (role != null)
                    {
                        role.Description = model.Description;
                        role.Name = model.Name;
                        IdentityResult result = RoleManager.Update(role);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Что-то пошло не так");
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
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string id)
        {

            try
            {
                ApplicationRole role = RoleManager.FindById(id);
                if (role != null)
                {
                    RoleManager.Delete(role);
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Error", new { error = e.Message });
            }
           
        }
        [Authorize(Roles = "Admin")]
        public ActionResult RoleUsersList(string id)
        {


            try
            {
                ApplicationRole role = RoleManager.FindById(id);
                if (role != null)
                {
                    ViewBag.RoleName = role.Name;
                    var users = role.Users.Select(x => x.UserId);
                    var result = new List<ApplicationUser>();
                    foreach (var userId in users)
                    {
                        result.Add(UserManager.FindById(userId));
                    }
                    return View(result);
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Error", new { error = e.Message });
            }

          
        }
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteUserFromRole(string userEmail,string role)
        {
            try
            {
                var user = UserManager.FindByEmail(userEmail);
                if (user != null)
                {
                    UserManager.RemoveFromRole(user.Id, role);
                    return RedirectToAction("RoleUsersList");
                }
                return HttpNotFound();
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Error", new { error = e.Message });
            }
        
        }
    }
}
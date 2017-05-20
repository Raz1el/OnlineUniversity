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

        public ActionResult Index()
        {
            return View(RoleManager.Roles);
        }

        [HttpGet]
        public ActionResult AddUserInRole(string id)
        {
            ApplicationRole role = RoleManager.FindById(id);
            if (role != null)
            {
                
                return View(new AddUserInRoleModel() {Role = role.Name});

            }
            return RedirectToAction("Index");
      
        }



        [HttpPost]
        public ActionResult AddUserInRole(AddUserInRoleModel model)
        {
            if (ModelState.IsValid)
            {
                var user = UserManager.FindByEmail(model.Email);
                if (user != null)
                {
                    IdentityResult result=UserManager.AddToRole(user.Id,model.Role);
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

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(CreateRoleModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result =  RoleManager.Create(new ApplicationRole
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

        public ActionResult Edit(string id)
        {
            ApplicationRole role =  RoleManager.FindById(id);
            if (role != null)
            {
                return View(new EditRoleModel { Id = role.Id, Name = role.Name, Description = role.Description });
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Edit(EditRoleModel model)
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

        public ActionResult Delete(string id)
        {
            ApplicationRole role = RoleManager.FindById(id);
            if (role != null)
            {
                RoleManager.Delete(role);
            }
            return RedirectToAction("Index");
        }

        public ActionResult RoleUsersList(string id)
        {
            ApplicationRole role = RoleManager.FindById(id);
            if (role != null)
            {
                var users = role.Users.Select(x=>x.UserId);
                var result=new List<ApplicationUser>();
                foreach (var userId in users)
                {
                    result.Add(UserManager.FindById(userId));
                }
                return View(result);
            }
            return RedirectToAction("Index");
        }

        public ActionResult DeleteUserFromRole(string userId)
        {
            return HttpNotFound();
        }
    }
}
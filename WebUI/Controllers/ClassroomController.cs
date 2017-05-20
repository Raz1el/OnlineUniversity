using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Models;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace WebUI.Controllers
{
    public class ClassroomController : Controller
    {
        ApplicationContext db = new ApplicationContext();
        private ApplicationRoleManager RoleManager { get { return HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>(); } }
        private ApplicationUserManager UserManager { get { return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); } }

        public ActionResult Index()
        {
            var classrooms = db.Classrooms.Include(x=>x.Teacher);
            return View(classrooms);
        }

        // GET: Classroom/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Classroom/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Classroom/Create
        [HttpPost]
        public ActionResult Create(Classroom model)
        {
            try
            {
                var teacher = UserManager.FindByEmail(model.Teacher.Email);
                if (teacher != null)
                {
                    if (UserManager.IsInRole(teacher.Id, "Teacher"))
                    {
                        model.Teacher = db.Users.Find(teacher.Id);
                        db.Classrooms.Add(model);
                        db.SaveChanges();
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        ModelState.AddModelError("", "Этот пользователь не преподаватель!");
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError("","Такого пользователя не существует!");
                    return View();
                }

            }
            catch
            {
                ModelState.AddModelError("", "Что-то пошло не так!");
                return View();
            }
        }

        // GET: Classroom/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Classroom/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Classroom/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Classroom/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}

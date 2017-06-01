using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Models;
using System.Data.Entity;
using System.Runtime.InteropServices;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace WebUI.Controllers
{
    public class ClassroomController : Controller
    {
        ApplicationContext db = new ApplicationContext();
        ApplicationUserManager UserManager { get { return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); } }


        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            try
            {
                var classrooms = db.Classrooms.Include(x => x.Teacher);
                return View(classrooms);

            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Error", new { error = e.Message });
            }
            
        }


        [Authorize(Roles = "Teacher, Admin, Student")]
        public ActionResult Details(int id)
        {

            try
            {
                var classroom = db.Classrooms.Include(x => x.Teacher).Include(x => x.Lectures).Include(x => x.Students).Include(x => x.Tasks).SingleOrDefault(x => x.Id == id);
                return View(classroom);

            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Error", new { error = e.Message });
            }
        
        }

        [Authorize(Roles = "Teacher, Admin")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Teacher, Admin")]
        [HttpPost]
        public ActionResult Create(Classroom model, string returnUrl)
        {
            try
            {
                ApplicationUser teacher;
                if (model.Teacher == null)
                {
                    teacher = UserManager.FindById(User.Identity.GetUserId());
                }
                else
                {
                    teacher = UserManager.FindByEmail(model.Teacher.Email);
                }
                if (teacher != null)
                {
                    if (UserManager.IsInRole(teacher.Id, "Teacher"))
                    {
                        model.Teacher = db.Users.Find(teacher.Id);
                        db.Classrooms.Add(model);
                        db.SaveChanges();
                        if (string.IsNullOrEmpty(returnUrl))
                            return RedirectToAction("Index","Home");
                        return Redirect(returnUrl);

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
            catch (Exception e)
            {
                return RedirectToAction("Index", "Error", new { error = e.Message });
            }
        }



        [Authorize(Roles = "Teacher, Admin")]
        public ActionResult Delete(int id,string returnUrl)
        {

            try
            {
                var classroom = db.Classrooms.Include(x=>x.Teacher).Include(x=>x.Tasks.Select(y=>y.Solutions)).Include(x=>x.Lectures).FirstOrDefault(x=>x.Id==id);
                if (classroom != null)
                {
                    if (User.IsInRole("Admin") || (classroom.Teacher.Id == User.Identity.GetUserId()))
                    {
                        var tasks = classroom.Tasks.ToList();
                        for (int i = 0; i < tasks.Count; i++)
                        {
                            var task = tasks[i];
                            var solutions = task.Solutions.ToList();
                            foreach (var solution in solutions)
                            {
                                db.Solutions.Remove(solution);
                            }
                            db.Tasks.Remove(task);
                        }
                     
                        foreach (var lecture in classroom.Lectures.ToList())
                        {
                            db.Lectures.Remove(lecture);
                        }
                        db.Classrooms.Remove(classroom);
                        db.SaveChanges();
                    }
                    else
                    {
                        return RedirectToAction("Index", "Error", new { error = "У вас нет прав!" });
                    }
                }

                if (string.IsNullOrEmpty(returnUrl))
                    return RedirectToAction("Index", "Home");
                return Redirect(returnUrl);

            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Error", new { error = e.Message });
            }
           
        }



        [Authorize(Roles = "Teacher, Admin, Student")]
        public ActionResult ShowClassrooms()
        {
            try
            {
                var user = db.Users.Include(x=>x.Classrooms).FirstOrDefault(x=>x.UserName==User.Identity.Name);
                if (user != null)
                {
                    if (UserManager.IsInRole(user.Id, "Teacher"))
                    {
                        var myClassrooms = db.Classrooms.Include(x => x.Teacher).Where(x => x.Teacher.Id == user.Id);
                        return View(myClassrooms);
                    }
                    else
                    {
                        var myClassrooms = user.Classrooms;
                        return View(myClassrooms);
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Error", new { error = "Такого пользователя не существует!" });
                }
          

             

            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Error", new { error = e.Message });
            }
       
        }

        [Authorize(Roles = "Teacher, Admin")]
        public ActionResult InviteStudent(int classroomId)
        {
            var inviteForm = new InviteUserModel() {ClassroomId = classroomId };
            return View(inviteForm);
        }
        [Authorize(Roles = "Teacher, Admin")]
        [HttpPost]
        public ActionResult InviteStudent(InviteUserModel model,string returnUrl)
        {
            try
            {
                var user = db.Users.FirstOrDefault(x=>x.Email==model.Email);
                if (user != null)
                {
                    var classroom = db.Classrooms.Include(x => x.Students)
                        .FirstOrDefault(x => x.Id == model.ClassroomId);
                    if (classroom != null)
                    {
                        classroom.Students.Add(user);
                        db.SaveChanges();
                        if (string.IsNullOrEmpty(returnUrl))
                            return RedirectToAction("Index", "Home");
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Такого класса не существует!");
                        return View(model);
                    }

                }
                else
                {
                    ModelState.AddModelError("","Такого пользователя не существует!");
                    return View(model);
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Error", new { error = e.Message });
            }
        }

        [Authorize(Roles = "Teacher, Admin")]
        public ActionResult StudentList(int classroomId)
        {
            try
            {
                var classroom = db.Classrooms.Include(x => x.Students).FirstOrDefault(x=>x.Id==classroomId);
                if (classroom != null)
                {
                    return View(classroom);
                }
                else
                {
                    return RedirectToAction("Index", "Error", new { error = "Класса не существует!" });
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Error", new { error = e.Message });
            }
        }

        [Authorize(Roles = "Teacher, Admin")]
        public ActionResult DeleteUser(int classroomId, string userId,string returnUrl)
        {
            try
            {
                var user = db.Users.FirstOrDefault(x => x.Id == userId);
                if (user != null)
                {
                    var classroom = db.Classrooms.Include(x => x.Students).Include(x=>x.Teacher)
                        .FirstOrDefault(x => x.Id == classroomId);
                    if (classroom != null)
                    {
                        if (classroom.Teacher.Id == User.Identity.GetUserId() || User.IsInRole("Admin"))
                        {
                            classroom.Students.Remove(user);
                            db.SaveChanges();
                            if (string.IsNullOrEmpty(returnUrl))
                                return RedirectToAction("Index", "Home");
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Error", new { error = "Недостаточно прав!" });
                        }
                    
                    }
                    else
                    {
                        return RedirectToAction("Index", "Error", new { error = "Такого класса не существует!" });
                    }

                }
                else
                {
                    return RedirectToAction("Index", "Error", new { error = "Такого пользователя не существует!" });
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Error", new { error = e.Message });
            }
        }



       



    }
}

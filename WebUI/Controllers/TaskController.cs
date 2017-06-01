using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class TaskController : Controller
    {
        ApplicationContext db = new ApplicationContext();

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            try
            {
                return View(db.Tasks);
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

                var task = db.Tasks.Find(id);
                if (task != null)
                {
                    return View(task);
                }
                return HttpNotFound();
            }
            catch (Exception e)
            {

                return RedirectToAction("Index", "Error", new { error = e.Message });
            }



        }

        [Authorize(Roles = "Teacher, Admin")]
        public ActionResult Create(int classroomId,string returnUrl)
        {
            try
            {


                ViewBag.returnUrl = returnUrl;
                var newTask = new NewTaskModel() { ClassroomId = classroomId };
                return View(newTask);
            }
            catch (Exception e)
            {

                return RedirectToAction("Index", "Error", new { error = e.Message });
            }


        }

        [Authorize(Roles = "Teacher, Admin")]
        [HttpPost]
        public ActionResult Create(NewTaskModel model, string returnUrl)
        {
            try
            {
                var taskCreator = db.Users.Find(User.Identity.GetUserId());
                if (taskCreator != null)
                {
                    var classroom = db.Classrooms.Include(x => x.Tasks).FirstOrDefault(x => model.ClassroomId == x.Id);
                    if (classroom != null)
                    {
                        classroom.Tasks.Add(new Task()
                        {
                            Title = model.Title,
                            Content = model.Content,
                            TaskCreator = taskCreator
                            
                        });
                        db.SaveChanges();
                        if (string.IsNullOrEmpty(returnUrl))
                            return RedirectToAction("Index");
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
                    ModelState.AddModelError("","Пользователя не существует!");
                    return View();
                }
            }
            catch(Exception e)
            {
                return RedirectToAction("Index", "Error", new { error = e.Message });
            }
        }

        [Authorize(Roles = "Teacher, Admin")]
        public ActionResult Edit(int id,string returnUrl)
        {

            try
            {


                ViewBag.returnUrl = returnUrl;
                var task = db.Tasks.Include(x=>x.TaskCreator).FirstOrDefault(x=>x.Id==id);
                if (task != null)
                {
                    if (User.IsInRole("Admin") || task.TaskCreator.Id == User.Identity.GetUserId())
                    {
                        return View(task);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Error", new { error = "Недостаточно прав" });
                    }
            
                }
                else
                {
                    return HttpNotFound();
                }
            }
            catch (Exception e)
            {

                return RedirectToAction("Index", "Error", new { error = e.Message });
            }

     
       
        }

        [Authorize(Roles = "Teacher, Admin")]
        [HttpPost]
        public ActionResult Edit(Task model, string returnUrl)
        {
            try
            {
                var task = db.Tasks.Include(x => x.TaskCreator).FirstOrDefault(x => x.Id == model.Id);
                if (task != null)
                {
                    if (User.IsInRole("Admin") || task.TaskCreator.Id == User.Identity.GetUserId())
                    {
                        task.Title = model.Title;
                        task.Content = model.Content;
                        db.SaveChanges();
                    }
                    else
                    {
                        return RedirectToAction("Index", "Error", new { error = "Недостаточно прав" });
                    }
                }
                if (string.IsNullOrEmpty(returnUrl))
                    return RedirectToAction("Index");
                return Redirect(returnUrl);
            }
            catch(Exception e)
            {
                return RedirectToAction("Index", "Error", new { error = e.Message });
            }
        }

        [Authorize(Roles = "Teacher, Admin")]
        public ActionResult Delete(int id, string returnUrl)
        {
            try
            {
                var task = db.Tasks.Include(x => x.TaskCreator).Include(x=>x.Solutions).FirstOrDefault(x => x.Id == id);
                if (task != null)
                {
                    if (User.IsInRole("Admin") || task.TaskCreator.Id == User.Identity.GetUserId())
                    {
                        var solutions = task.Solutions.ToList();
                        foreach (var solution in solutions)
                        {
                            db.Solutions.Remove(solution);
                        }
                        db.Tasks.Remove(task);
                        db.SaveChanges();
                        if (string.IsNullOrEmpty(returnUrl))
                            return RedirectToAction("Index");
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Error", new { error = "Недостаточно прав" });
                    }

    
                }
                else
                {
                    return HttpNotFound();
                }

            }
            catch(Exception e)
            {
                return RedirectToAction("Index", "Error", new { error = e.Message });
            }
        }



        [Authorize(Roles = "Teacher, Admin")]
        public ActionResult ShowSolutions(int taskId)
        {


            try
            {
                var task = db.Tasks.Include(x => x.Solutions.Select(y => y.SolutionCreator)).Include(x=>x.TaskCreator).FirstOrDefault(x => x.Id == taskId);
                if (task != null)
                {
                    if (User.IsInRole("Admin") || task.TaskCreator.Id == User.Identity.GetUserId())
                    {
                        ViewBag.TaskTitle = task.Title;
                        return View(task.Solutions);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Error", new { error = "Недостаточно прав" });
                    }
                }
                else
                {
                    return HttpNotFound();
                }

            }
            catch(Exception e)
            {
                return RedirectToAction("Index", "Error", new { error = e.Message });
            }

           
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using WebUI.Models;
using System.Data.Entity;

namespace WebUI.Controllers
{
    public class SolutionController : Controller
    {
        ApplicationContext db = new ApplicationContext();

        [Authorize(Roles = "Teacher, Admin")]
        public ActionResult Index()
        {
           

            try
            {
                return View(db.Solutions);

            }
            catch(Exception e)
            {
                return RedirectToAction("Index", "Error", new { error = e.Message });
            }

        }

        [Authorize(Roles = "Teacher, Admin, Student")]
        public ActionResult Details(int id)
        {

            try
            {
                
                var solution = db.Solutions.Include(x=>x.SolutionCreator).FirstOrDefault(x=>x.Id==id);
                if (solution != null)
                {
                    if (User.IsInRole("Admin") || User.IsInRole("Teacher") ||solution.SolutionCreator.Id==User.Identity.GetUserId())
                        return View(solution);
                    else
                    {
                        return RedirectToAction("Index", "Error", new { error = "Недостаточно прав" });
                    }
                }
                return HttpNotFound();

            }
            catch(Exception e)
            {
                return RedirectToAction("Index", "Error", new { error = e.Message });
            }
           
        }

        [Authorize(Roles = "Teacher, Admin, Student")]
        public ActionResult Create(int taksId,string returnUrl)
        {


            try
            {
                ViewBag.returnUrl = returnUrl;
                var task = new NewSolutionModel() { TaskId = taksId };
                return View(task);

            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Error", new { error = e.Message });
            }


          
        }

        [Authorize(Roles = "Teacher, Admin, Student")]
        [HttpPost]
        public ActionResult Create(NewSolutionModel model, string returnUrl)
        {
            try
            {
                var solutionCreator = db.Users.Find(User.Identity.GetUserId());
                var parentTask = db.Tasks.Include(x=>x.Solutions).FirstOrDefault(x=>x.Id==model.TaskId);
                if (solutionCreator != null)
                {
                    if (parentTask != null)
                    {
                        var solution = new Solution() {Content = model.Content,SolutionCreator = solutionCreator};
                        parentTask.Solutions.Add(solution);
                        db.SaveChanges();

                        if (string.IsNullOrEmpty(returnUrl))
                            return RedirectToAction("Index");
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Задачи не существует!");
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Пользователя не существует!");
                    return View();
                }




            }
            catch(Exception e)
            {
                return RedirectToAction("Index", "Error", new { error = e.Message });
            }
        }

        [Authorize(Roles = "Teacher, Admin, Student")]
        public ActionResult Edit(int id, string returnUrl)
        {


            try
            {
                ViewBag.returnUrl = returnUrl;
                var solution = db.Solutions.Find(id);
                if (solution != null)
                {
                    if (User.IsInRole("Admin") || User.IsInRole("Teacher") || solution.SolutionCreator.Id == User.Identity.GetUserId())
                        return View(solution);
                    else
                    {
                        return RedirectToAction("Index", "Error", new { error = "Недостаточно прав" });
                    }
                }
                return HttpNotFound();

            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Error", new { error = e.Message });
            }
           
        }

        [Authorize(Roles = "Teacher, Admin, Student")]
        [HttpPost]
        public ActionResult Edit(Solution model, string returnUrl)
        {
            try
            {
                var solution = db.Solutions.Include(x => x.SolutionCreator).FirstOrDefault(x => x.Id == model.Id);
                if (solution != null)
                {
                    if (User.IsInRole("Admin") || User.IsInRole("Teacher") ||
                        solution.SolutionCreator.Id == User.Identity.GetUserId())
                    {
                        solution.Content = model.Content;
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
            catch (Exception e)
            {
                return RedirectToAction("Index", "Error", new { error = e.Message });
            }
        }

        [Authorize(Roles = "Teacher, Admin, Student")]
        public ActionResult Delete(int id, string returnUrl)
        {
            try
            {
                var solution = db.Solutions.Include(x => x.SolutionCreator).FirstOrDefault(x => x.Id == id);
                if (solution != null)
                {
                    if (User.IsInRole("Admin") || User.IsInRole("Teacher") ||
                        solution.SolutionCreator.Id == User.Identity.GetUserId())
                    {
                        db.Solutions.Remove(solution);
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
            catch (Exception e)
            {
                return RedirectToAction("Index", "Error", new { error = e.Message });
            }
        }
    }
}

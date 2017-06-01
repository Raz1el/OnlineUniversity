using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Models;
using System.Data.Entity;
using Microsoft.AspNet.Identity;

namespace WebUI.Controllers
{
    public class LectureController : Controller
    {
        ApplicationContext db = new ApplicationContext();

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {

            try
            {
                return View(db.Lectures);
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
                var lecture = db.Lectures.Find(id);
                if (lecture != null)
                {
                    return View(lecture);
                }
                return HttpNotFound();
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Error", new { error = e.Message });
            }
           

        }
        [Authorize(Roles = "Teacher, Admin")]
        public ActionResult Create(int classroomId, string returnUrl)
        {

            try
            {
                ViewBag.returnUrl = returnUrl;
                return View(new NewLectureModel() { ClassroomId = classroomId });
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Error", new { error = e.Message });
            }

           
        }
        [Authorize(Roles = "Teacher, Admin")]
        [HttpPost]
        public ActionResult Create(NewLectureModel model, string returnUrl)
        {
            try
            {
                var classroom = db.Classrooms.Include(x => x.Lectures).Include(x=>x.Teacher).FirstOrDefault(x => model.ClassroomId == x.Id);
                var user = db.Users.Find(User.Identity.GetUserId());
                if (classroom != null)
                {
                    if (classroom.Teacher.Id == User.Identity.GetUserId() || User.IsInRole("Admin"))
                    {
                        classroom.Lectures.Add(new Lecture()
                        {
                            Content = model.Content,
                            Description = model.Description,
                            Theme = model.Theme,
                            LectureCreator = user
                        });
                        db.SaveChanges();
                        if (string.IsNullOrEmpty(returnUrl))
                            return RedirectToAction("Index");
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Error", new { error = "Недостаточно прав!" });
                    }
                 
                }
                else
                {
                    ModelState.AddModelError("", "Такого класса не существует!");
                    return View(model);
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Error", new { error = e.Message });
            }


        }

        [Authorize(Roles = "Teacher, Admin")]
        public ActionResult Edit(int id, string returnUrl)
        {
            try
            {
                ViewBag.returnUrl = returnUrl;
                var lecture = db.Lectures.Include(x=>x.LectureCreator).FirstOrDefault(x=>x.Id==id);
                if (lecture != null)
                {
                    return View(lecture);
                }
                return HttpNotFound();
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Error", new { error = e.Message });
            }
           
        }

        [Authorize(Roles = "Teacher, Admin")]
        [HttpPost]
        public ActionResult Edit(Lecture model, string returnUrl)
        {
            try
            {
                var lecture = db.Lectures.Include(x=>x.LectureCreator).FirstOrDefault(x=>x.Id==model.Id);
                if (lecture != null)
                {
                    if (User.IsInRole("Admin") || lecture.LectureCreator.Id == User.Identity.GetUserId())
                    {
                        lecture.Theme = model.Theme;
                        lecture.Content = model.Content;
                        lecture.Description = model.Description;
                        db.SaveChanges();
                    }
                    else
                    {
                        return RedirectToAction("Index", "Error", new { error = "Недостаточно прав!" });
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

        [Authorize(Roles = "Teacher, Admin")]
        public ActionResult Delete(int id, string returnUrl)
        {
            try
            {
                var lecture = db.Lectures.Include(x => x.LectureCreator).FirstOrDefault(x => x.Id == id);
                if (lecture != null)
                {
                    if (User.IsInRole("Admin") || lecture.LectureCreator.Id == User.Identity.GetUserId())
                    {
                        db.Lectures.Remove(lecture);
                        db.SaveChanges();
                        if (string.IsNullOrEmpty(returnUrl))
                            return RedirectToAction("Index");
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Error", new { error = "Недостаточно прав!" });
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

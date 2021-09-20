using JqueryAjaxInMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace JqueryAjaxInMVC.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewAll()
        {
         
            return View(GetAllEmployees());
        }

        IEnumerable<Employee> GetAllEmployees()
        {
            using (JQueryAjaxDBEntities db = new JQueryAjaxDBEntities())
            {
                IEnumerable<Employee> emp = db.Employees.ToList();
                return emp;
            }
           

        }

        public ActionResult AddOrEdit(int id=0)
        {
            if (id == 0)
            {
                Employee emp = new Employee();
                return View(emp);
            }
            else
            {
                using (JQueryAjaxDBEntities db = new JQueryAjaxDBEntities())
                {
                   Employee emp= db.Employees.Where(x => x.EmployeeID == id).SingleOrDefault();
                    return View(emp);

                }
            }
        }

        [HttpPost]
        public ActionResult AddOrEdit(Employee emp)
        {
            try
            {
                if (emp.ImageUpload != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(emp.ImageUpload.FileName);
                    string extension = Path.GetExtension(emp.ImageUpload.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    emp.ImagePath = "~/App_Files/Images/" + fileName;
                    emp.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/App_Files/Images/"), fileName));
                }
                using (JQueryAjaxDBEntities db = new JQueryAjaxDBEntities())
                {
                    if (emp.EmployeeID == 0)
                    {
                        db.Employees.Add(emp);
                        db.SaveChanges();
                    }
                    else
                    {
                        db.Entry(emp).State = EntityState.Modified;
                        db.SaveChanges();

                    }
                    
                }
                return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAllEmployees()), message = "Submitted successfully", JsonRequestBehavior.AllowGet });

            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = ex.Message, JsonRequestBehavior.AllowGet });

            }
        }   

        public ActionResult Delete(int id)
        {
            using (JQueryAjaxDBEntities db = new JQueryAjaxDBEntities())
            {
                Employee emp = db.Employees.Where(x => x.EmployeeID == id).SingleOrDefault();
                db.Employees.Remove(emp);
                db.SaveChanges();
            }
            return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAllEmployees()), message = "Deleted successfully", JsonRequestBehavior.AllowGet });

        }


    }
}
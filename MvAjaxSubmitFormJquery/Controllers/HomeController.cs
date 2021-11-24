using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvAjaxSubmitFormJquery.Models;

namespace MvAjaxSubmitFormJquery.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        StudentRepository _StudentsRepository = new StudentRepository();

        #region Login success are fails
        /// <summary>
        /// index
        /// </summary>
        /// <param name="Email_id"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public ActionResult Index(string Email_id, string Password)
        {
            try
            {
                List<StudentsModel> AllStudents = new List<StudentsModel>();

                if (TempData["mySesstion0"] != null)
                {
                    Email_id = TempData["mySesstion0"].ToString(); Password = TempData["mySesstion1"].ToString();
                }
                if (Email_id != null && Password != null)
                {
                    AllStudents = _StudentsRepository.GetStudents().Where(s => s.Email_id == Email_id && s.Password == Password).ToList();
                    if (AllStudents.Count != 0)
                    {
                        Session["UserData"] = AllStudents;
                        TempData["Email"] = AllStudents[0].Email_id;
                        TempData["Pwd"] = AllStudents[0].Password;
                        TempData["MyID"] = AllStudents[0].ID;
                        return View(AllStudents);
                    }
                    else
                    {
                        TempData["ErrorLogin"] = "username or password you entered is incorrect...";
                        return View("../Home/LoginPage");
                    }
                }
                else
                {
                    return View("../Home/LoginPage");
                }
            }
            catch (Exception)
            {

                throw new NotImplementedException();
            }

        }
        #endregion

        #region Login access for student
        /// <summary>
        /// LoginPage
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult LoginPage()
        {
            return View();
        }
        #endregion

        #region Creating student registration
        /// <summary>
        /// CreatStudentRegistartion
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CreatStudentRegistartion()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreatStudentRegistartion(StudentsModel students, HttpPostedFileBase file)
        {
            try
            {
                string ImageName = ""; if (file != null)
                {
                    ImageName = Path.GetFileName(file.FileName);
                    var physicalPath = Path.Combine(Server.MapPath("~/StudentImg"), students.Email_id + ImageName);
                    students.Upload_img = students.Email_id + ImageName;
                    _StudentsRepository.InsertStudentsModel(students);
                    file.SaveAs(physicalPath);
                }
                TempData["mySesstion0"] = students.Email_id; TempData["mySesstion1"] = students.Password;
                return RedirectToAction("../Home/Index");
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }
        #endregion

        #region Update student registration
        /// <summary>
        /// CreatStudentRegistartion
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UpdateStudentRegistartion(int studentID)
        {
            try
            {
                return View(_StudentsRepository.GetStudents().Where(s => s.ID == studentID).ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult UpdateStudentRegistartion(StudentsModel students, HttpPostedFileBase file)
        {
            try
            {
                _StudentsRepository.EditStudentsModel(students);
                TempData["Sucss"] = "You are record update successfully..";
                TempData["mySesstion0"] = students.Email_id; TempData["mySesstion1"] = students.Password;
                return RedirectToAction("../Home/Index");
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }
        #endregion

        #region Delete Selected Student Records
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int id)
        {
            StudentsModel AllStudents = _StudentsRepository.GetStudentByID(id);
            if (AllStudents == null)
                return RedirectToAction("Index");
            return View(AllStudents);
        }


        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                _StudentsRepository.DeleteStudentsModel(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region session Log off controlle
        /// <summary>
        /// log - off session  
        /// </summary>
        /// <returns></returns>
        public ActionResult Signout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index");
        }
        #endregion
    }
}

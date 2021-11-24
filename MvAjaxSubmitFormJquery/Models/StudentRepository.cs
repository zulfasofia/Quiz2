using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace MvAjaxSubmitFormJquery.Models
{
    public class StudentRepository : IStudentRepository
    {

        private List<StudentsModel> allStudents;
        private XDocument StudentsData;

        public StudentRepository()
        {
            try
            {
                allStudents = new List<StudentsModel>();
                StudentsData = XDocument.Load(HttpContext.Current.Server.MapPath("~/App_Data/StudentData.xml"));
                var Students = from t in StudentsData.Descendants("item")
                               select new StudentsModel(
                                   (int)t.Element("id"),
                                   t.Element("first_name").Value,
                               t.Element("last_name").Value,
                               t.Element("email_id").Value,
                               t.Element("password").Value,
                               t.Element("upload_img").Value,
                               (DateTime)t.Element("dob"),
                               t.Element("gender").Value,
                               t.Element("cell_number").Value,
                               t.Element("college").Value,
                               t.Element("adress").Value,
                               t.Element("city").Value,
                               t.Element("state").Value,
                               t.Element("pin").Value);

                allStudents.AddRange(Students.ToList<StudentsModel>());
            }
            catch (Exception)
            {

                throw new NotImplementedException();
            }
        }

        public IEnumerable<StudentsModel> GetStudents()
        {
            return allStudents;
        }

        public List<string> Getemail()
        {
            throw new NotImplementedException();
        }

        public StudentsModel GetStudentByEmailPwd(string email, string pwd)
        {
            return allStudents.Find(t => t.Email_id == email && t.Password == pwd);
        }

        public StudentsModel GetStudentByID(int id)
        {
            return allStudents.Find(item => item.ID == id);            
        }

        public void InsertStudentsModel(StudentsModel Student)
        {
            Student.ID = (int)(from S in StudentsData.Descendants("item") orderby (int)S.Element("id") descending select (int)S.Element("id")).FirstOrDefault() + 1;

            StudentsData.Root.Add(new XElement("item", new XElement("id", Student.ID),
                new XElement("first_name", Student.First_Name),
                new XElement("last_name", Student.Last_Name),
                new XElement("email_id", Student.Email_id),
                new XElement("password", Student.Password),
                new XElement("upload_img", Student.Upload_img),
                new XElement("dob", Student.Dob.Date.ToShortDateString()),
                new XElement("gender", Student.Gender),
                new XElement("cell_number", Student.Cell_number),
                new XElement("college", Student.College),
                new XElement("adress", Student.Adress),
                new XElement("city", Student.City),
                new XElement("state", Student.State),
                new XElement("pin", Student.Pin)));


            StudentsData.Save(HttpContext.Current.Server.MapPath("~/App_Data/StudentData.xml"));
        }       

        public void EditStudentsModel(StudentsModel Student)
        {
            try
            {
                XElement node = StudentsData.Root.Elements("item").Where(i => (int)i.Element("id") == Student.ID).FirstOrDefault();

                node.SetElementValue("first_name", Student.First_Name);
                node.SetElementValue("last_name", Student.Last_Name);
                //node.SetElementValue("email_id", Student.Email_id);
                //node.SetElementValue("password", Student.Password);
                //node.SetElementValue("upload_img", Student.Upload_img);
                node.SetElementValue("dob", Student.Dob.ToShortDateString());
                node.SetElementValue("gender", Student.Gender);
                node.SetElementValue("cell_number", Student.Cell_number);
                node.SetElementValue("college", Student.College);
                node.SetElementValue("adress", Student.Adress);
                node.SetElementValue("city", Student.City);
                node.SetElementValue("state", Student.State);
                node.SetElementValue("pin", Student.Pin);
                StudentsData.Save(HttpContext.Current.Server.MapPath("~/App_Data/StudentData.xml"));
            }
            catch (Exception)
            {

                throw new NotImplementedException();
            }
        }

        public void DeleteStudentsModel(int id)
        {
            try
            {
                StudentsData.Root.Elements("item").Where(i => (int)i.Element("id") == id).Remove();

                StudentsData.Save(HttpContext.Current.Server.MapPath("~/App_Data/StudentData.xml"));

            }
            catch (Exception)
            {

                throw new NotImplementedException();
            }
        }


        
    }
}
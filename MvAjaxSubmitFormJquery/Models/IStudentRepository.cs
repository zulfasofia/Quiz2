using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvAjaxSubmitFormJquery.Models
{
    public interface IStudentRepository
    {
        IEnumerable<StudentsModel> GetStudents();
        List<string> Getemail();
        StudentsModel GetStudentByID(int id);
        StudentsModel GetStudentByEmailPwd(string email, string pwd);
        void InsertStudentsModel(StudentsModel Student);
        void DeleteStudentsModel(int id);
        void EditStudentsModel(StudentsModel Student);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using BO;


namespace BLAPI
{
    public interface IBL
    {
        //Add Person to Course
        //get all courses for student
        //etc...
        BO.user GetStudent(int id);
        IEnumerable<BO.user> GetAllStudents();
        IEnumerable<BO.ListedPerson> GetStudentIDs();

        IEnumerable<BO.user> GetStudentsBy(Predicate<BO.user> predicate);
    }
}

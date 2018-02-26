using Repository.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var unitOfWork = new UnitOfWork(new RepositoryContext()))
            {
                // Example1
                var course = unitOfWork.Courses.Get(1);

                // Example2
                var courses = unitOfWork.Courses.GetCoursesWithAuthors(1, 4);

                // Example3
                var author = unitOfWork.Authors.GetAuthorWithCourses(1);
                unitOfWork.Courses.RemoveRange(author.Courses);
                unitOfWork.Authors.Remove(author);
                unitOfWork.Complete();
            }
        }
    }
}

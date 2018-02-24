using System;
using System.Linq;
using System.Data.Entity;
namespace Queries
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (var context = new PlutoContext())
                {
                    AddExample(context);
                    UpdateExample(context);
                    RemoveExample(context);
                    DeferredExecutionExample(context);
                    LinqSyntaxExamples(context);
                    ExtensionMethodExamples(context);
                    IQueryableExample(context);
                    LazyLoadingExample(context);
                    Nplus1Issue(context);
                    EagerLoadingExample(context);
                    ExplicitLoading(context);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadLine();
        }

        private static void RemoveExample(PlutoContext context)
        {
            // with cascade delete
            var course = context.Courses.Find(6);
            context.Courses.Remove(course);
            context.SaveChanges();

            var author = context.Authors.Include(a => a.Courses).Single(a => a.Id == 2);
            context.Courses.RemoveRange(author.Courses);
            context.Authors.Remove(author);
            context.SaveChanges();

            //prefer logical deletes instead of physical deletes
            //ex author.IsDeleted = true;
        }

        private static void UpdateExample(PlutoContext context)
        {
            var course = context.Courses.Find(4);
            course.Name = "New Name";
            course.AuthorId = 2;

            context.SaveChanges();
        }

        private static void AddExample(PlutoContext context)
        {
            //wrong way to set author
            var course = new Course
            {
                Name = "New Course",
                Description = "New Description",
                FullPrice = 19.95f,
                Level = 1,
                Author = new Author { Id = 1, Name = "Mosh Hamedani" }
            };
            var authors = context.Authors.ToList();
            var author = authors.SingleOrDefault(x => x.Id == 1);
            course.Author = author;
            course.Name = "New Course 2";

            course.Name = "New Course 3";
            course.AuthorId = 2;

            context.Courses.Add(course);
            context.SaveChanges();
        }

        private static void ExplicitLoading(PlutoContext context)
        {
            var author = context.Authors.Include(a => a.Courses).SingleOrDefault(a => a.Id == 1);
            // msdn way
            context.Entry(author).Collection(a => a.Courses).Load();

            //better way
            context.Courses.Where(c => c.AuthorId == author.Id).Load();

            foreach (var course in author.Courses)
            {
                Console.WriteLine($"{course.Name}");
            }

            var authors = context.Authors.ToList();
            var authorIDs = authors.Select(a => a.Id);
            context.Courses.Where(c => authorIDs.Contains(c.AuthorId) && c.FullPrice == 0).Load();
        }

        private static void EagerLoadingExample(PlutoContext context)
        {
            //bad because of magic string
            var courses = context.Courses.Include("Author").ToList();
            //better
            courses = context.Courses.Include(c => c.Author).ToList();
            foreach (var course in courses)
            {
                Console.WriteLine($"{course.Name} by {course.Author.Name}");
            }
        }

        private static void Nplus1Issue(PlutoContext context)
        {
            //N + 1 (To get N entities and their related entities, end up with N + 1 queries)
            var courses = context.Courses.ToList();
            foreach (var course in courses)
            {
                Console.WriteLine($"{course.Name} by {course.Author.Name}");
            }
        }

        private static void LazyLoadingExample(PlutoContext context)
        {
            //tags property must be declared as virtual
            var course = context.Courses.SingleOrDefault(c => c.Id == 2);
            //course tags not loaded yet
            foreach (var tag in course.Tags)
                //course tags loaded
                Console.WriteLine(tag.Name);
        }

        private static void IQueryableExample(PlutoContext context)
        {
            IQueryable<Course> courses = context.Courses;
            var filtered = courses.Where(c => c.Level == 1);
        }

        private static void DeferredExecutionExample(PlutoContext context)
        {
            var courses = context.Courses;
            var filtered = courses.Where(c => c.Level == 1);
            var sorted = filtered.OrderBy(c => c.Name);
            //no queries executed yet
            foreach (var c in courses)
            {
                //query executed
                Console.WriteLine(c.Name);
            }

            //in order to use helper properties on classes, must call extension method such as ToList to execute query
            //but performance can be bad due to having to get all records in table
            var r = context.Courses.ToList().Where(c => c.IsBeginnerCourse);
        }

        private static void ExtensionMethodExamples(PlutoContext context)
        {
            //extension methods
            var courses = context.Courses
                .Where(c => c.Name.Contains("c#"))
                .OrderBy(c => c.Name);

            var courses2 = context.Courses.Where(c => c.Level == 1)
                .OrderBy(c => c.Name)
                .ThenByDescending(c => c.Level)
                .Select(c => new { CourseName = c.Name, AuthorName = c.Author.Name })
                .Distinct();

            var courseGroups = context.Courses
                .GroupBy(c => c.Level);
            foreach(var group in courseGroups)
            {
                Console.WriteLine($"{group.Key} {group.Count()}");
                foreach(var course in group)
                {
                    Console.WriteLine($"\t{course.Name}");
                }
            }

            var result = context.Courses
                .Join(
                    context.Authors, 
                    c => c.AuthorId, a => a.Id, 
                    (course,author) => new { CourseName = course.Name, AuthorName = author.Name }
                );

            var result2 = context.Authors
                .GroupJoin(
                    context.Courses,
                    a => a.Id,
                    c => c.AuthorId,
                    (author, g) => new { AuthorName = author.Name, CourseCount = g.Count() }
                    );
            var result3 =
                context.Authors
                .SelectMany(
                    a => context.Courses,
                    (author, course) => new { a1 = author.Name, c2 = course.Name });

            var result4 = context.Courses.OrderBy(c => c.Level).FirstOrDefault(c => c.FullPrice > 100);
            var result5 = context.Courses.SingleOrDefault(c => c.Id == 1);

            var result6 = context.Courses.All(c => c.FullPrice > 10);
            var result7 = context.Courses.Any(c => c.Level == 1);
            var result8 = context.Courses.Count();
            var result9 = context.Courses.Max(c => c.FullPrice);
        }

        private static void LinqSyntaxExamples(PlutoContext context)
        {
            //linq syntax
            var query =
                from c in context.Courses
                where c.Name.Contains("c#")
                orderby c.Name
                select c;

            var query2 =
                from c in context.Courses
                where c.Author.Id == 1
                orderby c.Level descending, c.Name
                select new { Name = c.Name, Author = c.Author };

            var query3 =
                from c in context.Courses
                group c by c.Level into g
                select g;
            foreach (var group in query3)
            {
                Console.WriteLine($"{group.Key} ({group.Count()})");
                foreach (var course in group)
                {
                    Console.WriteLine($"\t{course.Name}");
                }
            }

            var query4 =
                from c in context.Courses
                join a in context.Authors on c.AuthorId equals a.Id
                select new { CourseName = c.Name, AuthorName = a.Name };

            var query5 =
                from a in context.Courses
                join c in context.Courses on a.Id equals c.AuthorId into g
                select new { AuthorName = a.Name, Courses = g.Count() };

            var query6 =
                from a in context.Authors
                from c in context.Courses
                select new { AuthorName = a.Name, CourseName = c.Name };
        }
    }
}

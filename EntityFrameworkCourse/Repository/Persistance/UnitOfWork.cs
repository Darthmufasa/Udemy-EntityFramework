using Repository.Core;
using Repository.Core.Repositories;
using Repository.Persistance.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Persistance
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RepositoryContext _context;

        public UnitOfWork(RepositoryContext context)
        {
            _context = context;
            Courses = new CourseRepository(_context);
            Authors = new AuthorRepository(_context);
        }

        public ICourseRepository Courses { get; private set; }
        public IAuthorRepository Authors { get; private set; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
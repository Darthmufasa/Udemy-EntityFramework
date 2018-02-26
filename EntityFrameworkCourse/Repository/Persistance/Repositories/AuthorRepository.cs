using Repository.Core.Domain;
using Repository.Core.Repositories;
using System.Linq;
using System.Data.Entity;

namespace Repository.Persistance.Repositories
{
    public class AuthorRepository : Repository<Author>, IAuthorRepository
    {
        public AuthorRepository(RepositoryContext context) : base(context)
        {
        }

        public Author GetAuthorWithCourses(int id)
        {
            return PlutoContext.Authors.Include(a => a.Courses).SingleOrDefault(a => a.Id == id);
        }

        public RepositoryContext PlutoContext
        {
            get { return Context as RepositoryContext; }
        }
    }
}
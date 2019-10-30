namespace WebStudy.Data.Repositories
{

    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using WebStudy.Data.Entities;

    public class UserTypeRepository : GenericRepository<UserType>, IUserTypeRepository
    {
        private readonly DataContext context;

        public UserTypeRepository(DataContext context) : base(context)
        {
            this.context = context;
        }


        /// <summary>
        /// Ejemplo de como incluir un método que retorna los objetos relacionados con la consulta
        /// </summary>
        /// <returns></returns>
        public IQueryable GetAllWithUsers()
        {
            return this.context.User.Include(p => p.UserType);
        }
    }
}

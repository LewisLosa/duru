using Duru.Library.Validation;
using Duru.UI.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Duru.UI.Data.Models
{
    public partial class DuruDbContext : DbContext
    {
        public DuruDbContext(DbContextOptions<DuruDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }

        public List<ValidationMessage> CreateValidationMessages(DbUpdateException ex)
        {
            List<ValidationMessage> ret = new List<ValidationMessage>();

            if (ex.InnerException is not null)
            {
                ret.Add(new ValidationMessage
                {
                    Message = ex.InnerException.Message,
                    PropertyName = "Database"
                });
            }
            else
            {
                ret.Add(new ValidationMessage
                {
                    Message = ex.Message,
                    PropertyName = "Database"
                });
            }

            return ret;
        }
    }
}
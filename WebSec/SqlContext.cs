using Microsoft.EntityFrameworkCore;
using WebSec.Models;

namespace WebSec
{
    public class SqlContext : DbContext
    {
        public SqlContext()
        {

        }

        public SqlContext(DbContextOptions<SqlContext> options) : base(options)
        {
        }

        public virtual DbSet<CommentEntity> Messages { get; set; }
    }
}

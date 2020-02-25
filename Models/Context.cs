using Microsoft.EntityFrameworkCore;
 
namespace BlackBelt.Models
{
    public class Context : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public Context(DbContextOptions options) : base(options) { }

	 public DbSet<User> User {get;set;}
     public DbSet<Actividad> Actividad{get;set;}
     public DbSet<UserActividadRelation> UserActividadRelation{get;set;}
    }
}
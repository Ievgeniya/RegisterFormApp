using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using TestProjectDomain.Entities;

namespace TestProjectDomain.Concrete
{
    public class MyDB : DbContext
    {
        public MyDB():
             base(nameOrConnectionString: "MyDb")
        {
        }

        public DbSet<Users> Users {get;set;}
    }
}

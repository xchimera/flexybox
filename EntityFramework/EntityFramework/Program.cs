using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace EntityFramework
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Student> liste = new List<Student>();
            liste.Add(new Student() { StudentName = "test1" });
            liste.Add(new Student() { StudentName = "test2" });
            liste.Add(new Student() { StudentName = "test3" });
            liste.Add(new Student() { StudentName = "test4" });
            liste.Add(new Student() { StudentName = "test5" });
            liste.Add(new Student() { StudentName = "test6" });
            liste.Add(new Student() { StudentName = "test7" });
            
            using (var ctx = new SchoolContext())
            {
                ctx.SaveEntities<Student>(liste); 
            }
            using (var ctx = new SchoolContext())
            {
                var student = ctx.Query<Student>().Where(x => x.Id == 1).Select(x => new someModel() { id = x.Id }).Single();
            }
            using (var ctx = new SchoolContext())
            {
                var student = new Student() { Id = 1 };
                ctx.DeleteEntity<Student>(student);
            }

        }
    }


    public class someModel
    {
        public int id { get; set; }
    }
}

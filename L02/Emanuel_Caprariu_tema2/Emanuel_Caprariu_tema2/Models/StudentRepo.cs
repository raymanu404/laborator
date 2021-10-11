using System;
using System.Collections.Generic;
//using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace Emanuel_Caprariu_tema2.Models
{
    public class StudentRepo: DbContext
    {    
        public StudentRepo(DbContextOptions<StudentRepo> options) : base(options) 
        {

        }
        public DbSet<Student> Students { get; set; }
        public static List<Student> Students1 { get; set; }
       
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Emanuel_Caprariu_tema2.Models
{
    public class Student
    {
        [Key]
        public long IdStudent { get; set; }
        public string Nume { get; set; }
        public string Prenume { get; set; }
        public string Facultate { get; set; }
        public int AnStudiu { get; set; }

       

    }
}

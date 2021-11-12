using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace DATC_EmanuelCaprariu_tema5.Models
{
     public class StudentsEntity : TableEntity
        {
            public StudentsEntity(string universitate, string email)
            {

                if (string.IsNullOrEmpty(universitate))
                    throw new ArgumentNullException("Camp universitate gol");

                if (string.IsNullOrEmpty(email))
                    throw new ArgumentNullException("Camp Email gol");
               

                this.PartitionKey = universitate;
                this.RowKey = email;
              
            }
            public StudentsEntity() { }
            public string Nume { get; set; }
            public string Prenume { get; set; }
            public string Facultate { get; set; }
            public int An { get; set; }
            public string Telefon { get; set; }

     }
}


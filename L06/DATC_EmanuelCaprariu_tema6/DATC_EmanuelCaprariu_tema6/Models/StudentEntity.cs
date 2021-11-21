using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;


namespace DATC_EmanuelCaprariu_tema6.Models
{
    public class StudentEntity:TableEntity
    {
        public StudentEntity(string universitate,string email) {
           
            if (string.IsNullOrEmpty(universitate))
                throw new ArgumentNullException("camp universitate gol");

            if (string.IsNullOrEmpty(email))
                throw new ArgumentNullException("email gol");           
           
      
            this.PartitionKey = universitate;
            this.RowKey = email;         

        }
        
    
    public StudentEntity() { }
        public string Nume { get; set; }
        public string Prenume { get; set; }
        public string Facultate { get; set; }
        public int An { get; set; }
        public string Telefon { get; set; }

    }
}

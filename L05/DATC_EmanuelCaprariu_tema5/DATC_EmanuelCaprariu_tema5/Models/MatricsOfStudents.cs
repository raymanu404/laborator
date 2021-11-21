using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;


namespace DATC_EmanuelCaprariu_tema5.Models
{
    public class MatricsOfStudents: TableEntity
    {
        public MatricsOfStudents(string universitate,string dataMetricii)
        {
            if (string.IsNullOrEmpty(universitate))
                throw new ArgumentNullException("Camp universitate gol");

            if (string.IsNullOrEmpty(dataMetricii))
                throw new ArgumentNullException("Camp data gol");


            this.PartitionKey = universitate;
            this.RowKey = dataMetricii;
        }
        public MatricsOfStudents() { }
        public int Count { get; set; }
    }
}

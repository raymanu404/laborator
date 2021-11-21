using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Threading.Tasks;
using DATC_EmanuelCaprariu_tema5.Models;
using Microsoft.WindowsAzure.Storage;

namespace DATC_EmanuelCaprariu_tema5
{
    class Program
    {
                   
        static void Main(string[] args)
        {
            StudentRepository student = new StudentRepository();
            
            MetricRepository metric = new MetricRepository(student.Count);
            metric.GenerateMetric();
        
        }
                          
    }
}

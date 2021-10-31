using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;


namespace DATC_EmanuelCaprariu_tema4.Models
{
    public class StudentEntity:TableEntity
    {
        public StudentEntity(string partitionKey,string rowkey,string nume,string facultate,int an) {
           
            if (string.IsNullOrEmpty(partitionKey))
                throw new ArgumentNullException("PartitionKey gol");

            if (string.IsNullOrEmpty(rowkey))
                throw new ArgumentNullException("Rowkey gol");
            if (string.IsNullOrEmpty(nume))
                throw new ArgumentNullException("Camp Nume gol");
            if (string.IsNullOrEmpty(facultate))
                throw new ArgumentException("Camp Facutalte gol");
            if (an < 0 && an > 6)
                throw new ArgumentException("Anul universitar incorect");

            this.PartitionKey = partitionKey;
            this.RowKey = rowkey;
            this.Nume = nume;
            this.Facultate = facultate;
            this.An = an;

        }
        //public StudentEntity(string storageAccount,string storageKey, string tableName)
        //{
        //    if (string.IsNullOrEmpty(storageAccount))
        //        throw new ArgumentNullException("StorageAccount");

        //    if (string.IsNullOrEmpty(storageKey))
        //        throw new ArgumentNullException("StorageKey");

        //    if (string.IsNullOrEmpty(tableName))
        //        throw new ArgumentNullException("TableName");

        //    this.StorageAccount = storageAccount;
        //    this.StorageKey = storageKey;
        //    this.TableName = tableName;
        //}
    
    public StudentEntity() { }
        public string Nume { get; set; }
        public string Facultate { get; set; }
        public int An { get; set; }
        //public string StorageAccount { get; }
        //public string StorageKey { get; }
        //public string TableName { get; }
    }
}

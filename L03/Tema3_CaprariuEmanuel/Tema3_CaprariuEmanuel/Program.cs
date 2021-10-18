using Google.Apis.Drive.v3.Data;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using Tema3_CaprariuEmanuel.home;
namespace Tema3_CaprariuEmanuel
{
    class Program
    {
        private static string ID_DRIVE = "1E5B8jxfVAPZM_uikzm_3aBUMs5Oz-bm_";
        private static DriveApiService driveApiService = driveApiService = new DriveApiService();
        private static IList<Google.Apis.Drive.v3.Data.File> files;
        
        private static void menu()
        {
            
            Console.WriteLine("1.Afisare fisiere");
            Console.WriteLine("2.Creeare fisiere");
            Console.WriteLine("3.Incarcare fisiere");
            Console.WriteLine("0.Iesire");
            Console.WriteLine("Alege optiune:");
            Console.WriteLine("");
        }
        private static void afisare_fisiere()
        {
            files = driveApiService.ListEntities(ID_DRIVE);
            Console.WriteLine("--------------------------");
            foreach (var file in files)
            {
                Console.WriteLine(file.Name);
            }

            Console.WriteLine("--------------------------");
        }
        private static void createDir()
        {
            Console.WriteLine("Dati numele folder pentru creeare:");
            string dir = Console.ReadLine();
            driveApiService.CreateFolder(dir, ID_DRIVE);
            Console.WriteLine("Folder creat cu succes!");
        }
        private static async void uploadFile()
        {
            Console.WriteLine("Dati path-ul fisierului pentru a urca pe server:");
            string path = Console.ReadLine();          
            if(path != "")
            {
                var stream = new FileStream(path, FileMode.Open);
               
                await driveApiService.Upload(stream, ID_DRIVE);
                Console.WriteLine("Incarcat cu succes!");
            }
           
        }      
        static void Main(string[] args)
        {
            string opt;
            do
            {
                menu();               
                opt = Console.ReadLine();
                Console.Clear();
                switch (opt)
                {                 
                    case "1":
                        afisare_fisiere();
                        break;
                    case "2":
                        createDir();
                        break;
                    case "3":
                        uploadFile();
                        break;
                 
                }

            } while (opt != "0");
            

        }
    }
}

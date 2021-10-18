using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tema3_CaprariuEmanuel.home
{
    public class DriveApiService
    {
        protected static string[] scopes = { DriveService.Scope.Drive };
        protected readonly UserCredential credential;
        static string ApplicationName = "DatcTema3";
        protected readonly DriveService service;
        protected readonly FileExtensionContentTypeProvider fileExtensionProvider;

        public string YOUR_GMAIL_EMAIL { get; } = "manu.caprariu.brediceanu@gmail.com";

        public DriveApiService()
        {
            using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = "token1.json";

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    scopes,
                    YOUR_GMAIL_EMAIL, // use a const or read it from a config file
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;

                fileExtensionProvider = new FileExtensionContentTypeProvider();
            }

            service = new DriveService(new BaseClientService.Initializer()

            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
        }
        public IList<Google.Apis.Drive.v3.Data.File> ListEntities(string id)
        {
            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.PageSize = 100;
            listRequest.Fields = "nextPageToken, files(id, name, parents, createdTime, modifiedTime, mimeType)";
            listRequest.Q = $"'{id}' in parents";

            return listRequest.Execute().Files;
        }
        public Google.Apis.Drive.v3.Data.File CreateFolder(string name, string id)
        {
            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = name,
                MimeType = "application/vnd.google-apps.folder",
                Parents = new[] { id }
            };

            var request = service.Files.Create(fileMetadata);
            request.Fields = "id, name, parents, createdTime, modifiedTime, mimeType";

            return request.Execute();
        }
        public async Task<Google.Apis.Drive.v3.Data.File> Upload(FileStream file, string documentId)
        {
            string[] file_name = file.Name.Split('\\');
            var name = ($"{ file_name[file_name.Length - 1]}");
            var mimeType = "plain/text";

            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = name,
                MimeType = mimeType,
                Parents = new[] { documentId }
            };

            FilesResource.CreateMediaUpload request;
            using (var stream = file)
            {
                request = service.Files.Create(fileMetadata, stream, mimeType);
                request.Fields = "id, name, parents, createdTime, modifiedTime, mimeType, thumbnailLink";
                await request.UploadAsync();
            }


            return request.ResponseBody;
        }
       
    }
}

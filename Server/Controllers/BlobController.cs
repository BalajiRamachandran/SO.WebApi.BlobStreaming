using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Server.Controllers
{
    public class BlobController : ApiController
    {
        public async Task<HttpResponseMessage> Post()
        {
            try
            {
                //return await EchoLength();
                return await UploadToAzureStorage(Request);
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent("Error: " + ex.Message)
                };
            }
        }

        private async Task<HttpResponseMessage> UploadToAzureStorage(HttpRequestMessage request)
        {
            var storageAccount = CloudStorageAccount.Parse("<YOUR-STORAGE-CONNECTION-STRING>");

            var blobClient = storageAccount.CreateCloudBlobClient();

            var container = blobClient.GetContainerReference("tmp");

            container.CreateIfNotExists();

            var blob = container.GetBlockBlobReference("test");

            await blob.UploadFromStreamAsync(await request.Content.ReadAsStreamAsync());

            return new HttpResponseMessage
            {
                Content = new StringContent("done!")
            };
        }

        private async Task<HttpResponseMessage> EchoLength()
        {
            var count = 0;

            using (var body = await Request.Content.ReadAsStreamAsync())
            {
                var chunk = new byte[1024];
                var read = await body.ReadAsync(chunk, 0, 1024);

                while (read > 0)
                {
                    count += read;
                    read = await body.ReadAsync(chunk, 0, 1024);
                }
            }

            return new HttpResponseMessage
            {
                Content = new StringContent(count.ToString())
            };
        }
    }
}

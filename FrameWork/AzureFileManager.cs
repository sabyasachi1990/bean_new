using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Threading.Tasks;
using System.IO;

namespace FrameWork
{
    public class AzureFileManager : IFileManager
    {
        private CloudStorageAccount StorageAccount { get; set; }
        private string ContainerName { get; set; }

        public AzureFileManager(CloudStorageAccount storageAccount, string containerName)
        {
            this.StorageAccount = storageAccount;
            this.ContainerName = containerName;
        }
        public async Task<string> UploadPDF(byte[] pdfBytes, string fileName)
        //public string UploadPDF(byte[] pdfBytes, string fileName)
        {
            CloudBlobClient blobClient = this.StorageAccount.CreateCloudBlobClient();
            CloudBlobContainer photoContainer = blobClient.GetContainerReference(this.ContainerName);
            string URL = null;
            try
            {
                Stream stream = new MemoryStream(pdfBytes);
                //var blob = await photoContainer.GetBlobReferenceFromServerAsync(fileName);
                CloudBlockBlob blob = photoContainer.GetBlockBlobReference(fileName);
                blob.UploadFromStream(stream);
                await blob.FetchAttributesAsync();

                URL = blob.Uri.AbsoluteUri;
            }
            catch (Exception ex)
            {
            }
            return URL;
        }

    }
}

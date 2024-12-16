using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.TemplateModule.Infra.V2
{
    public class AzureFileManager
    {
        private CloudStorageAccount StorageAccount { get; set; }
        private string ContainerName { get; set; }

        public AzureFileManager(CloudStorageAccount storageAccount, string containerName)
        {
            this.StorageAccount = storageAccount;
            this.ContainerName = containerName;
        }
        public async Task<string> UploadPDF(byte[] pdfBytes, string fileName)
        {
            CloudBlobClient blobClient = this.StorageAccount.CreateCloudBlobClient();
            CloudBlobContainer photoContainer = blobClient.GetContainerReference(this.ContainerName);
            string URL = null;
            try
            {
                Stream stream = new MemoryStream(pdfBytes);
                CloudBlockBlob blob = photoContainer.GetBlockBlobReference(fileName);
                blob.UploadFromStream(stream);
                URL = blob.Uri.AbsoluteUri;
            }
            catch (Exception ex)
            {
            }
            return URL;
        }

    }
    public interface IFileManager
    {

    }
}

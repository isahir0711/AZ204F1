using Azure.Storage;
using Azure.Storage.Blobs;

namespace FunctionP1.Services
{
    public class BlobStorageService
    {
        private const string containerName = "demoimages";
        private const string accountName = "iztestblob";
        private const string accountEnvVarName = "STORAGEKEY";
        public BlobServiceClient GetBlobServiceClient()
        {
            string? accountKey = Environment.GetEnvironmentVariable(accountEnvVarName);

            if (string.IsNullOrEmpty(accountKey))
            {
                Console.WriteLine("Woops no account key");
            }

            StorageSharedKeyCredential storageSharedKeyCredential =
                new(accountName, accountKey);
            BlobServiceClient blobServiceClient = new BlobServiceClient(
            new Uri($"https://{accountName}.blob.core.windows.net"),
            storageSharedKeyCredential);

            return blobServiceClient;
        }


        //List the blobs that have a size of > 5MB 
        public async Task<List<string>> ListBlobs(BlobServiceClient blobServiceClient)
        {
            BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);

            List<string> blobList = [];

            var resSegment = blobContainerClient.GetBlobsAsync();

            await foreach (var blobItem in resSegment)
            {
                var createdOn = blobItem.Properties.CreatedOn;
                var lastModifyOn = blobItem.Properties.LastModified;
                //Get the blobSize
                var blobSize = blobItem.Properties.ContentLength;
                if (blobSize > 5_000_000)
                {
                    blobList.Add(blobItem.Name);
                }
            }

            return blobList;
        }

        public async Task DeleteBlob(string blobName, BlobServiceClient blobServiceClient)
        {
            BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = blobContainerClient.GetBlobClient(blobName);

            try
            {
                await blobClient.DeleteAsync();
            }
            catch (System.Exception)
            {

                throw;
            }
        }

    }
}
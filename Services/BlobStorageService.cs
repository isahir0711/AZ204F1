using Azure.Storage;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;

namespace FunctionP1.Services
{
    public class BlobStorageService
    {
        private readonly ILogger _logger;
        public BlobStorageService(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<BlobStorageService>();
        }

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

        public async Task<List<string>> ListOldBlobs(BlobServiceClient blobServiceClient)
        {
            BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);

            List<string> blobList = [];

            var resSegment = blobContainerClient.GetBlobsAsync();

            //Blobs creados hace mas de 5 dias
            var desiredDate = DateTime.Now.AddDays(-5);

            await foreach (var blobItem in resSegment)
            {
                var createdOn = blobItem.Properties.CreatedOn;

                if (createdOn < desiredDate)
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
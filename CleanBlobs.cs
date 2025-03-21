using System;
using Azure.Storage.Blobs;
using FunctionP1.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FunctionP1
{
    public class CleanBlobs
    {
        private readonly ILogger _logger;
        private readonly BlobStorageService _blobStorageService;

        private const string Cron = "0 */2 * * * *";

        public CleanBlobs(ILoggerFactory loggerFactory, BlobStorageService blobStorageService)
        {
            _logger = loggerFactory.CreateLogger<CleanBlobs>();
            _blobStorageService = blobStorageService;
        }

        [Function("CleanBlobs")]
        public async Task RunAsync([TimerTrigger(Cron)] TimerInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            BlobServiceClient blobServiceClient = _blobStorageService.GetBlobServiceClient();

            var res = await _blobStorageService.ListBlobs(blobServiceClient);

            foreach (var blob in res)
            {
                await _blobStorageService.DeleteBlob(blob, blobServiceClient);
            }
        }
    }
}

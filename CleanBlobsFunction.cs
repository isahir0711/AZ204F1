using System;
using Azure.Storage.Blobs;
using FunctionP1.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FunctionP1
{
    public class CleanBlobsFunction
    {
        private readonly ILogger _logger;
        private readonly BlobStorageService _blobStorageService;

        //Every day at 10:30
        private const string Cron = "30 10 * * *";

        public CleanBlobsFunction(ILoggerFactory loggerFactory, BlobStorageService blobStorageService)
        {
            _logger = loggerFactory.CreateLogger<CleanBlobsFunction>();
            _blobStorageService = blobStorageService;
        }

        [Function("CleanBlobs")]
        public async Task RunAsync([TimerTrigger(Cron)] TimerInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            BlobServiceClient blobServiceClient = _blobStorageService.GetBlobServiceClient();

            var res = await _blobStorageService.ListOldBlobs(blobServiceClient);

            if (res.Count > 1)
            {
                foreach (var blob in res)
                {
                    await _blobStorageService.DeleteBlob(blob, blobServiceClient);
                }
            }

        }
    }
}

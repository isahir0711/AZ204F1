using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FunctionP1
{
    public class BlobUpload
    {
        private readonly ILogger<BlobUpload> _logger;

        public BlobUpload(ILogger<BlobUpload> logger)
        {
            _logger = logger;
        }

        [Function(nameof(BlobUpload))]
        public async Task Run([BlobTrigger("demoimages/{name}", Connection = "BlobConnString")] Stream stream, string name)
        {
            using var blobStreamReader = new StreamReader(stream);
            // var content = await blobStreamReader.ReadToEndAsync();
            _logger.LogInformation($"C# Blob trigger function Processed blob\n Name: {name} \n");
        }
    }
}

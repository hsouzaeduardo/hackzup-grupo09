using Azure.Storage.Blobs;

public class BlobStorageHelper
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _containerName = "files-container";

    public BlobStorageHelper(string storageConnectionString)
    {
        _blobServiceClient = new BlobServiceClient(storageConnectionString);
    }

    public async Task<string> DownloadFileAsTextAsync(string fileName)
    {
        var blobContainerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = blobContainerClient.GetBlobClient(fileName);

        if (await blobClient.ExistsAsync())
        {
            var blobDownloadInfo = await blobClient.DownloadAsync();
            using var reader = new StreamReader(blobDownloadInfo.Value.Content);
            return await reader.ReadToEndAsync();  // Lê o conteúdo como texto
        }

        throw new FileNotFoundException($"Arquivo {fileName} não encontrado no Blob Storage.");
    }
}

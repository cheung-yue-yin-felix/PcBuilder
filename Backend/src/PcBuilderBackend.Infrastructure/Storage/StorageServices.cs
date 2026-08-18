using Amazon.S3;
using Microsoft.Extensions.Configuration;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Infrastructure.Storage;

public class StorageServices : IStorageServices
{
    private readonly string _bucketName;
    private readonly IAmazonS3 _s3Client;

    public StorageServices(IConfiguration configuration)
    {
        _bucketName = configuration["BackBlaze:BucketName"] ?? "";
        var accessKey = configuration["BackBlaze:KeyID"] ?? "";
        var secretKey = configuration["BackBlaze:Key"] ?? "";
        var serviceUrl = configuration["BackBlaze:ServiceUrl"] ?? "";
        _s3Client = new AmazonS3Client(accessKey, secretKey, new AmazonS3Config
        {
            ServiceURL = serviceUrl,
            ForcePathStyle = true
        });
    }

    public async Task<string> UploadAsync(Stream fileStream, string fileName, string contentType)
    {
        var key = $"photos/{Guid.NewGuid()}{Path.GetExtension(fileName)}";
        
        var request = new Amazon.S3.Model.PutObjectRequest
        {
            BucketName = _bucketName,
            Key = key,
            InputStream = fileStream,
            ContentType = contentType
        };

        await _s3Client.PutObjectAsync(request);
        return key;
    }

    public async Task<Stream> GetPhotoStreamAsync(string key)
    {
        var response = await _s3Client.GetObjectAsync(_bucketName, key);
        return response.ResponseStream;
    }
}


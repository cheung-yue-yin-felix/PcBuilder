using Amazon.S3;
using Microsoft.Extensions.Configuration;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Infrastructure.Services;

public class StorageServices : IStorageServices
{
    private readonly string _bucketName;
    private readonly IAmazonS3 _s3Client;

    public StorageServices(IConfiguration configuration)
    {
        _bucketName = configuration["S3:BucketName"] ?? "";
        var accessKey = configuration["S3:AccessKey"] ?? "";
        var secretKey = configuration["S3:SecretKey"] ?? "";
        var serviceUrl = configuration["S3:ServiceUrl"] ?? "";
        _s3Client = new AmazonS3Client(accessKey, secretKey, new AmazonS3Config
        {
            ServiceURL = serviceUrl,
            ForcePathStyle = true
        });
    }

    public async Task<string> UploadAsync(Stream fileStream, string fileName, string contentType)
    {
        var key = $"pc-builder/{Guid.NewGuid()}{Path.GetExtension(fileName)}";

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
}


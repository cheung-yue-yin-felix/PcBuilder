namespace PcBuilderBackend.Application.Common.Interfaces;

public interface IStorageServices
{
    Task<string> UploadAsync(Stream fileStream, string fileName, string contentType);
}


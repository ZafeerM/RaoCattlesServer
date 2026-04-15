namespace RaoCattles.Modules.Products.Application.Contracts;

public interface IImageService
{
    Task<Stream> CompressAsync(Stream input, string contentType, long fileSize, CancellationToken ct = default);
    Task<string> UploadAsync(Stream stream, string fileName, CancellationToken ct = default);
    Task DeleteAsync(string publicId, CancellationToken ct = default);
}

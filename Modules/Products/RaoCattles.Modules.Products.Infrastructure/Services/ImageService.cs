using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using RaoCattles.BuildingBlocks.Exceptions;
using RaoCattles.Modules.Products.Application.Contracts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace RaoCattles.Modules.Products.Infrastructure.Services;

public class ImageService(Cloudinary cloudinary) : IImageService
{
    private static readonly HashSet<string> AllowedContentTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "image/jpeg",
        "image/png",
        "image/webp"
    };

    private const int MaxLongestSide = 2000;
    private const long SkipResizeThreshold = 2 * 1024 * 1024; // 2 MB
    private const int JpegQuality = 90;

    public async Task<Stream> CompressAsync(Stream input, string contentType, long fileSize, CancellationToken ct = default)
    {
        if (!AllowedContentTypes.Contains(contentType))
            throw new ValidationException($"Invalid image type '{contentType}'. Allowed types: jpg, png, webp.");

        using var image = await Image.LoadAsync(input, ct);

        if (fileSize >= SkipResizeThreshold)
        {
            var longestSide = Math.Max(image.Width, image.Height);
            if (longestSide > MaxLongestSide)
            {
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Mode = ResizeMode.Max,
                    Size = new SixLabors.ImageSharp.Size(MaxLongestSide, MaxLongestSide)
                }));
            }
        }

        var output = new MemoryStream();
        var encoder = new JpegEncoder { Quality = JpegQuality };
        await image.SaveAsync(output, encoder, ct);
        output.Position = 0;
        return output;
    }

    public async Task<string> UploadAsync(Stream stream, string fileName, CancellationToken ct = default)
    {
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(fileName, stream),
            Folder = "products"
        };

        var result = await cloudinary.UploadAsync(uploadParams, ct);

        if (result.Error is not null)
            throw new Exception($"Cloudinary upload failed: {result.Error.Message}");

        return result.PublicId;
    }

    public async Task DeleteAsync(string publicId, CancellationToken ct = default)
    {
        var result = await cloudinary.DestroyAsync(new DeletionParams(publicId) { Invalidate = true });

        if (result.Result is not "ok" and not "not found")
            throw new Exception($"Cloudinary deletion failed for '{publicId}': {result.Result}");
    }
}

namespace RaoCattles.Modules.Products.Application.Dtos;

public record ImageInput(Stream Stream, string ContentType, string FileName, long Size);

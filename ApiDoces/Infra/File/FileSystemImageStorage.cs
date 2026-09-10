using Application.Interfaces;

public class FileSystemImageStorage(IWebHostEnvironment env) : IImageStorage
{
    public string SaveFromBase64(string base64)
    {
        var base64Data = base64.Contains(",") ? base64.Split(',')[1] : base64;
        var bytes = Convert.FromBase64String(base64Data);
        var fileName = $"{Guid.NewGuid()}.jpg";

        var directory = Path.Combine(env.WebRootPath, "images");
        Directory.CreateDirectory(directory);

        var path = Path.Combine(directory, fileName);
        File.WriteAllBytes(path, bytes);

        return $"/images/{fileName}";
    }
}
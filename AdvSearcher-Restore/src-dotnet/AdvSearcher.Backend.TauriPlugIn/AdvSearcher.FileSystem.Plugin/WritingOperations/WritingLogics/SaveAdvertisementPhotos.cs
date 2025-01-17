using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Text;
using AdvSearcher.Core.Entities.AdvertisementAttachments;
using AdvSearcher.Core.Entities.Advertisements;
using AdvSearcher.FileSystem.SDK.Contracts;

namespace AdvSearcher.FileSystem.Plugin.WritingOperations.WritingLogics;

internal sealed class SaveAdvertisementPhotos : IAdvertisementWritingLogic
{
    private const string PhotosPath = "Photos";
    private readonly IAdvertisementDirectory _directory;
    private readonly string _advertisementDirectory;

    public SaveAdvertisementPhotos(
        IAdvertisementDirectory directory,
        string advertisementDirectory
    ) => (_directory, _advertisementDirectory) = (directory, advertisementDirectory);

    public void Process(Advertisement advertisement)
    {
        if (!advertisement.Attachments.Any())
            return;
        Console.WriteLine("Creating photos folder path");
        string photosDirectory = Path.Combine(
            _directory.CurrentPath,
            _advertisementDirectory,
            PhotosPath
        );
        Directory.CreateDirectory(photosDirectory);
        Console.WriteLine($"Photos folder path created {photosDirectory}");
        int index = 1;
        Console.WriteLine("Downloading advertisement");
        using HttpClient client = new HttpClient();
        foreach (var attachment in advertisement.Attachments)
        {
            Console.WriteLine("Creating photo file");
            string fileName = CreateFileName(ref index, photosDirectory);
            Console.WriteLine($"Photo file created {fileName}");
            Console.WriteLine("Loading advertisement");
            LoadAdvertisementPhoto(attachment, fileName, client);
            Console.WriteLine("Advertisement loaded");
            ResizeImageIfNeeded(fileName);
            Console.WriteLine("Advertisement saved");
        }
    }

    private string CreateFileName(ref int index, string photoDirectory)
    {
        StringBuilder fileName = new StringBuilder();
        fileName.Append("Photo");
        fileName.Append('_');
        fileName.Append(index);
        fileName.Append(".png");
        index++;
        return Path.Combine(photoDirectory, fileName.ToString());
    }

    private void LoadAdvertisementPhoto(
        AdvertisementAttachment attachment,
        string fileName,
        HttpClient client
    )
    {
        string url = attachment.Url.Value;
        byte[] imageBytes = client.GetByteArrayAsync(url).GetAwaiter().GetResult();
        File.WriteAllBytes(fileName, imageBytes);
    }

    private void ResizeImageIfNeeded(string filePath)
    {
        using (MemoryStream memoryStream = new MemoryStream(File.ReadAllBytes(filePath)))
        {
            using (Image image = Image.FromStream(memoryStream))
            {
                if (image.Width > 1280 || image.Height > 720)
                {
                    double num = image.Width / image.Height;
                    int width2;
                    int height2;
                    if (image.Width > image.Height)
                    {
                        width2 = 1280;
                        height2 = (int)Math.Round(width2 / num);
                    }
                    else
                    {
                        height2 = 720;
                        width2 = (int)Math.Round(height2 * num);
                    }
                    if (width2 > 1280)
                    {
                        width2 = 1280;
                        height2 = (int)Math.Round(width2 / num);
                    }
                    else if (height2 > 720)
                    {
                        height2 = 720;
                        width2 = (int)Math.Round(height2 * num);
                    }

                    using (Bitmap bitmap = new Bitmap(width2, height2))
                    {
                        using (Graphics graphics = Graphics.FromImage(bitmap))
                        {
                            graphics.CompositingQuality = CompositingQuality.HighQuality;
                            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            graphics.SmoothingMode = SmoothingMode.HighQuality;
                            graphics.DrawImage(image, 0, 0, width2, height2);
                        }

                        bitmap.Save(filePath, ImageFormat.Png);
                    }
                }
                else
                    image.Save(filePath, ImageFormat.Png);
            }
        }
    }
}

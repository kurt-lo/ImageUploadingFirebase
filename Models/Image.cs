using System.IO;
namespace ImageUploadingFirebase.Models
{
    public class Image
    {
        public string? Id { get; set; }
        public FileInfo[] FileImage { get; set; }
    }
}

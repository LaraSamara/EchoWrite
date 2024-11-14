namespace Blog.PL.Helpers
{
    public class FilesSettings
    {
        public static string UploadFile(IFormFile file, string FolderName)
        {
            string FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files\\", FolderName);
            string FileName = $"{Guid.NewGuid()}{file.FileName}";
            string FilePath = Path.Combine(FolderPath, FileName);
            var fileStream = new FileStream(FilePath, FileMode.Create);
            file.CopyTo(fileStream);
            return FileName;
        }
        public static void Delete(string FolderName, string FileName)
        {
            string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files\\", FolderName, FileName);
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }
        }
    }
}

namespace SchoolAttendanceManager.Image_Services
{
    public class imageService
    {

        public const string DefaultImage = "/Image/DefaultImage.png/";
        public const string ProfileImagePath = "/Image/Photos/Img/";

        public static async Task<string> SaveImageAsync(IFormFile file, string uploadPath)
        {
            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            using var fileStream = new FileStream(Path.Combine(uploadPath, fileName), FileMode.Create);
            await file.CopyToAsync(fileStream);
            return fileName;
        }
        public static string SaveImage(IFormFile file, string uploadPath)
        {
            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            using (var fileStream = new FileStream(Path.Combine(uploadPath, fileName), FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            return fileName;
        }



        public static void DeleteImage(string webrootpath, string? oldfilefromdb)
        {
            if (string.IsNullOrWhiteSpace(oldfilefromdb))
                return;

            var fileName = oldfilefromdb.Split(@"/").LastOrDefault();
            if (fileName == null || Defaultimages(fileName))
                return;

            var weblocation = $"{webrootpath}{oldfilefromdb}";
            var old = Path.Combine(weblocation);
            if (File.Exists(old))
                File.Delete(old);
        }




        public static Boolean Defaultimages(string? fileName)
        {
            string? Default = DefaultImage.Split(@"/").LastOrDefault();
            return fileName?.Trim().ToLower() == Default?.Trim().ToLower();
        }
    }
}

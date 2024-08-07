namespace ShopApp.Extensions

{
    public static class Extensions
    {
        public static async Task<string> SaveFile(this IFormFile file)
        {
            string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);
            using FileStream fileStream = new(path, FileMode.Create);
            await file.CopyToAsync(fileStream);
            return fileName;
        }
    }
}

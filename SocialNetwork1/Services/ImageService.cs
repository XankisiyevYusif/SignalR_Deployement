
namespace SocialNetwork1.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ImageService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> SaveFileAsync(IFormFile file)
        {
            var saveImg = Path.Combine(_webHostEnvironment.WebRootPath, "images", file.FileName);
            using (var img=new FileStream(saveImg,FileMode.OpenOrCreate))
            {
                await file.CopyToAsync(img);
            }
            return file.FileName.ToString();
        }
    }
}

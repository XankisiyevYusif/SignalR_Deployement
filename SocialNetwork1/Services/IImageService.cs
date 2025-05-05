namespace SocialNetwork1.Services
{
    public interface IImageService
    {
        Task<string> SaveFileAsync(IFormFile file);
    }
}

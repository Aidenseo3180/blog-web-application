using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Blog.Web.Repositories
{
    public class CloudinaryImageRepository : IImageRepository
    {
        private readonly IConfiguration configuration;
        private readonly Account account;   //to save account

        //constructor injection
        //inject IConfiguration to use Cloudinary (in appsettings.json)
        public CloudinaryImageRepository(IConfiguration configuration)
        {
            this.configuration = configuration;

            //initialize cloudinary account
            //Get Cloudinary section from appsetting
            //Account takes CloudName, ApiKey, ApiSecret
            account = new Account(
                configuration.GetSection("Cloudinary")["CloudName"],
                configuration.GetSection("Cloudinary")["ApiKey"],
                configuration.GetSection("Cloudinary")["ApiSecret"]
                );
        }

        //implementation of cloudinary
        public async Task<string> UploadAsync(IFormFile file)
        {
            //use account to create client
            var client = new Cloudinary(account);

            // Upload
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()), //takes name and stream
                DisplayName = file.FileName
            };

            //call api. Cloudinary take name and stream
            var uploadResult = await client.UploadAsync(uploadParams);

            //Check if not null and return 200 success response
            if (uploadResult != null && uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //return Uri 
                return uploadResult.SecureUri.ToString();
            }

            return null;
        }
    }
}

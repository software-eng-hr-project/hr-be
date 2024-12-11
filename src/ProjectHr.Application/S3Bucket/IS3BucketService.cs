using System.Threading.Tasks;

namespace ProjectHr.S3Bucket;

public interface IS3BucketService
{
    Task<string> UploadPhotoFromBase64Async(string base64String, string objectKey);
}
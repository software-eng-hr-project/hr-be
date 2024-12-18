using System.Threading.Tasks;
using Abp.Dependency;

namespace ProjectHr.S3Bucket;

public interface IS3BucketService :ITransientDependency
{
    Task<string> UploadPhotoFromBase64Async(string base64String, string objectKey);
}
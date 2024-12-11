using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using ProjectHr.Authorization.Users;
using ProjectHr.S3Options;

namespace ProjectHr.S3Bucket;

public class S3BucketService : IS3BucketService
{
    private readonly string BucketName = "";
    private readonly string BaseUrl = "";
    private readonly IAmazonS3 _s3Client;
    private readonly IRepository<User, long> _userRepository;

    public S3BucketService(IOptions<S3Settings> options, IRepository<User, long> userRepository)
    {
        BucketName = options.Value.BucketName;
        BaseUrl = options.Value.BaseUrl;
        _s3Client = new AmazonS3Client(
            options.Value.AccessKeyId,
            options.Value.SecretKey,
            RegionEndpoint.GetBySystemName(options.Value.Region));
        _userRepository = userRepository;
    }

    public async Task<string> UploadPhotoFromBase64Async(string base64String, string objectKey)
    {
        try
        {
            var data = Convert.FromBase64String(base64String);

            using MemoryStream stream = new MemoryStream(data);
            var request = new PutObjectRequest
            {
                BucketName = BucketName,
                Key = objectKey,
                InputStream = stream,
                ContentType = "image/jpeg" // Set the appropriate content type
            };

            var response = await _s3Client.PutObjectAsync(request);

            return response.HttpStatusCode == HttpStatusCode.OK ? $"Successfully uploaded photo to S3: {objectKey}" : $"Failed to upload photo to S3: {objectKey}";
        }
        catch (Exception ex)
        {
            return $"Error uploading photo to S3: {ex.Message}";
        }
    }
}
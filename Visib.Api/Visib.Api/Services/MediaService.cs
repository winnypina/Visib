using System;
using System.IO;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Hosting;

namespace Visib.Api.Services
{
    public class MediaService : IMediaService
    {
        private IHostingEnvironment _hostingEnvironment;
        private AmazonS3Client _s3Client = new AmazonS3Client(RegionEndpoint.USEast1);
        private string _bucketName = "visib";//this is my Amazon Bucket name
        private static string _bucketSubdirectory = String.Empty;

        public MediaService(IHostingEnvironment environment)
        {
            _hostingEnvironment = environment;
        }

        public async Task UploadAsync(string fileName, MemoryStream file)
        {
            try
            {
                TransferUtility fileTransferUtility = new
                    TransferUtility(new AmazonS3Client("AKIAJVFAJSKBRJSGGUSA", "On7uL/X9SOSifrL3+aI70nDyilJOkuK7mrgJIXji", Amazon.RegionEndpoint.USEast1));

                string bucketName;


                if (string.IsNullOrEmpty(_bucketSubdirectory))
                {
                    bucketName = _bucketName; //no subdirectory just bucket name  
                }
                else
                {   // subdirectory and bucket name  
                    bucketName = _bucketName + @"/" + _bucketSubdirectory;
                }

                var request = new TransferUtilityUploadRequest
                {
                    InputStream = file,
                    BucketName = bucketName,
                    Key = fileName,
                    CannedACL = S3CannedACL.PublicRead
                };

                // 1. Upload a file, file name is used as the object key name.
                await fileTransferUtility.UploadAsync(request);


            }
            catch (AmazonS3Exception s3Exception)
            {
                Console.WriteLine(s3Exception.Message,
                    s3Exception.InnerException);
            }
        }
    }
}
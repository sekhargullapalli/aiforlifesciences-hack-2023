using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
namespace S3TestApp;

class Program
{
    public static async Task Main(string[] args)
    {
        var unsigned = new AnonymousAWSCredentials();
        var client = new AmazonS3Client(unsigned, Amazon.RegionEndpoint.USEast1);
        var bucketName = "ai-for-life-sciences-1";

        var listRequest = new ListObjectsRequest
        {
            BucketName = bucketName,
            Delimiter = "/",
        };
        var sublistRequest = new ListObjectsRequest
        {
            BucketName = bucketName,
            Delimiter = "/",
        };
        ListObjectsResponse listResponse;
        int count = 0;
        using TextWriter textWriter = new StreamWriter($"{bucketName}-contents.txt");

        do
        {
            listResponse = await client.ListObjectsAsync(listRequest);
            foreach (var obj in listResponse.CommonPrefixes)
            {
                sublistRequest.Prefix = obj;
                var sublistResponse = await client.ListObjectsAsync(sublistRequest);
                foreach (var subobj in sublistResponse.S3Objects)
                {
                    textWriter.WriteLine(subobj.Key);
                    Console.WriteLine((++count) + " {0} {1}", subobj.Size, subobj.Key);
                }
            }
            foreach (var obj in listResponse.S3Objects)
            {
                textWriter.WriteLine(obj.Key);
                Console.WriteLine((++count) + " {0} {1}", obj.Size, obj.Key);
            }
            listRequest.Marker = listResponse.NextMarker;
        } while (listResponse.IsTruncated);

    }
}
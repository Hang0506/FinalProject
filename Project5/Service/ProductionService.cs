using Abp.UI;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using DynamoStudentManager.Models;
using Project5.Model;
using Project5.Service;

namespace Project5.Service
{
    public class Productionervice : IProductionService
    {
        private readonly IDynamoDBContext _context;
        //private readonly IAmazonS3 _s3Client;
        public IConfiguration Configuration { get; }

        public Productionervice(IDynamoDBContext context, IConfiguration configuration)
        {
            _context = context;
            //_s3Client = s3;
            Configuration = configuration;
        }
        /// <summary>
        /// get thông tin của sản phẩm
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        public async Task<IEnumerable<Production>> GetProduction(string Id)
        {
            try
            {
                var credentials = new BasicAWSCredentials("AKIAVQLZ5C7SIAGBLUGE", "gCyYGbR7sE139GRlgtwoKIU1TJ7zlTCnuE0b8YuG");
                var client = new AmazonDynamoDBClient(credentials, RegionEndpoint.USEast1);
                var context = new DynamoDBContext(client);
                //var tableResponse = await client.ListTablesAsync();
                //if (!tableResponse.TableNames.Contains("Production"))
                //{
                //    await client.CreateTableAsync(new CreateTableRequest
                //    {
                //        TableName = "Production",
                //        ProvisionedThroughput = new ProvisionedThroughput
                //        {
                //            ReadCapacityUnits = 10,
                //            WriteCapacityUnits = 10
                //        },
                //        KeySchema = new List<KeySchemaElement>
                //            {
                //                new KeySchemaElement
                //                {
                //                    AttributeName = "Id",
                //                    KeyType = KeyType.HASH
                //                }
                //            },
                //        AttributeDefinitions = new List<AttributeDefinition>
                //            {
                //                new AttributeDefinition {
                //                    AttributeName = "Id",
                //                    AttributeType =ScalarAttributeType.S
                //                }
                //            }
                //    });
                //}


                return await context.QueryAsync<Production>(Id).GetRemainingAsync();
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);

            }

        }
        /// <summary>
        /// insert thông tin sản phẩm
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        public async Task<bool> Insert(Production request)
        {
            try
            {
                var credentials = new BasicAWSCredentials("AKIAVQLZ5C7SIAGBLUGE", "gCyYGbR7sE139GRlgtwoKIU1TJ7zlTCnuE0b8YuG");
                var client = new AmazonDynamoDBClient(credentials, RegionEndpoint.USEast1);
                var context = new DynamoDBContext(client);
                Production pro = await context.LoadAsync<Production>(request.Id, request.Name);
                if (pro == null)
                {

                    await context.SaveAsync(request);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// Update sản phẩm
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<bool> Update(ProductionDto request)
        {
            try
            {
                var credentials = new BasicAWSCredentials("AKIAVQLZ5C7SIAGBLUGE", "gCyYGbR7sE139GRlgtwoKIU1TJ7zlTCnuE0b8YuG");
                var client = new AmazonDynamoDBClient(credentials, RegionEndpoint.USEast1);
                var context = new DynamoDBContext(client);
                Production pro = await context.LoadAsync<Production>(request.Id, request.Name);
                pro.Name = request.Name;
                pro.Price = request.Price == 0 ? pro.Price : request.Price;
                pro.Image = request.Image;
                await context.SaveAsync(pro);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        /// <summary>
        /// Xóa sản phẩm
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<bool> Delete(DeteleDto request)
        {
            try
            {
                var credentials = new BasicAWSCredentials("AKIAVQLZ5C7SIAGBLUGE", "gCyYGbR7sE139GRlgtwoKIU1TJ7zlTCnuE0b8YuG");
                var client = new AmazonDynamoDBClient(credentials, RegionEndpoint.USEast1);
                var context = new DynamoDBContext(client);
                Production pro = await context.LoadAsync<Production>(request.Id, request.Name);
                await context.DeleteAsync(pro);
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }
        /// <summary>
        /// get all sản phẩm
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<Production>> GetAll()
        {
            try
            {
                var credentials = new BasicAWSCredentials("AKIAVQLZ5C7SIAGBLUGE", "gCyYGbR7sE139GRlgtwoKIU1TJ7zlTCnuE0b8YuG");
                var client = new AmazonDynamoDBClient(credentials, RegionEndpoint.USEast1);
                var context = new DynamoDBContext(client);
                var conditions = new List<ScanCondition>();
                // you can add scan conditions, or leave empty
                return await context.ScanAsync<Production>(conditions).GetRemainingAsync();
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);

            }
        }
        /// <summary>
        /// upload image on S3
        /// </summary>
        /// <param name="url"></param>
        /// <param name="keyName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<string> UploadFile(string url, string keyName)
        {

            try
            {
                var credentials = new BasicAWSCredentials("AKIAVQLZ5C7SGH2GFTOQ", "NcW1kf7hfzC+So4h9r3zFfggiedoQ4pFiBXvK6Ly");
                var client = new AmazonS3Client(credentials, RegionEndpoint.USEast1);

                //var bucketExists = await _s3Client.DoesS3BucketExistAsync(Configuration["AWS:BucketName"]);
                PutObjectRequest putRequest = new PutObjectRequest
                {
                    FilePath = url,
                    Key = keyName,
                    BucketName = Configuration["AWS:BucketName"],
                    ContentType = "image/png"
                };

                PutObjectResponse response = await client.PutObjectAsync(putRequest);
                GetPreSignedUrlRequest request = new GetPreSignedUrlRequest();
                request.BucketName = Configuration["AWS:BucketName"];
                request.Key = keyName;
                request.Expires = DateTime.Now.AddYears(1);
                request.Protocol = Protocol.HTTP;
                string urlFinal = client.GetPreSignedURL(request);

                return urlFinal;
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId")
                    ||
                    amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    throw new Exception("Check the provided AWS Credentials.");
                }
                else
                {
                    throw new Exception("Error occurred: " + amazonS3Exception.Message);
                }
            }
        }
    }
}

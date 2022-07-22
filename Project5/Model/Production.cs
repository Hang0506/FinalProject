using Amazon.DynamoDBv2.DataModel;
namespace DynamoStudentManager.Models
{
    [DynamoDBTable("Production")]
    public class Production
    {
        [DynamoDBHashKey("Id")]
        public string Id { get; set; }
        [DynamoDBRangeKey("Name")]
        public string? Name { get; set; }
        [DynamoDBProperty("Image")]
        public string? Image { get; set; }
        [DynamoDBProperty("Price")]
        public int Price { get; set; }
    }
}
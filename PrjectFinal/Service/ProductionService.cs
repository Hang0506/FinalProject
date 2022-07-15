using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using DynamoStudentManager.Models;
using PrjectFinal.Model;

namespace PrjectFinal.Service
{
    public class ProductionService : IProductionService
    {
        private readonly IDynamoDBContext _context;

        public ProductionService(IDynamoDBContext context)
        {
            _context = context;
        }


        public async Task<object> GetProductions(int? id)
        {
            try
            {
                var pro = await _context.LoadAsync<Production>(id);
                return pro;
            }
            catch (Exception ex)
            {

                throw new UserFre
            }
           
        }

    }
}

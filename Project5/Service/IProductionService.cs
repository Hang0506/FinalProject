using DynamoStudentManager.Models;
using Project5.Model;

namespace Project5.Service
{
    public interface IProductionService
    {
        Task<IEnumerable<Production>> GetProduction(string Id);
        Task<bool> Insert(Production request);
        Task<bool> Update(ProductionDto request);
        Task<bool> Delete(DeteleDto request);
        Task<IEnumerable<Production>> GetAll();
        Task<string> UploadFile(IFormFile myfile);
    }
}

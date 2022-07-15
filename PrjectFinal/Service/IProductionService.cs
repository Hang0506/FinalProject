using PrjectFinal.Model;

namespace PrjectFinal.Service
{
    public interface IProductionService
    {
        Task<object> GetProductions(int? id);
    }

}

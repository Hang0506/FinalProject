using Microsoft.AspNetCore.Mvc;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using PrjectFinal.Model;
using PrjectFinal.Service;

namespace PrjectFinal.Controllers;

[Route("api/[controller]")]
public class ProductionController : ControllerBase
{
    private readonly IProductionService _productionService;
    public ProductionController(IProductionService productionService)
    {
        _productionService = productionService;
    }
    // GET api/production
    [HttpGet("{id}")]
    public async Task<object> GetProdution(int id)
    {
        return await _productionService.GetProductions(id);
    }
}
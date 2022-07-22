using DynamoStudentManager.Models;
using Microsoft.AspNetCore.Mvc;
using Project5.Model;
using Project5.Service;

namespace Project5.Controllers;

[Route("api/[controller]")]
public class ProductionController : ControllerBase
{
    private readonly IProductionService _productionService;
    public ProductionController(IProductionService productionService)
    {
        _productionService = productionService;
    }
    //// GET api/production/5
    [HttpGet("{id}")]
    public async Task<IEnumerable<Production>> Get(string Id)
    {
        return await _productionService.GetProduction(Id);
    }
    [HttpPost]
    public async Task<bool> Insert([FromBody] Production request)
    {
        return await _productionService.Insert(request);
    }
    [HttpPut]
    public async Task<bool> Update([FromBody] ProductionDto request)
    {
        return await _productionService.Update(request);
    }

    // DELETE api/production/5
    [HttpDelete]
    public async Task<bool> Delete([FromQuery] DeteleDto request)
    {
        return await _productionService.Delete(request);
    }
    [HttpGet]
    //get all sản phẩm 
    public async Task<IEnumerable<Production>> GetAll()
    {
        return await _productionService.GetAll();
    }
    [HttpPost("UploadFiles")]
    public async Task<IActionResult> Post(List<IFormFile> files, string Id, string Name)
    {
        try
        {
            long size = files.Sum(f => f.Length);

            // full path to file in temp location
            var filePath = Path.GetTempFileName();

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            // process uploaded files
            // Don't rely on or trust the FileName property without validation.
            var filename = Path.GetFileName(filePath) + ".png";
            var urlS3 = await _productionService.UploadFile(filePath, filename);
            await _productionService.Update(new ProductionDto { Id = Id, Name = Name, Image = urlS3, Price = 0 });
            return Ok(new { count = files.Count, size, urlS3 });
        }
        catch (Exception)
        {

            throw;
        }
    }
}
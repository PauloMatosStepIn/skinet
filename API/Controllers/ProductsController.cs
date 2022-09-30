using Infrastructure.Data;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;

namespace API.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class ProductsController : ControllerBase
  {
    private readonly IProductRepository _repo;
    public ProductsController(IProductRepository repo)
    {
      _repo = repo;

    }

    [HttpGet]
    public async Task<ActionResult<List<Product>>> getProducts()
    {
      // var products = await _context.Products.ToListAsync();
      var products = await _repo.GetProductsAsync();

      return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> getProduct(int id)
    {
      // return await _context.Products.FindAsync(id);
      return await _repo.GetProductByIdAsync(id);
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<ProductBrand>>> getProductBrands()
    {
      var productBrands = await _repo.GetProductBrandsAsync();

      return Ok(productBrands);
    }
    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<ProductType>>> getProductTypes()
    {
      var productTypes = await _repo.GetProductTypesAsync();

      return Ok(productTypes);
    }

  }
}
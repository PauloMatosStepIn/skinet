using Infrastructure.Data;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;
using Core.Specifications;
using AutoMapper;
using API.Dtos;

namespace API.Controllers
{
  public class ProductsController : BaseApiController
  {

    // private readonly IProductRepository _repo;
    // public ProductsController(IProductRepository repo)
    // {
    //   _repo = repo;
    // }

    private readonly IGenericRepository<Product> _productsRepo;
    private readonly IGenericRepository<ProductBrand> _productBrandsRepo;
    private readonly IGenericRepository<ProductType> _productTypesRepo;
    private readonly IMapper _mapper;

    public ProductsController(IGenericRepository<Product> productsRepo,
                              IGenericRepository<ProductBrand> productBrandsRepo,
                              IGenericRepository<ProductType> productTypesRepo,
                              IMapper mapper)
    {
      _productsRepo = productsRepo;
      _productBrandsRepo = productBrandsRepo;
      _productTypesRepo = productTypesRepo;
      _mapper = mapper;
    }
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> getProducts()
    {
      // var products = await _context.Products.ToListAsync();
      // var products = await _repo.GetProductsAsync();
      // var products = await _productsRepo.ListAllAsync();

      var spec = new ProductsWithTypesAndBrandsSpecification();

      var products = await _productsRepo.ListAsync(spec);

      return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products));

      // return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductToReturnDto>> getProduct(int id)
    {
      // return await _context.Products.FindAsync(id);
      // return await _repo.GetProductByIdAsync(id);
      // return await _productsRepo.GetByIdAsync(id);

      var spec = new ProductsWithTypesAndBrandsSpecification(id);

      var product = await _productsRepo.GetEntityWithSpec(spec);

      return _mapper.Map<Product, ProductToReturnDto>(product);
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<ProductBrand>>> getProductBrands()
    {
      // var productBrands = await _repo.GetProductBrandsAsync();
      var productBrands = await _productBrandsRepo.ListAllAsync();

      return Ok(productBrands);
    }
    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<ProductType>>> getProductTypes()
    {
      // var productTypes = await _repo.GetProductTypesAsync();
      var productTypes = await _productTypesRepo.ListAllAsync();

      return Ok(productTypes);
    }

  }
}
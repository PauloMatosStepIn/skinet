using Infrastructure.Data;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;
using Core.Specifications;
using AutoMapper;
using API.Dtos;
using API.Helpers;
using API.Errors;

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

    [Cached(600)]
    [HttpGet]
    public async Task<ActionResult<Pagination<ProductToReturnDto>>> getProducts(
      //string sort, int? brandId, int? typeId
      [FromQuery] ProductsSpecParams productParams //params is reserved word
      )
    {
      // var products = await _context.Products.ToListAsync();
      // var products = await _repo.GetProductsAsync();
      // var products = await _productsRepo.ListAllAsync();

      // var spec = new ProductsWithTypesAndBrandsSpecification(sort, brandId, typeId);
      var spec = new ProductsWithTypesAndBrandsSpecification(productParams);

      var countSpec = new ProductWithFiltersForCountSpecification(productParams);

      var totalItems = await _productsRepo.CountAsync(countSpec);

      var products = await _productsRepo.ListAsync(spec);

      // return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products));

      var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

      return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex, productParams.PageSize, totalItems, data));

      // return Ok(products);
    }

    [Cached(600)]
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductToReturnDto>> getProduct(int id)
    {
      // return await _context.Products.FindAsync(id);
      // return await _repo.GetProductByIdAsync(id);
      // return await _productsRepo.GetByIdAsync(id);

      var spec = new ProductsWithTypesAndBrandsSpecification(id);

      var product = await _productsRepo.GetEntityWithSpec(spec);

      if (product == null) return NotFound(new ApiResponse(404));

      return _mapper.Map<Product, ProductToReturnDto>(product);
    }

    [Cached(600)]
    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<ProductBrand>>> getProductBrands()
    {
      // var productBrands = await _repo.GetProductBrandsAsync();
      var productBrands = await _productBrandsRepo.ListAllAsync();

      return Ok(productBrands);
    }

    [Cached(600)]
    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<ProductType>>> getProductTypes()
    {
      // var productTypes = await _repo.GetProductTypesAsync();
      var productTypes = await _productTypesRepo.ListAllAsync();

      return Ok(productTypes);
    }

  }
}
using System.Text.Json;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data
{
  public class StoreContextSeed
  {
    public static async Task SeedAsync(StoreContext context, ILoggerFactory loggerFactory)
    {
      try
      {
        if (!context.ProductBrands.Any())
        {
          var brandsData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/brands.json");

          var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

          foreach (var brand in brands)
          {
            await context.ProductBrands.AddAsync(brand);
          }

          await context.SaveChangesAsync();
        }
        if (!context.ProductTypes.Any())
        {
          var typesData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/types.json");

          var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);

          foreach (var type in types)
          {
            await context.ProductTypes.AddAsync(type);
          }

          await context.SaveChangesAsync();
        }
        if (!context.Products.Any())
        {
          var productData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/products.json");

          var products = JsonSerializer.Deserialize<List<Product>>(productData);

          foreach (var product in products)
          {
            await context.Products.AddAsync(product);
          }

          await context.SaveChangesAsync();
        }

        if (!context.DeliveryMethods.Any())
        {
          var deliveryData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/delivery.json");

          var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryData);

          foreach (var method in deliveryMethods)
          {
            await context.DeliveryMethods.AddAsync(method);
          }

          await context.SaveChangesAsync();
        }



      }
      catch (Exception ex)
      {
        var logger = loggerFactory.CreateLogger<StoreContextSeed>();
        logger.LogError(ex.Message);
      }
    }
  }
}
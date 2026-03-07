using Store.Core.Entities;
using Store.Core.Entities.Order;
using Store.Repository.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Store.Repository.Data
{
    public class StoreDbContextSeed
    {
        public async static Task SeedAsync(StoreDbContext _context) {
            // Brands
            if (_context.Brands.Count() == 0) {
                var brandsData = File.ReadAllText(@"..\Store.Repository\Data\SeedData\brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                if (brands is not null && brands.Count() > 0)
                {
                    await _context.Brands.AddRangeAsync(brands);
                    await _context.SaveChangesAsync();
                }
            }
            // Types
            if (_context.Types.Count() == 0) {
                var typesData = File.ReadAllText(@"..\Store.Repository\Data\SeedData\types.json");
                var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                if (types is not null && types.Count() > 0)
                {
                    await _context.Types.AddRangeAsync(types);
                    await _context.SaveChangesAsync();
                }
            }
            // Products
            if (_context.Products.Count() == 0) {
                var productsData = File.ReadAllText(@"..\Store.Repository\Data\SeedData\products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                if (products is not null && products.Count() > 0)
                {
                    await _context.Products.AddRangeAsync(products);
                    await _context.SaveChangesAsync();
                }
            }
            // Delivery Methods
            if (_context.DeliveryMethods.Count() == 0) {
                var DeliveryData = File.ReadAllText(@"..\Store.Repository\Data\SeedData\delivery.json");
                var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryData);
                if (deliveryMethods is not null && deliveryMethods.Count() > 0)
                {
                    await _context.DeliveryMethods.AddRangeAsync(deliveryMethods);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}

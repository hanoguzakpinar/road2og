using System;
using System.Text.Json;
using RedisExample.API.Models;
using RedisExample.Cache;
using StackExchange.Redis;

namespace RedisExample.API.Repositories;

public class ProductRepositoryWithCacheDecorator : IProductRepository
{
    private const string productKey = "productCaches";
    private readonly IProductRepository _repository;
    private readonly RedisService _redis;
    private readonly IDatabase _db;

    public ProductRepositoryWithCacheDecorator(IProductRepository repository, RedisService redis)
    {
        _repository = repository;
        _redis = redis;
        _db = _redis.GetDb(2);
    }

    public async Task<Product> CreateAsync(Product product)
    {
        var nProduct = await _repository.CreateAsync(product);

        if (await _db.KeyExistsAsync(productKey))
        {
            await _db.HashSetAsync(productKey, nProduct.Id, JsonSerializer.Serialize(product));
        }
        return nProduct;
    }

    public async Task<List<Product>> GetAllAsync()
    {
        if (!await _db.KeyExistsAsync(productKey))
            return await Load2CacheFromDbAsync();

        var products = new List<Product>();

        var cachedProducts = await _db.HashGetAllAsync(productKey);
        foreach (var item in cachedProducts.ToList())
        {
            var product = JsonSerializer.Deserialize<Product>(item.Value.ToString());

            products.Add(product);
        }
        return products;
    }

    public async Task<Product> GetByIdAsync(int id)
    {
        if (await _db.KeyExistsAsync(productKey))
        {
            var product = await _db.HashGetAsync(productKey, id);
            return product.HasValue ? JsonSerializer.Deserialize<Product>(product.ToString()) : null;
        }

        var products = await Load2CacheFromDbAsync();
        return products.FirstOrDefault(p => p.Id == id);
    }

    private async Task<List<Product>> Load2CacheFromDbAsync()
    {
        var products = await _repository.GetAllAsync();

        products.ForEach(async p =>
        {
            await _db.HashSetAsync(productKey, p.Id, JsonSerializer.Serialize(p));
        });

        return products;
    }
}

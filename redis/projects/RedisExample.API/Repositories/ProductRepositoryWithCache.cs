using System;
using RedisExample.API.Models;
using RedisExample.Cache;

namespace RedisExample.API.Repositories;

public class ProductRepositoryWithCache : IProductRepository
{
    private readonly IProductRepository _repository;
    private readonly RedisService _redis;

    public ProductRepositoryWithCache(IProductRepository repository, RedisService redis)
    {
        _repository = repository;
        _redis = redis;
    }

    public Task<Product> CreateAsync(Product product)
    {
        throw new NotImplementedException();
    }

    public Task<List<Product>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Product> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
}

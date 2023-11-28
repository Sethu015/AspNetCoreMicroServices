using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;
using System.Xml.Linq;

namespace Catalog.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _catalogContext;

        public ProductRepository(ICatalogContext catalogContext)
        {
            _catalogContext = catalogContext;
        }

        public async Task CreateProduct(Product product)
        {
            await _catalogContext.Products.InsertOneAsync(product);
        }

        public async Task<bool> DeleteProduct(string id)
        {
            FilterDefinition<Product> filterDefinition = Builders<Product>.Filter.Eq(f => f.Id, id);
            var deletedResult = await _catalogContext.Products.DeleteOneAsync(filterDefinition);
            return deletedResult.IsAcknowledged && deletedResult.DeletedCount > 0;
        }

        public async Task<IEnumerable<Product>> GetProductByCategoryName(string categoryName)
        {
            FilterDefinition<Product> filterDefinition = Builders<Product>.Filter.Eq(f => f.Category, categoryName);
            return await _catalogContext.Products.Find(filterDefinition).ToListAsync();
        }

        public async Task<Product> GetProductById(string id)
        {
            return await _catalogContext.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            FilterDefinition<Product> filterDefinition = Builders<Product>.Filter.Eq(f => f.Name, name);
            return await _catalogContext.Products.Find(filterDefinition).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _catalogContext.Products.Find(p=>true).ToListAsync();
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var updatedResult = await _catalogContext.Products.ReplaceOneAsync(filter: g=>g.Id == product.Id,replacement: product);
            return updatedResult.IsAcknowledged && updatedResult.ModifiedCount > 0;
        }
    }
}

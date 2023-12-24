using Dapper;
using Discount.API.Entities;
using Npgsql;

namespace Discount.API.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _configuration;

        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected = await connection.ExecuteAsync
                ("INSERT INTO COUPON (PRODUCTNAME, DESCRIPTION, AMOUNT) VALUES(@ProductName, @Description, @Amount)",
                new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount });

            if (affected == 0)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var coupon = await connection.ExecuteAsync
                ("DELETE FROM COUPON WHERE PRODUCTNAME = @ProductName", new { ProductName = productName });
            if (coupon == 0)
            {
                return false;
            }
            return true;
        }

        public async Task<Coupon> GetDiscount(string productName)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>
                ("SELECT * FROM COUPON WHERE PRODUCTNAME = @ProductName", new { ProductName = productName });
            if (coupon == null)
            {
                return new Coupon{ Amount = 0, ProductName = "No Discount", Description = "No Discount Desc" };
            }
            return coupon;

        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var affected = await connection.ExecuteAsync
                ("UPDATE COUPON SET PRODUCTNAME = @ProductName, DESCRIPTION = @Description, AMOUNT = @Amount WHERE ID = @Id",
                new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount, Id = coupon.Id });

            if (affected == 0)
            {
                return false;
            }
            return true;
        }
    }
}

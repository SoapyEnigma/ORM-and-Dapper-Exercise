using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM_Dapper
{
    public class DapperProductRepository : IProductRepository
    {
        private readonly IDbConnection _conn;

        public DapperProductRepository(IDbConnection conn)
        {
            _conn = conn;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _conn.Query<Product>("SELECT * FROM products");
        }

        public Product GetProduct(int id)
        {
            return _conn.QuerySingle<Product>("SELECT * FROM products WHERE ProductID = @ID;",
                new { id = id });
        }

        public void UpdateProduct(Product product)
        {
            _conn.Execute(
                  "UPDATE products"
                + "SET"
                + "Name = @name,"
                + "Price = @price,"
                + "CategoryID = @categoryID,"
                + "OnSale = @onSale,"
                + "StockLevel = @stock"
                + "WHERE ProductId = @id;",
                  new
                  {
                      id = product.ProductID,
                      name = product.Name,
                      price = product.Price,
                      categoryID = product.CategoryId,
                      onSale = product.OnSale,
                      stock = product.StockLevel
                  });
        }

        public void InsertProduct(int id, string name, double price, int categoryID)
        {
            _conn.Execute("INSERT INTO products (ProductID, Name, Price, CategoryID) "
                + "VALUES (@productID, @name, @price, @categoryID);",
            new { productID = id, name = name, price = price, categoryID = categoryID});
        }

        public void DeleteProduct(int id)
        {
            _conn.Execute("DELETE FROM sales WHERE ProductId = @id;",
                new { id = id });

            _conn.Execute("DELETE FROM reviews WHERE ProductId = @id;",
                new { id = id });

            _conn.Execute("DELETE FROM products WHERE ProductId = @id;",
                new { id = id });
        }
    }
}

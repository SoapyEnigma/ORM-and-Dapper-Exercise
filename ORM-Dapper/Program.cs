using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;

namespace ORM_Dapper
{
    public class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            string connString = config.GetConnectionString("DefaultConnection");

            IDbConnection conn = new MySqlConnection(connString);

            #region department
            var departmentRepo = new DapperDepartmentRepository(conn);

            departmentRepo.InsertDepartment("Clothing");

            var departments = departmentRepo.GetAllDepartments();

            foreach (var department in departments)
            {
                Console.WriteLine(department.DepartmentID);
                Console.WriteLine(department.Name + "\n\n");
            }
            #endregion

            #region product
            var productRepo = new DapperProductRepository(conn);

            var productToUpdate = productRepo.GetProduct(886);

            productToUpdate.Name = "UPDATED!";
            productToUpdate.Price = 12.99m;
            productToUpdate.CategoryId = 1;
            productToUpdate.OnSale = false;
            productToUpdate.StockLevel = 1000;

            productRepo.InsertProduct(999, "New Product!", 9.99, 1);
            productRepo.InsertProduct(1000, "Delete this Product!", 9.99, 1);

            productRepo.DeleteProduct(1000);
            productRepo.DeleteProduct(58); //Celine Dion: A New Day Has Come

            var products = productRepo.GetAllProducts();

            foreach (var product in products)
            {
                Console.WriteLine(product.ProductID);
                Console.WriteLine(product.Name);
                Console.WriteLine(product.Price);
                Console.WriteLine(product.CategoryId);
                Console.WriteLine(product.OnSale);
                Console.WriteLine(product.StockLevel);
                Console.WriteLine();
            }
            #endregion
        }
    }
}

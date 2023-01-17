using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StoreFunctions;

namespace ProductFunctions
{
  internal class ProductService
  {

    public async Task<bool> DeleteProduct(int productId)
    {
      try
      {
        using (var dataBase = new TrainingAlinContext())
        {
          Product productToRemove = dataBase.Products.Where(prod => prod.Id == productId).First();
          dataBase.Products.Remove(productToRemove);
          await dataBase.SaveChangesAsync();
          return true;
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        return false;
      }

    }

    public int getProductCount(string filter)
    {
      int dbCount = 0;
      try
      {
        using (var dataBase = new TrainingAlinContext())
        {
          if (filter == null)
          {
            dbCount = dataBase.Products.Count();
          }
          else
          {
            dbCount = dataBase.Products.Where(product => product.Name.Contains(filter)).Count();
          }
          return dbCount;
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        return -1;
      }

    }

    public List<Product> GetNewProducts(int pageNumber, int pageSize, string orderBy, string order, string filter)
    {
      List<Product> newProducts = new();
      try
      {
        using (var dataBase = new TrainingAlinContext())
        {
          var fitleredProducts = string.IsNullOrEmpty(filter) ? dataBase.Products :
              dataBase.Products.Where(product => product.Name.Contains(filter));
          int dbCount = fitleredProducts.ToList().Count;
          int skip = pageNumber * pageSize;
          int take = dbCount < (pageNumber + 1) * pageSize ? dbCount - pageNumber * pageSize : pageSize;
          switch (orderBy)
          {
            case "price":
              if (order == "ASC")
                newProducts = fitleredProducts.OrderBy(product => product.Price).Skip(skip).Take(take).ToList();
              else
                newProducts = fitleredProducts.OrderByDescending(product => product.Price).Skip(skip).Take(take).ToList();
              break;
            case "name":
              if (order == "ASC")
                newProducts = fitleredProducts.OrderBy(product => product.Name).Skip(skip).Take(take).ToList();
              else
                newProducts = fitleredProducts.OrderByDescending(product => product.Name).Skip(skip).Take(take).ToList();
              break;
            default:
              if (order == "ASC")
                newProducts = fitleredProducts.OrderBy(product => product.Id).Skip(skip).Take(take).ToList();
              else
                newProducts = fitleredProducts.OrderByDescending(product => product.Id).Skip(skip).Take(take).ToList();
              break;
          }
          return newProducts;
        }
      }
      catch (Exception ex)
      {

        Console.WriteLine(ex.Message);
        return newProducts;
      }
    }

    public List<Product> GetAllProducts()
    {

      List<Product> allProducts = new();
      try
      {
        using (var dataBase = new TrainingAlinContext())
        {
          allProducts = dataBase.Products.Select(prod => prod).ToList();
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
      return allProducts;
    }

    public async Task<bool> AddNewProduct(string data)
    {
      try
      {
        using (var dataBase = new TrainingAlinContext())
        {
          Product newProduct = JsonConvert.DeserializeObject<Product>(data);
          dataBase.Products.Add(newProduct);
          await dataBase.SaveChangesAsync();
          return true;
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        return false;
      }

    }

    public async Task<bool> UpdatePrice(int id, int newPrice)
    {
      try
      {
        using (var dataBase = new TrainingAlinContext())
        {
          Product wantedProduct = dataBase.Products.Where(product => product.Id == id).First();
          wantedProduct.Price = newPrice;
          await dataBase.SaveChangesAsync();
          return true;
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        return false;
      }
    }

  }
}


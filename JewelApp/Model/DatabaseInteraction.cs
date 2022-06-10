using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JewelApp.Model
{
    public static class DatabaseInteraction
    {

        private static DatabaseEntities _Entities;

        static DatabaseInteraction()
        {
            try
            {
                _Entities = new DatabaseEntities();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        public static User TryLoginUser(string login, string password)
        {

            return _Entities.User.SingleOrDefault(u => u.UserLogin == login && u.UserPassword == password);

        }

        public static List<Product> GetAllProducts()
        {

            return _Entities.Product.ToList();


        }

        public static List<string> GetAllManufacturers()
        {

            return _Entities.Product.Select(p => p.ProductManufacturer).Distinct().ToList();

        }

        public static Product GetProduct(int productId)
        {

            return _Entities.Product.SingleOrDefault(p => p.ProductId == productId);

        }

        public static List<ProductCategory> GetProductCategories()
        {

            return _Entities.ProductCategory.ToList();

        }

        public static void AddNewProduct(Product newProduct)
        {
            _Entities.Product.Add(newProduct);

            _Entities.SaveChanges();

        }

        public static void SaveEditedProduct(Product newProduct)
        {

            _Entities.SaveChanges();

        }

        public static bool TryDeleteProduct(Product selectedProduct)
        {

            var productOrders = selectedProduct.OrderProduct;

            if (productOrders.Count == 0)
            {
                _Entities.Product.Remove(selectedProduct);
                _Entities.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }


        }



    }
}

using JewelApp.Data;
using JewelApp.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace JewelApp.Windows
{
    /// <summary>
    /// Interaction logic for AddEditProduct.xaml
    /// </summary>
    public partial class AddEditProduct : Window
    {
        public Product SelectedProduct{ get; set; }


        private string MaterialImageName;

        public AddEditProduct()
        {
            InitializeComponent();


            ProductCategoryComboBox.ItemsSource = DatabaseInteraction.GetProductCategories();

        }

        public AddEditProduct(Product selectedProduct) : this()
        {
            

            TitleTextBlock.Text = "Редактирование товара";
            AddEditMaterialButton.Content = "Изменить товар";
            MaterialImageName = selectedProduct.ProductPhoto;
            IdTextBox.Visibility = Visibility.Visible;

            SelectedProduct = selectedProduct;
           
            DataContext = SelectedProduct;
        }
      
        private void PriceTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (InputValidation.ValidatePrice(e))
                e.Handled = true;
        }


        private void NumberTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            char character = (char)KeyInterop.VirtualKeyFromKey(e.Key);

            if (InputValidation.ValidateDigit(e))
                e.Handled = true;

        }

       


        

        private void ChangeImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Image (*.png);(*.jpg);(*.jpeg) | *.png;*.jpg;*.jpeg;";


            if (fileDialog.ShowDialog() == true)
            {
                string path = fileDialog.FileName;

                using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var bitmapFrame = BitmapFrame.Create(stream, BitmapCreateOptions.DelayCreation, BitmapCacheOption.None);
                    var width = bitmapFrame.PixelWidth;
                    var height = bitmapFrame.PixelHeight;

                    if(width > 300 || height > 200)
                    {
                        MessageBox.Show("Фото слишком большое", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                }

                MaterialImage.Source = new BitmapImage(new Uri(path, UriKind.Absolute));

                try
                {
                    File.Copy(path, Directory.GetCurrentDirectory() + $@"/product/{Path.GetFileName(path)}", true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                }

                if (MaterialImageName != null && MaterialImageName != "")
                {

                    File.Delete(Directory.GetCurrentDirectory() + $@"/product/{MaterialImageName}");

                }

                

                

                

                MaterialImageName = Path.GetFileName(path);

                
            }


        }

        private void AddEditMaterialButton_Click(object sender, RoutedEventArgs e)
        {

            string ErrorMessage = "";

            if (NameTextBox.Text.Equals(string.Empty))
                ErrorMessage += "Не введено имя товара \n";

            if (CountInStockTextBox.Text.Equals(string.Empty))
                ErrorMessage += "Не введено количество товара на складе \n";

            if (ProductUnitTextBox.Text.Equals(string.Empty))
                ErrorMessage += "Не введена единица измерения товара \n";

            if (SupplierTextBox.Text.Equals(string.Empty))
                ErrorMessage += "Не введен поставщик товара \n";

            if (PriceTextBox.Text.Equals(string.Empty))
                ErrorMessage += "Не введена стоимость товара \n";

            if (ProductCategoryComboBox.SelectedItem is  null)
                ErrorMessage += "Не выбрана категория товара \n";



            if (ErrorMessage.Length != 0)
            {
                MessageBox.Show("Ошибка: \n" + ErrorMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (SelectedProduct is null)
            {
                AddProduct();
            }
            else
            {
                EditProduct();
            }

        }


        private void EditProduct()
        {

            DatabaseInteraction.SaveEditedProduct(FillMaterial(SelectedProduct));

            MessageBox.Show("Товар успешно изменён", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);

            this.Close();

        }

        private void AddProduct()
        {

            Product NewProduct = FillMaterial(new Product());

            DatabaseInteraction.AddNewProduct(NewProduct);

            MessageBox.Show("Товар успешно добавлен", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);


            this.Close();

        }

        private Product FillMaterial(Product product)
        {

            product.ProductName = NameTextBox.Text;
            product.ProductCategoryId = ((ProductCategory)ProductCategoryComboBox.SelectedItem).ProductCategoryId;
            product.ProductQuantityInStock = Convert.ToInt32(CountInStockTextBox.Text);
            product.ProductUnit = ProductUnitTextBox.Text;
            product.ProductDescription = DescriptionTextBox.Text;
            product.SupplierName = SupplierTextBox.Text;
            product.ProductCost = Convert.ToDecimal(PriceTextBox.Text.Replace('.', ','));
            product.ProductPhoto = MaterialImageName;

            return product;
        }

        private void PriceTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            string AllowedSymbols = "1234567890,";


            if ((e.Text == "," && PriceTextBox.Text.Equals(string.Empty)) || (PriceTextBox.Text.Contains(',') && e.Text == ","))
                e.Handled = true;



            if (!e.Text.Any(t => AllowedSymbols.Contains(t)))
                e.Handled = true;


        }

        

    }
}

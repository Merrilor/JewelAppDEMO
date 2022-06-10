using JewelApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace JewelApp.Windows
{
    /// <summary>
    /// Interaction logic for ProductListWindow.xaml
    /// </summary>
    public partial class ProductListWindow : Window
    {

        public ObservableCollection<Product> ProductList { get; set; } = new ObservableCollection<Product>();

        private int TotalProducts = 0;

        public ProductListWindow()
        {
            InitializeComponent();

            var manufacturers = DatabaseInteraction.GetAllManufacturers();
            manufacturers.Insert(0, "Все производители");

            FilterComboBox.ItemsSource = manufacturers;

            UserFullNameTextBlock.Text = CurrentUser.FullName;

            ProductListView.ItemsSource = ProductList;

            if(CurrentUser.Role != "Администратор")
            {

                AddProductButton.Visibility = Visibility.Collapsed;

            }


        }

        private void AccountExitButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentUser.Role = "";
            CurrentUser.FullName = "";

            new LoginWindow().Show();

            this.Close();
        }

        private void FilterMaterials()
        {

            if (ProductListView is null)
                return;

            var products = DatabaseInteraction.GetAllProducts();

            TotalProducts = products.Count;

            var searchText = SearchTextBox.Text.ToLower();

            products = products.Where(m => m.ProductName.ToLower().Contains(searchText)
            || m.ProductDescription?.ToLower().Contains(searchText) == true 
            || m.ProductManufacturer?.ToLower().Contains(searchText) == true
            || m.ProductCost.ToString().ToLower().Contains(searchText)
            || m.ProductQuantityInStock.ToString().ToLower().Contains(searchText))
                .ToList();

            if (FilterComboBox.SelectedValue.ToString() != "Все производители")
                products = products.Where(m => m.ProductManufacturer == FilterComboBox.SelectedValue.ToString()).ToList();


            switch (SortComboBox.SelectedValue.ToString())
            {

                case "По возрастанию стоимости":
                    products = products.OrderBy(m => m.ProductCost).ToList();
                    break;
                case "По убыванию стоимости":
                    products = products.OrderByDescending(m => m.ProductCost).ToList();
                    break;               
                default:
                    products = products.OrderBy(m => m.ProductId).ToList();
                    break;


            }


            ProductList.Clear();

            foreach (var product in products)
            {
                ProductList.Add(product);
            }

            UpdateItemAmountText();
            NoItemsFoundTextBlock.Visibility = ProductList.Count == 0 ? Visibility.Visible : Visibility.Collapsed;


        }



        private void UpdateItemAmountText()
        {
            ItemsShownTextBlock.Text = $"Выведено {ProductList.Count} из {TotalProducts}";
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterMaterials();
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterMaterials();
        }

        private void FilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterMaterials();
        }

        private void MaterialListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (CurrentUser.Role != "Администратор")
            {

                return;

            }

            if (ProductListView.SelectedItems.Count > 0)
                DeleteProductButton.Visibility = Visibility.Visible;
            else
                DeleteProductButton.Visibility = Visibility.Collapsed;

        }        

        private void EditMaterialButton_Click(object sender, RoutedEventArgs e)
        {

            if (CurrentUser.Role != "Администратор")
            {

                MessageBox.Show("У вас нет возможности изменять товары", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;

            }

            var selectedMaterial = DatabaseInteraction.GetProduct((int)((sender as Button).Tag));

            new AddEditProduct(selectedMaterial).ShowDialog();

            FilterMaterials();
        }

        

        private void AddMaterialButton_Click(object sender, RoutedEventArgs e)
        {
            new AddEditProduct().ShowDialog();
            FilterMaterials();
        }

        private void DeleteProductButton_Click(object sender, RoutedEventArgs e)
        {

            var result = MessageBox.Show("Удалить товар?", "Вопрос", MessageBoxButton.YesNo, MessageBoxImage.Question);


            if (result == MessageBoxResult.Yes)
            {

                if (DatabaseInteraction.TryDeleteProduct((Product)ProductListView.SelectedItem))
                {

                    MessageBox.Show("Товар успешно удален", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                    FilterMaterials();
                }
                else
                {

                    MessageBox.Show("Этот товар используется в заказах", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}

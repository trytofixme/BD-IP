using System;
using System.Collections.Generic;
using System.Windows;
using GasStations.ViewModels;
using Npgsql;

namespace GasStations.Views.AddWindows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class SaleView : Window
    {
        private readonly UserViewModel _userVm;
        private readonly List<object> _itemsToAdd = new List<object>();
        public SaleView(UserViewModel userVm)
        {
            InitializeComponent();
            DataContext = this;
            _userVm = userVm;
        }
        
        private void AddButton_OnClick(object sender, RoutedEventArgs e)  
        {  
            if (!string.IsNullOrEmpty(OperationDateTextBox.Text) || !string.IsNullOrEmpty(ShopTextBox.Text) ||
                !string.IsNullOrEmpty(ItemTextBox.Text) || !string.IsNullOrEmpty(PriceTextBox.Text) || !string.IsNullOrEmpty(QuantityTextBox.Text))  
            {
                _itemsToAdd.Add(OperationDateTextBox.Text ?? "");
                _itemsToAdd.Add(ShopTextBox.Text ?? "");
                _itemsToAdd.Add(ItemTextBox.Text ?? "");
                _itemsToAdd.Add(PriceTextBox.Text ?? "");
                _itemsToAdd.Add(QuantityTextBox.Text ?? "");
                _userVm.InsertTableRows(_itemsToAdd.ToArray());
                Close();
            } 
            else  
            {
                MessageBox.Show("Необходимо заполнить хотя бы одно поле");  
            }  
        }

        private void MinimizeButton_OnClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
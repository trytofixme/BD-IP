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
    public partial class ItemView : Window
    {
        private readonly UserViewModel _userVm;
        private readonly List<object> _itemsToAdd = new List<object>();
        public ItemView(UserViewModel userVm)
        {
            InitializeComponent();
            DataContext = this;
            _userVm = userVm;
        }
        
        private void AddButton_OnClick(object sender, RoutedEventArgs e)  
        {  
            if (!string.IsNullOrEmpty(NameTextBox.Text) || !string.IsNullOrEmpty(MeasuringUnitTextBox.Text) ||
                !string.IsNullOrEmpty(МanufacturerTextBox.Text) || !string.IsNullOrEmpty(ItemTypeTextBox.Text))  
            {
                _itemsToAdd.Add(NameTextBox.Text ?? "");
                _itemsToAdd.Add(MeasuringUnitTextBox.Text ?? "");
                _itemsToAdd.Add(МanufacturerTextBox.Text ?? "");
                _itemsToAdd.Add(ItemTypeTextBox.Text ?? "");
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
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using GasStations.ViewModels;
using GasStations.Views.AddWindows;

namespace GasStations.Views
{
    /// <summary>
    /// Логика взаимодействия для FileManagerView.xaml
    /// </summary>
    public partial class AccountatView
    {
        private readonly UserViewModel _userVm;
        public AccountatView(UserViewModel userVm)
        {
            InitializeComponent();
            _userVm = userVm;
            DataContext = _userVm;
        }
        private void MinimizeButton_OnClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MaximizeButton_OnClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }
        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            _userVm.UpdateSelectedTable("Товары");
            _userVm.UpdateDataView();
        }

        private void TableNameButton_OnClick(object sender, RoutedEventArgs e)
        {
            _userVm.UpdateSelectedTable(((RadioButton)sender).Content.ToString());
            _userVm.UpdateDataView();
        }

        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            _userVm.DeleteTableRows();
        }

        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedTableName = _userVm.SelectedTableName;
            switch (selectedTableName)
            {
                case "Покупки":
                    var purachseView = new PurchaseView(_userVm);
                    purachseView.Show();
                    break;
                case "Продажи":
                    var saleView = new SaleView(_userVm);
                    saleView.Show();
                    break;
            }
        }
        
        private void dataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            var selectedItems = ((DataGrid)sender).SelectedItems;
            if (selectedItems.Count == 0)
            {
                return;
            }
            _userVm.UpdateSelectedRows(selectedItems);
        }

        private void DataGrid_OnCellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction != DataGridEditAction.Commit)
            {
                return;
            }
            var column = (DataGridBoundColumn)e.Column;
            if (column == null)
            {
                return;
            }
            var columnName = ((Binding)column.Binding).Path.Path;
            var rowId = ((DataRowView)e.Row.Item).Row.ItemArray.First();
            _userVm.UpdateTableRows(columnName, rowId, ((TextBox)e.EditingElement).Text);
        }
    }
}

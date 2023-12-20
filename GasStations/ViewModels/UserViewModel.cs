using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using GasStations.Models;

namespace GasStations.ViewModels
{
    public class UserViewModel : Notifier
    {
        private readonly DBManager _dbManager;
        private DataView _gridData;
        private string _userData;

        private readonly Dictionary<string, string> _tableNames = new Dictionary<string, string>
        {
            { "Товары", "items" },
            { "Покупки", "purchases" },
            { "Продажи", "sales" },
            { "Пользователи", "user_info" },
            { "Остатки", "remains" },
            { "Производители", "manufacturers" },
            { "Точки продажи", "shops" },
        };

        public UserViewModel(DBManager dbManager, string userData)
        {
            _dbManager = dbManager;
            UserData = userData;
            GridData = new DataView();
        }

        public DataView GridData
        {
            get => _gridData;

            set
            {
                _gridData = value;
                NotifyPropertyChanged(nameof(GridData));
            }
        }

        private ObservableCollection<DataRowView> SelectedRows { get; } = new ObservableCollection<DataRowView>();

        public string SelectedTableName { get; set; }

        public string UserData
        {
            get => _userData;

            set
            {
                _userData = value;
                NotifyPropertyChanged(nameof(UserData));
            }
        }

        public void UpdateDataView()
        {
            try
            {
                _dbManager.Reconnect();
                GridData = _dbManager.GetDataGrid(_tableNames[SelectedTableName]);
                // foreach (DataColumn col in GridData.Table.Columns)
                // {
                //     col.ReadOnly = true;
                // }
            }
            catch (Exception)
            {
                MessageBox.Show("Не удалось получить данную таблицу");
            }
        }

        public void UpdateSelectedRows(IList selectedItems)
        {
            SelectedRows.Clear();
            foreach (var item in selectedItems)
            {
                if (item.GetType() != typeof(DataRowView))
                {
                    continue;
                }
                SelectedRows.Add((DataRowView)item);
            }
        }
        
        public void UpdateSelectedTable(string tableName)
        {
            SelectedTableName = tableName;
        }

        public void DeleteTableRows()
        {
            foreach (var row in SelectedRows)
            {
                _dbManager.Delete(_tableNames[SelectedTableName], row.Row.ItemArray);
            }
            GridData = _dbManager.GetDataGrid(_tableNames[SelectedTableName]);
        }
        
        
        public void InsertTableRows(object[] itemsToAdd)
        {
            _dbManager.Insert(_tableNames[SelectedTableName], itemsToAdd);
            GridData = _dbManager.GetDataGrid(_tableNames[SelectedTableName]);
        }

        public void UpdateTableRows(string columnName, object rowId, string newValue)
        {
            _dbManager.Update(_tableNames[SelectedTableName], columnName, rowId, newValue);
            //GridData = _dbManager.GetDataGrid(_tableNames[SelectedTableName]);
        }
    }
}
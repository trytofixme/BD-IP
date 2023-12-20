using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using Npgsql;
using Npgsql.Replication;

namespace GasStations
{
    public class DBManager
    {
        public DBManager(ConfigurationManager configurationManager)
        {
            _configManager = configurationManager;
        }
        private readonly ConfigurationManager _configManager;
        public static NpgsqlConnection Connection { get; private set; }
        
        private DataColumnCollection SelectedColumns { get; set; }

        public void Connect()
        {
            Reconnect();
        }

        public void Reconnect()
        {
            if (_configManager == null)
            {
                throw new Exception("Конфигурация не была инициализирована");
            }

            if (Connection != null)
            {
                if (Connection.State != ConnectionState.Closed)
                {
                    Connection.Close();
                }
            }

            Connection = new NpgsqlConnection(_configManager.GetConnectionsString());
            Connection.Open();
        }

        public DataView GetDataGrid(string tableName)
        {
            Reconnect();
            var pgCmd = new NpgsqlCommand();
            pgCmd.Connection = Connection;
            var idName = tableName.Remove(tableName.Length - 1) + "_id";
            pgCmd.CommandText = $"select * from \"{tableName}\"" + (tableName == "remains" ? "" : $" order by {idName} asc");
            
            var dataAdapter = new NpgsqlDataAdapter(pgCmd);
            var dataSet = new DataSet(tableName);
            dataAdapter.Fill(dataSet);
            SelectedColumns = dataSet.Tables[0].Columns;
            return dataSet.Tables[0].DefaultView;
        }
        
        public void Insert(string tableName, object[] items)
        {
            try
            {
                var nextId = GetMaxId(tableName);
                var sb = new StringBuilder();
                sb.Append(nextId);
                sb.Append(", ");
                for (var i = 0; i < items.Length; i++)
                {
                    if (SelectedColumns[i + 1].DataType == typeof(int))
                    {
                        sb.Append($"{items[i]}");
                    }
                    else if (SelectedColumns[i + 1].DataType == typeof(string))
                    {
                        sb.Append($"'{items[i]}'");
                    }
                    else if (SelectedColumns[i + 1].DataType == typeof(DateTime) &&
                             DateTime.TryParse(items[i].ToString(), out var dateTime))
                    {
                        var dto = new DateTimeOffset(dateTime.ToUniversalTime());
                        sb.Append($"'{dto.ToUnixTimeSeconds().ToString()}'");
                    }

                    if (i != items.Length - 1)
                        sb.Append(", ");
                }
                
                Reconnect();
                var pgCmd = new NpgsqlCommand();
                pgCmd.Connection = Connection;
                pgCmd.CommandText = $"insert into \"{tableName}\" values ({sb})";
                pgCmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                MessageBox.Show("Невозможно добавить строку");
            }
        }
        
        public void Delete(string tableName, object[] items)
        {
            if (items == null || items.Length == 0)
            {
                return;
            }

            try
            {
                Reconnect();
                var pgCmd = new NpgsqlCommand();
                pgCmd.Connection = Connection;
                var idName = tableName.Remove(tableName.Length - 1) + "_id";
                pgCmd.CommandText = $"delete from \"{tableName}\" where \"{idName}\" = {items.First()}";
                pgCmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                MessageBox.Show("Невозможно удалить строку");
            }
        }
        
        public void Update(string tableName, string columnName, object rowId, string newValue)
        {
            if (string.IsNullOrEmpty(columnName) || !(int.TryParse(rowId.ToString(), out var id) && id >= 0))
            {
                return;
            }

            try
            {
                Reconnect();
                var pgCmd = new NpgsqlCommand();
                pgCmd.Connection = Connection;
                var idName = tableName.Remove(tableName.Length - 1) + "_id";
                pgCmd.CommandText = $"update \"{tableName}\" set {columnName} = '{newValue}' where {idName} = {id}";
                pgCmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                MessageBox.Show("Невозможно обновить значение");
            }
        }

        private int GetMaxId(string tableName)
        {
            Reconnect();
            var pgCmd = new NpgsqlCommand();
            pgCmd.Connection = Connection;
            var idName = tableName.Remove(tableName.Length - 1) + "_id";
            pgCmd.CommandText = $"select \"{idName}\" from \"{tableName}\" order by \"{idName}\" desc limit 1";

            var maxId = string.Empty;
            var reader = pgCmd.ExecuteReader();
            while (reader.Read())
            {
                maxId += reader[0].ToString();
                break;
            }

            return string.IsNullOrEmpty(maxId) ? 0 : int.Parse(maxId) + 1;
        }
    }
}
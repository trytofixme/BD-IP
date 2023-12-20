using System;
using System.Windows;
using GasStations.ViewModels;
using Npgsql;

namespace GasStations.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private const string AppDataPath = "C:\\Projects\\GasStations\\appsettings.json";
        private readonly ConfigurationManager _configurationManager;
        
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            
            _configurationManager = new ConfigurationManager(AppDataPath);
            var dbManager = new DBManager(_configurationManager);
            dbManager.Connect();
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)  
        {  
            if (LoginTextBox.Text != "" && PasswordTextBox.Text != "")  
            {
                try
                {
                    var dbManager = new DBManager(_configurationManager);
                    dbManager.Connect();
                    var pgCmd = new NpgsqlCommand
                    {
                        Parameters =
                        {
                            new NpgsqlParameter("_login", LoginTextBox.Text),
                            new NpgsqlParameter("_password", PasswordTextBox.Text)
                        }
                    };
                    
                    pgCmd.Connection = DBManager.Connection;
                    pgCmd.CommandText = "select \"user_status_info\"((@_login), (@_password))";
                    var userStatus = string.Empty;
                    var reader = pgCmd.ExecuteReader();
                    while (reader.Read())
                    {
                        userStatus += reader[0].ToString();
                        break;
                    }
                    DBManager.Connection.Close();
                    var userData = $"{LoginTextBox.Text}, {PasswordTextBox.Text} ({userStatus})";
                    UserViewModel userVm;
                    switch (userStatus)
                    { 
                        case "":
                            MessageBox.Show("Некорректное имя пользователя или пароль");
                            break;
                        case "Директор":
                            userVm = new UserViewModel(dbManager, userData);
                            var directorView = new DirectorView(userVm);
                            directorView.Show();
                            break;
                        case "Менеджер":
                            userVm = new UserViewModel(dbManager, userData);
                            var managerView = new ManagerView(userVm);
                            managerView.Show();
                            break;
                        case "Бухгалтер":
                            userVm = new UserViewModel(dbManager, userData);
                            var accountatView = new AccountatView(userVm);
                            accountatView.Show();
                            break;
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Подключение к базе данных отсутствует");
                    Application.Current.Shutdown();
                }
            }  
            else  
            {
                MessageBox.Show("Необходимо заполнить имя пользователя и пароль!");  
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
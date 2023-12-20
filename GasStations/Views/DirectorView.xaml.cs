using System;
using System.Windows;
using System.Windows.Controls;
using GasStations.ViewModels;

namespace GasStations.Views
{
    /// <summary>
    /// Логика взаимодействия для FileManagerView.xaml
    /// </summary>
    public partial class DirectorView : Window
    {
        private readonly UserViewModel _userVm;
        public DirectorView(UserViewModel userVm) 
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

        private void Window_ContentRendered(object sender, EventArgs  e)
        {
            _userVm.UpdateSelectedTable("Товары");
            _userVm.UpdateDataView();
        }

        private void TableNameButton_OnClick(object sender, RoutedEventArgs e)
        {
            _userVm.UpdateSelectedTable(((RadioButton)sender).Content.ToString());
            _userVm.UpdateDataView();
        }
    }
}

namespace Wpf.Views;

using System.Windows;

using Wpf.ViewModels;

public partial class ImportWindow : Window
{
    public ImportWindow(ImportViewModel viewModel)
    {
        InitializeComponent();

        viewModel.Controller = new WindowNavigator(this);
        DataContext          = viewModel;

        Loaded += async (_, _) => await viewModel.InitializeDataAsync();
    }
}

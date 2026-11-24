namespace Wpf.Views;

using System.Windows;

using Wpf.ViewModels;

public partial class PicturePreviewWindow : Window
{
    public PicturePreviewWindow(PicturePreviewViewModel viewModel)
    {
        InitializeComponent();

        viewModel.Controller = new WindowNavigator(this);
        DataContext          = viewModel;

        Loaded += async (_, _) => await viewModel.InitializeDataAsync();
    }
}

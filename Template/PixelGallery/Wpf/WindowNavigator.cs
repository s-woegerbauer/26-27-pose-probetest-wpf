namespace Wpf;

using System.Windows;

using Base.Core;
using Base.WpfMvvm;

using Wpf.ViewModels;
using Wpf.Views;

public class WindowNavigator : BaseWindowNavigator, IWindowNavigator
{
    public WindowNavigator(Window window) : base(window)
    {
    }

    public void ShowPicturePreview(int pictureId)
    {
        var window = AppService.GetRequiredService<PicturePreviewWindow>();
        ((PicturePreviewViewModel)window.DataContext).PictureId = pictureId;
        window.Owner = Window;
        window.ShowDialog();
    }

    public bool ShowImport()
    {
        var window = AppService.GetRequiredService<ImportWindow>();
        window.Owner = Window;
        return window.ShowDialog() == true;
    }
}

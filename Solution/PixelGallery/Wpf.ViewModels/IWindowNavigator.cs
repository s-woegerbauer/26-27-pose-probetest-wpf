namespace Wpf.ViewModels;

using Base.WpfMvvm;

public interface IWindowNavigator : IBaseWindowNavigator
{
    /// <summary>
    /// Opens the preview window (modal) for the picture.
    /// </summary>
    void ShowPicturePreview(int pictureId);

    /// <summary>
    /// Opens the import window (modal). Returns true if a picture was imported.
    /// </summary>
    bool ShowImport();
}

namespace Wpf.Controls;

using System.Windows;
using System.Windows.Media;

using Core.Entities;

/// <summary>
/// Custom draw control: renders the strokes of a picture, scaled to the available size.
/// </summary>
public class PicturePreviewControl : FrameworkElement
{
    #region Dependency properties

    public static readonly DependencyProperty StrokesProperty = DependencyProperty.Register(
        nameof(Strokes), typeof(IList<Stroke>), typeof(PicturePreviewControl),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

    public IList<Stroke>? Strokes
    {
        get => (IList<Stroke>?)GetValue(StrokesProperty);
        set => SetValue(StrokesProperty, value);
    }

    public static readonly DependencyProperty PictureWidthProperty = DependencyProperty.Register(
        nameof(PictureWidth), typeof(int), typeof(PicturePreviewControl),
        new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsRender));

    public int PictureWidth
    {
        get => (int)GetValue(PictureWidthProperty);
        set => SetValue(PictureWidthProperty, value);
    }

    public static readonly DependencyProperty PictureHeightProperty = DependencyProperty.Register(
        nameof(PictureHeight), typeof(int), typeof(PicturePreviewControl),
        new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsRender));

    public int PictureHeight
    {
        get => (int)GetValue(PictureHeightProperty);
        set => SetValue(PictureHeightProperty, value);
    }

    public static readonly DependencyProperty ShowGridProperty = DependencyProperty.Register(
        nameof(ShowGrid), typeof(bool), typeof(PicturePreviewControl),
        new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

    public bool ShowGrid
    {
        get => (bool)GetValue(ShowGridProperty);
        set => SetValue(ShowGridProperty, value);
    }

    #endregion

    protected override void OnRender(DrawingContext dc)
    {
        // transparent background => the whole area is hit-testable and has a defined size
        dc.DrawRectangle(Brushes.Transparent, null, new Rect(0, 0, ActualWidth, ActualHeight));

        // TODO (Task 3): custom draw
        //  - draw every stroke as a filled rectangle in its palette color (PaletteColor.Hex)
        //  - uniform scaling to the available size (ActualWidth / ActualHeight), centered
        //  - ShowGrid == true => draw a line grid over all pixels
    }
}

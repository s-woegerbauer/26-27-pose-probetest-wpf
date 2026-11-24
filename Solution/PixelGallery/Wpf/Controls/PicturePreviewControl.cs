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

        if (Strokes == null || PictureWidth <= 0 || PictureHeight <= 0)
        {
            return;
        }

        // uniform scaling, centered
        double scale   = Math.Min(ActualWidth / PictureWidth, ActualHeight / PictureHeight);
        double offsetX = (ActualWidth  - PictureWidth  * scale) / 2;
        double offsetY = (ActualHeight - PictureHeight * scale) / 2;

        var brushes = new Dictionary<string, Brush>();
        foreach (var stroke in Strokes)
        {
            var hex = stroke.PaletteColor?.Hex ?? "#FF00FF" /* unknown color */;
            if (!brushes.TryGetValue(hex, out var brush))
            {
                brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hex));
                brush.Freeze();
                brushes[hex] = brush;
            }

            var rect = new Rect(
                offsetX + stroke.Column * scale,
                offsetY + stroke.Row    * scale,
                stroke.Length * scale,
                scale);
            dc.DrawRectangle(brush, null, rect);
        }

        // optional pixel grid
        if (ShowGrid)
        {
            var gridPen = new Pen(Brushes.LightGray, 0.5);
            gridPen.Freeze();
            for (int x = 0; x <= PictureWidth; x++)
            {
                dc.DrawLine(gridPen, new Point(offsetX + x * scale, offsetY),
                    new Point(offsetX + x * scale, offsetY + PictureHeight * scale));
            }

            for (int y = 0; y <= PictureHeight; y++)
            {
                dc.DrawLine(gridPen, new Point(offsetX, offsetY + y * scale),
                    new Point(offsetX + PictureWidth * scale, offsetY + y * scale));
            }
        }

        // frame of the picture area
        dc.DrawRectangle(null, new Pen(Brushes.LightGray, 1),
            new Rect(offsetX, offsetY, PictureWidth * scale, PictureHeight * scale));
    }
}

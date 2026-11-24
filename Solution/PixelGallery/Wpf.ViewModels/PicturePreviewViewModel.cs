namespace Wpf.ViewModels;

using System.Collections.ObjectModel;
using System.Windows.Input;

using Base.WpfMvvm;

using Core.Contracts;
using Core.Entities;

using Wpf.ViewModels.Models;

public class PicturePreviewViewModel : BaseViewModel
{
    private readonly IUnitOfWork       _uow;
    public           IWindowNavigator? Controller { get; set; }

    public PicturePreviewViewModel(IUnitOfWork uow)
    {
        _uow         = uow;
        CloseCommand = new RelayCommand(() => Controller?.CloseWindow());
    }

    /// <summary>
    /// Id of the picture to show; set by the WindowNavigator before the window is shown.
    /// </summary>
    public int PictureId { get; set; }

    private Picture? _picture;

    public Picture? Picture
    {
        get => _picture;
        set
        {
            if (SetProperty(ref _picture, value))
            {
                OnPropertyChanged(nameof(ArtistName));
            }
        }
    }

    public string ArtistName => Picture?.Artist?.Name ?? "(unknown)";

    private bool _showGrid;

    public bool ShowGrid
    {
        get => _showGrid;
        set => SetProperty(ref _showGrid, value);
    }

    /// <summary>
    /// Strokes of the picture (incl. PaletteColor) for the custom draw control.
    /// </summary>
    private IList<Stroke> _strokes = new List<Stroke>();

    public IList<Stroke> Strokes
    {
        get => _strokes;
        set => SetProperty(ref _strokes, value);
    }

    /// <summary>
    /// Color legend: one entry per used color, sorted by PixelCount descending.
    /// </summary>
    public ObservableCollection<ColorUsage> ColorUsages { get; } = new();

    public ICommand CloseCommand { get; }

    public override async Task InitializeDataAsync()
    {
        Picture = await _uow.PictureRepository.GetNoTrackingByIdAsync(PictureId,
            nameof(Core.Entities.Picture.Artist),
            $"{nameof(Core.Entities.Picture.Strokes)}.{nameof(Stroke.PaletteColor)}");

        Strokes = Picture?.Strokes ?? new List<Stroke>();

        ColorUsages.Clear();
        foreach (var usage in Strokes
                     .GroupBy(s => s.PaletteColor!.Code) // NoTracking: group by key, not by instance
                     .Select(g => new ColorUsage(g.First().PaletteColor!.Name, g.First().PaletteColor!.Hex, g.Sum(s => s.Length)))
                     .OrderByDescending(u => u.PixelCount)
                     .ThenBy(u => u.Name))
        {
            ColorUsages.Add(usage);
        }
    }
}

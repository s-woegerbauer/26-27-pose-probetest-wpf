namespace Wpf.ViewModels;

using System.Collections.ObjectModel;
using System.Windows.Input;

using Base.WpfMvvm;

using Core.Contracts;
using Core.DataTransferObjects;
using Core.Entities;

using Wpf.ViewModels.Models;

public class MainWindowViewModel : BaseViewModel
{
    #region crt

    public           IWindowNavigator? Controller { get; set; }
    private readonly IUnitOfWork       _uow;
    private          bool              _isLoading; // suppresses reloads while the lists are rebuilt

    public MainWindowViewModel(IUnitOfWork uow)
    {
        _uow = uow;

        SearchCommand      = new RelayCommand(async () => await LoadPicturesAsync(), () => !IsEditing);
        // TODO (Task 3): the CanExecute conditions of the following four commands are missing
        OpenPreviewCommand = new RelayCommand(() => Controller!.ShowPicturePreview(SelectedPicture!.Id));
        EditCommand        = new RelayCommand(StartEdit);
        SaveCommand        = new RelayCommand(async () => await SaveAsync());
        CancelCommand      = new RelayCommand(CancelEdit);
        ImportCommand = new RelayCommand(async () =>
        {
            if (Controller!.ShowImport())
            {
                await InitializeDataAsync();
            }
        }, () => !IsEditing);
    }

    #endregion

    #region Filter

    public ObservableCollection<ArtistItem> Artists { get; } = new();

    private ArtistItem? _selectedArtist;

    /// <summary>
    /// Selecting an artist in the list reloads the pictures immediately.
    /// </summary>
    public ArtistItem? SelectedArtist
    {
        get => _selectedArtist;
        set
        {
            if (SetProperty(ref _selectedArtist, value) && !_isLoading && value != null)
            {
                _ = LoadPicturesAsync();
            }
        }
    }

    private string? _tagFilter;

    public string? TagFilter
    {
        get => _tagFilter;
        set => SetProperty(ref _tagFilter, value);
    }

    #endregion

    #region Pictures

    public ObservableCollection<PictureOverview> Pictures { get; } = new();

    private PictureOverview? _selectedPicture;

    public PictureOverview? SelectedPicture
    {
        get => _selectedPicture;
        set
        {
            if (SetProperty(ref _selectedPicture, value) && !_isLoading)
            {
                _ = LoadSelectedPictureAsync();
            }
        }
    }

    private string _statusText = string.Empty;

    public string StatusText
    {
        get => _statusText;
        set => SetProperty(ref _statusText, value);
    }

    #endregion

    #region Selected picture (mini preview + edit)

    private IList<Stroke> _selectedStrokes = new List<Stroke>();

    public IList<Stroke> SelectedStrokes
    {
        get => _selectedStrokes;
        set => SetProperty(ref _selectedStrokes, value);
    }

    public ObservableCollection<ArtistItem> EditArtists { get; } = new();

    private ArtistItem? _selectedEditArtist;

    public ArtistItem? SelectedEditArtist
    {
        get => _selectedEditArtist;
        set => SetProperty(ref _selectedEditArtist, value);
    }

    private bool _isEditing;

    public bool IsEditing
    {
        get => _isEditing;
        set
        {
            if (SetProperty(ref _isEditing, value))
            {
                OnPropertyChanged(nameof(IsNotEditing));
            }
        }
    }

    public bool IsNotEditing => !IsEditing;

    #endregion

    #region Commands

    public ICommand SearchCommand      { get; }
    public ICommand OpenPreviewCommand { get; }
    public ICommand EditCommand        { get; }
    public ICommand SaveCommand        { get; }
    public ICommand CancelCommand      { get; }
    public ICommand ImportCommand      { get; }

    #endregion

    #region Operations

    public override async Task InitializeDataAsync()
    {
        _isLoading = true;
        try
        {
            var artists    = await _uow.ArtistRepository.GetNoTrackingAsync(orderBy: q => q.OrderBy(a => a.Name));
            var selectedId = SelectedArtist?.Id;

            Artists.Clear();
            Artists.Add(new ArtistItem(null, "<All>"));
            EditArtists.Clear();
            EditArtists.Add(new ArtistItem(null, "(none)"));
            foreach (var artist in artists)
            {
                Artists.Add(new ArtistItem(artist.Id, artist.Name));
                EditArtists.Add(new ArtistItem(artist.Id, artist.Name));
            }

            SelectedArtist = Artists.FirstOrDefault(a => a.Id == selectedId) ?? Artists[0];
        }
        finally
        {
            _isLoading = false;
        }

        await LoadPicturesAsync();
    }

    private async Task LoadPicturesAsync()
    {
        _isLoading = true;
        try
        {
            var selectedId = SelectedPicture?.Id;
            var pictures   = await _uow.PictureRepository.GetOverviewAsync(SelectedArtist?.Id, TagFilter);

            Pictures.Clear();
            foreach (var picture in pictures)
            {
                Pictures.Add(picture);
            }

            StatusText      = $"{Pictures.Count} pictures, {Pictures.Sum(p => p.PixelCount)} pixels";
            SelectedPicture = Pictures.FirstOrDefault(p => p.Id == selectedId) ?? Pictures.FirstOrDefault();
        }
        finally
        {
            _isLoading = false;
        }

        await LoadSelectedPictureAsync();
    }

    private async Task LoadSelectedPictureAsync()
    {
        SelectedEditArtist = EditArtists.FirstOrDefault(a => a.Id == SelectedPicture?.ArtistId);
        if (SelectedPicture == null)
        {
            SelectedStrokes = new List<Stroke>();
            return;
        }

        SelectedStrokes = await _uow.StrokeRepository.GetNoTrackingAsync(
            s => s.PictureId == SelectedPicture.Id,
            null,
            nameof(Stroke.PaletteColor));
    }

    private void StartEdit()
    {
        IsEditing = true;
    }

    private void CancelEdit()
    {
        SelectedEditArtist = EditArtists.FirstOrDefault(a => a.Id == SelectedPicture?.ArtistId);
        IsEditing          = false;
    }

    private async Task SaveAsync()
    {
        // TODO (Task 3): save the new artist (nullable!) of the selected picture, end edit mode, reload the list
        await Task.CompletedTask;
    }

    #endregion
}

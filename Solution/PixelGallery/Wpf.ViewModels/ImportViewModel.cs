namespace Wpf.ViewModels;

using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Windows.Input;

using Base.WpfMvvm;

using Core.Contracts;
using Core.Entities;
using Core.Pixel;

public class ImportViewModel : ValidatableBaseViewModel
{
    private readonly IImportService                    _import;
    private readonly IUnitOfWork                       _uow;
    private          Dictionary<char, PaletteColor>    _palette = new();
    public           IWindowNavigator?                 Controller { get; set; }

    public ImportViewModel(IImportService import, IUnitOfWork uow)
    {
        _import       = import;
        _uow          = uow;
        ImportCommand = new RelayCommand(async () => await ImportAsync(), () => IsValid);
        CancelCommand = new RelayCommand(() => Controller?.CloseWindow());
    }

    #region Properties

    private string _name = "Rocket";

    [Required(ErrorMessage = "Name is required")]
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    private string? _description = "Demo picture";

    public string? Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    private string? _artist = "Demo";

    public string? Artist
    {
        get => _artist;
        set => SetProperty(ref _artist, value);
    }

    private string _fileName = "ImportData/DemoPicture.txt";

    [Required(ErrorMessage = "File name is required")]
    [CustomValidation(typeof(ImportViewModel), nameof(ValidateFileName))]
    public string FileName
    {
        get => _fileName;
        set
        {
            if (SetProperty(ref _fileName, value))
            {
                UpdatePreview();
            }
        }
    }

    public static ValidationResult? ValidateFileName(string? fileName, ValidationContext context)
        => File.Exists(fileName)
            ? ValidationResult.Success
            : new ValidationResult("File does not exist", new[] { nameof(FileName) });

    #endregion

    #region Preview

    private IList<Stroke> _previewStrokes = new List<Stroke>();

    public IList<Stroke> PreviewStrokes
    {
        get => _previewStrokes;
        set => SetProperty(ref _previewStrokes, value);
    }

    private int _previewWidth;

    public int PreviewWidth
    {
        get => _previewWidth;
        set => SetProperty(ref _previewWidth, value);
    }

    private int _previewHeight;

    public int PreviewHeight
    {
        get => _previewHeight;
        set => SetProperty(ref _previewHeight, value);
    }

    private string _previewInfo = string.Empty;

    public string PreviewInfo
    {
        get => _previewInfo;
        set => SetProperty(ref _previewInfo, value);
    }

    /// <summary>
    /// Parses the file (if it exists) and shows it in the preview.
    /// Strokes are NOT saved here - they only exist for the custom draw control.
    /// </summary>
    private void UpdatePreview()
    {
        if (!File.Exists(FileName))
        {
            PreviewStrokes = new List<Stroke>();
            PreviewInfo    = "no file";
            return;
        }

        ParseResult result;
        try
        {
            result = PixelParser.Parse(File.ReadAllText(FileName));
        }
        catch (Exception ex)
        {
            PreviewStrokes = new List<Stroke>();
            PreviewInfo    = $"no preview: {ex.Message}";
            return;
        }

        PreviewWidth  = result.Width;
        PreviewHeight = result.Height;
        PreviewStrokes = result.Runs
            .Select(r => new Stroke
            {
                Row          = r.Row,
                Column       = r.Column,
                Length       = r.Length,
                PaletteColor = _palette.GetValueOrDefault(r.ColorCode),
            })
            .ToList();
        PreviewInfo = $"{result.Width}x{result.Height}, {result.Runs.Count} strokes, tags: {string.Join(", ", result.Tags)}";
    }

    #endregion

    public ICommand ImportCommand { get; }
    public ICommand CancelCommand { get; }

    public override async Task InitializeDataAsync()
    {
        _palette = (await _uow.PaletteColorRepository.GetNoTrackingAsync()).ToDictionary(c => c.Code[0]);
        ValidateViewModelProperties();
        UpdatePreview();
    }

    private async Task ImportAsync()
    {
        try
        {
            await _import.ImportPictureAsync(Name, Description, Artist, DateOnly.FromDateTime(DateTime.Today), FileName);
            Controller?.CloseWindow(); // DialogResult = true
        }
        catch (Exception ex)
        {
            Controller?.ShowMessageBox(ex.GetBaseException().Message);
        }
    }
}

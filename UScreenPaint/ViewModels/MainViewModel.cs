using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Media;
using NullSoftware;
using NullSoftware.Services;
using PropertyChanged;
using UScreenPaint.Models;
using UScreenPaint.Services;

namespace UScreenPaint.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private IDialogService _dialogService;
        private IWindowService _mainWindow;
        private IWindowService _editorWindow;
        private IImagesStorage _imagesStorage;

        public bool IsEditModeEnabled { get; set; } = true;

        [OnChangedMethod(nameof(OnIsHighlighterChanged))]
        public bool IsHighlighter { get; set; }

        public InkCanvasEditingMode EditingMode { get; set; } = InkCanvasEditingMode.Ink;

        public double InkSize { get; set; } = 1;

        public Color SelectedColor { get; set; } = Colors.Red;
        public Color[] AvaliableColors { get; } = new Color[] 
        {
            Colors.Black,
            Colors.White,
            Colors.Red,
            Colors.Blue,
            Colors.Yellow,
            Colors.Green,
            Colors.Magenta,
            Colors.Cyan,
        };

        public DrawingAttributes DrawingAttributes { get; }

        public StrokeCollection Strokes { get; set; } = new StrokeCollection();

        [DoNotNotify]
        public IRefreshableCommand CreateNewCommand { get; }

        [DoNotNotify]
        public IRefreshableCommand SaveCommand { get; }

        [DoNotNotify]
        public IRefreshableCommand OpenCommand { get; }

        [DoNotNotify]
        public IRefreshableCommand CloseCommand { get; }

        [DoNotNotify]
        public IRefreshableCommand ClearAllCommand { get; }

        [DoNotNotify]
        public IRefreshableCommand ShowAboutCommand { get; }

        public MainViewModel()
        {
            _dialogService = new DialogService(App.Current.MainWindow);
            _mainWindow = new WindowService(App.Current.MainWindow);
            _imagesStorage = new ImagesStorage();

            CreateNewCommand = new RelayCommand(CreateNew);
            SaveCommand = new RelayCommand(Save, CanSave);
            OpenCommand = new RelayCommand(Open);
            CloseCommand = new RelayCommand(_mainWindow.Close);
            ClearAllCommand = new RelayCommand(Strokes.Clear);
            ShowAboutCommand = new RelayCommand(_dialogService.ShowAboutInfo);

            DrawingAttributes = new DrawingAttributes()
            {
                FitToCurve = true,
                Color = Colors.Red
            };


            Task.Run(InitializeAsync);
        }

        private async Task InitializeAsync()
        {
            _editorWindow = await _dialogService.ShowEditorAsync();
            
        }

        private void CreateNew()
        {
            Strokes.Clear();
        }

        private bool CanSave() => Strokes.Count > 0;

        private void Save()
        {
            VectorImage img = new VectorImage()
            {
                Width = (int)SystemParameters.PrimaryScreenWidth,
                Height = (int)SystemParameters.PrimaryScreenHeight,
                Timestamp = DateTime.Now,
                Strokes = Strokes,
            };

            _imagesStorage.SaveImage(img);
        }

        private void Open()
        {
            OpenImageViewModel viewModel = new OpenImageViewModel(_imagesStorage.GetAllImages());

            if (_dialogService.ShowOpenImageDialog(viewModel))
            {
                Strokes = viewModel.SelectedImage.Strokes;
            }
        }

        private void OnIsHighlighterChanged()
        {
            DrawingAttributes.IsHighlighter = IsHighlighter;
            DrawingAttributes.StylusTip = IsHighlighter ? StylusTip.Rectangle : StylusTip.Ellipse;
        }

        private void OnInkSizeChanged()
        {
            DrawingAttributes.Height = InkSize;
            DrawingAttributes.Width = InkSize;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Media;
using NullSoftware;
using NullSoftware.Services;
using PropertyChanged;
using UScreenPaint.Services;

namespace UScreenPaint.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private IDialogService _dialogService;
        private IWindowService _mainWindow;
        private IWindowService _editorWindow;

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

        public StrokeCollection Strokes { get; } = new StrokeCollection();


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

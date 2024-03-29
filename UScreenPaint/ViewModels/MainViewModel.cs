﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
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

        public bool Topmost { get; set; }

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
        public IRefreshableCommand ToggleEditModeCommand { get; }

        [DoNotNotify]
        public IRefreshableCommand ClearAllCommand { get; }

        [DoNotNotify]
        public IRefreshableCommand FillScreenCommand { get; }

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
            ToggleEditModeCommand = new RelayCommand(() => IsEditModeEnabled = !IsEditModeEnabled);
            CloseCommand = new RelayCommand(_mainWindow.Close);
            FillScreenCommand = new RelayCommand(FillScreen);
            ClearAllCommand = new RelayCommand(CreateNew);
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
                IsEditModeEnabled = true;
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

        private void FillScreen()
        {
            DrawingAttributes attrib = DrawingAttributes.Clone();
            attrib.FitToCurve = false;
            attrib.Width = 12;
            attrib.Height = 12;

            var spCollection = new StylusPointCollection()
            {
                //new StylusPoint(0,10),
                //new StylusPoint(6,0),
                //new StylusPoint(12,10),
                //new StylusPoint(0,10),
            };

            int width = (int)SystemParameters.PrimaryScreenWidth;
            int height = (int)SystemParameters.PrimaryScreenHeight;
            int errorX = 6; //Math.Max(1, (int)(attrib.Width));
            int errorY = 6; // Math.Max(1, (int)(attrib.Height));

            bool odd = false;
            for (int y = 0; y < height; y += errorY)
            {
                if (odd)
                {
                    for (int x = 0; x < width; x += errorX)
                    {
                        spCollection.Add(new StylusPoint(x, y));
                    }
                }
                else
                {
                    for (int x = (width - 1); x >= 0; x -= errorX)
                    {
                        spCollection.Add(new StylusPoint(x, y));
                    }
                }

                odd = !odd;

            }

            Strokes.Add(new Stroke(spCollection, attrib));
        }


    }
}

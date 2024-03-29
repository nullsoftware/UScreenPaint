﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using NullSoftware.Services;
using UScreenPaint.ViewModels;
using UScreenPaint.Views;

namespace UScreenPaint.Services
{
    public class DialogService : IDialogService
    {
        private Window _owner;
        private AboutWindow _aboutWindow;

        public DialogService(Window owner)
        {
            _owner = owner;
        }

        public async Task<IWindowService> ShowEditorAsync()
        {
            EditorWindow win = null;

            await _owner.Dispatcher.BeginInvoke(new Action(() => 
            {
                win = new EditorWindow();
                win.InputBindings.AddRange(_owner.InputBindings);
                win.Owner = _owner;
                win.DataContext = _owner.DataContext;
                win.Show();
            }), 
            DispatcherPriority.ApplicationIdle);

            return new WindowService(win);
        }

        public bool ShowOpenImageDialog(OpenImageViewModel viewModel)
        {
            OpenImageDialog dialog = new OpenImageDialog();
            dialog.DataContext = viewModel;
            dialog.Owner = _owner;

            return dialog.ShowDialog() == true;
        }

        public void ShowAboutInfo()
        {
            if (_aboutWindow == null)
            {
                _aboutWindow = new AboutWindow();
                _aboutWindow.Closed += OnAboutWindowClosed;
                _aboutWindow.Owner = _owner;
                _aboutWindow.DataContext = new AboutViewModel(new WindowService(_aboutWindow));
                _aboutWindow.Show();
            }
            else
            {
                _aboutWindow.Activate();
            }
        }

        private void OnAboutWindowClosed(object sender, EventArgs e)
        {
            ((Window)sender).Closed -= OnAboutWindowClosed;
            _aboutWindow = null;
        }
    }
}

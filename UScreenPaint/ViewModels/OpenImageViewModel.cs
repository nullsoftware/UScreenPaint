using NullSoftware;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UScreenPaint.Models;

namespace UScreenPaint.ViewModels
{
    public class OpenImageViewModel : ViewModelBase
    {
        public ObservableCollection<VectorImage> Images { get; }

        [OnChangedMethod(nameof(OnSelectedImageChanged))]
        public VectorImage SelectedImage { get; set; }

        [DoNotNotify]
        public IRefreshableCommand ConfirmCommand { get; }

        [DoNotNotify]
        public IRefreshableCommand RemoveCommand { get; }

        public OpenImageViewModel(IEnumerable<VectorImage> images) 
        {
            Images = new ObservableCollection<VectorImage>(images.OrderByDescending(t => t.Timestamp));

            ConfirmCommand = new RelayCommand<Window>(Confirm, CanConfirm);
            RemoveCommand = new RelayCommand<VectorImage>(Remove);
        }

        private bool CanConfirm(Window win) => SelectedImage != null;

        private void Confirm(Window win)
        {
            win.DialogResult = true;
        }

        private void OnSelectedImageChanged()
        {
            ConfirmCommand.Refresh();
        }

        private void Remove(VectorImage image)
        {
            File.Delete(image.FileName);

            Images.Remove(image);
        }
    }
}

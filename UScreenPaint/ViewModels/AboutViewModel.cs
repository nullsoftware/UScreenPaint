using NullSoftware;
using NullSoftware.Services;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UScreenPaint.ViewModels
{
    public class AboutViewModel : ViewModelBase
    {
        [DoNotNotify]
        public IRefreshableCommand CloseCommand { get; }

        public AboutViewModel(IWindowService windowService)
        {
            CloseCommand = new RelayCommand(windowService.Close);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NullSoftware.Services;

namespace UScreenPaint.Services
{
    public interface IDialogService
    {
        Task<IWindowService> ShowEditorAsync();

        void ShowAboutInfo();
    }
}

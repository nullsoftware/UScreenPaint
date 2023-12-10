using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UScreenPaint.Models;

namespace UScreenPaint.Services
{
    public interface IImagesStorage
    {
        void SaveImage(VectorImage img);

        IEnumerable<VectorImage> GetAllImages();
    }
}

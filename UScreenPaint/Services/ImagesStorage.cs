using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UScreenPaint.Models;

namespace UScreenPaint.Services
{
    public class ImagesStorage : IImagesStorage
    {
        public const string FileExtension = ".vimg";

        public string DirectoryName { get; }

        public ImagesStorage()
        {
            DirectoryName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "UScreenPaint\\Storage");
        }

        public IEnumerable<VectorImage> GetAllImages()
        {
            if (!Directory.Exists(DirectoryName))
            {
                yield break;
            }

            string[] files = Directory.GetFiles(DirectoryName, "*" + FileExtension, SearchOption.TopDirectoryOnly);
            VectorImage img = null;
            
            foreach (string file in files)
            {
                try
                {
                    using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                    {
                        img = VectorImage.Deserialize(fs);
                        img.FileName = file;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error: failed to load vector image file. Exception: " + ex.Message);
#if DEBUG
                    Debugger.Break();
#endif
                }

                if (img != null)
                {
                    yield return img;
                }
            }
        }

        public void SaveImage(VectorImage img)
        {
            PrepareDirectory();

            string fileName = Guid.NewGuid().ToString() + FileExtension;

            fileName = Path.Combine(DirectoryName, fileName);

            using (FileStream fs = new FileStream(fileName, FileMode.CreateNew, FileAccess.ReadWrite))
            {
                VectorImage.Serialize(img, fs);
            }
        }

        private void PrepareDirectory()
        {
            if (!Directory.Exists(DirectoryName))
            {
                Directory.CreateDirectory(DirectoryName);
            }
        }
    }
}

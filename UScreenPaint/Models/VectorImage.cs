using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;

namespace UScreenPaint.Models
{
    public class VectorImage
    {
        public const string FourCC = "VIMG";

        public const uint Version = 1;

        public DateTime Timestamp { get; set; }

        public string DisplayTimestamp => Timestamp.ToString();

        public int Width { get; set; }

        public int Height { get; set; }

        public StrokeCollection Strokes { get; set; }

        public string FileName { get; set; }


        public static void Serialize(VectorImage img, Stream stream)
        {
            using (BinaryWriter writer = new BinaryWriter(stream, Encoding.ASCII, true))
            {
                writer.Write(Encoding.ASCII.GetBytes(FourCC));
                writer.Write(Version);
                writer.Write(img.Timestamp.Ticks);
                writer.Write(img.Width);
                writer.Write(img.Height);
                img.Strokes.Save(stream);
            }
        }

        public static VectorImage Deserialize(Stream stream)
        {
            using (BinaryReader reader = new BinaryReader(stream, Encoding.ASCII, true))
            {
                string fourcc = Encoding.ASCII.GetString(reader.ReadBytes(FourCC.Length));

                if (fourcc != FourCC)
                    throw new InvalidOperationException("Current file is corrupted.");

                uint version = reader.ReadUInt32();

                if (version != Version)
                    throw new NotSupportedException("Current file version is outdated.");

                VectorImage img = new VectorImage();
                img.Timestamp = new DateTime(reader.ReadInt64());
                img.Width = reader.ReadInt32();
                img.Height = reader.ReadInt32();
                img.Strokes = new StrokeCollection(stream);

                return img;
            }
        }

        public override string ToString()
        {
            return $"{FourCC}, {Width}x{Height}, Count: ({Strokes.Count})";
        }
    }
}

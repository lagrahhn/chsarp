using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ImageProcessor;
using ImageProcessor.Imaging;
using ImageProcessor.Imaging.Formats;

namespace ImageHandle;

class Program
{
    static void Main(string[] args)
    {
        CompressImage("F:\\项目\\C#项目\\控制台应用\\chsarp\\ImageCompress\\source.jpg",
            "F:\\项目\\C#项目\\控制台应用\\chsarp\\ImageCompress\\source2.jpg", flag: 1);

        // NOTE:使用第三方库的压缩，第三方的较差一些
        // string inputPath = "F:\\项目\\C#项目\\控制台应用\\chsarp\\ImageCompress\\source.jpg";
        // string outputPath = "F:\\项目\\C#项目\\控制台应用\\chsarp\\ImageCompress\\compressed.jpg";
        //
        // byte[] imageBytes = System.IO.File.ReadAllBytes(inputPath);
        // using (MemoryStream memoryStream = new MemoryStream(imageBytes))
        // using (MemoryStream stream = new MemoryStream())
        // {
        //     using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
        //     {
        //         imageFactory.Load(memoryStream).Resize(new ResizeLayer(new Size(1920, 1200), ResizeMode.Max))
        //             .Format(new JpegFormat { Quality = 50 }).Save(stream);
        //     }
        //
        //     byte[] compressedImageBytes = stream.ToArray();
        //
        //     // 保存压缩后的图片到文件系统
        //     System.IO.File.WriteAllBytes(outputPath, compressedImageBytes);
        // }
        //
        // Console.WriteLine("Image compression completed.");
    }

    // NOTE:来源与网络上的
    // 博客园：https://www.cnblogs.com/ZXdeveloper/p/6841878.html
    /// <summary>
    /// 无损压缩图片
    /// </summary>
    /// <param name="sFile">原图片地址</param>
    /// <param name="dFile">压缩后保存图片地址</param>
    /// <param name="flag">压缩质量（数字越小压缩率越高）1-100</param>
    /// <param name="size">压缩后图片的最大大小</param>
    /// <param name="sfsc">是否是第一次调用</param>
    /// <returns></returns>
    public static bool CompressImage(string sFile, string dFile, int flag = 90, int size = 300, bool sfsc = true)
    {
        //如果是第一次调用，原始图像的大小小于要压缩的大小，则直接复制文件，并且返回true
        FileInfo firstFileInfo = new FileInfo(sFile);
        if (sfsc && firstFileInfo.Length < size * 1024)
        {
            firstFileInfo.CopyTo(dFile);
            return true;
        }

        Image iSource = Image.FromFile(sFile);
        ImageFormat tFormat = iSource.RawFormat;
        int dHeight = iSource.Height / 2;
        int dWidth = iSource.Width / 2;
        int sW = 0, sH = 0;
        //按比例缩放
        Size tem_size = new Size(iSource.Width, iSource.Height);
        if (tem_size.Width > dHeight || tem_size.Width > dWidth)
        {
            if (tem_size.Width * dHeight > tem_size.Width * dWidth)
            {
                sW = dWidth;
                sH = (dWidth * tem_size.Height) / tem_size.Width;
            }
            else
            {
                sH = dHeight;
                sW = (tem_size.Width * dHeight) / tem_size.Height;
            }
        }
        else
        {
            sW = tem_size.Width;
            sH = tem_size.Height;
        }

        Bitmap ob = new Bitmap(dWidth, dHeight);
        Graphics g = Graphics.FromImage(ob);

        g.Clear(Color.WhiteSmoke);
        g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

        g.DrawImage(iSource, new Rectangle((dWidth - sW) / 2, (dHeight - sH) / 2, sW, sH), 0, 0, iSource.Width,
            iSource.Height, GraphicsUnit.Pixel);

        g.Dispose();

        //以下代码为保存图片时，设置压缩质量
        EncoderParameters ep = new EncoderParameters();
        long[] qy = new long[1];
        qy[0] = flag; //设置压缩的比例1-100
        EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
        ep.Param[0] = eParam;

        try
        {
            ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo jpegICIinfo = null;
            for (int x = 0; x < arrayICI.Length; x++)
            {
                if (arrayICI[x].FormatDescription.Equals("JPEG"))
                {
                    jpegICIinfo = arrayICI[x];
                    break;
                }
            }

            if (jpegICIinfo != null)
            {
                ob.Save(dFile, jpegICIinfo, ep); //dFile是压缩后的新路径
                FileInfo fi = new FileInfo(dFile);
                if (fi.Length > 1024 * size)
                {
                    flag = flag - 10;
                    CompressImage(sFile, dFile, flag, size, false);
                }
            }
            else
            {
                ob.Save(dFile, tFormat);
            }

            return true;
        }
        catch
        {
            return false;
        }
        finally
        {
            iSource.Dispose();
            ob.Dispose();
        }
    }
}
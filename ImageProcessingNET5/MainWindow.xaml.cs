using ImageProcessingNET5.src;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ImageProcessingNET5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Stopwatch a = new Stopwatch();

            var bmp = new BitmapImage(new Uri("E://lena.jpg"));
            var rect = new Int32Rect(0, 0, bmp.PixelWidth, bmp.PixelHeight);
            imgSource.Source = bmp;

            int blurrFactor = 2;


            int stride = (bmp.Format.BitsPerPixel * bmp.PixelWidth) / 8;
            var bytesPixels = new byte[stride * bmp.PixelHeight];

            bmp.CopyPixels(bytesPixels, stride, 0);

            CImage originalCImage = new CImage(bmp.PixelWidth, bmp.PixelHeight, bmp.Format.BitsPerPixel, bytesPixels);


            CImage cImageBytes = new CImage(bmp.PixelWidth, bmp.PixelHeight, bmp.Format.BitsPerPixel, bytesPixels);
            a.Start();
            cImageBytes.AveragingBytes(originalCImage, blurrFactor);
            a.Stop();
            WriteableBitmap wrtBitmapBytes = new WriteableBitmap(bmp.PixelWidth, bmp.PixelHeight, bmp.DpiX, bmp.DpiY, bmp.Format, null);
            wrtBitmapBytes.WritePixels(rect, cImageBytes.Bytes, cImageBytes.stride, 0);
            imgBytes.Source = wrtBitmapBytes;
            lblBytes.Content = "Bytes: " + a.ElapsedMilliseconds + "ms";

             
            CImage cImageGrid = new CImage(bmp.PixelWidth, bmp.PixelHeight, bmp.Format.BitsPerPixel, bytesPixels);
            a.Start();
            cImageGrid.AveragingGrid(originalCImage, blurrFactor);
            a.Stop();
            WriteableBitmap wrtBitmapGrid = new WriteableBitmap(bmp.PixelWidth, bmp.PixelHeight, bmp.DpiX, bmp.DpiY, bmp.Format, null);
            wrtBitmapGrid.WritePixels(rect, cImageGrid.GridAsBytes(), cImageGrid.stride, 0);
            imgGrid.Source = wrtBitmapGrid;
            lblGrid.Content = "Grid: " + a.ElapsedMilliseconds + "ms";
            

            CImage cImageMatrix = new CImage(bmp.PixelWidth, bmp.PixelHeight, bmp.Format.BitsPerPixel, bytesPixels);
            a.Start();
            cImageMatrix.AveragingMatrix(originalCImage, blurrFactor);
            a.Stop();
            WriteableBitmap wrtBitmapMatrix = new WriteableBitmap(bmp.PixelWidth, bmp.PixelHeight, bmp.DpiX, bmp.DpiY, bmp.Format, null);
            wrtBitmapMatrix.WritePixels(rect, cImageMatrix.MatrixAsBytes(), cImageMatrix.stride, 0);
            imgMatrix.Source = wrtBitmapMatrix;
            lblMatrix.Content = "Matrix: " + a.ElapsedMilliseconds + "ms";
            
        }
    }
}

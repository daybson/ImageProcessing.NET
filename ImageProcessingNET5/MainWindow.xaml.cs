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

            a.Start();

            int stride = (bmp.Format.BitsPerPixel * bmp.PixelWidth) / 8;
            var bytesPixels = new byte[stride * bmp.PixelHeight];

            bmp.CopyPixels(bytesPixels, stride, 0);

            CImage cImage   =   new CImage(bmp.PixelWidth, bmp.PixelHeight, bmp.Format.BitsPerPixel, bytesPixels);
            CImage cImage2  =   new CImage(bmp.PixelWidth, bmp.PixelHeight, bmp.Format.BitsPerPixel, bytesPixels);

            //cImage2.Averaging(cImage, 2);

            WriteableBitmap wrtBitmap = new WriteableBitmap(bmp.PixelWidth, bmp.PixelHeight, bmp.DpiX, bmp.DpiY, bmp.Format, null);
            //wrtBitmap.WritePixels(rect, cImage2.GridAsBytes(), stride, 0);
            wrtBitmap.WritePixels(rect, cImage2.MatrixAsBytes(), stride, 0);

            a.Stop();

            imgResult.Source = wrtBitmap;
            lblTime.Content = a.ElapsedMilliseconds + "ms";
        }
    }
}

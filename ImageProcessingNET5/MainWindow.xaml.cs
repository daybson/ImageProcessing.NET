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

            var bmp = new BitmapImage(new Uri("D://lena.jpg"));
            imgSource.Source = bmp;

            a.Start();
            int stride = bmp.Format.BitsPerPixel / 8 * bmp.PixelWidth;

            var pixels = new byte[stride * bmp.PixelHeight];

            bmp.CopyPixels(pixels, stride, 0);

            CImage cImage = new CImage(bmp.PixelWidth, bmp.PixelHeight, bmp.Format.BitsPerPixel, pixels);

            WriteableBitmap writeableBitmap = new WriteableBitmap(bmp.PixelWidth, bmp.PixelHeight, bmp.DpiX, bmp.DpiY, bmp.Format, null);

            writeableBitmap.WritePixels(new Int32Rect(0, 0, bmp.PixelWidth, bmp.PixelHeight), cImage.Grid, stride, 0);

            a.Stop();

            imgResult.Source = writeableBitmap;

            lblTime.Content = a.ElapsedMilliseconds + "ms";
        }
    }
}

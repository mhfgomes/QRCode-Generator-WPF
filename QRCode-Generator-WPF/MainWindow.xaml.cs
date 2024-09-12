using QRCoder;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace QRCode_Generator_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BitmapImage qrcodeimage = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using QRCodeGenerator generator = new();
            using QRCodeData codedata = generator.CreateQrCode(qrcontent.Text, QRCodeGenerator.ECCLevel.H);
            using QRCode qrcode = new(codedata);
            using Bitmap bmp = qrcode.GetGraphic(100);
            qrcodeimage = ConvertBitmapToImageSource(bmp);
            qrimage.Source = qrcodeimage;
            bmp.Dispose();
            qrcode.Dispose();
            codedata.Dispose();
            generator.Dispose();
        }

        static BitmapImage ConvertBitmapToImageSource(Bitmap bitmap)
        {
            using MemoryStream memoryStream = new();
            bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
            memoryStream.Position = 0;

            BitmapImage bitmapImage = new();
            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.StreamSource = memoryStream;
            bitmapImage.EndInit();
            bitmapImage.Freeze();

            memoryStream.Close();
            memoryStream.Dispose();

            return bitmapImage;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            using QRCodeGenerator generator = new();
            using QRCodeData codedata = generator.CreateQrCode(qrcontent.Text, QRCodeGenerator.ECCLevel.H);
            using QRCode qrcode = new(codedata);
            using Bitmap bmp = qrcode.GetGraphic(100);

            Microsoft.Win32.SaveFileDialog dlg = new()
            {
                Title = "QRCode",
                FileName = "qrcode",
                DefaultExt = ".jpg",
                Filter = "Jpeg (.jpg)|*.jpg"
            };

            if (dlg.ShowDialog() == true)
            {
                string filename = dlg.FileName;
                bmp.Save(filename);
            }

        }
    }
}
using Avalonia.Controls;
using Avalonia.Interactivity;
using Proje.Scripts;
using Scripts.Robot;
using System.Diagnostics;
using System.IO;

namespace Proje
{
    public partial class MainWindow : Window
    {
        // Dosya yolları
        readonly string url1 = Path.Combine(Directory.GetCurrentDirectory(), "örnek text dosyaları", "url1.txt");
        readonly string url2 = Path.Combine(Directory.GetCurrentDirectory(), "örnek text dosyaları", "url2.txt");
        // 1 ve 2 arasında geçmek için bool
        bool Url1Yuklu = true;
#pragma warning disable CS8618 // Null atanamaz alan, oluşturucudan çıkış yaparken null olmayan bir değer içermelidir. Alanı null atanabilir olarak bildirmeyi düşünün.
        public MainWindow()
#pragma warning restore CS8618 // Null atanamaz alan, oluşturucudan çıkış yaparken null olmayan bir değer içermelidir. Alanı null atanabilir olarak bildirmeyi düşünün.
        {
            InitializeComponent();

            // Problemlerin objelerinin oluşması için foonksiyonları çağır
            Problem1iAyarla();
            Problem2_Ayarla();

        }
        #region 1. Problem Kodları

        FileRobot robot1;
        private void Problem1iAyarla()
        {
            // 1. urlyi yükle
            grid.LoadFromFile(url1);
            // Robotu oluştur
            robot1 = new FileRobot(grid);
        }
        private async void Problem1_YolBul(object sender, RoutedEventArgs e)
        {
            // Yol bulunurken yeni gridi yüklemeye izin verme
            Problem1_Kisitla(false);

            // Yolu bul
            var sonuc = await robot1.YolBul(Pathfinding.Algoritma.BFS, skipAnim.IsChecked ?? false);

            // Sonuçları yazdır
            sure2.Text = $"{sonuc.AlgoritmaZamani:ss\\.fff} sn";
            sure1.Text = $"{sonuc.ToplamAnimasyonZamani:ss\\.fff} sn";
            adim.Text = $"{sonuc.Adim} adım";

            // Yüklemeye izin ver
            Problem1_Kisitla(true);
        }

        private void Problem1_DosyaDegistir(object sender, RoutedEventArgs e)
        {
            // Dosyayi bool'a göre yükle
            grid.LoadFromFile(Url1Yuklu ? url2 : url1);
            Url1Yuklu = !Url1Yuklu;
        }

        private void Problem1_HedefDegistir(object sender, RoutedEventArgs e)
        {
            // Hedef pozisyonunu değiştir
            robot1.HedefYeriniDegistir();
        }

        private void Problem1_RobotDegistir(object sender, RoutedEventArgs e)
        {
            // Robot pozisyonunu değiştir
            robot1.RobotYeriniDegistir();
        }

        private void Problem1_AnimasyonGec(object sender, RoutedEventArgs e)
        {
            if (skipAnim.IsChecked.HasValue)
            {
                // Animasyon geçişini ayarlar
                robot1.AnimasyonuGecAyarla(skipAnim.IsChecked.Value);
            }
        }
        private void Problem1_Kisitla(bool enable)
        {
            dosyaDegistir.IsEnabled = enable;
            pozisyonlarıDegistir.IsEnabled = enable;
        }


        #endregion

        #region Problem 2 Kodları

        LabirentRobot labirentRobot;
        private void Problem2_Ayarla()
        {
            // Başlangıçta 15,15 labirent oluştur
            labirentGrid.CreateMaze(15, 15);
            labirentRobot = new LabirentRobot(labirentGrid);
        }
        private void LabirentGridOlustur(object sender, RoutedEventArgs e)
        {
            // Genişlik ve yüksekliği al
            int genislik = (int)labirentGenislik.Value;
            int yukseklik = (int)labirentYukseklik.Value;

            //Yeni labirent oluştur
            labirentGrid.CreateMaze(genislik, yukseklik);
        }
        private async void LabirentYolBul(object sender, RoutedEventArgs e)
        {
            // Yol bulunurken gridi değiştirmeye izin verme
            Problem2_Kisitla(false);

            // Yolu bul
            var sonuc = await labirentRobot.YolBul(Pathfinding.Algoritma.Wilson, skipAnimCheckBox.IsChecked ?? false);

            // Sonuçları yazdır
            LabirentSure1.Text = $"{sonuc.ToplamAnimasyonZamani:ss\\.fff} sn";
            LabirentSure2.Text = $"{sonuc.AlgoritmaZamani:ss\\.fff} sn";
            LabirentAdim.Text = $"{sonuc.Adim} adım";

            // Gridi değiştirmeye izin ver
            Problem2_Kisitla(true);
        }

        private void Problem2_Kisitla(bool enable)
        {
            labirentOlustur.IsEnabled = enable;
            Problem2YolBul.IsEnabled = enable;
        }

        private void Problem2_AnimasyonGec(object sender, RoutedEventArgs e)
        {
            if (skipAnimCheckBox.IsChecked.HasValue)
            {
                // Animasyon geçişini ayarlar
                labirentRobot.AnimasyonuGecAyarla(skipAnimCheckBox.IsChecked.Value);
            }
        }

        #endregion

    }
}
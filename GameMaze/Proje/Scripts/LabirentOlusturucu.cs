using Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Proje.Scripts
{
    public class LabirentOlusturucu
    {
        static readonly Random random = new();
        public static int[,] Olustur(int genislik, int yukseklik, out Point giris,out Point cikis)
        {
            // Eğer genislik veya yükseklik çift ise algoritma bozuluyor. O yüzden 1 artır.
            if(genislik % 2 == 0) { genislik++; }
            if(yukseklik % 2 == 0) { yukseklik++; }

            int[,] labirent = new int[yukseklik, genislik];

            // Her hücreyi duvar yap
            for (int i = 0; i < yukseklik; i++)
            {
                for (int j = 0; j < genislik; j++)
                {
                    labirent[i, j] = 1;
                }
            }
            // Giriş köşesini belirler
            (int girisSatır, int girisSutun) = random.Next(2) == 0 ? (1, 0) : (0, 1);
            giris = new Point(girisSutun, girisSatır);

            // Çıkış köşesini belirler
            (int cikisSatır, int cikisSutun) = random.Next(2) == 0 ? (genislik - 2, yukseklik - 1) : (genislik - 1, yukseklik - 2);
            cikis = new Point(cikisSutun, cikisSatır);

            labirent[1, 1] = 0;
            // Algoritma
            RecursiveBacktrackAlgoritmasi(labirent, 1,1);
            return labirent;
        }
        public static void RecursiveBacktrackAlgoritmasi(int[,] maze, int x, int y)
        {
            // Yönleri karıştır
            List<int> yonler = new(){ 1, 2, 3, 4 };
            yonler = yonler.OrderBy(item => random.Next()).ToList();

            foreach (int yon in yonler)
            {
                int nextX = x;
                int nextY = y;

                switch (yon)
                {
                    case 1: // Yukarı
                        if (y - 2 <= 0) continue;
                        nextY = y - 2;
                        break;
                    case 2: // Sağ
                        if (x + 2 >= maze.GetLength(1) - 1) continue;
                        nextX = x + 2;
                        break;
                    case 3: // Aşağı
                        if (y + 2 >= maze.GetLength(0) - 1) continue;
                        nextY = y + 2;
                        break;
                    case 4: // Sol
                        if (x - 2 <= 0) continue;
                        nextX = x - 2;
                        break;
                }

                if (maze[nextY, nextX] == 1)
                {
                    // Yolu oluştur
                    maze[nextY, nextX] = 0;
                    maze[y + (nextY - y) / 2, x + (nextX - x) / 2] = 0;

                    // Bir sonraki yolu açmak için recursion kullan
                    RecursiveBacktrackAlgoritmasi(maze, nextX, nextY);
                }
            }
        }
     
    }
}

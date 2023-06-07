using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Proje.Grid.Cell;
using Proje.Grid.Classlar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Proje.Grid.Controls
{
    /// <summary>
    /// User Control ile Abstract temel grid sistemi
    /// </summary>
    public abstract class GridTemel : UserControl,IGrid
    {
        public int Satirlar { get; protected set; }
        public int Sutunlar { get; protected set; }

        protected Hucre[,] _hucreler;

        public IEnumerable<Hucre> Hucreler
        {
            get
            {
                for (int row = 0; row < Satirlar; row++)
                {
                    for (int col = 0; col < Sutunlar; col++)
                    {
                        yield return _hucreler[row, col];
                    }
                }
            }
        }
#pragma warning disable CS8618 // Null atanamaz alan, oluşturucudan çıkış yaparken null olmayan bir değer içermelidir. Alanı null atanabilir olarak bildirmeyi düşünün.
        public GridTemel()
#pragma warning restore CS8618 // Null atanamaz alan, oluşturucudan çıkış yaparken null olmayan bir değer içermelidir. Alanı null atanabilir olarak bildirmeyi düşünün.
        {

            // Grid boyutu
            this.Width = 550;
            this.Height = 550;

            this.Satirlar = 10;
            this.Sutunlar = 10;

            HucreleriOlustur();
        }

        /// <summary>
        /// Hücrelerin hepsini boş hücre yapar.
        /// </summary>
        /// SubClasslar için override edebilinmesi ve varsayılan olarak boş çizmesi için virtual.
        public virtual void HucreleriOlustur()
        {
            _hucreler = new Hucre[Satirlar, Sutunlar];
            for (int row = 0; row < Satirlar; row++)
            {
                for (int col = 0; col < Sutunlar; col++)
                {
                    _hucreler[row, col] = new Hucre(row, col, HucreTipi.Empty);
                }
            }
            RenderaZorla();
        }

        /// <summary>
        /// Grid'i bir daha çizemeye zorlar.
        /// </summary>
        public void RenderaZorla()
        {
            this.InvalidateVisual();     
        }
        // Çizmek için Render metodunu override etmemiz gerekiyor
        public override void Render(DrawingContext context)
        {
            base.Render(context);

            if (context is null)
                return;
            if (Double.IsNaN(Width) || Double.IsNaN(Height))
                return;

            // Arkaplanı Siyah yapma
            context.DrawRectangle(Brushes.Black, null, new Rect(0, 0, Width, Height));

            // Her hücrenin boyutunu hesaplama
            int hucreGenislik = (int)(Width / Sutunlar);
            int hucreYukseklik = (int)(Height / Satirlar);

            // Hücreleri çizme
            for (int satir = 0; satir < Satirlar; satir++)
            {
                for (int sutun = 0; sutun < Sutunlar; sutun++)
                {
                    _hucreler[satir, sutun]?.Ciz(context, hucreGenislik, hucreYukseklik);
                }
            }

            // Hücre sınırlarını çizme
            Pen pen = new(Brushes.Black, 2);

            // Satırlar arası çizgiler
            for (int satir = 0; satir <= Satirlar; satir++)
            {
                int y = satir * hucreYukseklik;
                context.DrawLine(pen, new Point(0, y), new Point(Width, y));
            }

            // Sutünlar arası çizgiler
            for (int sutun = 0; sutun <= Sutunlar; sutun++)
            {
                int x = sutun * hucreGenislik;
                context.DrawLine(pen, new Point(x, 0), new Point(x, Height));
            }
        }


        #region Grid fonksiyoonları

        /// <summary>
        /// İki hücre birbirine komşu mu diye kontrol eder
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool KomsuMu(Hucre a, Hucre b)
        {
            return Math.Abs(a.Satir - b.Satir) + Math.Abs(a.Sutun - b.Sutun) == 1;
        }
        /// <summary>
        /// Hücrenin koomşularını döndürür
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public List<Hucre> KomsulariAl(Hucre cell)
        {
            List<Hucre> neighbors = new();
            int row = cell.Satir;
            int col = cell. Sutun;

            if (row > 0 && !_hucreler[row - 1, col].DuvarMı) // up
                neighbors.Add(_hucreler[row - 1, col]);

            if (col < Sutunlar - 1 && !_hucreler[row, col + 1].DuvarMı) // right
                neighbors.Add(_hucreler[row, col + 1]);

            if (row < Satirlar - 1 && !_hucreler[row + 1, col].DuvarMı) // down
                neighbors.Add(_hucreler[row + 1, col]);

            if (col > 0 && !_hucreler[row, col - 1].DuvarMı) // left
                neighbors.Add(_hucreler[row, col - 1]);

            return neighbors;
        }

        /// <summary>
        /// Verilen satır ve sutündaki hücreyi döndürür
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public Hucre HucreyiBul(int row, int col)
        {
            if (row < 0 || row >= Satirlar || col < 0 || col >= Sutunlar)
            {
                throw new ArgumentException("Satır veya sutün index'i yanlış!");
            }

            return _hucreler[row, col];
        }

        private readonly Random random = new ();
        /// <summary>
        /// Rastgele boş hücre bulur
        /// </summary>
        /// <returns></returns>
        public Hucre RastgeleBosHucreBul()
        {
            int row, col;
            do
            {
                row = random.Next(Satirlar);
                col = random.Next(Sutunlar);
            } while (HucreyiBul(row, col).HucreTipi != 0);
            return HucreyiBul(row, col);
        }
        /// <summary>
        /// Bütün hücrelerin renklerini sıfırlar
        /// </summary>
        public void HucreRenkleriniSifirla()
        {
            foreach (var item in _hucreler)
            {
                item.RengiSifirla();
            }
        }

        #endregion

    }
}

using Proje.Grid.Cell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proje.Grid.Controls
{
    public abstract class IntGrid : GridTemel
    {
        protected int[,] _gridDegerleri;
        public IntGrid() : base ()
        {
            _gridDegerleri = new int[Satirlar, Sutunlar];
        }
        /// <summary>
        /// Gridi 0 ve 1 lerden oluşan int[,] ile oluşturur
        /// </summary>
        /// <param name="ints"></param>
        public void LoadFromInts(int[,] ints)
        {
            _gridDegerleri = ints;

            Satirlar = ints.GetLength(0);
            Sutunlar = ints.GetLength(1);

            HucreleriOlustur();
        }
        /// <summary>
        /// Temel class'tan oluşturulmuş hücreler oluşturma metodu
        /// </summary>
        public override void HucreleriOlustur()
        {
            _hucreler = new Hucre[Satirlar, Sutunlar];
            for (int row = 0; row < Satirlar; row++)
            {
                for (int col = 0; col < Sutunlar; col++)
                {
                    _hucreler[row, col] = new Hucre(row, col, (HucreTipi)(_gridDegerleri?[row, col] ?? 0));
                }
            }
            RenderaZorla();
        }

    }
}

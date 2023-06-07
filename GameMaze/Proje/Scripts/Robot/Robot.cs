using Proje.Grid.Cell;
using Proje.Grid.Classlar;
using Proje.Grid.Controls;
using Proje.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.Robot
{
    public abstract class Robot<T> where T : IGrid
    {
        protected T grid;

#pragma warning disable CS8618 // Null atanamaz alan, oluşturucudan çıkış yaparken null olmayan bir değer içermelidir. Alanı null atanabilir olarak bildirmeyi düşünün.
        public Robot(T grid)
#pragma warning restore CS8618 // Null atanamaz alan, oluşturucudan çıkış yaparken null olmayan bir değer içermelidir. Alanı null atanabilir olarak bildirmeyi düşünün.
        {
            this.grid = grid;
            RobotveHedefYeriniGuncelle();
        }

        /// <summary>
        ///  Robot ve hedefin yerlerini ayarlar
        /// </summary>
        protected void RobotveHedefYeriniGuncelle()
        {
            HedefYeriniDegistir(false);
            RobotYeriniDegistir(false);
            grid.RenderaZorla();
        }
        protected Hucre? _targetCell;
        protected Hucre? _robotCell;

        /// <summary>
        /// Hedef önceden atanmşsa, tipini resetler
        /// </summary>
        public void HedefYeriniSifirla()
        {
            _targetCell?.TipiSifirla();
        }
        /// <summary>
        /// Hedefi rastgele boş bir hücreye yerleştirir.
        /// </summary>
        /// <param name="render"></param>
        public virtual void HedefYeriniDegistir(bool render = true)
        {
            HedefYeriniSifirla();
            if (render)
            {
                grid.HucreRenkleriniSifirla();
            }
            _targetCell = grid.RastgeleBosHucreBul();
            _targetCell.TipiAta(HucreTipi.Target);
            if (render)
            {
                grid.RenderaZorla();
            }
        }

        /// <summary>
        /// Robot önceden atanmşsa, tipini resetler
        /// </summary>
        public void RobotYeriniSifirla()
        {
            _robotCell?.TipiSifirla();
        }
        /// <summary>
        /// Robotu rastgele boş bir hücreye yerleştirir.
        /// </summary>
        /// <param name="render"></param>
        public virtual void RobotYeriniDegistir(bool render = true)
        {
            RobotYeriniSifirla();
            if (render)
            {
                grid.HucreRenkleriniSifirla();
            }
            _robotCell = grid.RastgeleBosHucreBul();
            _robotCell.TipiAta(HucreTipi.Robot);
            if (render)
            {
                grid.RenderaZorla();
            }
        }


        #region Animasyonu Geç
        public void AnimasyonuGecAyarla(bool gec)
        {
            Pathfinding?.AnimasyonGecis(gec);
        }
        #endregion

        #region Yol bulma

        private Pathfinding Pathfinding;
        public async Task<PathFindSonuc> YolBul(Pathfinding.Algoritma? algorithm, bool animasyonuGec)
        {
            grid.HucreRenkleriniSifirla();

            // Eğer öğe ayarlanmışsa animasyonu direkt bitiriyoruz
            Pathfinding?.AnimasyonGecis(true);

            if (_robotCell == null || _targetCell == null)
                return new PathFindSonuc() { Hata = "Hedef veya robot atanmamış!" };
            Pathfinding = new Pathfinding(() => grid, _robotCell, _targetCell, animasyonuGec);
            return await Pathfinding.FindPath(algorithm);
        }
        #endregion
    }
}

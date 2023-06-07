using Proje.Grid.Cell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proje.Grid.Classlar
{
    public interface IGrid
    {
        public abstract void HucreleriOlustur();
        public void RenderaZorla();
        public List<Hucre> KomsulariAl(Hucre cell);
        public Hucre HucreyiBul(int row, int col);
        public Hucre RastgeleBosHucreBul();
        public void HucreRenkleriniSifirla();
    }
}

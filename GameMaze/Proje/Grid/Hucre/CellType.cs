//using Avalonia.Media;
using Avalonia.Media;

namespace Proje.Grid.Cell
{
    public enum HucreTipi
    {
        Empty = 0,

        Obstacle1 = 1,
        Obstacle2 = 2,
        Obstacle3 = 3,

        Robot = 99,
        Target = 100,
    }
    public static class Extensions
    {
        public static bool DuvarMı(this HucreTipi cellType)
        {
            return cellType == HucreTipi.Obstacle1 || cellType == HucreTipi.Obstacle2 || cellType == HucreTipi.Obstacle3;
        }
        public static Color Renk(this HucreTipi cellType)
        {
            return cellType switch
            {
                HucreTipi.Empty => HucreRenkleri.Bos,
                HucreTipi.Robot => HucreRenkleri.Robot,
                HucreTipi.Target => HucreRenkleri.Hedef,
                _ => HucreRenkleri.Duvar,
            };
        }
    }
}
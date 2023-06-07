using Proje.Grid.Cell;
using Proje.Grid.Controls;

namespace Scripts.Robot
{
    class LabirentRobot : Robot<LabirentGrid>
    {
        public LabirentRobot(LabirentGrid grid) : base(grid)
        {
            grid.OnCreatedNewMaze += (s, e) =>
            {
                RobotveHedefYeriniGuncelle();
            };
        }
        public override void RobotYeriniDegistir(bool render = true)
        {
            RobotYeriniSifirla();
            if (render)
            {
                grid.HucreRenkleriniSifirla();
            }
            _robotCell = grid.HucreyiBul((int)grid.Entrance.X, (int)grid.Entrance.Y);
            _robotCell?.TipiAta(HucreTipi.Robot);
            if (render)
            {
                grid.RenderaZorla();
            }
        }
        public override void HedefYeriniDegistir(bool render = true)
        {
            HedefYeriniSifirla();
            if (render)
            {
                grid.HucreRenkleriniSifirla();
            }
            _targetCell = grid.HucreyiBul((int)grid.Exit.X, (int)grid.Exit.Y);
            _targetCell?.TipiAta(HucreTipi.Target);
            if (render)
            {
                grid.RenderaZorla();
            }

        }
    }
}

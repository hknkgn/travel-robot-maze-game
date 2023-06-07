using Proje.Grid.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.Robot
{
    /// <summary>
    /// FileGrid için oluşturulmuş Robot
    /// </summary>
    public class FileRobot : Robot<FileGrid>
    {
        public FileRobot(FileGrid fileGrid) : base(fileGrid)
        {
            fileGrid.OnLoadedFileChanged += (s, e) =>
            {
                RobotveHedefYeriniGuncelle();
            };
        }
    }
}

using Avalonia;
using Proje.Grid.Cell;
using Proje.Scripts;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Proje.Grid.Controls
{
    public class LabirentGrid : IntGrid
    {
        public event EventHandler? OnCreatedNewMaze;

        // Labirentin girişini yani robotun yerini belirler
        public Point Entrance { get; private set; } = new Point(0,0);

        // Labirentin çıkışını yani hedefin yerini belirler
        public Point Exit { get; private set; } = new Point(0, 0);
        public void CreateMaze(int width,int height)
        {
            Satirlar = width;
            Sutunlar = height;
            // Labirent oluştur
            var maze = LabirentOlusturucu.Olustur(width, height, out Point robot, out Point hedef);
            //var maze = LabirentOlusturucu.Olustur(width, height);
            // Print out the maze
            //for (int j = 0; j < maze.GetLength(1); j++)
            //{
            //    for (int i = 0; i < maze.GetLength(0); i++)
            //    {
            //        Debug.Write(maze[i, j] + " ");
            //    }
            //    Debug.WriteLine("");
            //}
            Entrance = robot;
            Exit = hedef;

            LoadFromInts(maze);

            OnCreatedNewMaze?.Invoke(this, EventArgs.Empty);
        }
    }
}

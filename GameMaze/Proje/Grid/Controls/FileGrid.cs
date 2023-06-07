using Proje.Grid.Cell;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proje.Grid.Controls
{
    /// <summary>
    /// Dosyadan yükelenbilir Grid
    /// </summary>
    public class FileGrid : IntGrid
    {
        // Dosya değiştiğinde bildirme için EventHandler
        public event EventHandler? OnLoadedFileChanged;
        public void LoadFromFile(string filePath)
        {
            // Dosyayının satırlarını oku
            string[] Lines = File.ReadAllLines(filePath);

            // yeni grid oluştur
            _gridDegerleri = new int[Lines.Length, Lines[0].Length];

            for (int i = 0; i < Lines.Length; i++)
            {
                // satırı karakter arrayine dönüştür ve grid değerlerini ata  
                char[] chars = Lines[i].ToCharArray();
                for (int j = 0; j < chars.Length; j++)
                {
                    if(int.TryParse(chars[j].ToString(), out int value))
                    {
                        _gridDegerleri[i, j] = value;
                    }
                }                
            }
            LoadFromInts(_gridDegerleri);
            OnLoadedFileChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}

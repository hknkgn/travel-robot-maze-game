using Proje.Grid.Cell;
using Proje.Grid.Classlar;
using Proje.Grid.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Proje.Scripts
{
    public class Pathfinding
    {
        public enum Algoritma
        {
            // Problem 1 için yaplan algooritma, 
            BFS,
            // Problem 2 için yapılan algoritma. Ama bazen  sorun çıkartabiliyor
            Wilson
        }


        // Animasyon arasındaki bekleme süresi (Milisaniye)
        private static readonly int beklemeSuresi = 1000 / 50;
        readonly Func<IGrid> GridFunc;
        public IGrid Grid
        {
            get
            {
                return GridFunc();
            }
        }


        public Hucre StartCell;
        public Hucre TargetCell;
        public Pathfinding(Func<IGrid> grid, Hucre start, Hucre end, bool animasyonuGec = false)
        {
            this.GridFunc = grid;
            this.StartCell = start;
            this.TargetCell = end;
            AnimasyonGecis(animasyonuGec);
        }

        private bool animasyonuGec;
        public void AnimasyonGecis(bool skip)
        {
            animasyonuGec = skip;
        }

        public TimeSpan BeklenenZaman { get; private set; }

        List<Hucre>? Yol = null;
        public async Task<PathFindSonuc> FindPath(Algoritma? algorithm)
        {
            var res = new PathFindSonuc();

            // Toplam zaman için süre
            Stopwatch sw = Stopwatch.StartNew();
            Yol = algorithm switch
            {
                Algoritma.BFS => await FindPathBFS(),
                Algoritma.Wilson => await FindPathWilson(),
                _ => await FindPathBFS(),
            };
            sw.Stop();
            Grid.RenderaZorla();
            if (Yol == null)
            {
                res.Hata = "Yol bulunamadı!";
                //MessageBox.Show("Yol bulunamadı!", "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return res;
            }
            await YoluCiz();

            res.Adim = Yol.Count;
            res.AlgoritmaZamani = BeklenenZaman;
            res.ToplamAnimasyonZamani = sw.Elapsed;

            return res;

        }
        private async Task YoluCiz()
        {
            if (Yol == null) return;
            foreach (var item in Yol)
            {
                item.Renk = HucreRenkleri.Yol;
                if (!animasyonuGec)
                {
                    Grid.RenderaZorla();
                    await Task.Delay(beklemeSuresi);
                }
            }
            if (animasyonuGec)
                Grid.RenderaZorla();
        }
        private async Task<List<Hucre>?> FindPathBFS()
        {
            Queue<Hucre> queue = new ();
            Dictionary<Hucre, Hucre> parentMap = new ();
            List<Hucre> ziyaretEdilenler = new();

            // Başlangıç hücresini sıraya ekle
            queue.Enqueue(StartCell);
            ziyaretEdilenler.Add(StartCell);

            var time = new List<TimeSpan>();
            while (queue.Count > 0)
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                // queue'dan bir sonraki hücreyi al
                Hucre? current = queue.Dequeue();

                // Seçilen hücre targetsa yolu oluşturup döndür
                if (current == TargetCell)
                {
                    List<Hucre> path = new ();
                    while (current != null)
                    {
                        path.Add(current);
                        current = parentMap.ContainsKey(current) ? parentMap[current] : null;
                    }
                    path.Reverse();

                    stopwatch.Stop();
                    time.Add(stopwatch.Elapsed);
                    BeklenenZaman = TimeSpan.Zero;
                    time.ForEach(x => { BeklenenZaman = BeklenenZaman.Add(x); });

                    return path;
                }

                // Komsularına bak
                foreach (Hucre komsu in Grid.KomsulariAl(current))
                {
                    // Eğer daha önce ziyaret edilmişse veya duvarsa geç
                    if (komsu.DuvarMı || ziyaretEdilenler.Contains(komsu))
                    {
                        continue;
                    }

                    queue.Enqueue(komsu);
                    ziyaretEdilenler.Add(komsu);

                    // Komsunun parent'ını current yap
                    parentMap[komsu] = current;
                }
                // süre ölçeri kapat
                stopwatch.Stop();
                time.Add(stopwatch.Elapsed);   

                // Renderlama için renkleri ayarla
                Grid.HucreRenkleriniSifirla();                
                foreach (var item in ziyaretEdilenler)
                {
                    item.Renk = HucreRenkleri.TarananAlan;
                }
                foreach (var item in queue)
                {
                    item.Renk = HucreRenkleri.KomsuAlan;
                }
                if (!animasyonuGec)
                {
                    Grid.RenderaZorla();
                    await Task.Delay(beklemeSuresi);
                }
            }

            // target'a herhangi bir yol yoksa null döndür.
            return null;
        }
        private async Task<List<Hucre>?> FindPathWilson()
        {
            Hucre baslangıcHucresi = StartCell;
            Hucre hedefHucre = TargetCell;

            Dictionary<Hucre, Hucre> parentMap = new ();
            HashSet<Hucre> ziyaretEdilenler = new ();
            Stack<Hucre> stack = new();

            stack.Push(baslangıcHucresi);

            while (stack.Count > 0)
            {
                // Stackten bir hücre al
                Hucre current = stack.Pop();

                // ziyaret edilmiş olarak işaretle
                ziyaretEdilenler.Add(current);

                // Render için renkleri ayarla
                Grid.HucreRenkleriniSifirla();
                foreach (var item in ziyaretEdilenler)
                {
                    item.Renk = HucreRenkleri.TarananAlan;
                }

                // Eğer hedef hücreyse yolu döndür
                if (current == hedefHucre)
                {
                    // Yolu oluştur
                    List<Hucre> path = new ();
                    Hucre node = hedefHucre;
                    while (node != baslangıcHucresi)
                    {
                        path.Add(node);
                        Hucre prev = parentMap[node];

                        // Eğer gideceğimiz yollardan birisi şu ankinin komşusuysa direkt komşuyu al
                        foreach (var item in parentMap)
                        {
                            if(Grid.KomsulariAl(item.Value).Contains(node))
                            {
                                prev = item.Value;
                                break;
                            }
                        }
                        node = prev;
                    }
                    path.Add(baslangıcHucresi);

                    path.Reverse();
                    return path;
                }

                // ziyaret edilmeyen komşuları al
                List<Hucre> neighbors = Grid.KomsulariAl(current);
                neighbors = neighbors.Where(n => !ziyaretEdilenler.Contains(n)).ToList();


                if (neighbors.Count == 0)
                {
                    // Eğer hiç komşu yoksa ve stack boşsa çıkmazdayız.
                    if (stack.Count > 0)
                    {
                        // Geri dön.
                        Hucre prev = stack.Pop();
                        // Bir önceki seçim hücresini bul
                        while (prev != null && !GridTemel.KomsuMu(current, prev))
                        {
                            prev = stack.Pop();
                        }
                        // Stack'e ekle
                        if (prev != null)
                        {
                            stack.Push(prev);
                            prev.Renk = HucreRenkleri.KomsuAlan;
                        }
                    }
                }
                else
                {
                    // Devam etmek için rastgele bir komşu seç
                    Hucre? komsu = neighbors.OrderBy(n => Guid.NewGuid()).FirstOrDefault();
                    if(komsu != null)
                    {
                        komsu.Renk = HucreRenkleri.KomsuAlan;
                        // Komşu parent'ını şimdiki hücre yap. (Son yoolu çizerken kullanıyoruz)
                        parentMap[komsu] = current;
                        stack.Push(current);
                        stack.Push(komsu);
                    }
                }


                // Renderla
                if (!animasyonuGec)
                {
                    Grid.RenderaZorla();
                    await Task.Delay(beklemeSuresi);
                }
            }

            // Yol bulunamadı
            return null; 
        }

    }

}

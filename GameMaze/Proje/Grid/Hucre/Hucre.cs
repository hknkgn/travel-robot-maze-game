using Avalonia;
using Avalonia.Media;

namespace Proje.Grid.Cell
{
    public class Hucre 
    {
        public Hucre(int row, int col, HucreTipi type)
        {
            this._row = row;
            this._column = col;
            this._hucreTipi = type;
        }

        private readonly int _row;
        private readonly int _column;

        private Color? _color;
        private HucreTipi _hucreTipi;

        public int Satir { get { return _row; } }
        public int Sutun { get { return _column; } }

        public bool DuvarMı
        {
            get
            {
                return _hucreTipi.DuvarMı();
            }
        }
        public HucreTipi HucreTipi
        {
            get
            {
                return _hucreTipi;
            }
        }

        public Color Renk
        {
            get
            {
                // Eğer renk manuel olarak ayarlanmadıya, hücre tipi rengini döndür.
                return _color ?? _hucreTipi.Renk();
            }
            set
            {
                _color = value;
            }
        }
        /// <summary>
        /// Hücreyi rengine göre çizer, ve üstüne hücre tipini yazar
        /// </summary>
        /// <param name="dc"></param>
        /// <param name="gensilik"></param>
        /// <param name="cellHeight"></param>
        public void Ciz(DrawingContext dc, int gensilik, int cellHeight)
        {
            int x = Sutun * gensilik;
            int y = Satir * cellHeight;
            Rect rect = new(x, y, gensilik, cellHeight);

            Brush brush = new SolidColorBrush(Renk);
            dc.DrawRectangle(brush, null, rect, 5, 5);

            if (_hucreTipi != 0)
            {
                string yazi = $"{(int)_hucreTipi}";

                // Yazıyı oluştur
                var formattedText = new FormattedText
                {
                    Text = yazi,
                    Typeface = new Typeface("Arial"),
                    FontSize = 13,
                    TextAlignment = TextAlignment.Left,
                    TextWrapping = TextWrapping.Wrap
                };
                // Ortayı belirle
                var textX = x + (gensilik - formattedText.Bounds.Width) / 2;
                var textY = y + (cellHeight - formattedText.Bounds.Height) / 2;

                // Çiz
                dc.DrawText(Brushes.Black, new Point(textX, textY), formattedText);
            }
        }
        /// <summary>
        /// Rengi HucreTipinin rengine çevirir
        /// </summary>
        public void RengiSifirla()
        {
            _color = null;
        }
        /// <summary>
        ///  Hücre tipini sıfırlar  
        /// </summary>
        public void TipiSifirla()
        {
            _hucreTipi = HucreTipi.Empty;
        }
        /// <summary>
        /// Hücre tipini değiştirir
        /// </summary>
        /// <param name="tip"></param>
        public void TipiAta(HucreTipi tip)
        {
            _hucreTipi = tip;
        }
    }
}

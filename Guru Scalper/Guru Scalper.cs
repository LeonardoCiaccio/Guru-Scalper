/*  CTRADER GURU --> Indicator Template 1.0.8

    Homepage    : https://ctrader.guru/
    Telegram    : https://t.me/ctraderguru
    Twitter     : https://twitter.com/cTraderGURU/
    Facebook    : https://www.facebook.com/ctrader.guru/
    YouTube     : https://www.youtube.com/channel/UCKkgbw09Fifj65W5t5lHeCQ
    GitHub      : https://github.com/ctrader-guru

*/

using System;
using cAlgo.API;
using cAlgo.API.Internals;
using cAlgo.API.Indicators;
using cAlgo.Indicators;

namespace cAlgo
{

    // --> Estensioni che rendono il codice più leggibile
    #region Extensions

    /// <summary>
    /// Estensione che fornisce metodi aggiuntivi per il simbolo
    /// </summary>
    public static class SymbolExtensions
    {

        /// <summary>
        /// Converte il numero di pips corrente da digits a double
        /// </summary>
        /// <param name="Pips">Il numero di pips nel formato Digits</param>
        /// <returns></returns>
        public static double DigitsToPips(this Symbol MySymbol, double Pips)
        {

            return Math.Round(Pips / MySymbol.PipSize, 2);

        }

        /// <summary>
        /// Converte il numero di pips corrente da double a digits
        /// </summary>
        /// <param name="Pips">Il numero di pips nel formato Double (2)</param>
        /// <returns></returns>
        public static double PipsToDigits(this Symbol MySymbol, double Pips)
        {

            return Math.Round(Pips * MySymbol.PipSize, MySymbol.Digits);

        }

    }

    /// <summary>
    /// Estensione che fornisce metodi aggiuntivi per le Bars
    /// </summary>
    public static class BarsExtensions
    {

        /// <summary>
        /// Converte l'indice di una bar partendo dalla data di apertura
        /// </summary>
        /// <param name="MyTime">La data e l'ora di apertura della candela</param>
        /// <returns></returns>
        public static int GetIndexByDate(this Bars MyBars, DateTime MyTime)
        {

            for (int i = MyBars.ClosePrices.Count - 1; i >= 0; i--)
            {

                if (MyTime == MyBars.OpenTimes[i])
                    return i;

            }

            return -1;

        }

    }

    #endregion

    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class GuruScalper : Indicator
    {


        #region Enums

        public enum MyColors
        {

            AliceBlue,
            AntiqueWhite,
            Aqua,
            Aquamarine,
            Azure,
            Beige,
            Bisque,
            Black,
            BlanchedAlmond,
            Blue,
            BlueViolet,
            Brown,
            BurlyWood,
            CadetBlue,
            Chartreuse,
            Chocolate,
            Coral,
            CornflowerBlue,
            Cornsilk,
            Crimson,
            Cyan,
            DarkBlue,
            DarkCyan,
            DarkGoldenrod,
            DarkGray,
            DarkGreen,
            DarkKhaki,
            DarkMagenta,
            DarkOliveGreen,
            DarkOrange,
            DarkOrchid,
            DarkRed,
            DarkSalmon,
            DarkSeaGreen,
            DarkSlateBlue,
            DarkSlateGray,
            DarkTurquoise,
            DarkViolet,
            DeepPink,
            DeepSkyBlue,
            DimGray,
            DodgerBlue,
            Firebrick,
            FloralWhite,
            ForestGreen,
            Fuchsia,
            Gainsboro,
            GhostWhite,
            Gold,
            Goldenrod,
            Gray,
            Green,
            GreenYellow,
            Honeydew,
            HotPink,
            IndianRed,
            Indigo,
            Ivory,
            Khaki,
            Lavender,
            LavenderBlush,
            LawnGreen,
            LemonChiffon,
            LightBlue,
            LightCoral,
            LightCyan,
            LightGoldenrodYellow,
            LightGray,
            LightGreen,
            LightPink,
            LightSalmon,
            LightSeaGreen,
            LightSkyBlue,
            LightSlateGray,
            LightSteelBlue,
            LightYellow,
            Lime,
            LimeGreen,
            Linen,
            Magenta,
            Maroon,
            MediumAquamarine,
            MediumBlue,
            MediumOrchid,
            MediumPurple,
            MediumSeaGreen,
            MediumSlateBlue,
            MediumSpringGreen,
            MediumTurquoise,
            MediumVioletRed,
            MidnightBlue,
            MintCream,
            MistyRose,
            Moccasin,
            NavajoWhite,
            Navy,
            OldLace,
            Olive,
            OliveDrab,
            Orange,
            OrangeRed,
            Orchid,
            PaleGoldenrod,
            PaleGreen,
            PaleTurquoise,
            PaleVioletRed,
            PapayaWhip,
            PeachPuff,
            Peru,
            Pink,
            Plum,
            PowderBlue,
            Purple,
            Red,
            RosyBrown,
            RoyalBlue,
            SaddleBrown,
            Salmon,
            SandyBrown,
            SeaGreen,
            SeaShell,
            Sienna,
            Silver,
            SkyBlue,
            SlateBlue,
            SlateGray,
            Snow,
            SpringGreen,
            SteelBlue,
            Tan,
            Teal,
            Thistle,
            Tomato,
            Transparent,
            Turquoise,
            Violet,
            Wheat,
            White,
            WhiteSmoke,
            Yellow,
            YellowGreen

        }

        #endregion

        #region Identity

        /// <summary>
        /// Nome del prodotto, identificativo, da modificare con il nome della propria creazione
        /// </summary>
        public const string NAME = "Guru Scalper";

        /// <summary>
        /// La versione del prodotto, progressivo, utilie per controllare gli aggiornamenti se viene reso disponibile sul sito ctrader.guru
        /// </summary>
        public const string VERSION = "1.0.0";

        #endregion

        #region Params
        
        /// <summary>
        /// Identità del prodotto nel contesto di ctrader.guru
        /// </summary>
        [Parameter(NAME + " " + VERSION, Group = "Identity", DefaultValue = "https://ctrader.guru/product/guru-scalper/")]
        public string ProductInfo { get; set; }

        [Parameter("Source", Group = "Params")]
        public DataSeries Source { get; set; }

        [Parameter("Period", Group = "Params", DefaultValue = 30, MinValue = 3)]
        public int Period { get; set; }

        [Parameter("MA Type", Group = "Params", DefaultValue = MovingAverageType.Exponential)]
        public MovingAverageType MAType { get; set; }

        [Parameter("K%", Group = "Filter", DefaultValue = 1.1)]
        public double K { get; set; }

        [Parameter("Period (zero = disable)", Group = "ATR", DefaultValue = 14, MinValue = 0)]
        public int ATRPeriod { get; set; }

        [Parameter("MA Type", Group = "ATR", DefaultValue = MovingAverageType.Exponential)]
        public MovingAverageType ATRMAType { get; set; }

        [Parameter("K%", Group = "ATR", DefaultValue = 6.5)]
        public double ATRK { get; set; }

        [Parameter("Long Color", Group = "Styles", DefaultValue = MyColors.DodgerBlue)]
        public MyColors LongColor { get; set; }

        [Parameter("Short Color", Group = "Styles", DefaultValue = MyColors.Red)]
        public MyColors ShortColor { get; set; }

        #endregion

        #region Property

        Color ColorLong;
        Color ColorShort;
        
        #endregion

        #region Indicator Events

        protected override void Initialize()
        {

            // --> Stampo nei log la versione corrente
            Print("{0} : {1}", NAME, VERSION);

            ColorLong = Color.FromName(LongColor.ToString("G"));
            ColorShort = Color.FromName(ShortColor.ToString("G"));

            K = Symbol.PipsToDigits(K);
            ATRK = Symbol.PipsToDigits(ATRK);

        }

        public override void Calculate(int index)
        {

            // --> Abbiamo bisogno di 3 candele per essere sicuri
            if (index < 3) return;

            // --> Gli indicatori che ci indicano il trend e il suo movimento
            DetrendedPriceOscillator Faster = Indicators.DetrendedPriceOscillator(Source, Period, MAType);
            DetrendedPriceOscillator Slower = Indicators.DetrendedPriceOscillator(Source, Period * 2, MAType);
            AverageTrueRange ATR = Indicators.AverageTrueRange(ATRPeriod, ATRMAType);
            
            // --> Imposto la logica della strategia
            bool LongFilterSlow = Slower.Result.LastValue > K;
            bool LongFilterFast = Faster.Result.LastValue > K;
            bool LongCross = Faster.Result.Last(1) > K && Faster.Result.Last(2) < -K && Faster.Result.Last(3) <= -K;

            bool ShortFilterSlow = Slower.Result.LastValue < -K;
            bool ShortFilterFast = Faster.Result.LastValue < -K;
            bool ShortCross = Faster.Result.Last(1) < -K && Faster.Result.Last(2) > K && Faster.Result.Last(3) >= K;

            bool LongCrossSlower = Slower.Result.Last(1) > K && Slower.Result.Last(2) < -K;
            bool ShortCrossSlower = Slower.Result.Last(1) < -K && Slower.Result.Last(2) > K;

            string SignalName = String.Format("{0}-{1}",NAME, index);

            // --> Se è attivato l'ATR con il periodo superiore a zero allora ne controllo la condizione
            if (ATRPeriod > 0 && ATR.Result.LastValue <= ATRK) return;

            if ( (LongFilterFast && LongCrossSlower) || (LongFilterSlow && LongCross))
            {

                Chart.DrawIcon(SignalName,ChartIconType.UpArrow,index, Bars.LowPrices[index], ColorLong);

            }
            else if ( ( ShortFilterFast && ShortCrossSlower ) || (ShortFilterSlow && ShortCross))
            {

                Chart.DrawIcon(SignalName, ChartIconType.DownArrow, index, Bars.HighPrices[index], ColorShort);

            }

        }

        #endregion

        #region Private Methods

        // --> Seguiamo la signature con underscore "_mioMetodo()"

        #endregion

    }

}

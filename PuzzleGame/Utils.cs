using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Point = System.Windows.Point;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;
using System.Windows;

namespace PuzzleGame {
    public static class Utils {


        public static Point Add ( this Point point, Point p2 ) {
            return new Point ( point.X + p2.X, point.Y + p2.Y );
        }

        public static Point Sub ( this Point point, Point p2 ) {
            return new Point ( point.X - p2.X, point.Y - p2.Y );
        }

        [DllImport ( "gdi32" )]
        static extern int DeleteObject ( IntPtr o );

        public static BitmapSource ToBitmapSource ( this System.Drawing.Bitmap source ) {
            IntPtr ip = source.GetHbitmap ();
            BitmapSource bs = null;
            try {
                bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap ( ip,
                   IntPtr.Zero, Int32Rect.Empty,
                   System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions () );
            }
            finally {
                DeleteObject ( ip );
            }

            return bs;
        }


        public static void Shuffle<T> ( this T[] list ) {
            Random rng = new Random ();
            int n = list.Length;
            while ( n > 1 ) {
                n--;
                int k = rng.Next ( n + 1 );
                T value = list[ k ];
                list[ k ] = list[ n ];
                list[ n ] = value;
            }
        }

    }
}

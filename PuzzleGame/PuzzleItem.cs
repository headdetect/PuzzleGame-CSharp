using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Point = System.Windows.Point;
using System.Drawing;

namespace PuzzleGame {
    public class PuzzleItem {

        public static readonly PuzzleItem EmptyPuzzle = new PuzzleItem ( null, new Point ( 2, 2 ) );


        public Bitmap Image { get; set; }

        public Point Location { get; set; }


        public PuzzleItem () :
            this ( null, new Point() ) {
        }

        public PuzzleItem ( Bitmap image, Point point ) {
            this.Image = image;
            this.Location = point;
        }
    }
}

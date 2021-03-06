﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using MessageBox = System.Windows.MessageBox;
using Utils = PuzzleGame.Utils;
using Point = System.Windows.Point;
using System.Drawing;
using System.Threading;
using System.Diagnostics;

namespace PuzzleGame {
    public class PuzzleController {

        public event EventHandler PuzzleUpdateInterface;
        public event EventHandler PuzzleFinished;

        public PuzzleItem[] Items {
            get;
            private set;
        }

        public Bitmap Image {
            get;
            set;
        }

        private readonly PuzzleItem[] _solveItems;


        public PuzzleController () {
            Items = new PuzzleItem[ 9 ];
            _solveItems = new PuzzleItem[ 9 ];

        }


        public bool Move ( PuzzleItem itm, bool update = true ) {
            if ( itm == null ) {
                return false;
            }

            Point curr = itm.Location;

            PuzzleItem top = GetItem ( curr.Add ( new Point ( 0, -1 ) ) );
            PuzzleItem left = GetItem ( curr.Add ( new Point ( -1, 0 ) ) );
            PuzzleItem bottom = GetItem ( curr.Add ( new Point ( 0, 1 ) ) );
            PuzzleItem right = GetItem ( curr.Add ( new Point ( 1, 0 ) ) );

            if ( top == PuzzleItem.EmptyPuzzle ) {
                itm.Location = curr.Add ( new Point ( 0, -1 ) );
                PuzzleItem.EmptyPuzzle.Location = curr;
                Items[ GetIndex ( itm ) ] = PuzzleItem.EmptyPuzzle;
                Items[ GetIndex ( top ) ] = itm;
            }
            else if ( left == PuzzleItem.EmptyPuzzle ) {
                itm.Location = curr.Add ( new Point ( -1, 0 ) );
                PuzzleItem.EmptyPuzzle.Location = curr;
                Items[ GetIndex ( itm ) ] = PuzzleItem.EmptyPuzzle;
                Items[ GetIndex ( left ) ] = itm;
            }
            else if ( bottom == PuzzleItem.EmptyPuzzle ) {
                itm.Location = curr.Add ( new Point ( 0, 1 ) );
                PuzzleItem.EmptyPuzzle.Location = curr;
                Items[ GetIndex ( itm ) ] = PuzzleItem.EmptyPuzzle;
                Items[ GetIndex ( bottom ) ] = itm;
            }
            else if ( right == PuzzleItem.EmptyPuzzle ) {
                itm.Location = curr.Add ( new Point ( 1, 0 ) );
                PuzzleItem.EmptyPuzzle.Location = curr;
                Items[ GetIndex ( itm ) ] = PuzzleItem.EmptyPuzzle;
                Items[ GetIndex ( right ) ] = itm;
            }
            else {
                if ( update )
                    CauseUpdate ();
                _checkFinish ();
                return false;
            }

            if ( update )
                CauseUpdate ();
            _checkFinish ();
            return true;
        }

        public void Shuffle () {
            var points = new Point[]{
                new Point(0, 0),
                new Point(1, 0),
                new Point(2, 0),
                new Point(0, 1),
                new Point(1, 1),
                new Point(2, 1),
                new Point(0, 2),
                new Point(1, 2),
                new Point(2, 2)
            };

            points.Shuffle ();


            for ( int i = 0; i < 9; i++ ) {
                Items[ i ].Location = points[ i ];
            }

            CauseUpdate ();
        }

        public PuzzleItem GetItem ( Point point ) {
            if ( point.X <= -1 || point.Y <= -1 || point.X >= 3 || point.Y >= 3 ) {
                return null;
            }

            for ( int i = 0; i < 9; i++ ) {
                PuzzleItem itm = Items[ i ];

                if ( itm.Location == point ) {
                    return itm;
                }
            }

            return null;
        }


        public void CreatePuzzles () {
            try {

                Bitmap full = Image;

                int w = full.Width;
                int h = full.Height;

                for ( int i = 0; i < 9; i++ ) {

                    int x = i / 3;
                    int y = i % 3;

                    int xOverflow = w % 3;
                    int yOverflow = h % 3;

                    int xPos = x * ( w - ( xOverflow ) ) / 3;
                    int yPos = y * ( h - ( yOverflow ) ) / 3;

                    int wSize = ( w - ( xOverflow ) ) / 3;
                    int hSize = ( h - ( yOverflow ) ) / 3;

                    if ( i != 8 ) {
                        Point point = new Point ( x, y );
                        Bitmap img = full.Clone ( new Rectangle ( xPos, yPos, wSize, hSize ), System.Drawing.Imaging.PixelFormat.Format32bppArgb );
                        Items[ i ] = new PuzzleItem ( img, point );
                        _solveItems[ i ] = new PuzzleItem ( img, point );
                    }
                    else {
                        Items[ i ] = PuzzleItem.EmptyPuzzle;
                        _solveItems[ i ] = PuzzleItem.EmptyPuzzle;
                    }

                }



                CauseUpdate ();

            }
            catch ( Exception e ) {
                MessageBox.Show ( "Error creating puzzle\n" + e.Message + Environment.NewLine + e.StackTrace );
            }
        }

        public int GetIndex ( PuzzleItem itm ) {
            for ( int i = 0; i < 9; i++ ) {
                if ( Items[ i ] == itm ) {
                    return i;
                }
            }

            throw new IndexOutOfRangeException ( "PuzzleItem not inbounds" );
        }


        internal void CauseUpdate () {
            if ( PuzzleUpdateInterface != null ) {
                PuzzleUpdateInterface ( this, new EventArgs () );
            }
        }

        private void _checkFinish () {

            for ( int i = 0; i < 9; i++ ) {
                PuzzleItem check = Items[ i ];
                PuzzleItem solve = _solveItems[ i ];

                if ( check.Location != solve.Location ) {
                    return;
                }
            }

            if ( PuzzleFinished != null ) {
                PuzzleFinished ( this, new EventArgs () );
            }

        }


        public void Reset () {
            for ( int i = 0; i < 9; i++ ) {
                PuzzleItem solve = _solveItems[ i ];

                Items[ i ].Location = solve.Location;
            }

            CauseUpdate ();
        }
    }

}

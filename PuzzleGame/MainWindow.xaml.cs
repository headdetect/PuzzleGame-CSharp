using System;
using System.Drawing;
using System.Windows;
using System.Windows.Input;
using System.Diagnostics;
using System.Timers;
using Microsoft.Win32;
using Image = System.Windows.Controls.Image;
using Point = System.Windows.Point;

namespace PuzzleGame {

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow {

        private readonly Image[] _imgs;
        private readonly PuzzleController _ctr;

        private readonly Timer _timer;
        private readonly Stopwatch _watch;

        private int _moveCount;

        public MainWindow () {
            _ctr = new PuzzleController ();

            InitializeComponent ();


            _imgs = new[] {
                img1,
                img2,
                img3,
                img4,
                img5,
                img6,
                img7,
                img8,
                img9,
            };



            for ( int i = 0; i < 9; i++ ) {
                Image img = _imgs[ i ];

                img.Tag = new Point ( i % 3, (int) ( i / 3 ) );

                img.MouseDown += ( a, e ) => {
                    if ( _started ) {
                        label2.Content = "Moves: " + ++_moveCount;
                        Point p = (Point) img.Tag;
                        _ctr.Move ( _ctr.GetItem ( p ) );
                    }
                };
            }

            _ctr.PuzzleFinished += ( a, e ) => {
                Dispatcher.BeginInvoke ( new Action ( () => {
                    _timer.Stop ();
                    _watch.Stop ();

                    //started = false;
                    _autoSolving = false;
                    MessageBox.Show ( "CONGRATS YOU WON. HAVE A COOKIE" );
                } ) );
            };

            _ctr.PuzzleUpdateInterface += ( a, e ) => {
                Dispatcher.BeginInvoke ( new Action ( () => {
                    for ( int i = 0; i < 9; i++ ) {
                        var img = _imgs[ i ];

                        var point = (Point) img.Tag;

                        for ( int j = 0; j < 9; j++ ) {
                            PuzzleItem itm = _ctr.Items[ j ];

                            if ( itm == PuzzleItem.EmptyPuzzle ) {
                                img.Source = null;
                                break;
                            }

                            if ( itm.Location == point ) {
                                img.Source = itm.Image.ToBitmapSource ();
                                break;
                            }

                        }

                    }
                } ) );
            };

            _watch = new Stopwatch ();
            _timer = new Timer ( 1 );

            
        }

        private bool _started;

        private void button1_Click ( object sender, RoutedEventArgs e ) {


            button1.Content = !_started ? "Stop" : "Start";
            _watch.Reset ();

            if ( !_started ) {
                _ctr.Image = getImage ();

                if ( _ctr.Image == null )
                    return;

                _ctr.CreatePuzzles ();

                _ctr.Shuffle ();

                _watch.Start ();

                _moveCount = 0;
                label2.Content = "Moves: 0";
                _timer.Elapsed += ( a, o ) => {
                    if ( _autoSolving ) {
                        Dispatcher.BeginInvoke ( new Action ( () => {
                            var points = new[]{
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

                                Point point = points[ i ];
                                PuzzleItem itm = _ctr.GetItem ( point );
                                _ctr.Move ( itm, false );

                            }

                            _ctr.CauseUpdate ();
                        } ) );
                    }

                    label1.Dispatcher.BeginInvoke ( new Action ( () => {
                        label1.Content = string.Format ( "Timer: {0:mm}:{0:ss}.{0:fff} ", _watch.Elapsed );
                    } ), null );
                };

                _timer.Start ();
            }
            else {
                _watch.Stop ();
                _timer.Stop ();

                _ctr.Reset ();
            }

            _started = !_started;
        }

        private Bitmap getImage() {
            var dialog = new OpenFileDialog {Filter = "Images (.bmp, .png, .jpg, .jpeg)|*.bmp;*.png;*.jpg;*.jpeg"};

            bool? result = dialog.ShowDialog();

            if ( result.HasValue && result.Value ) {
                return (Bitmap) System.Drawing.Image.FromFile( dialog.FileName );
            }
            return null;
        }

        private bool _autoSolving;

        private void Window_KeyDown ( object sender, KeyEventArgs e ) {
            if ( e.Key != Key.H || !_started )
                return;

            if ( MessageBox.Show ( "Would you like to auto solve?", "You cheater", MessageBoxButton.YesNo ) == MessageBoxResult.Yes ) {
                _autoSolving = true;
            }
        }
    }
}

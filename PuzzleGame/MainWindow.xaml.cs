using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Timers;
using Thread = System.Threading.Thread;
using ThreadStart = System.Threading.ThreadStart;

namespace PuzzleGame {

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        private Image[] imgs;
        private PuzzleController ctr;

        private Timer timer;
        private Stopwatch watch;

        public MainWindow () {
            ctr = new PuzzleController ();

            InitializeComponent ();


            imgs = new Image[] {
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
                Image img = imgs[ i ];

                img.Tag = new Point ( i % 3, i / 3 );

                img.MouseDown += ( a, e ) => {
                    if ( started ) {
                        Point p = (Point) img.Tag;
                        ctr.Move ( ctr.GetItem ( p ) );
                    }
                };
            }

            ctr.PuzzleFinished += ( a, e ) => {
                Dispatcher.BeginInvoke ( new Action ( () => {
                    timer.Stop ();
                    autoSolving = false;
                    MessageBox.Show ( "CONGRATS YOU WON. HAVE A COOKIE" );
                } ) );
            };

            ctr.PuzzleUpdateInterface += ( a, e ) => {
                Dispatcher.BeginInvoke ( new Action ( () => {
                    for ( int i = 0; i < 9; i++ ) {
                        Image img = imgs[ i ];

                        Point point = (Point) img.Tag;

                        for ( int j = 0; j < 9; j++ ) {
                            PuzzleItem itm = ctr.Items[ j ];

                            if ( itm == PuzzleItem.EMPTY_PUZZLE ) {
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

            watch = new Stopwatch ();
            timer = new Timer ( 1 );

            ctr.CreatePuzzles ( "woof.jpg" );
        }

        private bool started = false;

        private void button1_Click ( object sender, RoutedEventArgs e ) {


            button1.Content = !started ? "Stop" : "Start";
            watch.Reset ();

            if ( !started ) {
                ctr.Shuffle ();

                watch.Start ();
                timer.Elapsed += ( a, o ) => {
                    if ( autoSolving ) {
                        Dispatcher.BeginInvoke ( new Action ( () => {
                            Point[] points = new Point[]{
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
                                PuzzleItem itm = ctr.GetItem ( point );
                                ctr.Move ( itm, false );

                            }

                            ctr.causeUpdate ();
                        } ) );
                    }

                    label1.Dispatcher.BeginInvoke ( new Action ( () => { label1.Content = string.Format ( "Timer: {0:mm}:{0:ss}.{0:fff} ", watch.Elapsed ); } ), null );
                };

                timer.Start ();
            } else {
                watch.Stop ();
                timer.Stop ();

                ctr.Reset ();
            }

            started = !started;
        }

        private bool autoSolving = false;

        private void Window_KeyDown ( object sender, KeyEventArgs e ) {
            if ( e.Key == Key.H && started) {
                if ( MessageBox.Show ( "Would you like to auto solve?", "You cheater", MessageBoxButton.YesNo ) == MessageBoxResult.Yes ) {
                    autoSolving = true;
                }
            }
        }

    }
}

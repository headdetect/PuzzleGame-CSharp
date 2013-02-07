using System.Drawing;

namespace PuzzleGame {

    /// <summary>
    /// Native Rectangle
    /// </summary>
    public struct RECT {
        private int left;
        private int top;
        private int right;
        private int bottom;

        /// <summary>
        /// Gets or sets the top.
        /// </summary>
        /// <value>
        /// The top.
        /// </value>
        public int Top {
            get { return top; }
            set { top = value; }
        }

        /// <summary>
        /// Gets or sets the right.
        /// </summary>
        /// <value>
        /// The right.
        /// </value>
        public int Right {
            get { return right; }
            set { right = value; }
        }

        /// <summary>
        /// Gets or sets the bottom.
        /// </summary>
        /// <value>
        /// The bottom.
        /// </value>
        public int Bottom {
            get { return bottom; }
            set { bottom = value; }
        }
        /// <summary>
        /// Gets or sets the left.
        /// </summary>
        /// <value>
        /// The left.
        /// </value>
        public int Left {
            get { return left; }
            set { left = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RECT"/> struct.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <param name="top">The top.</param>
        /// <param name="bottom">The bottom.</param>
        public RECT( int left, int right, int top, int bottom ) {
            this.top = top;
            this.bottom = bottom;
            this.right = right;
            this.left = left;
        }

        /// <summary>
        /// Gets the height.
        /// </summary>
        public int Height {
            get {
                return Bottom - Top + 1;
            }
        }
        /// <summary>
        /// Gets the width.
        /// </summary>
        public int Width {
            get {
                return Right - Left + 1;
            }
        }
        /// <summary>
        /// Gets the size.
        /// </summary>
        public Size Size {
            get {
                return new Size( Width, Height );
            }
        }

        /// <summary>
        /// Gets the location.
        /// </summary>
        public Point Location {
            get {
                return new Point( Left, Top );
            }
        }


        /// <summary>
        /// Inflates the rectangle.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public void Inflate( int width, int height ) {
            this.Left -= width;
            this.Top -= height;
            this.Right += width;
            this.Bottom += height;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace EXYL
{
    public class GameObject
    {
        #region Member data

        private float _x;
        private float _xPrev;
        private float _y;
        private float _yPrev;
        private float _yOffset;
        private float _gravity;
        private float _gravX;
        private Bitmap _bmp;
        private float _xinc;
        private float _yinc;
        private float _yDrag;
        private bool _ownsBitmap;

        private bool _inverted;
        private bool _invertedPrev;

        private bool _onGround;

        private int _currentFrame;
        private int _totalFrames;
        private int _frameCountDown;
        private int _initialFrameCountDown;
        private int _frameWidth;

        #endregion member data
        #region properties

        public float X
        {
            get { return _x; }
            set { _x = value; }
        }
        public float XPrev
        {
            get { return _xPrev; }
            set { _xPrev = value; }
        }
        public float Y
        {
            get { return _y; }
            set { _y = value; }
        }
        public float YPrev
        {
            get { return _yPrev; }
            set { _yPrev = value; }
        }
        public float YOffset
        {
            get { return _yOffset; }
            set { _yOffset = value; }
        }
        public Bitmap Bmp
        {
            get { return _bmp; }
            set { _bmp = value; }
        }
        public int Width
        {
            get { return _bmp.Width; }
        }
        public int Height
        {
            get { return _bmp.Height; }
        }
        public float Xinc
        {
            get { return _xinc; }
            set { _xinc = value; }
        }
        public float Yinc
        {
            get { return _yinc; }
            set { _yinc = value; }
        }
        public float Gravity
        {
            get { return _gravity; }
            set { _gravity = value; }
        }
        public float GravX
        {
            get { return _gravX; }
            set { _gravX = value; }
        }
        public float YDrag
        {
            get { return _yDrag; }
            set { _yDrag = value; }
        }
        public bool Inverted
        {
            get { return _inverted; }
            set { _inverted = value; }
        }
        public bool InvertedPrev
        {
            get { return _invertedPrev; }
            set { _invertedPrev = value; }
        }
        public bool OnGround
        {
            get { return _onGround; }
            set { _onGround = value; }
        }
        public int CurrentFrame
        {
            get { return _currentFrame; }
            set { _currentFrame = value; }
        }
        public int TotalFrames
        {
            get { return _totalFrames; }
            set { _totalFrames = value; }
        }
        public int FrameCountDown
        {
            get { return _frameCountDown; }
            set { _frameCountDown = value; }
        }
        public int FrameWidth
        {
            get { return _frameWidth; }
            set { _frameWidth = value; }
        }
        public int InitialFrameCountDown
        {
            get { return _initialFrameCountDown; }
            set { _initialFrameCountDown = value; }
        }
        #endregion
        public void SetBitmap(Bitmap bmp, bool ownsBitmap)
        {
            _bmp = bmp;
            _ownsBitmap = ownsBitmap;

        }


        #region Constructorz and destructors
        ~GameObject()
        {
            if (_bmp != null && _ownsBitmap)
                _bmp.Dispose();
        }
        #endregion

        #region initialize

        #endregion //initialize
    }
}

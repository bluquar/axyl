using System;
using System.Collections.Generic;
using System.Text;

namespace EXYL
{
    public enum AnimationType
    {
        DoubleTap,
        Warp,
        Hit,
    }
    public class AnimationDef
    {
        public AnimationType _type;
        public int _totalFrames;
        public int _frameWidth;
        public int _initialFrameCountDown;

        public AnimationDef(AnimationType type, int totalFrames, int frameWidth, int initialFrameCountDown)
        {
            _type = type;
            _totalFrames = totalFrames;
            _frameWidth = frameWidth;
            _initialFrameCountDown = initialFrameCountDown;
        }
    }
    class Animation : GameObject
    {
        #region member data

        public static AnimationDef[] animationDefs =
            {
                new AnimationDef(AnimationType.DoubleTap, 4, 150, 3),
                new AnimationDef(AnimationType.Warp, 6, 75, 2),
                new AnimationDef(AnimationType.Hit, 3, 55, 2)
            };
        private AnimationType _type;
        private bool _facingLeft;
        private bool _terminate;

        #endregion //member data.

        #region properties

        public AnimationType type
        {
            get { return _type; }
        }
        public bool FacingLeft
        {
            get { return _facingLeft; }
            set { _facingLeft = value; }
        }
        public bool Terminate
        {
            get { return _terminate; }
            set { _terminate = value; }
        }
        
        #endregion //properties

        #region constructors

        public Animation(AnimationType type)
        {
            _type = type;
        }
        public Animation(AnimationDef ad)
        {
            _type = ad._type;
        }

        #endregion //constructors

        public static Animation GetAnimation(int x, int y, AnimationType type, bool facingLeft)
        {
            List<AnimationDef> defs = new List<AnimationDef>();
            foreach (AnimationDef ad in animationDefs)
            {
                if (type == ad._type)
                {
                    defs.Add(ad);
                    break;
                }
            }

            Animation a = new Animation(defs[0]);

            a.Yinc = 0;
            a.Xinc = 0;
            a.X = x;
            a.Y = y;
            a.CurrentFrame = 1;
            a.FrameCountDown = (defs[0]._initialFrameCountDown);
            a.InitialFrameCountDown = a.FrameCountDown;
            a.FrameWidth = (defs[0]._frameWidth);
            a.TotalFrames = (defs[0]._totalFrames);
            a.Gravity = 0;
            a.FacingLeft = facingLeft;
            if (a.FacingLeft)
            {
                a.Inverted = false;
                a.InvertedPrev = false;
            }
            else if (!(a.FacingLeft))
            {
                a.Inverted = true;
                a.InvertedPrev = false;
            }
            a.XPrev = a.X;
            a.YPrev = a.Y;
            a.YOffset = 0;
            a.Terminate = false;

            return a;
        }

    }
}

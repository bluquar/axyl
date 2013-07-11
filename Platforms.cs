using System;
using System.Collections.Generic;
using System.Text;

namespace EXYL
{
    public class PlatformPoint
    {
        
        public int _id;
        public int _leftPointX;
        public int _leftPointY;
        public int _rightPointX;
        public int _rightPointY;
        public int _yOffset;
        public int _map;
        

        public PlatformPoint(int id, int leftPointX, int leftPointY, int rightPointX,
                              int rightPointY, int yOffset, int map)
        {
            _id = id;
            _leftPointX = leftPointX;
            _leftPointY = leftPointY;
            _rightPointX = rightPointX;
            _rightPointY = rightPointY;
            _yOffset = yOffset;
            _map = map;
        }
    }

    class Platforms : GameObject
    {
        #region member data
        public static PlatformPoint[] platformPoints = 
            {
                new PlatformPoint(1, 0, 530, 1160, 530, 0, 1),
                new PlatformPoint(2, 100, 200, 720, 200, 0, 1),
                new PlatformPoint(3, 400, 365, 525, 365, 15, 1),
                new PlatformPoint(4, 780, 180, 907, 180, 15, 1),
                new PlatformPoint(5, 500, -150, 625, -150, 15, 1),
                new PlatformPoint(6, -150, -450, 290, -450, 0, 1),
                new PlatformPoint(7, -1600, 530, -440, 530, 0, 1),
                new PlatformPoint(8, -1450, 100, -1325, 100, 15, 1),
                new PlatformPoint(9, 650, 430, 796, 430, 0, 1),
                new PlatformPoint(10, -1700, 120, -1556, 120, 0, 1),
                new PlatformPoint(11, -1950, 75, -1803, 75, 0, 1),
                new PlatformPoint(12, -1550, 1000, -1260, 1000, 0, 1),
                new PlatformPoint(13, -1472, 986, -1338, 986, 0, 1),
                new PlatformPoint(14, -2800, 400, -2180, 400, 0, 1),
                new PlatformPoint(15, -3200, 550, -2580, 550, 0, 1),
                new PlatformPoint(16, 1800, 570, 2420, 570, 0, 1),

                new PlatformPoint(17, 0, 530, 147, 530, 3, 2),
                new PlatformPoint(18, 200, 700, 490, 700, 0, 2),
                new PlatformPoint(19, -350, 600, 50, 600, 0, 2),
                new PlatformPoint(20, -500, 245, -353, 245, 3, 2),
                new PlatformPoint(21, 550, 100, 890, 115, 2, 2),
                new PlatformPoint(22, 1200, -110, 1322, -110, 4, 2),
                new PlatformPoint(23, 1550, -340, 1672, -340, 4, 2),
                new PlatformPoint(24, 1200, -579, 1322, -579, 4, 2),
                new PlatformPoint(25, 350, -710, 1034, -710, 3, 2),

                new PlatformPoint(26, 0, 500, 620, 500, 0, 3),
                new PlatformPoint(27, 800, 400, 1318, 316, 89, 3),
                new PlatformPoint(28, -1200, 0, -596, 0, 89, 3),
                new PlatformPoint(29, -500, -650, 700, -850, 220, 3),

                new PlatformPoint(30, 0, 500, 290, 500, 0, 4),
                new PlatformPoint(31, 78, 486, 212, 486, 0, 4),
                new PlatformPoint(32, 1100, 0, 1433, 0, 68, 4),
                new PlatformPoint(33, 1833, 0, 2133, 0, 68, 4),
                new PlatformPoint(34, 1466, -250, 1799, -250, 68, 4),
                new PlatformPoint(35, 1100, -400, 1433, -400, 68, 4),
                new PlatformPoint(36, 1833, -500, 2133, -500, 68, 4),
                new PlatformPoint(37, 300, 500, 590, 500, 0, 4)

            };
        private int _leftX;
        private int _leftY;
        private int _rightX;
        private int _rightY;
        private int _yOffsett;
        private float _length;
        private int _map;

        #endregion //member data

        #region properties

        public int LeftX
        {
            get { return _leftX; }
            set { _leftX = value; }
        }
        public int LeftY
        {
            get { return _leftY; }
            set { _leftY = value; }
        }
        public int RightX
        {
            get { return _rightX; }
            set { _rightX = value; }
        }
        public int RightY
        {
            get { return _rightY; }
            set { _rightY = value; }
        }
        public float Length
        {
            get { return _length; }
            set { _length = value; }
        }
        public int YOffsett
        {
            get { return _yOffsett; }
            set { _yOffsett = value; }
        }
        public int Map
        {
            get { return _map; }
            set { _map = value; }
        }

        #endregion //properties

        #region constructors

        public static Platforms GetPlatform(int id)
        {
            List<PlatformPoint> points = new List<PlatformPoint>();
            
            foreach (PlatformPoint pp in platformPoints)
            {
                if (id == pp._id)
                    points.Add(pp);
            }
            Platforms plat = new Platforms();
            plat.LeftX = points[0]._leftPointX;
            plat.LeftY = points[0]._leftPointY;
            plat.RightX = points[0]._rightPointX;
            plat.RightY = points[0]._rightPointY;
            plat.X = plat.LeftX;
            plat.Y = plat.LeftY;
            plat.Gravity = 0;
            plat.Xinc = 0;
            plat.Yinc = 0;
            plat.Length = (float)Math.Sqrt(((plat.RightX - plat.LeftX) * (plat.RightX - plat.LeftX))
                                    + ((plat.LeftY - plat.RightY) * (plat.LeftY - plat.RightY)));
            plat.YOffsett = points[0]._yOffset;

            plat.CurrentFrame = 1;
            plat.FrameCountDown = 1;
            
            plat.InitialFrameCountDown = 1;
            plat.TotalFrames = 1;            

            return plat;
        }

        #endregion //constructors

    }
}

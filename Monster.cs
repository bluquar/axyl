using System;
using System.Collections.Generic;
using System.Text;

namespace EXYL
{
    public enum MonsterType
    {
        Blob,
        Snail,
        GreenHat,
        Boomy,
        YellowHat,
        Cel,
        BluePotion,

        Robiera2,
        Pepe,
        ManaElixir,
        OrangeSnail,
        FireBoar,

        SnowRed,
        SnowMan,
        Yeti,

        Robiera,
    }
    public class MonsterDef
    {
        public MonsterType _type;
        public int _initialHP;
        public int _initialSpeed;
        public int _initialJump;
        public int _exp;
        public int[] _spawnPtsX;
        public int[] _spawnPtsY;
        public int _map;
        public int _id;
        public int _idd;
        public int _mpMax;

        public int _totalFrames;
        public int _frameWidth;
        public int _initialFrameCountDown;
        public int _yOffset;

        public MonsterDef(MonsterType type, int initialHP, int initialSpeed, int initialJump,
                          int exp, int[] spawnPtsX, int[] spawnPtsY, int map, int id, int idd, int mpMax,
                          int totalFrames, int frameWidth, int initialFrameCountDown, int yOffset)
        {
            _type = type;
            _initialHP = initialHP;
            _initialSpeed = initialSpeed;
            _initialJump = initialJump;
            _exp = exp;
            _spawnPtsX = spawnPtsX;
            _spawnPtsY = spawnPtsY;
            _map = map;
            _id = id;
            _idd = idd;
            _mpMax = mpMax;

            _totalFrames = totalFrames;
            _frameWidth = frameWidth;
            _initialFrameCountDown = initialFrameCountDown;
            _yOffset = yOffset;
        }
    }
    class Monster : GameObject
    {
        #region member data

        public static MonsterDef[] monsterDefs = 
            {
                new MonsterDef(MonsterType.Blob, 50, 2, 15, 55, new int[]{400,200}, new int[]{530,200}, 1, 1, 1, 4, 4, 76, 14, 1),
                new MonsterDef(MonsterType.Snail, 10, 1, 0, 7, new int[]{100,200,300,400}, new int[]{530,530,530,530}, 1, 2, 2, 0, 6, 40, 10, 0),
                new MonsterDef(MonsterType.GreenHat, 250, 3, 20, 100, new int[]{-1000,-800}, new int[]{200,200}, 1, 3, 3, 12, 4, 170, 16, 0),
                new MonsterDef(MonsterType.Boomy, 64444, 0, 10, 1000, new int[]{-1440, -1405}, new int[]{986,986}, 1, 4, 4, 8, 4, 60, 5, 0),
                new MonsterDef(MonsterType.YellowHat, 5000, 1, 5, 450, new int[]{-2700,-2900,-2900,-2900,2100}, new int[]{398,548,547,546,530}, 1, 5, 5, 16, 4, 69, 7, 0),        
                new MonsterDef(MonsterType.Cel, 999999, 5, 0, 30000, new int[]{2000}, new int[]{530}, 1, 6, 6, 124, 4, 70, 4, 0),
                new MonsterDef(MonsterType.BluePotion, 1, 0, 0, 0, new int[]{2000,2293,-2900,-800}, new int[]{530,-301,548,200}, 1, 7, 7, 600, 4, 38, 10, 0),

                new MonsterDef(MonsterType.Robiera2, 12000, 1, 0, 1200, new int[]{10},new int[]{500}, 2, 1, 8, 32, 5, 51, 8, 0),
                new MonsterDef(MonsterType.Pepe, 25000, 4, 0, 2350, new int[]{210,450}, new int[]{700,700}, 2, 2, 9, 100, 3, 77, 4, 0) ,
                new MonsterDef(MonsterType.ManaElixir, 1, 0, 0, 0, new int[]{0,10,20}, new int[]{600,600,600}, 2, 3, 10, 1500, 4, 32, 10, 0),
                new MonsterDef(MonsterType.OrangeSnail, 10000, 1, 0, 1100, new int[]{650}, new int[]{100}, 2, 4, 11, 50, 4, 41, 6, 0),
                new MonsterDef(MonsterType.FireBoar, 40000, 4, 6, 3750, new int[]{500,800}, new int[]{-711,-711}, 2, 5, 12, 40, 4, 85, 4, 0),

                new MonsterDef(MonsterType.SnowRed, 110000, 3, 3, 7000, new int[]{10}, new int[]{500}, 3, 1, 13, 150, 4, 42, 6,0),
                new MonsterDef(MonsterType.SnowMan, 250000, 5, 2, 14000, new int[]{1000,1200}, new int[]{320,300}, 3, 2, 14, 120, 4, 60, 5, 0),
                new MonsterDef(MonsterType.Yeti, 500000, 1, 0, 25000, new int[]{-700,-800,0}, new int[]{0,0, -1000}, 3, 3, 15, 600, 4, 121, 6, 0),

                new MonsterDef(MonsterType.Robiera, 600000, 1, 0, 36250, new int[]{1260,1993,1596,1656,1260,1993}, new int[]{-1,-1,-251,-251,-401,-501}, 4, 1, 16, 200, 5, 51, 8, 0),
            };
        private static Random _rnd = new Random((int)DateTime.Now.Ticks);
        private MonsterType _type;
        private int _hp;
        private int _hpInit;
        private int _mp;
        private int _mpMax;
        private int _speed;
        private int _jump;
        private List<Platforms> _platOn;
        private int _exp;
        private int _id;
        
        private bool _facingLeft;

        #endregion //member data

        #region properties

        public MonsterType Type
        {
            get { return _type; }
        }
        public int Hp
        {
            get { return _hp; }
            set { _hp = value; }
        }
        public int HpInit
        {
            get { return _hpInit; }
            set { _hpInit = value; }
        }
        public int Mp
        {
            get { return _mp; }
            set { _mp = value; }
        }
        public int MpMax
        {
            get { return _mpMax; }
            set { _mpMax = value; }
        }
        public int Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }
        public int Jump
        {
            get { return _jump; }
            set { _jump = value; }
        }
        public List<Platforms> PlatOn
        {
            get { return _platOn; }
            set { _platOn = value; }
        }
        public int Exp
        {
            get { return _exp; }
            set { _exp = value; }
        }
        public bool FacingLeft
        {
            get { return _facingLeft; }
            set { _facingLeft = value; }
        }
        public int Id
        {
            get { return _id; }
        }

        #endregion properties

        #region constructors

        public Monster(MonsterType type)
        {
            _type = type;
        }
        public Monster(MonsterDef md)
        {
            _type = md._type;
            _hp = md._initialHP;
            _hpInit = md._initialHP;
            _mp = md._mpMax;
            _mpMax = md._mpMax;
            _speed = md._initialSpeed;
            _jump = md._initialJump;
            _exp = md._exp;
            _id = md._id;
            
        }

        #endregion //constructors

        public static Monster GetMonster(int id, int map)
        {
            List<MonsterDef> defs = new List<MonsterDef>();
            foreach (MonsterDef md in monsterDefs)
            {
                if (id == md._id && map == md._map)
                {
                    defs.Add(md);
                    break;
                }
            }

            Monster m = new Monster(defs[0]);

            m.Yinc = 0;

            
             

            int[] spawnx = new int[]{44};
            spawnx = (defs[0]._spawnPtsX);
            int[] spawny = new int[] { 1337 };
            spawny = (defs[0]._spawnPtsY);

            int whichspawn = _rnd.Next(0, spawnx.Length);

            m.X = spawnx[whichspawn];           
            m.Y = spawny[whichspawn];

            m.CurrentFrame = 1;
            m.FrameCountDown = (defs[0]._initialFrameCountDown);
            m.InitialFrameCountDown = m.FrameCountDown;
            m.FrameWidth = (defs[0]._frameWidth);
            m.TotalFrames = (defs[0]._totalFrames);
            m.Jump = (defs[0]._initialJump);
            m.Gravity = -0.5f;
            m.FacingLeft = false;
            if (_rnd.Next(1, 10) <= 5)
            {
                m.FacingLeft = true;
            }
            if (m.FacingLeft)
            {
                m.Inverted = false;
                m.InvertedPrev = false;
                m.Xinc = -(m.Speed);
            }
            else if (!(m.FacingLeft))
            {
                m.Inverted = true;
                m.InvertedPrev = false;
                m.Xinc = m.Speed;
            }
            m.XPrev = m.X;
            m.YPrev = m.Y;
            m.YOffset = (defs[0]._yOffset);
            m.PlatOn = new List<Platforms>();
            
            return m;
        }
    }
}

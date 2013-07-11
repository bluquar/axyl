using System;
using System.Collections.Generic;
using System.Text;

namespace EXYL
{
    public enum DamageType
    {
        Zero,
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine
    }
    public class DamageDef
    {
        public DamageType _type;
        public int _number;

        public DamageDef(DamageType type, int number)
        {
            _type = type;
            _number = number;
        }
    }



    public class Damage : GameObject
    {
        #region member data

        public static DamageDef[] damageDefs =
            {
                new DamageDef(DamageType.Zero, 0),
                new DamageDef(DamageType.One, 1),
                new DamageDef(DamageType.Two, 2),
                new DamageDef(DamageType.Three, 3),
                new DamageDef(DamageType.Four, 4),
                new DamageDef(DamageType.Five, 5),
                new DamageDef(DamageType.Six, 6),
                new DamageDef(DamageType.Seven, 7),
                new DamageDef(DamageType.Eight, 8),
                new DamageDef(DamageType.Nine, 9),
            };
        private DamageType _type;
        private int _number;

        #endregion //memberdata


        #region properties

        public DamageType Type
        {
            get { return _type; }
        }
        public int Number
        {
            get { return _number; }
        }


        #endregion


        #region constructors

        public Damage(DamageType type)
        {
            _type = type;
        }
        public Damage(DamageDef dd)
        {
            _type = dd._type;
            _number = dd._number;
        }


        #endregion


        public static Damage CreateNumber(int n, float x, float y)
        {
            List<DamageDef> defs = new List<DamageDef>();

            foreach (DamageDef dd in damageDefs)
            {
                if (n == dd._number)
                    defs.Add(dd);
            }

            Damage m = new Damage(defs[0]);
            m.X = x;
            m.XPrev = m.X;
            
            m.Y = y;
            m.YPrev = m.Y;

            m.Xinc = 0;
            m.Yinc = -2;;
            m.InitialFrameCountDown = 40;
            m.FrameCountDown = m.InitialFrameCountDown;
            m.TotalFrames = 1;
            m.CurrentFrame = 1;
            m.Inverted = false;
            
            m.YDrag = .985f;
            return m;
        }
    }
}


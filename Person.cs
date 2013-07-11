using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace EXYL
{
    class Person : GameObject
    {
        private int _weaponAttack;
        private float _xRendOffset;
        private float _speed;
        private Bitmap _walkingBmp;
        private Bitmap _jumpingBmp;
        private Bitmap _standingBmp;
        private Bitmap _throwingBmp;
        private Bitmap _throwing2Bmp;
        private Bitmap _throwing3Bmp;
        private List<Platforms> _platOn = new List<Platforms>();

        public int WeaponAttack
        {
            get { return _weaponAttack; }
            set { _weaponAttack = value; }
        }
        public float XRendOffset
        {
            get { return _xRendOffset; }
            set { _xRendOffset = value; }
        }
        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }
        public Bitmap WalkingBmp
        {
            get { return _walkingBmp; }
            set { _walkingBmp = value; }
        }
        public Bitmap JumpingBmp
        {
            get { return _jumpingBmp; }
            set { _jumpingBmp = value; }
        }
        public Bitmap StandingBmp
        {
            get { return _standingBmp; }
            set { _standingBmp = value; }
        }
        public Bitmap ThrowingBmp
        {
            get { return _throwingBmp; }
            set { _throwingBmp = value; }
        }
        public Bitmap Throwing2Bmp
        {
            get { return _throwing2Bmp; }
            set { _throwing2Bmp = value; }
        }
        public Bitmap Throwing3Bmp
        {
            get { return _throwing3Bmp; }
            set { _throwing3Bmp = value; }
        }
        public List<Platforms> PlatOn
        {
            get { return _platOn; }
            set { _platOn = value; }
        }
        public void SetBitmap1(Bitmap bmp)
        {
            _standingBmp = bmp;
        }
        public void SetBitmap2(Bitmap bmp)
        {
            _walkingBmp = bmp;
        }
        public void SetBitmap3(Bitmap bmp)
        {
            _jumpingBmp = bmp;
        }
        public void SetBitmap4(Bitmap bmp)
        {
            _throwingBmp = bmp;
        }
        public void SetBitmap5(Bitmap bmp)
        {
            _throwing2Bmp = bmp;
        }
        public void SetBitmap6(Bitmap bmp)
        {
            _throwing3Bmp = bmp;
        }
    }
}

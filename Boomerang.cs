using System;
using System.Collections.Generic;
using System.Text;

namespace EXYL
{
    /*   So basically this class will be like the Monster class in terms of structure (not)
     *   And then, similar to Jumping, there will be variables/fxns in Game.cs for throwing it
     * 
     *   There needs to be GravX (and GravY) so the boomerang gravitates toward the person
     *   And it needs to set off from the person with a certain Xinc speed
     * 
     *   When the boomerang hits a monster, it creates damage and decrements the monsters hp (or kills)
     *   When the boomerang hits the person, it disappears and the person is then able to use the skill again
     * 
     *   
     *   
    */
    class Boomerang : GameObject
    {
        private int _damageRate;
        private int _hitsLeft;
        private int _timeLeft;
        private int _cooldown;
        private int _range;
        private int _trajectory;
        private int _yDamageOffset;
        private bool _primaryBoomerang;
        private float _gravRate;
        private List<ListRemover> _monstersHit;

        public int DamageRate
        {
            get { return _damageRate; }
            set { _damageRate = value; }
        }
        public int HitsLeft
        {
            get { return _hitsLeft; }
            set { _hitsLeft = value; }
        }
        public int Cooldown
        {
            get { return _cooldown; }
            set { _cooldown = value; }
        }
        public int TimeLeft
        {
            get { return _timeLeft; }
            set { _timeLeft = value; }
        }
        public int Range
        {
            get { return _range; }
            set { _range = value; }
        }
        public int Trajectory
        {
            get { return _trajectory; }
            set { _trajectory = value; }
        }
        public int YDamageOffset
        {
            get { return _yDamageOffset; }
            set { _yDamageOffset = value; }
        }
        public bool PrimaryBoomerang
        {
            get { return _primaryBoomerang; }
            set { _primaryBoomerang = value; }
        }
        public float GravRate
        {
            get { return _gravRate; }
            set { _gravRate = value; }
        }
        public List<ListRemover> MonstersHit
        {
            get { return _monstersHit; }
            set { _monstersHit = value; }
        }
    }
}

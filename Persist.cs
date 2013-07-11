using System;
using System.Collections.Generic;
using System.Text;

namespace EXYL
{
    public class GameSave
    {
        List<PlayerPersist> _players;

        public List<PlayerPersist> Players
        {
            get { return _players; }
            set { _players = value; }
        }

        public GameSave()
        {

        }
    }
    public class PlayerPersist
    {
        private const double _version = 1.00015;

        private string _name;
        private int _level;
        private int _job;
        private long _currentExp;
        private long _expTNL;
        private int _damageRate;
        private int _skillPoints;
        private int _maxBoomerangsActive;

        private int[] _firstJobSkillLvls;

        private int _powerThrowLevel;
        private int _doubleTapLevel;
        private int _doubleThrowLevel;
        private int _eagleEyeLevel;

        private int[] _secondJobSkillLvls;

        private int _warpLevel;
        private int _powerSliceLevel;
        private int _assassinateLevel;
        private int _mpEaterLevel;

        private int[] _thirdJobSkillLvls;

        private int _skullCrusherLevel;
        private int _spectrumLevel;
        private int _fleetLevel;
        private int _orbitLevel;

        private int[] _fourthJobSkillLvls;

        private int _firebladeLevel;
        private int _grappleLevel;

        private int _mp;

        private int _patchVersion;
        

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public int Level
        {
            get { return _level; }
            set { _level = value; }
        }
        public int Job
        {
            get { return _job; }
            set { _job = value; }
        }
        public long CurrentExp
        {
            get { return _currentExp; }
            set { _currentExp = value; }
        }
        public long ExpTNL
        {
            get { return _expTNL; }
            set { _expTNL = value; }
        }
        public int DamageRate
        {
            get { return _damageRate; }
            set { _damageRate = value; }
        }
        public int SkillPoints
        {
            get { return _skillPoints; }
            set { _skillPoints = value; }
        }
        public int MaxBoomerangsActive
        {
            get { return _maxBoomerangsActive; }
            set { _maxBoomerangsActive = value; }
        }
        public int[] FirstJSL //first job skill levels
        {
            get { return _firstJobSkillLvls; }
            set { _firstJobSkillLvls = value; }
        }
        public int[] SecondJSL //second job skill levels
        {
            get { return _secondJobSkillLvls; }
            set { _secondJobSkillLvls = value; }
        }
        public int[] ThirdJSL //third job skill levels
        {
            get { return _thirdJobSkillLvls; }
            set { _thirdJobSkillLvls = value; }
        }
        public int[] FourthJSL //fourth job skill levels
        {
            get { return _fourthJobSkillLvls; }
            set { _fourthJobSkillLvls = value; }
        }

        public int PowerThrowLevel
        {
            get { return _powerThrowLevel; }
            set { _powerThrowLevel = value; }
        }
        public int DoubleTapLevel
        {
            get { return _doubleTapLevel; }
            set { _doubleTapLevel = value; }
        }
        public int DoubleThrowLevel
        {
            get { return _doubleThrowLevel; }
            set { _doubleThrowLevel = value; }
        }
        public int EagleEyeLevel
        {
            get { return _eagleEyeLevel; }
            set { _eagleEyeLevel = value; }
        }
        public int WarpLevel
        {
            get { return _warpLevel; }
            set { _warpLevel = value; }
        }
        public int PowerSliceLevel
        {
            get { return _powerSliceLevel; }
            set { _powerSliceLevel = value; }
        }
        public int AssassinateLevel
        {
            get { return _assassinateLevel; }
            set { _assassinateLevel = value; }
        }
        public int SkullCrusherLevel
        {
            get { return _skullCrusherLevel; }
            set { _skullCrusherLevel = value; }
        }
        public int SpectrumLevel
        {
            get { return _spectrumLevel; }
            set { _spectrumLevel = value; }
        }
        public int MpEaterLevel
        {
            get { return _mpEaterLevel; }
            set { _mpEaterLevel = value; }
        }
        public int FleetLevel
        {
            get { return _fleetLevel; }
            set { _fleetLevel = value; }
        }
        public int Mp
        {
            get { return _mp; }
            set { _mp = value; }
        }
        public double Version
        {
            get { return _version; }
        }
        public int OrbitLevel
        {
            get { return _orbitLevel; }
            set { _orbitLevel = value; }
        }
        public int FirebladeLevel
        {
            get { return _firebladeLevel; }
            set { _firebladeLevel = value; }
        }
        public int GrappleLevel
        {
            get { return _grappleLevel; }
            set { _grappleLevel = value; }
        }
        public int PatchVersion
        {
            get { return _patchVersion; }
            set { _patchVersion = value; }
        }
        

        //public PlayerPersist(string name, int level, int job, long currentExp, long expTNL, int damageRate,
        //                     int skillPoints, int maxBoomerangsActive, int powerThrowLevel, int doubleTapLevel,
        //                     int doubleThrowLevel, int eagleEyeLevel, int warpLevel, int powerSliceLevel,
        //                     int assassinateLevel, int mp, int mpEaterLevel, int spectrumLevel, int fleetLevel,
        //                     int skullCrusherLevel, int orbitLevel, int firebladeLevel)
        public PlayerPersist(string name, int level, int job, long currentExp, long expTNL, int damageRate,
                         int skillPoints, int maxBoomerangsActive, int mp, int[] firstJobSkillLvls, int[] secondJobSkillLvls,
                         int[] thirdJobSkillLvls, int[] fourthJobSkillLvls, int patchVersion)
        {
            _name = name;
            _level = level;
            _job = job;
            _currentExp = currentExp;
            _expTNL = expTNL;
            _damageRate = damageRate;
            _skillPoints = skillPoints;
            _patchVersion = patchVersion;


            /*_powerThrowLevel = powerThrowLevel;
            _doubleTapLevel = doubleTapLevel;
            _doubleThrowLevel = doubleThrowLevel;
            _eagleEyeLevel = eagleEyeLevel;
            _warpLevel = warpLevel;
            _powerSliceLevel = powerSliceLevel;
            _assassinateLevel = assassinateLevel;
            _mpEaterLevel = mpEaterLevel;
            _skullCrusherLevel = skullCrusherLevel;
            _spectrumLevel = spectrumLevel;
            _fleetLevel = fleetLevel;
            _orbitLevel = orbitLevel;
            _firebladeLevel = firebladeLevel;*/

            _firstJobSkillLvls = firstJobSkillLvls;
            _secondJobSkillLvls = secondJobSkillLvls;
            _thirdJobSkillLvls = thirdJobSkillLvls;
            _fourthJobSkillLvls = fourthJobSkillLvls;
            
            _mp = mp;

        }
        public PlayerPersist()
        {

        }
    }
}

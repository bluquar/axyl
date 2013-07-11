using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.IO;
using System.Xml.Serialization;

namespace EXYL
{
    public partial class Game : Form
    {
        #region constants

        const int _monsterSpawnRate = 50;

        /* Experiment with different spawn rates.
         * Create a new monster <<MORE POWERFUL>>
         * Implement SKILLS (different sloping boomerangs, more powerful, more range etc)
         * Implement passive skills that give more range,speed,throwing speed, etc
         * Make a way to switch between maps, and make the maps bigger
         * get rid of the sammich
         * add hp, and some way for the monsters to hit you
         * weapons? better boomerangs?
         * hp, mp, stats on the bottom... level up animation
         * animations when you use skills!
         */

        #endregion //constants

        #region member data
        Random _rnd = new Random((int)DateTime.Now.Ticks);

        private float _screenOffsetX = 0;
        private float _screenOffsetY = 0;

        private bool _paused = true;
        private Bitmap _bmpCanvas;
        private Bitmap _bmpInfoBar;
        private Person _person;

        //private List<GameObject> _activePieces = new List<GameObject>();
        private List<Platforms> _activePlatforms = new List<Platforms>();
        private List<Monster> _activeMonsters = new List<Monster>();
        private List<Boomerang> _activeBoomerangs = new List<Boomerang>();
        private List<Animation> _activeAnimations = new List<Animation>();
        private List<Damage> _activeDamages = new List<Damage>();
        private List<Message> _activeMessages = new List<Message>();
        private List<Message> _MessagesTD = new List<Message>();

        private List<GameObject> _toDelete = new List<GameObject>();
        private List<GameObject> _toAdd = new List<GameObject>();


        private Bitmap[] _bmpMonsters = new Bitmap[Monster.monsterDefs.Length];
        private Bitmap[] _bmpAnimations = new Bitmap[Animation.animationDefs.Length];
        private Bitmap[] _bmpDamage = new Bitmap[Damage.damageDefs.Length];

        //private Bitmap _grayButton = new Bitmap("GraySkillButton.bmp");
        // private Bitmap _orangeButton = new Bitmap("OrangeSkillButton.bmp");


        Bitmap _graySkillButton;
        Bitmap _orangeSkillButton;
        Bitmap _instructions;
        Bitmap _jobArrows;
        Bitmap _toolBar;

        private Dictionary<MonsterType, int> _monsterTypeToBmpIdx = new Dictionary<MonsterType, int>();
        private Dictionary<AnimationType, int> _animationTypeToBmpIdx = new Dictionary<AnimationType, int>();
        private Dictionary<DamageType, int> _damageTypeToBmpIdx = new Dictionary<DamageType, int>();

        private bool _movingLeft = false;
        private bool _movingRight = false;
        private bool _startJumping = false;
        private bool _startDoubleTap = false;
        private bool _doubleTapReady = true;

        private bool _blobInverted = false;
        private bool _snailInverted = false;
        private bool _greenHatInverted = false;
        private bool _yellowHatInverted = false;
        private bool _robieraInverted = false;
        private bool _celInverted = false;
        private bool _robiera2Inverted = false;
        private bool _fireboarInverted = false;
        private bool _pepeInverted = false;
        private bool _snowRedInverted = false;
        private bool _orangeSnailInverted = false;
        private bool _snowManInverted = false;
        private bool _yetiInverted = false;

        private int _boomerangsActive;
        private int _warpsActive = 0;
        private int _grapplesActive = 0;

        private int _levelTimer = 0;
        private int _throwTimer = 0;
        private int _throwCooldown = 0;

        private bool _throwBoomerang = false;
        private bool _warpReady = false;
        private bool _warpUsable = true;

        private bool _grappleReady = false;
        private bool _grappleActive = false;
        private int _grappleX = 0;
        private int _grappleY = 0;
        private float _grappleDistance = 0;

        private int[] _monsCount = new int[] { };
        private int[] _maxMonCount = new int[] { };

        private int _clickX;
        private int _clickY;

        private int _canvasClickX;
        private int _canvasClickY;

        private Brush _brush1 = new SolidBrush(Color.Green);
        private Brush _orangeBrush = new SolidBrush(Color.Orange);
        private Brush _whiteBrush = new SolidBrush(Color.White);
        private Brush _blackBrush = new SolidBrush(Color.Black);
        private Brush _greenBrush = new SolidBrush(Color.Green);
        private Brush _blueBrush = new SolidBrush(Color.Blue);
        private Brush _skyBlueBrush = new SolidBrush(Color.SkyBlue);
        private Brush _grayBrush = new SolidBrush(Color.Gray);
        private Brush _redBrush = new SolidBrush(Color.Red);
        private Brush _yellowBrush = new SolidBrush(Color.Yellow);

        private Pen _pen1 = new Pen(Color.Linen);
        private Pen _whitePen = new Pen(Color.White);
        private Pen _orangePen = new Pen(Color.Orange);
        private Pen _blackPen = new Pen(Color.Black);
        private Pen _bluePen = new Pen(Color.Blue);

        private Font _levelTitleFont = new Font("Copperplate Gothic Bold", 15);
        private Font _levelFont = new Font("Bradley Hand ITC", 27);
        private Font _labelFont1 = new Font("Impact", 12);
        private Font _labelFont2 = new Font("Copperplate Gothic Bold", 9);
        private Font _expFont = new Font("Courier New", 10);
        private Font _skillFont = new Font("Arial", 10);


        private int _personDirection = 0;
        private int _monsterJumpFrequency = 100;
        private int _monsterDirectionFrequency = 123;

        private int _map = 1;
        private int _mapMax = 1;
        private bool _mapChanging = false;
        private int _level = 1;
        private int _job = 1;
        private long _currentExp = 0;
        private long _expTNL = 300;
        private int _mp = 100;
        private int _maxMp = 100;
        private int _mpRecoveryRate = 10;
        private int _mpRecoveryAmount = 1;
        private int _damageRate = 24;
        private int _skillPoints = 0;
        private int _maxBoomerangsActive = 1;
        private int _powerThrowLevel = 0;
        private int _powerThrowLevelMax = 20;
        private int _doubleTapLevel = 0;
        private int _doubleTapLevelMax = 10;
        private int _doubleThrowLevel = 0;
        private int _doubleThrowLevelMax = 20;
        private int _eagleEyeLevel = 0;
        private int _eagleEyeLevelMax = 8;
        private int _warpLevel = 0;
        private int _warpLevelMax = 10;
        private int _powerSliceLevel = 0;
        private int _powerSliceLevelMax = 30;
        private int _assassinateLevel = 0;
        private int _assassinateLevelMax = 50;
        private int _mpEaterLevel = 0;
        private int _mpEaterLevelMax = 25;
        private int _skullCrusherLevel = 0;
        private int _skullCrusherLevelMax = 30;
        private int _spectrumLevel = 0;
        private int _spectrumLevelMax = 30;
        private int _fleetLevel = 0;
        private int _fleetLevelMax = 30;
        private int _orbitLevel = 0;
        private int _orbitLevelMax = 30;
        private int _firebladeLevel = 0;
        private int _firebladeLevelMax = 30;
        private int _grappleLevel = 0;
        private int _grappleLevelMax = 30;

        private int _patchVersion = 6; //PATCH VERSION

        private int _mpRecoveryCTDWN;

        private GameSave _gameSave;

        private PlayerPersist _currentPlayer = null;

        private enum PersonState
        {
            walking,
            standing,
            jumping,
        }
        private bool _personSpecialState = false;
        private enum PersonShowState
        {
            throwing,
        }
        private enum ActiveSkill
        {
            //First job
            Normal,
            PowerThrow,
            DoubleThrow,

            //Second job
            Warp,
            PowerSlice,
            Assassinate,

            //Third job
            SkullCrusher,
            Spectrum,
            Orbit,

            //Fourth Job
            FireBlade,
            Grapple,
        }



        ActiveSkill _activeSkill = ActiveSkill.Normal;
        PersonState _personState = PersonState.jumping;
        PersonShowState _personShowState = PersonShowState.throwing;

        private int _mmX = 2;
        private int _mmY = 22;
        private int _mmWidth = 250;
        private int _mmHeight = 20;
        private bool _mmExpanded = false;
        private float _mmMarkerX = 114;

        private bool _instructionsShowing = false;

        private bool _upperJobsShowing = false;

        #endregion //member data
        public Game()
        {
            InitializeComponent();
            InitializeCanvas();
            InitializeInfoBar();
            InitializeGamePieces();

            LoadGame();
            //_level *= -1;

            InitializeGameButtons();
            this.KeyDown += new KeyEventHandler(GotKeyDown);
            this.KeyUp += new KeyEventHandler(GotKeyUp);
            GameTimer.Enabled = true;
            _paused = false;

        }
        private void InitializeCanvas()
        {
            _bmpCanvas = new Bitmap(Canvas.Width, Canvas.Height);
            Canvas.Image = (Image)_bmpCanvas;
        }
        private void InitializeInfoBar()
        {
            _bmpInfoBar = new Bitmap(InfoBar.Width, InfoBar.Height);
            InfoBar.Image = (Image)_bmpInfoBar;
        }

        private void InitializeGamePieces()
        {
            InitializePlatforms(_map);
            MonsterInitializer(_map);
            InitializePerson();
            InitializeDamage();
            InitializeAnimations();

        }
        private void InitializeGameButtons()
        {
            _graySkillButton = new Bitmap(GetType(), "GraySkillButton.bmp");
            _orangeSkillButton = new Bitmap(GetType(), "OrangeSkillButton.bmp");
            _instructions = new Bitmap(GetType(), "Instructions.bmp");
            _jobArrows = new Bitmap(GetType(), "JobArrows.bmp");
            _toolBar = new Bitmap(GetType(), "Toolbar.bmp");

            _graySkillButton.MakeTransparent(Color.Magenta);
            _orangeSkillButton.MakeTransparent(Color.Magenta);
        }
        private void InitializePerson()
        {
            _person = new Person();
            _person.SetBitmap1(new Bitmap(GetType(), "PersonStanding.bmp"));
            _person.SetBitmap2(new Bitmap(GetType(), "PersonWalking.bmp"));
            _person.SetBitmap3(new Bitmap(GetType(), "PersonJumping.bmp"));
            _person.SetBitmap4(new Bitmap(GetType(), "PersonThrowing.bmp"));
            _person.SetBitmap5(new Bitmap(GetType(), "PersonThrowing2.bmp"));
            _person.SetBitmap6(new Bitmap(GetType(), "PersonThrowing3.bmp"));
            _person.WalkingBmp.MakeTransparent(Color.Magenta);
            _person.JumpingBmp.MakeTransparent(Color.Magenta);
            _person.StandingBmp.MakeTransparent(Color.Magenta);
            _person.ThrowingBmp.MakeTransparent(Color.Magenta);
            _person.Throwing2Bmp.MakeTransparent(Color.Magenta);
            _person.Throwing3Bmp.MakeTransparent(Color.Magenta);
            _person.SetBitmap(_person.JumpingBmp, true);
            ChangePersonState(PersonState.jumping);
            _person.X = (Canvas.Width / 2) + (_person.Bmp.Width / 2);
            _person.Y = 400;
            _person.YPrev = _person.Y;
            _person.Speed = 6;
            _person.Xinc = 0;
            _person.Yinc = -20;
            _person.Gravity = -1;
            _person.YDrag = 1;
            _person.YOffset = 5;

            _person.CurrentFrame = 1;
            _person.TotalFrames = 4;
            _person.FrameCountDown = 7;
            _person.InitialFrameCountDown = 7;
            _person.FrameWidth = 49;

        }

        private void MonsterInitializer(int map)
        {
            InitializeMonsterArray(map);
            for (int idx = 0; idx < Monster.monsterDefs.Length; idx++)
            {
                if (Monster.monsterDefs[idx]._map == map)
                    InitializeMonster(Monster.monsterDefs[idx]._idd - 1, Monster.monsterDefs[idx]._id - 1);
            }
        }
        private void InitializeMonsterArray(int map)
        {
            switch (map)
            {
                case 1:
                    _monsCount = new int[] { 0, 0, 0, 0, 0, 0, 0 };
                    _maxMonCount = new int[] { 4, 4, 6, 2, 6, 1, 3 };
                    _bmpMonsters = new Bitmap[_monsCount.Length];
                    break;
                case 2:
                    _monsCount = new int[] { 0, 0, 0, 0, 0 };
                    _maxMonCount = new int[] { 2, 5, 1, 10, 7 };
                    _bmpMonsters = new Bitmap[_monsCount.Length];
                    break;
                case 3:
                    _monsCount = new int[] { 0, 0, 0 };
                    _maxMonCount = new int[] { 7, 7, 20 };
                    _bmpMonsters = new Bitmap[_monsCount.Length];
                    break;
                case 4:
                    _monsCount = new int[] { 0 };
                    _maxMonCount = new int[] { 12 };
                    _bmpMonsters = new Bitmap[_monsCount.Length];
                    break;
                case 5:
                    _monsCount = new int[] { };
                    _maxMonCount = new int[] { };
                    _bmpMonsters = new Bitmap[_monsCount.Length];
                    break;
                case 6:
                    _monsCount = new int[] { };
                    _maxMonCount = new int[] { };
                    _bmpMonsters = new Bitmap[_monsCount.Length];
                    break;
                case 7:
                    _monsCount = new int[] { };
                    _maxMonCount = new int[] { };
                    _bmpMonsters = new Bitmap[_monsCount.Length];
                    break;
                case 8:
                    _monsCount = new int[] { };
                    _maxMonCount = new int[] { };
                    _bmpMonsters = new Bitmap[_monsCount.Length];
                    break;
                case 9:
                    _monsCount = new int[] { };
                    _maxMonCount = new int[] { };
                    _bmpMonsters = new Bitmap[_monsCount.Length];
                    break;
                case 10:
                    _monsCount = new int[] { };
                    _maxMonCount = new int[] { };
                    _bmpMonsters = new Bitmap[_monsCount.Length];
                    break;
                case 11:
                    _monsCount = new int[] { };
                    _maxMonCount = new int[] { };
                    _bmpMonsters = new Bitmap[_monsCount.Length];
                    break;
                case 12:
                    _monsCount = new int[] { };
                    _maxMonCount = new int[] { };
                    _bmpMonsters = new Bitmap[_monsCount.Length];
                    break;
                default:
                    break;
            }
        }
        private void InitializeMonster(int idx, int idk)
        {
            switch (Monster.monsterDefs[idx]._type)
            {
                //map 1
                case MonsterType.Blob:
                    _bmpMonsters[idk] = new Bitmap(GetType(), "Blob.bmp");
                    _bmpMonsters[idk].MakeTransparent(Color.Magenta);
                    _monsterTypeToBmpIdx[MonsterType.Blob] = idk;
                    break;
                case MonsterType.Snail:
                    _bmpMonsters[idk] = new Bitmap(GetType(), "Snail.bmp");
                    _bmpMonsters[idk].MakeTransparent(Color.Magenta);
                    _monsterTypeToBmpIdx[MonsterType.Snail] = idk;
                    break;
                case MonsterType.GreenHat:
                    _bmpMonsters[idk] = new Bitmap(GetType(), "GreenHat.bmp");
                    _bmpMonsters[idk].MakeTransparent(Color.Magenta);
                    _monsterTypeToBmpIdx[MonsterType.GreenHat] = idk;
                    break;
                case MonsterType.Boomy:
                    _bmpMonsters[idk] = new Bitmap(GetType(), "Boomy.bmp");
                    _bmpMonsters[idk].MakeTransparent(Color.Magenta);
                    _monsterTypeToBmpIdx[MonsterType.Boomy] = idk;
                    break;
                case MonsterType.YellowHat:
                    _bmpMonsters[idk] = new Bitmap(GetType(), "YellowHat.bmp");
                    _bmpMonsters[idk].MakeTransparent(Color.Magenta);
                    _monsterTypeToBmpIdx[MonsterType.YellowHat] = idk;
                    break;
                case MonsterType.Cel:
                    _bmpMonsters[idk] = new Bitmap(GetType(), "Cel.bmp");
                    _bmpMonsters[idk].MakeTransparent(Color.Magenta);
                    _monsterTypeToBmpIdx[MonsterType.Cel] = idk;
                    break;
                case MonsterType.BluePotion:
                    _bmpMonsters[idk] = new Bitmap(GetType(), "BluePotion.bmp");
                    _bmpMonsters[idk].MakeTransparent(Color.Magenta);
                    _monsterTypeToBmpIdx[MonsterType.BluePotion] = idk;
                    break;

                //map 2
                case MonsterType.Robiera2:
                    _bmpMonsters[idk] = new Bitmap(GetType(), "Robiera.bmp");
                    _bmpMonsters[idk].MakeTransparent(Color.Magenta);
                    _monsterTypeToBmpIdx[MonsterType.Robiera2] = idk;
                    break;
                case MonsterType.Pepe:
                    _bmpMonsters[idk] = new Bitmap(GetType(), "Pepe.bmp");
                    _bmpMonsters[idk].MakeTransparent(Color.Magenta);
                    _monsterTypeToBmpIdx[MonsterType.Pepe] = idk;
                    break;
                case MonsterType.ManaElixir:
                    _bmpMonsters[idk] = new Bitmap(GetType(), "ManaElixir.bmp");
                    _bmpMonsters[idk].MakeTransparent(Color.Magenta);
                    _monsterTypeToBmpIdx[MonsterType.ManaElixir] = idk;
                    break;
                case MonsterType.OrangeSnail:
                    _bmpMonsters[idk] = new Bitmap(GetType(), "OrangeSnail.bmp");
                    _bmpMonsters[idk].MakeTransparent(Color.Magenta);
                    _monsterTypeToBmpIdx[MonsterType.OrangeSnail] = idk;
                    break;
                case MonsterType.FireBoar:
                    _bmpMonsters[idk] = new Bitmap(GetType(), "FireBoar.bmp");
                    _bmpMonsters[idk].MakeTransparent(Color.Magenta);
                    _monsterTypeToBmpIdx[MonsterType.FireBoar] = idk;
                    break;

                //map 3
                case MonsterType.SnowRed:
                    _bmpMonsters[idk] = new Bitmap(GetType(), "SnowRed.bmp");
                    _bmpMonsters[idk].MakeTransparent(Color.Magenta);
                    _monsterTypeToBmpIdx[MonsterType.SnowRed] = idk;
                    break;
                case MonsterType.SnowMan:
                    _bmpMonsters[idk] = new Bitmap(GetType(), "SnowMan.bmp");
                    _bmpMonsters[idk].MakeTransparent(Color.Magenta);
                    _monsterTypeToBmpIdx[MonsterType.SnowMan] = idk;
                    break;
                case MonsterType.Yeti:
                    _bmpMonsters[idk] = new Bitmap(GetType(), "Yeti.bmp");
                    _bmpMonsters[idk].MakeTransparent(Color.Magenta);
                    _monsterTypeToBmpIdx[MonsterType.Yeti] = idk;
                    break;

                //map 4
                case MonsterType.Robiera:
                    _bmpMonsters[idk] = new Bitmap(GetType(), "Robiera.bmp");
                    _bmpMonsters[idk].MakeTransparent(Color.Magenta);
                    _monsterTypeToBmpIdx[MonsterType.Robiera] = idk;
                    break;

                default:
                    break;

            }
        }

        private void InitializeAnimations()
        {
            for (int idx = 0; idx < Animation.animationDefs.Length; idx++)
            {
                switch (Animation.animationDefs[idx]._type)
                {
                    case AnimationType.DoubleTap:
                        _bmpAnimations[idx] = new Bitmap(GetType(), "DoubleTap.bmp");
                        _bmpAnimations[idx].MakeTransparent(Color.Magenta);
                        _animationTypeToBmpIdx[AnimationType.DoubleTap] = idx;
                        break;
                    case AnimationType.Warp:
                        _bmpAnimations[idx] = new Bitmap(GetType(), "WarpAnim.bmp");
                        _bmpAnimations[idx].MakeTransparent(Color.Magenta);
                        _animationTypeToBmpIdx[AnimationType.Warp] = idx;
                        break;
                    case AnimationType.Hit:
                        _bmpAnimations[idx] = new Bitmap(GetType(), "HitAnim.bmp");
                        _bmpAnimations[idx].MakeTransparent(Color.Magenta);
                        _animationTypeToBmpIdx[AnimationType.Hit] = idx;
                        break;
                    default:
                        break;

                }
            }
        }
        private void InitializeDamage()
        {
            for (int idx = 0; idx < Damage.damageDefs.Length; idx++)
            {
                switch (Damage.damageDefs[idx]._type)
                {
                    case DamageType.Zero:
                        _bmpDamage[idx] = new Bitmap(GetType(), "NumZero.bmp");
                        _bmpDamage[idx].MakeTransparent(Color.Magenta);
                        _damageTypeToBmpIdx[DamageType.Zero] = idx;
                        break;
                    case DamageType.One:
                        _bmpDamage[idx] = new Bitmap(GetType(), "NumOne.bmp");
                        _bmpDamage[idx].MakeTransparent(Color.Magenta);
                        _damageTypeToBmpIdx[DamageType.One] = idx;
                        break;
                    case DamageType.Two:
                        _bmpDamage[idx] = new Bitmap(GetType(), "NumTwo.bmp");
                        _bmpDamage[idx].MakeTransparent(Color.Magenta);
                        _damageTypeToBmpIdx[DamageType.Two] = idx;
                        break;
                    case DamageType.Three:
                        _bmpDamage[idx] = new Bitmap(GetType(), "NumThree.bmp");
                        _bmpDamage[idx].MakeTransparent(Color.Magenta);
                        _damageTypeToBmpIdx[DamageType.Three] = idx;
                        break;
                    case DamageType.Four:
                        _bmpDamage[idx] = new Bitmap(GetType(), "NumFour.bmp");
                        _bmpDamage[idx].MakeTransparent(Color.Magenta);
                        _damageTypeToBmpIdx[DamageType.Four] = idx;
                        break;
                    case DamageType.Five:
                        _bmpDamage[idx] = new Bitmap(GetType(), "NumFive.bmp");
                        _bmpDamage[idx].MakeTransparent(Color.Magenta);
                        _damageTypeToBmpIdx[DamageType.Five] = idx;
                        break;
                    case DamageType.Six:
                        _bmpDamage[idx] = new Bitmap(GetType(), "NumSix.bmp");
                        _bmpDamage[idx].MakeTransparent(Color.Magenta);
                        _damageTypeToBmpIdx[DamageType.Six] = idx;
                        break;
                    case DamageType.Seven:
                        _bmpDamage[idx] = new Bitmap(GetType(), "NumSeven.bmp");
                        _bmpDamage[idx].MakeTransparent(Color.Magenta);
                        _damageTypeToBmpIdx[DamageType.Seven] = idx;
                        break;
                    case DamageType.Eight:
                        _bmpDamage[idx] = new Bitmap(GetType(), "NumEight.bmp");
                        _bmpDamage[idx].MakeTransparent(Color.Magenta);
                        _damageTypeToBmpIdx[DamageType.Eight] = idx;
                        break;
                    case DamageType.Nine:
                        _bmpDamage[idx] = new Bitmap(GetType(), "NumNine.bmp");
                        _bmpDamage[idx].MakeTransparent(Color.Magenta);
                        _damageTypeToBmpIdx[DamageType.Nine] = idx;
                        break;
                }
            }
        }
        private void InitializePlatforms(int map)
        {
            List<Platforms> _platforms;
            _platforms = new List<Platforms>();
            for (int idx = 1; idx <= Platforms.platformPoints.Length; idx++)
            {
                if (Platforms.platformPoints[idx - 1]._map == map)
                {
                    Platforms plat = Platforms.GetPlatform(idx);
                    switch (idx)
                    {
                        case 1:
                            plat.SetBitmap(new Bitmap(GetType(), "Platform1.bmp"), true);
                            plat.Bmp.MakeTransparent(Color.Magenta);
                            break;
                        case 2:
                            plat.SetBitmap(new Bitmap(GetType(), "Platform2.bmp"), true);
                            plat.Bmp.MakeTransparent(Color.Magenta);
                            break;
                        case 3:
                            plat.SetBitmap(new Bitmap(GetType(), "Platform3.bmp"), true);
                            plat.Bmp.MakeTransparent(Color.Magenta);
                            break;
                        case 4:
                            plat.SetBitmap(new Bitmap(GetType(), "Platform3.bmp"), true);
                            plat.Bmp.MakeTransparent(Color.Magenta);
                            break;
                        case 5:
                            plat.SetBitmap(new Bitmap(GetType(), "Platform3.bmp"), true);
                            plat.Bmp.MakeTransparent(Color.Magenta);
                            break;
                        case 6:
                            plat.SetBitmap(new Bitmap(GetType(), "Platform5.bmp"), true);
                            plat.Bmp.MakeTransparent(Color.Magenta);
                            break;
                        case 7:
                            plat.SetBitmap(new Bitmap(GetType(), "Platform1.bmp"), true);
                            plat.Bmp.MakeTransparent(Color.Magenta);
                            break;
                        case 8:
                            plat.SetBitmap(new Bitmap(GetType(), "Platform3.bmp"), true);
                            plat.Bmp.MakeTransparent(Color.Magenta);
                            break;
                        case 9:
                            plat.SetBitmap(new Bitmap(GetType(), "Platform4.bmp"), true);
                            plat.Bmp.MakeTransparent(Color.Magenta);
                            break;
                        case 10:
                            plat.SetBitmap(new Bitmap(GetType(), "Platform6.bmp"), true);
                            plat.Bmp.MakeTransparent(Color.Magenta);
                            break;
                        case 11:
                            plat.SetBitmap(new Bitmap(GetType(), "Platform7.bmp"), true);
                            plat.Bmp.MakeTransparent(Color.Magenta);
                            break;
                        case 12:
                            plat.SetBitmap(new Bitmap(GetType(), "Platform8.bmp"), true);
                            plat.Bmp.MakeTransparent(Color.Magenta);
                            break;
                        case 13:
                            plat.SetBitmap(new Bitmap(GetType(), "Platform9.bmp"), true);
                            plat.Bmp.MakeTransparent(Color.Magenta);
                            break;
                        case 14:
                            plat.SetBitmap(new Bitmap(GetType(), "Platform2.bmp"), true);
                            plat.Bmp.MakeTransparent(Color.Magenta);
                            break;
                        case 15:
                            plat.SetBitmap(new Bitmap(GetType(), "Platform2.bmp"), true);
                            plat.Bmp.MakeTransparent(Color.Magenta);
                            break;
                        case 16:
                            plat.SetBitmap(new Bitmap(GetType(), "Platform2.bmp"), true);
                            plat.Bmp.MakeTransparent(Color.Magenta);
                            break;
                        case 17:
                            plat.SetBitmap(new Bitmap(GetType(), "Platform7.bmp"), true);
                            plat.Bmp.MakeTransparent(Color.Magenta);
                            break;
                        case 18:
                            plat.SetBitmap(new Bitmap(GetType(), "Platform8.bmp"), true);
                            plat.Bmp.MakeTransparent(Color.Magenta);
                            break;
                        case 19:
                            plat.SetBitmap(new Bitmap(GetType(), "Platform11.bmp"), true);
                            plat.Bmp.MakeTransparent(Color.Magenta);
                            break;
                        case 20:
                            plat.SetBitmap(new Bitmap(GetType(), "Platform7.bmp"), true);
                            plat.Bmp.MakeTransparent(Color.Magenta);
                            break;
                        case 21:
                            plat.SetBitmap(new Bitmap(GetType(), "Platform12.bmp"), true);
                            plat.Bmp.MakeTransparent(Color.Magenta);
                            break;
                        case 22:
                            plat.SetBitmap(new Bitmap(GetType(), "Platform13.bmp"), true);
                            plat.Bmp.MakeTransparent(Color.Magenta);
                            break;
                        case 23:
                            plat.SetBitmap(new Bitmap(GetType(), "Platform13.bmp"), true);
                            plat.Bmp.MakeTransparent(Color.Magenta);
                            break;
                        case 24:
                            plat.SetBitmap(new Bitmap(GetType(), "Platform13.bmp"), true);
                            plat.Bmp.MakeTransparent(Color.Magenta);
                            break;
                        case 25:
                            plat.SetBitmap(new Bitmap(GetType(), "Platform15.bmp"), true);
                            plat.Bmp.MakeTransparent(Color.Magenta);
                            break;

                        case 26:
                            plat.SetBitmap(new Bitmap(GetType(), "Platform2.bmp"), true);
                            plat.Bmp.MakeTransparent(Color.Magenta);
                            break;
                        case 27:
                            plat.SetBitmap(new Bitmap(GetType(), "Platform14.bmp"), true);
                            plat.Bmp.MakeTransparent(Color.Magenta);
                            break;
                        case 28:
                            plat.SetBitmap(new Bitmap(GetType(), "Platform16.bmp"), true);
                            plat.Bmp.MakeTransparent(Color.Magenta);
                            break;
                        case 29:
                            plat.SetBitmap(new Bitmap(GetType(), "Platform17.bmp"), true);
                            plat.Bmp.MakeTransparent(Color.Magenta);
                            break;

                        case 30:
                            plat.SetBitmap(new Bitmap(GetType(), "Platform8.bmp"), true);
                            plat.Bmp.MakeTransparent(Color.Magenta);
                            break;
                        case 31:
                            plat.SetBitmap(new Bitmap(GetType(), "Platform9.bmp"), true);
                            plat.Bmp.MakeTransparent(Color.Magenta);
                            break;
                        case 32:
                            plat.SetBitmap(new Bitmap(GetType(), "Platform10.bmp"), true);
                            plat.Bmp.MakeTransparent(Color.Magenta);
                            break;
                        case 33:
                            plat.SetBitmap(new Bitmap(GetType(), "Platform10.bmp"), true);
                            plat.Bmp.MakeTransparent(Color.Magenta);
                            break;
                        case 34:
                            plat.SetBitmap(new Bitmap(GetType(), "Platform10.bmp"), true);
                            plat.Bmp.MakeTransparent(Color.Magenta);
                            break;
                        case 35:
                            plat.SetBitmap(new Bitmap(GetType(), "Platform10.bmp"), true);
                            plat.Bmp.MakeTransparent(Color.Magenta);
                            break;
                        case 36:
                            plat.SetBitmap(new Bitmap(GetType(), "Platform10.bmp"), true);
                            plat.Bmp.MakeTransparent(Color.Magenta);
                            break;
                        case 37:
                            plat.SetBitmap(new Bitmap(GetType(), "Platform8.bmp"), true);
                            plat.Bmp.MakeTransparent(Color.Magenta);
                            break;
                        default:
                            break;
                    }
                    plat.FrameWidth = plat.Bmp.Width;
                    _platforms.Add(plat);
                }
            }
            foreach (Platforms platf in _platforms)
            {
                _activePlatforms.Add(platf);
            }

        }
        private void GameTimer_Tick(object sender, EventArgs e)
        {
            if (!_paused)
            {
                DoAI();
            }
            ManageCanvasClick();
            DoRender();
        }

        private void ManageCanvasClick()
        {
            if ((_canvasClickX != 0) && (_canvasClickY != 0))
            {
                if (_canvasClickY >= 3 && _canvasClickY <= 13)
                {
                    if (_canvasClickX >= 82 && _canvasClickX <= 98)
                    {
                        _mmExpanded = true;
                    }
                    if (_canvasClickX >= 114 && _canvasClickX <= 130)
                    {
                        _mmExpanded = false;
                    }
                }
                if (_canvasClickY >= (Canvas.Height - 19) && (_canvasClickY <= Canvas.Height - 8))
                {
                    if (_canvasClickX >= 92 && _canvasClickX <= 108)
                    {
                        _instructionsShowing = true;
                    }
                    if (_canvasClickX >= 114 && _canvasClickX <= 128)
                    {
                        _instructionsShowing = false;
                    }
                }

                if (_paused)
                {
                    if ((_canvasClickX >= 448) && (_canvasClickX <= 558))
                    {
                        for (int map = 1; map < 13; map++)
                        {
                            if ((_canvasClickY >= 110 + (20 * map)) && (_canvasClickY <= 125 + (20 * map)) && _mp >= 800 && _mapMax >= map)
                            {
                                _map = map;
                                _mapChanging = true;
                            }
                        }
                    }
                    if ((_canvasClickX >= 548 && _canvasClickX <= 608) &&
                         (_canvasClickY >= 450 && _canvasClickY <= 470))
                        _paused = false;
                }
                _canvasClickX = 0;
                _canvasClickY = 0;
            }
        }


        #region AI

        private void DoAI()
        {
            TestForMapChange();
            ConsiderMonsterCreation();
            DeterminePersonDirectionX();
            ManageMonsterMovement();
            ManageMessages();
            ManageClick();
            TestForWarp();
            TestForGrapple();
            TestForJump();
            TestForDoubleTap();
            if (_throwCooldown <= 0)
                TestForBoomerangThrow();
            TestForLevelUp();
            ManageTimers();
            ManageAnimationFrames();
            _maxBoomerangsActive = GetMBAforLevel(_level);
            GetJobForLevel();
            GetMapMaxForLevel();
            ManageMP();
            DoPhysics();
            TestForDeadMonsters();
            MoveScreen();
            ProcessListRemovers();
            DeleteAndAddObjects();
            ManageMiniMap();
        }

        private void ManageClick()
        {
            if ((_clickX != 0) && (_clickY != 0))
            {
                if ((_clickX >= InfoBar.Width - 60) && (_clickX <= InfoBar.Width - 10)
                         && (_clickY >= 70) && (_clickY <= 95))
                {
                    InitializeGameState();
                    SetNewPlayerVars();
                    _level *= -1;
                    if (_map != 1)
                    {
                        _map = 1;
                        _mapChanging = true;
                    }
                }
                if ((_clickX >= InfoBar.Width - 180) && (_clickX <= InfoBar.Width - 130)
                     && (_clickY >= 70) && (_clickY <= 95))
                {
                    SaveGame();
                }
                if ((_clickX >= 915) && (_clickX <= 935))
                {
                    if (_clickY <= 45)
                        _upperJobsShowing = true;
                    if (_clickY > 45)
                        _upperJobsShowing = false;
                }
                if ((_clickX >= InfoBar.Width - 80) && (_clickX <= InfoBar.Width - 10)
                         && (_clickY >= 40) && (_clickY >= 67))
                {
                    _skillPoints = ((_level - 1) * 3);
                    _powerThrowLevel = 0;
                    _doubleTapLevel = 0;
                    _doubleThrowLevel = 0;
                    _eagleEyeLevel = 0;
                    _warpLevel = 0;
                    _powerSliceLevel = 0;
                    _assassinateLevel = 0;
                    _mpEaterLevel = 0;
                    _skullCrusherLevel = 0;
                    _spectrumLevel = 0;
                    _fleetLevel = 0;
                    _orbitLevel = 0;
                    _firebladeLevel = 0;
                    _grappleLevel = 0;
                    _mp = 0;
                }
                if (_skillPoints > 0)
                {
                    if (!_upperJobsShowing)
                    {
                        if ((_clickX >= 520) && (_clickX <= 532) && (_job >= 1))
                        {
                            if ((_clickY >= 17) && (_clickY <= 29) && (_powerThrowLevel < _powerThrowLevelMax))
                            {
                                _skillPoints--;
                                _powerThrowLevel++;
                            }
                            if ((_clickY >= 32) && (_clickY <= 44) && (_doubleTapLevel < _doubleTapLevelMax))
                            {
                                _skillPoints--;
                                _doubleTapLevel++;
                            }
                            if ((_clickY >= 47) && (_clickY <= 59) && (_doubleThrowLevel < _doubleThrowLevelMax))
                            {
                                _skillPoints--;
                                _doubleThrowLevel++;
                            }
                            if ((_clickY >= 62) && (_clickY <= 74) && (_eagleEyeLevel < _eagleEyeLevelMax))
                            {
                                _skillPoints--;
                                _eagleEyeLevel++;
                            }
                        }
                        if ((_clickX >= 700) && (_clickX <= 712) && (_job >= 2))
                        {
                            if ((_clickY >= 2) && (_clickY <= 14) && (_warpLevel < _warpLevelMax))
                            {
                                _skillPoints--;
                                _warpLevel++;
                            }
                            if ((_clickY >= 17) && (_clickY <= 29) && (_powerSliceLevel < _powerSliceLevelMax))
                            {
                                _skillPoints--;
                                _powerSliceLevel++;
                            }
                            if ((_clickY > 32) && (_clickY <= 44) && (_assassinateLevel < _assassinateLevelMax))
                            {
                                _skillPoints--;
                                _assassinateLevel++;
                            }
                            if ((_clickY > 47) && (_clickY <= 59) && (_mpEaterLevel < _mpEaterLevelMax))
                            {
                                _skillPoints--;
                                _mpEaterLevel++;
                            }
                        }
                        if ((_clickX >= 890) && (_clickX <= 902) && (_job >= 3))
                        {
                            if ((_clickY >= 2) && (_clickY <= 14) && (_skullCrusherLevel < _skullCrusherLevelMax))
                            {
                                _skillPoints--;
                                _skullCrusherLevel++;
                            }
                            if ((_clickY >= 17) && (_clickY <= 29) && (_spectrumLevel < _spectrumLevelMax))
                            {
                                _skillPoints--;
                                _spectrumLevel++;
                            }
                            if ((_clickY >= 32) && (_clickY <= 44) && (_fleetLevel < _fleetLevelMax))
                            {
                                _skillPoints--;
                                _fleetLevel++;
                            }
                            if ((_clickY >= 47) && (_clickY <= 59) && (_orbitLevel < _orbitLevelMax))
                            {
                                _skillPoints--;
                                _orbitLevel++;
                            }
                        }
                    }
                    if (_upperJobsShowing)
                    {
                        if ((_clickX >= 520) && (_clickX <= 532) && (_job >= 4))
                        {
                            if ((_clickY >= 2) && (_clickY <= 14) && (_firebladeLevel < _firebladeLevelMax))
                            {
                                _skillPoints--;
                                _firebladeLevel++;
                            }
                            if ((_clickY >= 17) && (_clickY <= 29) && (_grappleLevel < _grappleLevelMax))
                            {
                                _skillPoints--;
                                _grappleLevel++;
                            }
                        }
                    }

                }
                _clickX = 0;
                _clickY = 0;
            }
        }
        private void TestForLevelUp()
        {
            if (_currentExp < 0)
                _currentExp = 0;
            while (_currentExp >= _expTNL && _level < 600)
                LevelUp();
        }
        private void LevelUp()
        {
            _currentExp -= _expTNL;
            _level++;
            _skillPoints += 3;
            _expTNL = GetTNLforLevel(_level);
            _damageRate = GetDamageRateForLevel(_level);
            _levelTimer = 20;

            _maxMp = (int)((100 + (20 * _level)) * 1.337f);
            _mpRecoveryRate = (int)(11 - (0.05f * _level)) + 1;
            _mpRecoveryAmount = 1 + (int)(0.05f * _level);
            _mp = _maxMp;



        }
        private void TestForMapChange()
        {
            if (_map == 0)
                _map = 1;
            if (_mapChanging)
            {
                ChangeMap(_map);
                _mapChanging = false;
            }

        }
        private void ChangeMap(int map)
        {
            _activeMonsters.Clear();
            _activeDamages.Clear();
            _activeAnimations.Clear();
            _activeBoomerangs.Clear();
            _activePlatforms.Clear();
            for (int idx = 0; idx < _bmpMonsters.Length; idx++)
                _bmpMonsters[idx].Dispose();
            _monsterTypeToBmpIdx.Clear();

            MonsterInitializer(map);
            InitializePlatforms(map);

            _person.X = 100;
            _person.Y = 400;
            _person.YPrev = _person.Y;
            _person.Speed = 6;
            _person.Xinc = 0;
            _person.Yinc = -20;
            _person.Gravity = -1;
            _person.OnGround = false;
            ChangePersonState(PersonState.jumping);

            _blobInverted = false;
            _snailInverted = false;
            _greenHatInverted = false;
            _yellowHatInverted = false;
            _robieraInverted = false;
            _celInverted = false;
            _robiera2Inverted = false;
            _pepeInverted = false;

            _screenOffsetX = 0;
            _screenOffsetY = 0;

            _toDelete.Clear();
            _toAdd.Clear();

            _movingLeft = false;
            _movingRight = false;
            _startJumping = false;
            _startDoubleTap = false;
            _doubleTapReady = true;

            _boomerangsActive = 0;
            _warpsActive = 0;
            _grapplesActive = 0;

            _throwBoomerang = false;
            _warpReady = false;
            _warpUsable = true;
            _grappleReady = false;

            _personDirection = 0;
            _person.PlatOn.Clear();

            _personDirection = 0;
            _movingLeft = false;
            _movingRight = false;

            _mp -= 800;
        }
        private void ManageTimers()
        {
            if (_levelTimer > 0)
            {
                _levelTimer--;
                this.Opacity = 1 - (_levelTimer * 0.05f);
                if (_levelTimer == 0)
                {
                    _activeMessages.Add(Message.NewMessage("You reached level " + _level.ToString() + "!!!", 60, 500));
                    switch (_level)
                    {
                        case 20:
                            _activeMessages.Add(Message.NewMessage("SECOND JOB SKILLS UNLOCKED", 60, 100));
                            break;
                        case 50:
                            _activeMessages.Add(Message.NewMessage("THIRD JOB SKILLS UNLOCKED", 60, 100));
                            break;
                        case 85:
                            _activeMessages.Add(Message.NewMessage("FOURTH JOB SKILLS UNLOCKED", 60, 100));
                            break;
                        default:
                            break;
                    }
                }
            }

            foreach (Boomerang boom in _activeBoomerangs)
            {
                boom.TimeLeft--;
                if (boom.TimeLeft <= 0)
                {
                    _toDelete.Add(boom);
                }
            }

            if (_throwTimer > 0)
            {
                _throwTimer--;
                if (_throwTimer <= 0)
                {
                    _personSpecialState = false;
                    ChangePersonState(_personState);
                }
            }
            if (_throwCooldown > 0)
            {
                _throwCooldown--;
            }
        }
        private void TestForWarp()
        {
            if (_warpReady && _warpsActive == 1 && _warpUsable)
            {
                foreach (Boomerang boom in _activeBoomerangs)
                {
                    if (boom.Trajectory == 70)
                    {
                        Animation a = Animation.GetAnimation((int)_person.X, (int)_person.Y, AnimationType.Warp, true);
                        a.SetBitmap(_bmpAnimations[_animationTypeToBmpIdx[a.type]], false);
                        a.X -= (((a.Width / a.TotalFrames) - (_person.Width / _person.TotalFrames)) / 2);
                        _activeAnimations.Add(a);
                        _person.Y = boom.Y - (_person.Height - boom.Height);
                        _person.X = boom.X;
                        _person.XPrev = _person.X;
                        _person.YPrev = _person.Y;
                        Animation b = Animation.GetAnimation((int)_person.X, (int)_person.Y, AnimationType.Warp, true);
                        b.SetBitmap(_bmpAnimations[_animationTypeToBmpIdx[b.type]], false);
                        b.X -= (((b.Width / b.TotalFrames) - (_person.Width / _person.TotalFrames)) / 2);
                        _activeAnimations.Add(b);
                        _person.OnGround = false;
                        ChangePersonState(PersonState.jumping);
                        _person.PlatOn.Clear();
                        _warpUsable = false;
                        _toDelete.Add(boom);
                    }
                }
            }
            _warpReady = false;
        }
        private void TestForGrapple()
        {
            if (_grappleReady && _grapplesActive == 1)
            {
                foreach (Boomerang boom in _activeBoomerangs)
                {
                    if (boom.Cooldown == 47)
                    {
                        float x = boom.X;
                        float y = boom.Y;
                        float xv = ((_person.X - boom.X) / (float)(Math.Sqrt(((_person.X - boom.X) * (_person.X - boom.X)) + ((_person.Y - boom.Y) * (_person.Y - boom.Y)))));
                        float yv = ((_person.Y - boom.Y) / (float)(Math.Sqrt(((_person.X - boom.X) * (_person.X - boom.X)) + ((_person.Y - boom.Y) * (_person.Y - boom.Y)))));
                        bool boomtoright = boom.X > _person.X;
                        bool boombelow = boom.Y > _person.Y;
                        bool boomstillright = boomtoright;
                        bool boomstillbelow = boombelow;
                        bool grappled = false;

                        while (boomstillright == boomtoright && boomstillbelow == boombelow)
                        {
                            foreach (Platforms plat in _activePlatforms)
                            {
                                if (plat.RightX >= x && plat.LeftX <= x)
                                {
                                    if (5 > Math.Abs(y - (plat.LeftY + ((x - plat.LeftX) * ((plat.RightY - plat.LeftY) / (plat.RightX - plat.LeftX))))))
                                    {
                                        _grappleX = (int)(x);
                                        _grappleY = (int)(plat.LeftY + ((x - plat.LeftX) * ((plat.RightY - plat.LeftY) / (plat.RightX - plat.LeftX))));
                                        //_grappleY = (int)(y);
                                        grappled = true;
                                        _grappleActive = true;
                                        _grappleDistance = (float)(Math.Sqrt(((_grappleX - _person.X) * (_grappleX - _person.X)) + ((_grappleY - _person.Y) * (_grappleY - _person.Y))));
                                        _toDelete.Add(boom);

                                        break;
                                    }
                                }
                            }
                            if (grappled)
                                break;
                            x += xv;
                            y += yv;
                            boomstillright = x > _person.X;
                            boomstillbelow = y > _person.Y;
                        }

                        /*foreach (Platforms plat in _activePlatforms)
                        {
                            if (plat.RightX >= (boom.X + (boom.FrameWidth / 2) && plat.LeftX <= x)
                            {
                                if (5 > Math.Abs(y - (plat.LeftY + ((x - plat.LeftX) * ((plat.RightY - plat.LeftY) / (plat.RightX - plat.LeftX))))))
                                {
                                    _grappleX = (int)(x);
                                    //_grappleY = (int)(plat.LeftY + ((x - plat.LeftX) * ((plat.RightY - plat.LeftY) / (plat.RightX - plat.LeftX))));
                                    _grappleY = (int)(y);
                                    grappled = true;
                                    _grappleActive = true;
                                    _toDelete.Add(boom);

                                    break;
                                }
                            }
                        }*/
                    }
                }
            }
        }
        private void ProcessListRemovers()
        {
            List<ListRemover> todelete = new List<ListRemover>();
            foreach (Boomerang obj in _activeBoomerangs)
            {
                foreach (ListRemover lr in ((Boomerang)obj).MonstersHit)
                {
                    lr.TimeLeft--;
                    if (lr.TimeLeft <= 0)
                        todelete.Add(lr);
                }
                foreach (ListRemover lr in todelete)
                {
                    ((Boomerang)obj).MonstersHit.Remove(lr);
                }
            }
        }
        private void ManageMP()
        {
            _maxMp = (int)((100 + (20 * _level)) * 1.337f);
            _mpRecoveryRate = (int)(11 - (0.05f * _level)) + 1;
            _mpRecoveryAmount = 1 + (int)(0.05f * _level);
            if (_mpRecoveryCTDWN > _mpRecoveryRate)
            {
                _mpRecoveryCTDWN = _mpRecoveryRate;
            }

            _mpRecoveryCTDWN--;
            if (_mpRecoveryCTDWN <= 0)
            {
                _mpRecoveryCTDWN = _mpRecoveryRate;
                _mp += _mpRecoveryAmount;
            }
            if (_mp > _maxMp)
            {
                _mp = _maxMp;
            }
        }
        private void ManageMessages()
        {
            foreach (Message m in _activeMessages)
            {
                m.CurrentTime--;
                if (m.CurrentTime == 0)
                    _MessagesTD.Add(m);
                if (m.CurrentTime < 50)
                {
                    if (m.Y < 170)
                        m.Y += 15;
                    if (m.Y >= 170 && m.Y <= 185)
                        m.Y += 1;
                    if (m.Y >= 185)
                        m.Y += 2;
                    if (m.Y >= 195)
                        m.Y += 4;
                    if (m.Y >= 210)
                        m.Y += 8;
                    if (m.Y >= 250)
                        m.Y += 4;
                }
                else
                {
                    m.Y -= 3;
                }

            }
            foreach (Message m in _MessagesTD)
                _activeMessages.Remove(m);
            _MessagesTD.Clear();
        }
        private static long GetTNLforLevel(int lvl)
        {
            long tnl = 1;
            if (lvl < 50)
                tnl = (long)(300 * Math.Pow(1.17, lvl));
            else if (lvl >= 50)
                tnl = (long)(48258.7814 * Math.Pow(1.0533190, lvl));
            return tnl;
        }
        private static int GetDamageRateForLevel(int lvl)
        {
            double dr = (((177.5 * lvl * lvl) + (7100 * lvl)) / 300);
            //((((71 * lvl) / 300) * (40 + lvl)) * 2.5);
            return (int)dr;
        }
        private void TestForDeadMonsters()
        {
            foreach (Monster mons in _activeMonsters)
            {
                if ((mons.Hp <= 0) && (!(_toDelete.Contains(mons))))
                {
                    _toDelete.Add(mons);
                    _currentExp += mons.Exp;
                    _activeMessages.Add(Message.NewMessage("You gained " + mons.Exp.ToString("#,##0") + " EXP!", 50, 950));
                }
            }
        }
        private static int GetMBAforLevel(int lvl)
        {
            int mba = 1;
            if (lvl >= 20)
                mba++;
            if (lvl >= 35)
                mba++;
            if (lvl >= 50)
                mba++;
            if (lvl >= 80)
                mba++;
            if (lvl >= 120)
                mba += 2;
            if (lvl >= 150)
                mba++;
            if (lvl >= 175)
                mba++;
            if (lvl >= 200)
                mba++;

            return mba;
        }
        private void GetJobForLevel()
        {
            int job = 1;
            if (_level >= 20 && (_doubleTapLevel + _eagleEyeLevel + _powerThrowLevel + _doubleThrowLevel >= 57))
                job++;
            if (_level >= 50 && (_warpLevel + _powerSliceLevel + _assassinateLevel + _mpEaterLevel >= 87))
                job++;
            if (_level >= 85 && (_skullCrusherLevel + _spectrumLevel + _fleetLevel + _orbitLevel >= 98))
                job++;
            _job = job;
        }
        private void GetMapMaxForLevel()
        {
            int mm = 1;
            if (_level >= 20) //2
                mm++;
            if (_level >= 45) //3
                mm++;
            if (_level >= 75) //4
                mm++;
            if (_level >= 100) //5
                mm++;
            if (_level >= 120)//6
                mm++;
            if (_level >= 145)//7
                mm++;
            if (_level >= 175)//8
                mm++;
            if (_level >= 205)//9
                mm++;
            if (_level >= 235)//10
                mm++;
            if (_level >= 270)//11
                mm++;
            if (_level >= 300)//12
                mm++;

            _mapMax = mm;
        }
        private void DeleteAndAddObjects()
        {
            foreach (GameObject obj in _toDelete)
            {
                if (obj is Monster)
                {
                    _monsCount[((Monster)obj).Id - 1]--;
                    _activeMonsters.Remove(((Monster)obj));
                }
                if (obj is Boomerang)
                {
                    if (((Boomerang)obj).PrimaryBoomerang == true)
                        _boomerangsActive--;
                    if (((Boomerang)obj).Trajectory == 70)
                        _warpsActive--;
                    if (((Boomerang)obj).Cooldown == 47)
                        _grapplesActive--;
                    _activeBoomerangs.Remove((Boomerang)obj);
                }
                if (obj is Animation)
                {
                    _activeAnimations.Remove((Animation)obj);
                }
                if (obj is Damage)
                {
                    _activeDamages.Remove((Damage)obj);
                }
            }
            _toDelete.RemoveRange(0, _toDelete.Count);

            foreach (GameObject obj in _toAdd)
            {
                if (obj is Damage)
                {
                    _activeDamages.Add((Damage)obj);
                }
                if (obj is Monster)
                {
                    _activeMonsters.Add((Monster)obj);
                }
                if (obj is Boomerang)
                {
                    _activeBoomerangs.Add((Boomerang)obj);
                }
                if (obj is Animation)
                {
                    _activeAnimations.Add((Animation)obj);
                }
            }
            _toAdd.RemoveRange(0, _toAdd.Count);
        }
        private void MoveScreen()
        {
            if ((_person.X - _screenOffsetX < (Canvas.Width / 2) - 50) || ((Canvas.Width + _screenOffsetX) - _person.X < (Canvas.Width / 2) - 50))
                _screenOffsetX -= (((_screenOffsetX + (Canvas.Width / 2)) - (_person.X + (_person.Width / (2 * _person.FrameCountDown)))) / 20);
            _screenOffsetY -= (((_screenOffsetY + (Canvas.Height / 2)) - (_person.Y + (_person.Height / 2))) / 20);
        }
        private void ConsiderMonsterCreation()
        {
            if (_rnd.Next(0, _monsterSpawnRate) == 1 && _monsCount.Length >= 1)
            {
                int id = _rnd.Next(_monsCount.Length) + 1;

                if (_monsCount[id - 1] >= _maxMonCount[id - 1])
                    id = _rnd.Next(_monsCount.Length) + 1;
                if (_monsCount[id - 1] >= _maxMonCount[id - 1])
                    id = _rnd.Next(_monsCount.Length) + 1;
                if (_monsCount[id - 1] >= _maxMonCount[id - 1])
                    id = _rnd.Next(_monsCount.Length) + 1;
                if (_monsCount[id - 1] >= _maxMonCount[id - 1])
                    id = _rnd.Next(_monsCount.Length) + 1;
                if (_monsCount[id - 1] < _maxMonCount[id - 1])
                {
                    CreateMonster(id);
                    _monsCount[id - 1]++;
                }
            }
        }
        private void CreateMonster(int id)
        {
            Monster m = Monster.GetMonster(id, _map);
            m.SetBitmap(_bmpMonsters[_monsterTypeToBmpIdx[m.Type]], false);
            m.Y -= m.Height;
            _activeMonsters.Add(m);
        }
        private void TestForJump()
        {
            if ((_person.OnGround) && (_startJumping == true))
            {
                _person.Yinc = -15;
                _person.OnGround = false;
                ChangePersonState(PersonState.jumping);
                _person.PlatOn.Clear();
            }
        }
        private void TestForDoubleTap()
        {
            if ((_person.OnGround == false) && (_startDoubleTap == true) && (_doubleTapReady == true) && (_doubleTapLevel > 0) && (_mp >= (_doubleTapLevel * 2)))
            {
                _person.Yinc -= (_doubleTapLevel * 2);
                _doubleTapReady = false;
                Animation a = Animation.GetAnimation((int)_person.X, (int)_person.Y, AnimationType.DoubleTap, true);
                a.SetBitmap(_bmpAnimations[_animationTypeToBmpIdx[a.type]], false);
                a.X -= (((a.Width / a.TotalFrames) - (_person.Width / _person.TotalFrames)) / 2);
                a.Y += ((_person.Height) / 3);
                _activeAnimations.Add(a);
                _mp -= (_doubleTapLevel * 2);
                if (!_movingRight && !_movingLeft)
                    _personDirection = 0;
                if (_movingRight ^ _movingLeft)
                {
                    if (_movingLeft)
                        _personDirection = -1;
                    if (_movingRight)
                        _personDirection = 1;
                    _person.Xinc = (_personDirection * (_person.Speed + (0.1f * _fleetLevel)));
                }
            }
        }
        private void TestForBoomerangThrow()
        {
            if ((_throwBoomerang) && (_boomerangsActive < _maxBoomerangsActive) && (_throwTimer <= 0))
            {
                bool enoughMP = true;
                #region MP Test //check MP and skill level
                switch (_activeSkill)
                {
                    case ActiveSkill.PowerThrow:
                        if (_mp < 15 || _powerThrowLevel < 1)
                            enoughMP = false;
                        break;
                    case ActiveSkill.DoubleThrow:
                        if (_mp < 15 || _doubleThrowLevel < 1)
                            enoughMP = false;
                        break;
                    case ActiveSkill.Warp:
                        if (_mp < 50 || _warpLevel < 1)
                            enoughMP = false;
                        break;
                    case ActiveSkill.PowerSlice:
                        if (_mp < 35 || _powerSliceLevel < 1)
                            enoughMP = false;
                        break;
                    case ActiveSkill.Assassinate:
                        if (_mp < 60 || _assassinateLevel < 1)
                            enoughMP = false;
                        break;
                    case ActiveSkill.SkullCrusher:
                        if (_mp < 55 || _skullCrusherLevel < 1)
                            enoughMP = false;
                        break;
                    case ActiveSkill.Spectrum:
                        if (_mp < 80 || _spectrumLevel < 1)
                            enoughMP = false;
                        break;
                    case ActiveSkill.Orbit:
                        if (_mp < 100 || _orbitLevel < 1)
                            enoughMP = false;
                        break;
                    case ActiveSkill.FireBlade:
                        if (_mp < 150 || _firebladeLevel < 1)
                            enoughMP = false;
                        break;
                    case ActiveSkill.Grapple:
                        if (_mp < 300 || _grappleLevel < 1)
                            enoughMP = false;
                        break;
                    default:
                        break;
                }
                #endregion
                if (enoughMP)
                {
                    _throwBoomerang = false;
                    ChangePersonShowState(PersonShowState.throwing);
                }
            }
            if (_throwTimer == 5)
            {
                switch (_activeSkill)
                {
                    #region Normal
                    case ActiveSkill.Normal:
                        CreateBoomerang(1 * _damageRate, 200, 32 + (_eagleEyeLevel * 3), 1.6f + (0.15f * _eagleEyeLevel), new Bitmap(GetType(), "Boomerang.bmp"), 4, 54, 0, 6, 40, true, 20, 10);
                        _boomerangsActive++;
                        break;
                    #endregion
                    #region Power Throw
                    case ActiveSkill.PowerThrow:
                        if (_mp >= 15)
                        {
                            CreateBoomerang((1 + (_powerThrowLevel / 10)) * _damageRate, 875, 63 + (_eagleEyeLevel * 6), 6.4f + (0.6f * _eagleEyeLevel), new Bitmap(GetType(), "Boomerang.bmp"), 4, 54, 0, 1, 20, true, 10, 9);
                            _boomerangsActive++;
                            _mp -= 15;
                        }
                        break;
                    #endregion
                    #region Double Throw
                    case ActiveSkill.DoubleThrow:
                        if (_mp >= 15)
                        {
                            CreateBoomerang((((25 + (_doubleThrowLevel * 5)) * _damageRate) / 100), 220, 32 + (_eagleEyeLevel * 3), 1.6f + (0.15f * _eagleEyeLevel), new Bitmap(GetType(), "Boomerang.bmp"), 4, 54, 0, 3, 40, true, 20, 12);
                            CreateBoomerang((((25 + (_doubleThrowLevel * 5)) * _damageRate) / 100), 170, 32 + (_eagleEyeLevel * 3), 1.6f + (0.15f * _eagleEyeLevel), new Bitmap(GetType(), "Boomerang.bmp"), 4, 54, 30, 3, 40, false, 20, 12);
                            _boomerangsActive++;
                            _mp -= 15;
                        }
                        break;
                    #endregion

                    #region Warp
                    case ActiveSkill.Warp:
                        if (_warpsActive == 0 && _mp >= 50)
                        {
                            CreateBoomerang(_damageRate / 2, 70, (_warpLevel * 3) + (_eagleEyeLevel * 3), 1.5f + (0.15f * _eagleEyeLevel), new Bitmap(GetType(), "WarpBoom.bmp"), 4, 54, 0, 12, 40, true, 20, 12);
                            _warpsActive++;
                            _boomerangsActive++;
                            _mp -= 50;
                        }
                        break;
                    #endregion
                    #region Power Slice
                    case ActiveSkill.PowerSlice:
                        if (_mp >= 35)
                        {
                            CreateBoomerang((1 + (_powerSliceLevel / 10)) * _damageRate, 1600, 110 + (_eagleEyeLevel * 6), 14 + (0.8f * _eagleEyeLevel), new Bitmap(GetType(), "Boomerang.bmp"), 4, 54, 0, 6, 15, true, 7, 12);
                            _boomerangsActive++;
                            _mp -= 35;
                        }
                        break;
                    #endregion
                    #region Assassinate
                    case ActiveSkill.Assassinate:
                        if (_mp >= 60)
                        {
                            CreateBoomerang((5 + (_assassinateLevel / 4)) * _damageRate, 5000, 15 + (_eagleEyeLevel), 5 + (_eagleEyeLevel * 0.2f), new Bitmap(GetType(), "Assassinate.bmp"), 4, 54, 0, 10, 8, true, 10, 20);
                            _boomerangsActive++;
                            _mp -= 60;
                        }
                        break;
                    #endregion
                    #region Skull Crusher
                    case ActiveSkill.SkullCrusher:
                        if (_mp >= 55)
                        {
                            CreateBoomerang((1 + (_skullCrusherLevel / 10)) * _damageRate, 720, 110 + (_eagleEyeLevel * 4), 6 + (_eagleEyeLevel), new Bitmap(GetType(), "SkullCrusher.bmp"), 8, 84, 0, 6, 20, true, 10, 13);
                            CreateBoomerang((1 + (_skullCrusherLevel / 10)) * _damageRate, 720, 110 + (_eagleEyeLevel * 4), 6 + (_eagleEyeLevel), new Bitmap(GetType(), "SkullCrusher.bmp"), 8, 84, 30, 6, 20, false, 10, 13);
                            CreateBoomerang((1 + (_skullCrusherLevel / 10)) * _damageRate, 720, 110 + (_eagleEyeLevel * 4), 6 + (_eagleEyeLevel), new Bitmap(GetType(), "SkullCrusher.bmp"), 8, 84, 60, 6, 20, false, 10, 13);
                            _boomerangsActive++;
                            _mp -= 55;
                        }
                        break;
                    #endregion
                    #region Spectrum
                    case ActiveSkill.Spectrum:
                        if (_mp >= 80)
                        {
                            CreateBoomerang((1 + (_spectrumLevel / 15)) * _damageRate, 10000, 52 + (_eagleEyeLevel * 3), (2 + (0.3f * _eagleEyeLevel)), new Bitmap(GetType(), "Boomerang.bmp"), 4, 54, 0, 3, 35, true, 17, 10);
                            CreateBoomerang((1 + (_spectrumLevel / 13)) * _damageRate, 200, 52 + (_eagleEyeLevel * 3), (2 + (0.3f * _eagleEyeLevel)), new Bitmap(GetType(), "Boomerang.bmp"), 4, 54, 12, 3, 35, false, 17, 10);
                            CreateBoomerang((1 + (_spectrumLevel / 11)) * _damageRate, 80, 52 + (_eagleEyeLevel * 3), (2 + (0.3f * _eagleEyeLevel)), new Bitmap(GetType(), "Boomerang.bmp"), 4, 54, 24, 3, 35, false, 17, 10);
                            CreateBoomerang((1 + (_spectrumLevel / 13)) * _damageRate, -200, 52 + (_eagleEyeLevel * 3), (2 + (0.3f * _eagleEyeLevel)), new Bitmap(GetType(), "Boomerang.bmp"), 4, 54, 36, 3, 35, false, 17, 10);
                            CreateBoomerang((1 + (_spectrumLevel / 11)) * _damageRate, -80, 52 + (_eagleEyeLevel * 3), (2 + (0.3f * _eagleEyeLevel)), new Bitmap(GetType(), "Boomerang.bmp"), 4, 54, 48, 3, 35, false, 17, 10);
                            _boomerangsActive++;
                            _mp -= 80;
                        }
                        break;
                    #endregion
                    #region Orbit
                    case ActiveSkill.Orbit:
                        if (_mp >= 100)
                        {
                            CreateBoomerang((1 + (_orbitLevel / 20)) * _damageRate, 250, 40 + (_eagleEyeLevel * 4), 2 + (0.5f * _eagleEyeLevel), new Bitmap(GetType(), "Boomerang.bmp"), 4, 54, 0, 400, 30 * _orbitLevel, true, 20, 0);
                            _boomerangsActive++;
                            _mp -= 100;
                        }
                        break;
                    #endregion

                    #region Fire Blade
                    case ActiveSkill.FireBlade:
                        if (_mp >= 150)
                        {
                            CreateBoomerang((3 + (_firebladeLevel / 6)) * _damageRate, 300, 30 + (_eagleEyeLevel * 3), 2 + (0.2f * _eagleEyeLevel), new Bitmap(GetType(), "FireBoom.bmp"), 4, 60, 0, 500, 30, true, 0, 3);
                            _boomerangsActive++;
                            _mp -= 150;
                        }
                        break;
                    #endregion
                    #region Grapple
                    case ActiveSkill.Grapple:
                        if (_grapplesActive == 0 && _mp >= 300)
                        {
                            CreateBoomerang(((_damageRate * (_grappleLevel)) / 10), 60 + _grappleLevel, (int)(_grappleLevel * 3.5f) + (_eagleEyeLevel * 3), 1.5f + (0.15f * _eagleEyeLevel), new Bitmap(GetType(), "GrappleBoom.bmp"), 4, 54, 0, 12, (int)((((_grappleLevel) * 8) + (_eagleEyeLevel * 3)) / ((1.5f) + (_eagleEyeLevel * 0.15f))), true, 47, 0);
                            _boomerangsActive++;
                            _grapplesActive++;
                            _mp -= 300;
                        }
                        break;
                    #endregion
                    default:
                        break;
                }
            }
        }
        private void CreateBoomerang(int dmgRate, int trajectory, int range, float gravity,
                                     Bitmap bmp, int totalFrames, int frameWidth, int yDmgOffset,
                                     int hits, int time, bool primaryboom, int cooldown, int rethrowrate)
        {
            Boomerang boom = new Boomerang();
            boom.SetBitmap(bmp, true);
            boom.Bmp.MakeTransparent(Color.Magenta);
            boom.CurrentFrame = 1;
            boom.DamageRate = dmgRate;
            boom.InitialFrameCountDown = 4;
            boom.FrameCountDown = boom.InitialFrameCountDown;
            boom.FrameWidth = frameWidth;
            boom.Gravity = 0;
            boom.GravX = 0;
            boom.GravRate = gravity;
            boom.OnGround = false;
            boom.Range = range;
            boom.YDamageOffset = yDmgOffset;
            boom.TotalFrames = totalFrames;
            boom.Trajectory = trajectory;
            if (_person.Inverted)
            {
                boom.Inverted = true;
                boom.InvertedPrev = false;
                boom.Xinc = boom.Range;
                boom.X = _person.X + (_person.Width / (_person.TotalFrames * 2));
                boom.XPrev = boom.X;
            }
            else if (!(_person.Inverted))
            {
                boom.Inverted = false;
                boom.InvertedPrev = false;
                boom.Xinc = -(boom.Range);
                boom.X = _person.X - (boom.Width / (boom.TotalFrames * 2));
                boom.XPrev = boom.X;
            }
            boom.HitsLeft = hits;
            boom.Y = _person.Y + ((_person.Height - boom.Height) * 2);
            boom.YPrev = boom.Y;
            boom.MonstersHit = new List<ListRemover>();
            boom.PrimaryBoomerang = primaryboom;
            boom.TimeLeft = time;
            boom.Cooldown = cooldown;
            _activeBoomerangs.Add(boom);
            _throwCooldown = rethrowrate;
        }
        private void DoPhysics()
        {
            foreach (Boomerang boom in _activeBoomerangs)
            {
                ChangeBoomerangGravity(boom);
                TestBoomCollisions(boom);
                boom.Xinc += boom.GravX;
                DoMovement(boom);
            }
            foreach (Damage dama in _activeDamages)
            {
                DoMovement(dama);
            }
            foreach (Animation anim in _activeAnimations)
            {
                DoGravity(anim);
                DoMovement(anim);
            }
            foreach (Monster mons in _activeMonsters)
            {
                DoGravity(mons);
                DoMonsterMovement(mons);
                PlatformInteraction(mons);
                if (mons.Y > 2000)
                {
                    _toDelete.Add(mons);
                }
            }
            DoPersonMovement();
            foreach (Boomerang boom in _activeBoomerangs)
            {
                boom.X += _person.Xinc;
            }
            PlatformInteraction(_person);
            if (_person.Y > 2000)
            {
                _person.X = 400;
                _person.Y = 400;
                _currentExp -= (_expTNL / 10);
                _person.OnGround = false;
                ChangePersonState(PersonState.jumping);
                _person.PlatOn.Clear();
                _person.Yinc = 0;
                _person.Gravity = -1;
                _doubleTapReady = true;
            }
        }
        private void DoGravity(GameObject obj)
        {
            bool dogravity = true;
            if (obj.OnGround == true)
                dogravity = false;
            if (dogravity)
            {
                obj.Yinc -= obj.Gravity;
                obj.Xinc += obj.GravX;
            }
        }
        private void TestBoomCollisions(Boomerang boom)
        {
            float bxl = boom.X;
            float bxr = boom.XPrev + (boom.Width / boom.TotalFrames);
            if (boom.Xinc > 0)
            {
                bxl = boom.XPrev;
                bxr = boom.X + (boom.Width / boom.TotalFrames);
            }
            foreach (Monster obj in _activeMonsters)
            {
                if (boom.HitsLeft > 0)
                {

                    float oxl = obj.X;
                    float oxr = obj.XPrev + (obj.Width / obj.TotalFrames);
                    if (obj.Xinc > 0)
                    {
                        oxl = obj.XPrev;
                        oxr = obj.X + (obj.Width / obj.TotalFrames);
                    }

                    if (((boom.Y < (obj.Y + obj.Height)) && ((boom.Y + boom.Height) > obj.Y)) &&
                        ((bxl < oxr) && (bxr > oxl)))
                    {

                        bool alreadyHit = false;
                        foreach (ListRemover lr in boom.MonstersHit)
                        {
                            if (lr.Piece == obj)
                                alreadyHit = true;
                            break;
                        }
                        if (!(alreadyHit))
                        {
                            boom.HitsLeft--;

                            Animation anim = Animation.GetAnimation((int)obj.X, (int)obj.Y, AnimationType.Hit, true);
                            anim.SetBitmap(_bmpAnimations[_animationTypeToBmpIdx[anim.type]], false);
                            anim.X -= (((anim.Width / anim.TotalFrames) - (obj.Width / obj.TotalFrames)) / 2);
                            anim.Y -= ((anim.Height - obj.Height) / 2);
                            _activeAnimations.Add(anim);

                            int damage = _rnd.Next(((int)(boom.DamageRate * 3 / 5)), boom.DamageRate);

                            if (boom.FrameWidth == 60 && obj.Type == MonsterType.Yeti || obj.Type == MonsterType.SnowMan)
                                damage *= (int)1.5f;
                                

                            obj.Hp -= damage;
                            if ((_rnd.Next(1, 26 - _mpEaterLevel) == 1 && obj.Mp >= (obj.MpMax / 4) && _mpEaterLevel > 0)
                                || ((obj.Type == MonsterType.BluePotion) || (obj.Type == MonsterType.ManaElixir)))
                            {
                                _mp += (int)(obj.MpMax / 4);
                                _activeMessages.Add(Message.NewMessage("You gained " + (obj.MpMax / 4).ToString() + " MP", 30, 750));
                                obj.Mp -= (int)(obj.MpMax / 4);
                            }



                            ListRemover lr = new ListRemover(obj, ((Boomerang)boom).Cooldown);
                            boom.MonstersHit.Add(lr);
                            int a = 1;
                            int b = damage;
                            int c = 0;
                            int d = 0;
                            float f = (float)(Math.Log(damage) * 25);
                            float e = (float)(((f) - (obj.Width / obj.TotalFrames)) / 2);

                            while (b > 0)
                            {
                                d = 0;
                                while (decimal.Remainder(b, (10 * a)) != 0)
                                {
                                    b -= a;
                                    d++;
                                }
                                Damage m = Damage.CreateNumber(d, obj.X - e + f, obj.Y - boom.YDamageOffset);
                                m.SetBitmap(_bmpDamage[_damageTypeToBmpIdx[m.Type]], false);
                                _toAdd.Add(m);
                                m.FrameWidth = m.Width;
                                m.Yinc = -1;
                                c += (m.Width / m.TotalFrames);

                                m.X -= c;
                                m.X += 2 * (float)(Math.Log(a));

                                switch (a)
                                {
                                    case 1:
                                        break;
                                    case 10:
                                        m.Y += 2;
                                        break;
                                    case 100:
                                        m.Y -= 3;
                                        break;
                                    case 1000:
                                        m.Y += 1;
                                        break;
                                    case 10000:
                                        m.Y -= 3;
                                        break;
                                    case 100000:
                                        m.Y -= 3;
                                        break;
                                    case 1000000:
                                        m.Y += 3;
                                        break;
                                    default:
                                        break;
                                }

                                a *= 10;
                            }
                        }
                    }
                }

            }
            /*float pxl = _person.X + _person.Xinc;
            float pxr = _person.X + (_person.Width / _person.TotalFrames);
            if (_person.Xinc > 0)
            {
                pxl = _person.X;
                pxr = _person.X + _person.Xinc + (_person.Width / _person.TotalFrames);
            }

            if (((boom.Y < (_person.Y + _person.Height)) && ((boom.Y + boom.Height) > _person.Y)) &&
                ((bxl < pxr) && (bxr > pxl)))
            {
                _toDelete.Add(boom);
            }*/
        }
        private void ChangeBoomerangGravity(Boomerang boom)
        {
            boom.GravX = -boom.GravRate;
            if (boom.X < _person.X)
                boom.GravX = boom.GravRate;
            boom.Y = _person.Y + (((_person.X - boom.X) * boom.Xinc) / boom.Trajectory);
        }
        private void ManageMonsterMovement()
        {

            foreach (Monster mon in _activeMonsters)
            {
                if ((_rnd.Next(0, _monsterJumpFrequency) == 0) && (mon.OnGround == true) && (mon.Jump > 0))
                {
                    mon.Yinc = -(mon.Jump);
                    mon.OnGround = false;
                    mon.PlatOn.Clear();
                }
                bool changeInversion = false;
                if ((_rnd.Next(0, _monsterDirectionFrequency) == 0))
                {
                    changeInversion = true;
                }
                if (mon.OnGround == true)
                {
                    if (((mon.X + (mon.Xinc * 2) < mon.PlatOn[0].LeftX - (mon.Width / (mon.TotalFrames * 2))) && (mon.Xinc < 0)) || (((mon.X + (mon.Xinc * 2)) > (mon.PlatOn[0].RightX)) && (mon.Xinc > 0)))
                        changeInversion = true;
                }
                if (changeInversion)
                {
                    if (mon.Inverted == true)
                        mon.Inverted = false;
                    else if (mon.Inverted == false)
                        mon.Inverted = true;
                    mon.Xinc *= -1;
                }
            }
        }
        private void DoPersonMovement()
        {
            /*
            if (_grappleActive)
            {
                /*float dist = (float)(Math.Sqrt(((_person.X + (_person.FrameWidth / 2) - (_grappleX)) *
                                                                              (_person.X + (_person.FrameWidth / 2) - _grappleX)) +
                                                                             ((_person.Y + (_person.Height) - _grappleY) *
                                                                              (_person.Y + (_person.Height) - (_grappleY)))));
                if (_grappleDistance <= dist)
                {
                    //float magnitude = dist / _grappleDistance;
                    //if (_person.Y > _grappleY)
                    //    ChangePersonState(PersonState.jumping);
                    //_person.Xinc += ((_grappleX - _person.X) / 30);
                    //_person.Yinc += ((_grappleY - _person.Y) / 30);
                    //_person.X -= (_person.X - _grappleX) * ((dist - _grappleDistance) / dist);
                    //_person.Y -= (_person.Y - _grappleY) * ((dist - _grappleDistance) / dist);

                    //_person.Xinc += ((_person.X - _grappleX) * _person.Gravity * (_grappleY - _person.Y)) / (dist * dist);
                    //_person.Yinc += ((_person.X - _grappleX) * _person.Gravity * (_person.X - _grappleX)) / (dist * dist);
                    //_person.Yinc -= _person.Gravity;

                }
                if (_grappleDistance < dist + 3)
                {
                    //float xcentrip = (float)Math.Sqrt((((_person.Xinc * _person.Xinc) + (_person.Yinc * _person.Yinc))) / (((dist * dist)) *
                    //                                   (1 + (((_person.Y - _grappleY) * (_person.Y - _grappleY)) / ((_person.X - _grappleX) * (_person.X - _grappleX))))));
                    //_person.Xinc += xcentrip;
                    //_person.Yinc += ((xcentrip) * ((_person.Y - _grappleY) / (_person.X - _grappleX)));

                    //_person.Xinc = (((_person.Y - _grappleY) * ((_person.Xinc * _person.Xinc))) / ((float)Math.Sqrt((_person.Xinc *_person.Xinc) + (_person.Yinc * _person.Yinc))));
                    //_person.Yinc = (((_grappleX - _person.X) * ((_person.Yinc * _person.Yinc))) / ((float)Math.Sqrt((_person.Xinc * _person.Xinc) + (_person.Yinc * _person.Yinc))));

                }
             * 
            }
        */
            DoGravity(_person);
            if (_person.OnGround)
            {
                float scale = _person.Xinc / _person.PlatOn[0].Length;
                _person.XPrev = _person.X;
                _person.X += scale * (_person.PlatOn[0].RightX - _person.PlatOn[0].LeftX);
                _person.Y = _person.PlatOn[0].LeftY + (((_person.PlatOn[0].RightY -
                    _person.PlatOn[0].LeftY) * ((_person.X + (_person.Width /
                    (_person.TotalFrames * 2))) - _person.PlatOn[0].LeftX)) /
                    (_person.PlatOn[0].RightX - _person.PlatOn[0].LeftX)) +
                    _person.PlatOn[0].YOffsett - _person.Height;
                //_person.Y -= scale * (_person.PlatOn[0].LeftY - _person.PlatOn[0].RightY);
            }
            else if (!(_person.OnGround))
            {
                float yc = _person.Y;
                float xc = _person.X;
                float vv = (_person.Xinc * _person.Xinc) + (_person.Yinc * _person.Yinc);

                _person.YPrev = _person.Y;
                _person.Y += _person.Yinc;
                _person.XPrev = _person.X;
                _person.X += _person.Xinc;

                if (_grappleActive)
                {
                    double dst = Math.Sqrt((_grappleX - _person.X) * (_grappleX - _person.X) + (_grappleY - _person.Y) * (_grappleY - _person.Y));
                    if (_grappleDistance < dst)
                    {

                        if (_personDirection == 1)
                            _person.X -= _person.Speed / 10;
                        if (_movingRight)
                            _person.X += _person.Speed / 10;

                        double ratio = (dst - _grappleDistance) / _grappleDistance;
                        _person.X += (float)(ratio * (_grappleX - _person.X));
                        _person.Y += (float)(ratio * (_grappleY - _person.Y));

                        //if (_person.Yinc > _person.Y - yc)
                        {

                        }

                        //_person.Xinc += _personDirection;
                    }
                    _person.Yinc = (float)(_person.Y - yc + ((vv / _grappleDistance) * ((_grappleY - _person.Y) / _grappleDistance)));
                    _person.Xinc = (float)(_person.X - xc + ((vv / _grappleDistance) * ((_grappleX - _person.X) / _grappleDistance)));
                    //_person.Yinc = (float)(_person.Y + ((vv / dst) * ((_grappleY - _person.Y) / dst)));
                    //_person.Xinc = (float)(_person.X + ((vv / dst) * ((_grappleX - _person.X) / dst)));

                }
            }
        }
        private void DoMonsterMovement(Monster mons)
        {
            if (mons.OnGround)
            {
                float scale = mons.Xinc / mons.PlatOn[0].Length;
                mons.XPrev = mons.X;
                mons.X += scale * (mons.PlatOn[0].RightX - mons.PlatOn[0].LeftX);
                mons.Y -= scale * (mons.PlatOn[0].LeftY - mons.PlatOn[0].RightY);
            }
            else if (!(mons.OnGround))
            {
                mons.YPrev = mons.Y;
                mons.Y += mons.Yinc;
                mons.XPrev = mons.X;
                mons.X += mons.Xinc;
            }
        }
        private void DoMovement(GameObject obj)
        {
            obj.YPrev = obj.Y;
            obj.Y += obj.Yinc;
            obj.XPrev = obj.X;
            obj.X += obj.Xinc;
        }
        private void PlatformInteraction(GameObject obj)
        {
            if (!obj.OnGround)
            {
                foreach (Platforms plat in _activePlatforms)
                {

                    //test if the object has passed through the platform between
                    //the current position and the previous position
                    float objX = obj.X + (obj.Width / (obj.TotalFrames * 2));
                    float objXP = obj.XPrev + (obj.Width / (obj.TotalFrames * 2));

                    float objY = obj.Y + obj.Height - obj.YOffset;
                    float objYp = obj.YPrev + obj.Height - obj.YOffset;

                    float platY = plat.LeftY + (((plat.RightY - plat.LeftY) * (objX - plat.LeftX)) / (plat.RightX - plat.LeftX)) + plat.YOffsett;
                    float platYP = plat.LeftY + (((plat.RightY - plat.LeftY) * (objXP - plat.LeftX)) / (plat.RightX - plat.LeftX)) + plat.YOffsett;

                    if (((objY >= platY) && (objYp <= platYP)) &&
                        ((obj.X + (obj.Width / obj.TotalFrames) >= (plat.LeftX)) && (obj.X <= plat.RightX)))
                    {
                        //puts the object on the platform and stops it from falling
                        obj.Y = platY - obj.Height;
                        obj.Yinc = 0;
                        obj.OnGround = true;
                        if (obj is Person)
                        {
                            ChangePersonState(PersonState.jumping);
                            _doubleTapReady = true;
                            _warpUsable = true;
                            if (((Person)obj).PlatOn.Count == 0)
                            {
                                ((Person)obj).PlatOn.Add(plat);
                            }
                        }
                        if (obj is Monster)
                        {
                            if (((Monster)obj).PlatOn.Count == 0)
                            {
                                ((Monster)obj).PlatOn.Add(plat);
                            }
                        }
                    }
                }
            }
            if (obj is Person)
            {
                if (((Person)obj).PlatOn.Count > 0)
                {
                    if (!(((obj.X + (obj.Width / obj.TotalFrames)) >= (((Person)obj).PlatOn[0].LeftX)) && //now is NOT in the X range of plat
                        ((obj.X) <= (((Person)obj).PlatOn[0].RightX))))
                    {
                        obj.OnGround = false; //you are no longer on the plat.
                        ((Person)obj).PlatOn.Clear();
                        ChangePersonState(PersonState.jumping);
                    }
                }
            }
            if (obj is Monster)
            {
                if (((Monster)obj).PlatOn.Count > 0)
                {
                    if (!((obj.X + obj.Width / obj.TotalFrames) > (((Monster)obj).PlatOn[0].LeftX)) && //now is NOT in the X range of plat
                        ((obj.X) < (((Monster)obj).PlatOn[0].RightX)))
                    {
                        obj.OnGround = false; //you are no longer on the plat.
                        ((Monster)obj).PlatOn.Clear();
                    }
                }
            }
        }
        private void ChangePersonShowState(PersonShowState state)
        {
            _personShowState = state;
            _personSpecialState = true;
            switch (state)
            {
                case PersonShowState.throwing:
                    int whichanim = _rnd.Next(1, 4);
                    switch (whichanim)
                    {
                        case 1:
                            _person.SetBitmap(_person.ThrowingBmp, true);
                            _person.TotalFrames = 3;
                            _person.InitialFrameCountDown = 4;
                            _person.FrameCountDown = 4;
                            _person.FrameWidth = 55;
                            _person.YOffset = 0;
                            _throwTimer = 12;
                            _person.XRendOffset = -20;
                            break;
                        case 2:
                            _person.SetBitmap(_person.Throwing2Bmp, true);
                            _person.TotalFrames = 4;
                            _person.InitialFrameCountDown = 3;
                            _person.FrameCountDown = 3;
                            _person.FrameWidth = 64;
                            _person.YOffset = 0;
                            _throwTimer = 12;
                            _person.XRendOffset = -10;
                            break;
                        case 3:
                            _person.SetBitmap(_person.Throwing3Bmp, true);
                            _person.TotalFrames = 3;
                            _person.InitialFrameCountDown = 4;
                            _person.FrameCountDown = 4;
                            _person.FrameWidth = 65;
                            _person.YOffset = 0;
                            _throwTimer = 12;
                            _person.XRendOffset = -20;
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            _person.CurrentFrame = 1;
        }
        private void ChangePersonState(PersonState state)
        {
            _personState = state;

            if (_personSpecialState == false)
            {
                int prevh = _person.Bmp.Height;
                switch (state)
                {
                    case PersonState.jumping:
                        _person.SetBitmap(_person.JumpingBmp, true);
                        _person.TotalFrames = 1;
                        _person.FrameWidth = 48;
                        _person.FrameCountDown = 7;
                        _person.InitialFrameCountDown = 7;
                        _person.YOffset = 5;
                        _person.XRendOffset = 0;
                        break;
                    case PersonState.standing:
                        _person.SetBitmap(_person.StandingBmp, true);
                        _person.TotalFrames = 4;
                        _person.FrameWidth = 49;
                        _person.FrameCountDown = 7;
                        _person.InitialFrameCountDown = 7;
                        _person.YOffset = 5;
                        _person.XRendOffset = 0;
                        break;
                    case PersonState.walking:
                        _person.SetBitmap(_person.WalkingBmp, true);
                        _person.TotalFrames = 4;
                        _person.FrameWidth = 55;
                        _person.FrameCountDown = 7;
                        _person.InitialFrameCountDown = 7;
                        _person.YOffset = 5;
                        _person.XRendOffset = 0;
                        break;
                    default:
                        break;
                }
                _person.CurrentFrame = 1;
            }

        }
        private void DeterminePersonDirectionX()
        {
            if (!(_movingRight ^ _movingLeft) && _person.OnGround)
                _personDirection = 0;
            if (_movingRight ^ _movingLeft)
            {
                if (_movingLeft && _person.OnGround)
                    _personDirection = -1;
                if (_movingRight && _person.OnGround)
                    _personDirection = 1;
            }
            if (_person.OnGround && _throwTimer > 5)
                _personDirection = 0;
            if (_person.OnGround)
                _person.Xinc = (_personDirection * (_person.Speed + (0.1f * _fleetLevel)));
            if (!(_person.OnGround))
            {
                if (_personDirection == 0 && _movingLeft == true)
                {
                    _person.Xinc -= ((((_person.Speed + (0.1f * _fleetLevel)) / 1) + _person.Xinc) / 8);
                }
                if (_personDirection == 0 && _movingRight == true)
                {
                    _person.Xinc += ((((_person.Speed + (0.1f * _fleetLevel)) / 1) - _person.Xinc) / 8);
                }
                if ((_personDirection == 1 && _movingLeft == true) || (_personDirection == -1 && _movingRight == true))
                {
                    _person.Xinc -= ((_person.Xinc) / 12);
                }
                if (_movingLeft == false && _movingRight == false)
                {
                    _person.Xinc -= ((_person.Xinc) / 20);
                }
                if (_personState != PersonState.jumping)
                    _personState = PersonState.jumping;
            }

            if (_person.OnGround)
            {
                if (!(_movingRight ^ _movingLeft) && (_personState != PersonState.standing))
                    ChangePersonState(PersonState.standing);
                if (_movingRight ^ _movingLeft && (_personState != PersonState.walking))
                    ChangePersonState(PersonState.walking);
            }
        }
        private void ManageMiniMap()
        {
            if (_mmExpanded == true)
            {
                _mmHeight = (int)((_mmWidth * Canvas.Height) / Canvas.Width) + 20;
                _mmMarkerX -= ((_mmMarkerX - 82) / 10);
            }
            else if (_mmExpanded == false)
            {
                _mmHeight = 20;
                _mmMarkerX -= ((_mmMarkerX - 114) / 10);
            }

        }
        private void DistributeSP()
        {
            if (_firebladeLevel < _firebladeLevelMax && _job >= 4)
            {
                _firebladeLevel++;
                _skillPoints--;
                return;
            }
            if (_grappleLevel < _grappleLevelMax && _job >= 4)
            {
                _grappleLevel++;
                _skillPoints--;
                return;
            }
            if (_skullCrusherLevel < _skullCrusherLevelMax && _job >= 3)
            {
                _skullCrusherLevel++;
                _skillPoints--;
                return;
            }
            if (_spectrumLevel < _spectrumLevelMax && _job >= 3)
            {
                _spectrumLevel++;
                _skillPoints--;
                return;
            }
            if (_fleetLevel < _fleetLevelMax && _job >= 3)
            {
                _fleetLevel++;
                _skillPoints--;
                return;
            }
            if (_orbitLevel < _orbitLevelMax && _job >= 3)
            {
                _orbitLevel++;
                _skillPoints--;
                return;
            }
            if (_warpLevel < _warpLevelMax && _job >= 2)
            {
                _warpLevel++;
                _skillPoints--;
                return;
            }
            if (_powerSliceLevel < _powerSliceLevelMax && _job >= 2)
            {
                _powerSliceLevel++;
                _skillPoints--;
                return;
            }
            if (_assassinateLevel < _assassinateLevelMax && _job >= 2)
            {
                _assassinateLevel++;
                _skillPoints--;
                return;
            }
            if (_mpEaterLevel < _mpEaterLevelMax && _job >= 2)
            {
                _mpEaterLevel++;
                _skillPoints--;
                return;
            }
            if (_powerThrowLevel < _powerThrowLevelMax)
            {
                _powerThrowLevel++;
                _skillPoints--;
                return;
            }
            if (_doubleTapLevel < _doubleTapLevelMax)
            {
                _doubleTapLevel++;
                _skillPoints--;
                return;
            }
            if (_doubleThrowLevel < _doubleThrowLevelMax)
            {
                _doubleThrowLevel++;
                _skillPoints--;
                return;
            }
            if (_eagleEyeLevel < _eagleEyeLevelMax)
            {
                _eagleEyeLevel++;
                _skillPoints--;
                return;
            }
        }

        private void GotKeyDown(object o, KeyEventArgs e)
        {
            e.Handled = ProcessKeyDown(e);
        }
        private void GotKeyUp(object o, KeyEventArgs e)
        {
            e.Handled = ProcessKeyUp(e);
        }

        public bool ProcessKeyDown(KeyEventArgs e)
        {
            bool handled = true;
            switch (e.KeyCode)
            {

                case System.Windows.Forms.Keys.Left:
                    _movingLeft = true;
                    _person.Inverted = false;
                    break;
                case System.Windows.Forms.Keys.Right:
                    _movingRight = true;
                    _person.Inverted = true;
                    break;
                case System.Windows.Forms.Keys.Space:
                    _startJumping = true;
                    break;
                case System.Windows.Forms.Keys.Z:
                    _startDoubleTap = true;
                    break;
                case System.Windows.Forms.Keys.X:
                    _throwBoomerang = true;
                    break;
                case System.Windows.Forms.Keys.Home: //hacks and cheats
                    _mp += 100;
                    break;
                case System.Windows.Forms.Keys.Q:
                    _activeSkill = ActiveSkill.Normal;
                    break;
                case System.Windows.Forms.Keys.W:
                    if (_powerThrowLevel > 0)
                        _activeSkill = ActiveSkill.PowerThrow;
                    break;
                case System.Windows.Forms.Keys.E:
                    if (_doubleThrowLevel > 0)
                        _activeSkill = ActiveSkill.DoubleThrow;
                    break;
                case System.Windows.Forms.Keys.R:
                    if (_warpLevel > 0)
                        _activeSkill = ActiveSkill.Warp;
                    break;
                case System.Windows.Forms.Keys.T:
                    if (_powerSliceLevel > 0)
                        _activeSkill = ActiveSkill.PowerSlice;
                    break;
                case System.Windows.Forms.Keys.Y:
                    if (_assassinateLevel > 0)
                        _activeSkill = ActiveSkill.Assassinate;
                    break;
                case System.Windows.Forms.Keys.U:
                    if (_skullCrusherLevel > 0)
                        _activeSkill = ActiveSkill.SkullCrusher;
                    break;
                case System.Windows.Forms.Keys.I:
                    if (_spectrumLevel > 0)
                        _activeSkill = ActiveSkill.Spectrum;
                    break;
                case System.Windows.Forms.Keys.O:
                    if (_orbitLevel > 0)
                        _activeSkill = ActiveSkill.Orbit;
                    break;
                case System.Windows.Forms.Keys.PageUp: //hacks and cheats
                    _currentExp += (_expTNL / 10);
                    break;
                case System.Windows.Forms.Keys.PageDown: //hacks and cheats
                    if (_skillPoints > 0)
                        DistributeSP();
                    break;
                case System.Windows.Forms.Keys.A:
                    if (_firebladeLevel > 0)
                        _activeSkill = ActiveSkill.FireBlade;
                    break;
                case System.Windows.Forms.Keys.S:
                    if (_grappleLevel > 0)
                        _activeSkill = ActiveSkill.Grapple;
                    break;
                case System.Windows.Forms.Keys.Tab:
                    _warpReady = true;
                    break;
                case System.Windows.Forms.Keys.C:
                    _grappleReady = true;
                    if (_grappleActive)
                        _grappleActive = false;
                    break;
                case System.Windows.Forms.Keys.Return:
                    if (_paused == false)
                        _paused = true;
                    else if (_paused == true)
                        _paused = false;
                    break;
                default:
                    handled = false;
                    break;
            }

            return handled;
        }
        public bool ProcessKeyUp(KeyEventArgs e)
        {
            bool handled = true;
            switch (e.KeyCode)
            {

                case System.Windows.Forms.Keys.Left:
                    _movingLeft = false;
                    break;
                case System.Windows.Forms.Keys.Right:
                    _movingRight = false;
                    break;
                case System.Windows.Forms.Keys.Space:
                    _startJumping = false;
                    break;
                case System.Windows.Forms.Keys.Z:
                    _startDoubleTap = false;
                    break;
                case System.Windows.Forms.Keys.X:
                    _throwBoomerang = false;
                    break;
                case System.Windows.Forms.Keys.Tab:
                    _warpReady = false;
                    break;
                case System.Windows.Forms.Keys.C:
                    _grappleReady = false;
                    break;
                default:
                    handled = false;
                    break;
            }
            return handled;
        }

        void InfoBar_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                _clickX = e.X;
                _clickY = e.Y;
            }
        }
        void Canvas_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                _canvasClickX = e.X;
                _canvasClickY = e.Y;
            }
        }

        private void ManageAnimationFrames()
        {
            foreach (Boomerang obj in _activeBoomerangs)
            {
                obj.FrameCountDown--;
                if (obj.FrameCountDown == 0)
                {
                    obj.FrameCountDown = obj.InitialFrameCountDown;
                    obj.CurrentFrame++;
                    if (obj.CurrentFrame > obj.TotalFrames)
                    {
                        obj.CurrentFrame = 1;
                    }
                }
            }
            foreach (Damage obj in _activeDamages)
            {
                obj.FrameCountDown--;
                if (obj.FrameCountDown == 0)
                {
                    obj.FrameCountDown = obj.InitialFrameCountDown;
                    obj.CurrentFrame++;
                    if (obj.CurrentFrame > obj.TotalFrames)
                    {
                        _toDelete.Add(obj);
                    }
                }
            }
            foreach (Animation obj in _activeAnimations)
            {
                obj.FrameCountDown--;
                if (obj.FrameCountDown == 0)
                {
                    obj.FrameCountDown = obj.InitialFrameCountDown;
                    obj.CurrentFrame++;
                    if (obj.CurrentFrame > obj.TotalFrames)
                    {
                        _toDelete.Add(obj);
                    }
                }
            }
            foreach (Platforms obj in _activePlatforms)
            {
                obj.FrameCountDown--;
                if (obj.FrameCountDown == 0)
                {
                    obj.FrameCountDown = obj.InitialFrameCountDown;
                    obj.CurrentFrame++;
                    if (obj.CurrentFrame > obj.TotalFrames)
                    {
                        obj.CurrentFrame = 1;
                    }
                }
            }
            foreach (Monster obj in _activeMonsters)
            {
                obj.FrameCountDown--;
                if (obj.FrameCountDown == 0)
                {
                    obj.FrameCountDown = obj.InitialFrameCountDown;
                    obj.CurrentFrame++;
                    if (obj.CurrentFrame > obj.TotalFrames)
                    {
                        obj.CurrentFrame = 1;
                    }
                }
            }

            if (_person.Inverted != _person.InvertedPrev)
            {
                _person.StandingBmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
                _person.WalkingBmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
                _person.JumpingBmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
                _person.ThrowingBmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
                _person.Throwing2Bmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
                _person.Throwing3Bmp.RotateFlip(RotateFlipType.RotateNoneFlipX);

                ChangePersonState(_personState);
            }
            _person.InvertedPrev = _person.Inverted;

            _person.FrameCountDown--;
            if (_person.FrameCountDown == 0)
            {
                _person.FrameCountDown = _person.InitialFrameCountDown;
                _person.CurrentFrame++;
                if (_person.CurrentFrame > _person.TotalFrames)
                {
                    _person.CurrentFrame = 1;
                }
            }
            /*(if (!(_person.OnGround))
            {
                
                _person.FrameCountDown--;
                if (_person.FrameCountDown == 0)
                {
                    _person.FrameCountDown = _person.InitialFrameCountDown;
                    _person.CurrentFrame++;
                    if (_person.CurrentFrame > _person.TotalFrames)
                        _person.CurrentFrame = 1;
                }
            }
            if (_person.OnGround)
            {
                if (_personState == PersonState.standing)
                {
                    _person.FrameCountDown--;
                    if (_person.FrameCountDown == 0)
                    {
                        _person.FrameCountDown = _person.InitialFrameCountDown;
                        _person.CurrentFrame++;
                        if (_person.CurrentFrame > _person.TotalFrames)
                            _person.CurrentFrame = 1;
                    }
                }
                else if (_personState == PersonState.walking)
                {
                    _person.WalkingFrameCountDown--;
                    if (_person.WalkingFrameCountDown == 0)
                    {
                        _person.WalkingFrameCountDown = _person.InitialWalkingFrameCountDown;
                        _person.WalkingCurrentFrame++;
                        if (_person.WalkingCurrentFrame > _person.WalkingTotalFrames)
                            _person.WalkingCurrentFrame = 1;
                    }
                }
            }*/
        }
        #endregion //AI

        #region Render
        private void DoRender()
        {
            RenderCanvas();
            RenderInfoBar();
        }
        private void RenderCanvas()
        {
            using (Graphics gr = Graphics.FromImage((Image)(Canvas.Image)))
            {
                #region background
                switch (_map)
                {
                    case 1:
                        gr.Clear(Color.LightGreen);
                        break;
                    case 2:
                        gr.Clear(Color.Tan);
                        break;
                    case 3:
                        gr.Clear(Color.SkyBlue);
                        break;
                    case 4:
                        gr.Clear(Color.Green);
                        break;
                    case 5:
                        gr.Clear(Color.Navy);
                        break;
                    case 6:
                        gr.Clear(Color.MediumOrchid);
                        break;
                    case 7:
                        gr.Clear(Color.Silver);
                        break;
                    case 8:
                        gr.Clear(Color.SteelBlue);
                        break;
                    case 9:
                        gr.Clear(Color.MediumSeaGreen);
                        break;
                    case 10:
                        gr.Clear(Color.White);
                        break;
                    case 11:
                        gr.Clear(Color.Maroon);
                        break;
                    case 12:
                        gr.Clear(Color.Gold);
                        break;
                    default:
                        break;
                }
                /*if (_levelTimer > 0)
                    gr.Clear(Color.DarkGreen);
                if (_levelTimer > 3)
                    gr.Clear(Color.SlateBlue);
                if (_levelTimer > 6)
                    gr.Clear(Color.Red);
                if (_levelTimer > 8)
                    gr.Clear(Color.Gold);
                if (_levelTimer > 11)
                    gr.Clear(Color.Black);
                if (_levelTimer > 13)
                    gr.Clear(Color.NavajoWhite);
                if (_levelTimer > 14)
                    gr.Clear(Color.Navy);
                if (_levelTimer > 17)
                    gr.Clear(Color.Lime);
                if (_levelTimer > 18)
                    gr.Clear(Color.YellowGreen);*/
                if (_levelTimer > 0)
                {
                    int a = 25;
                    while (a > 0)
                    {
                        gr.DrawImage(_orangeSkillButton, _rnd.Next(Canvas.Width), _rnd.Next(Canvas.Height));
                        a--;
                    }
                }
                foreach (Boomerang boom in _activeBoomerangs)
                {
                    if (boom.Cooldown == 47)
                    {
                        gr.DrawLine(_blackPen, _person.X - _screenOffsetX + (_person.FrameWidth / 2), _person.Y - _screenOffsetY + (_person.Height / 2),
                                                                             boom.X - _screenOffsetX + (boom.FrameWidth / 2), boom.Y - _screenOffsetY + (boom.Height / 2));
                    }
                }
                if (_grappleActive)
                {
                    gr.DrawLine(_blackPen, _person.X - _screenOffsetX + (_person.FrameWidth / 2), _person.Y - _screenOffsetY + (_person.Height / 2),
                                                                         _grappleX - _screenOffsetX, _grappleY - _screenOffsetY);
                }
                #endregion //background
                #region platforms
                foreach (Platforms piece in _activePlatforms)
                {
                    if (((piece.X + (piece.Width / piece.TotalFrames) > _screenOffsetX) &&
                         (piece.X < _screenOffsetX + Canvas.Width)) &&
                         ((piece.Y + (piece.Height) > _screenOffsetY) &&
                         (piece.Y < _screenOffsetY + Canvas.Height)))
                    {
                        gr.DrawImage((Image)piece.Bmp, piece.X - _screenOffsetX, piece.Y + piece.YOffset - _screenOffsetY,
                                      new Rectangle((piece.CurrentFrame - 1) * piece.FrameWidth, 0,
                                      piece.FrameWidth, piece.Height), GraphicsUnit.Pixel);
                    }
                }
                #endregion //platforms
                #region monsters
                foreach (Monster piece in _activeMonsters)
                {
                    if (((piece.X + (piece.Width / piece.TotalFrames) > _screenOffsetX) &&
                         (piece.X < _screenOffsetX + Canvas.Width)) &&
                         ((piece.Y + (piece.Height) > _screenOffsetY) &&
                         (piece.Y < _screenOffsetY + Canvas.Height)))
                    {
                        switch (_map)
                        {
                            case 1:
                                switch (piece.Type)
                                {
                                    case MonsterType.Blob:
                                        if (!((piece.Inverted == true) ^ (_blobInverted == false)))
                                        {
                                            piece.Bmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
                                            if (_blobInverted == true)
                                                _blobInverted = false;
                                            else if (_blobInverted == false)
                                                _blobInverted = true;
                                        }
                                        break;
                                    case MonsterType.Snail:
                                        if (!((piece.Inverted == true) ^ (_snailInverted == false)))
                                        {
                                            piece.Bmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
                                            if (_snailInverted == true)
                                                _snailInverted = false;
                                            else if (_snailInverted == false)
                                                _snailInverted = true;
                                        }
                                        break;
                                    case MonsterType.GreenHat:
                                        if (!((piece.Inverted == true) ^ (_greenHatInverted == false)))
                                        {
                                            piece.Bmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
                                            if (_greenHatInverted == true)
                                                _greenHatInverted = false;
                                            else if (_greenHatInverted == false)
                                                _greenHatInverted = true;
                                        }
                                        break;
                                    case MonsterType.YellowHat:
                                        if (!((piece.Inverted == true) ^ (_yellowHatInverted == false)))
                                        {
                                            piece.Bmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
                                            if (_yellowHatInverted == true)
                                                _yellowHatInverted = false;
                                            else if (_yellowHatInverted == false)
                                                _yellowHatInverted = true;
                                        }
                                        break;
                                    case MonsterType.Cel:
                                        if (!((piece.Inverted == true) ^ (_celInverted == false)))
                                        {
                                            piece.Bmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
                                            if (_celInverted == true)
                                                _celInverted = false;
                                            else if (_celInverted == false)
                                                _celInverted = true;
                                        }
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case 2:
                                {
                                    switch (piece.Type)
                                    {
                                        case MonsterType.Robiera2:
                                            if (!((piece.Inverted == true) ^ (_robiera2Inverted == false)))
                                            {
                                                piece.Bmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
                                                if (_robiera2Inverted == true)
                                                    _robiera2Inverted = false;
                                                else if (_robiera2Inverted == false)
                                                    _robiera2Inverted = true;
                                            }
                                            break;
                                        case MonsterType.Pepe:
                                            if (!((piece.Inverted == true) ^ (_pepeInverted == false)))
                                            {
                                                piece.Bmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
                                                if (_pepeInverted == true)
                                                    _pepeInverted = false;
                                                else if (_pepeInverted == false)
                                                    _pepeInverted = true;
                                            }
                                            break;
                                        case MonsterType.OrangeSnail:
                                            if (!((piece.Inverted == true) ^ (_orangeSnailInverted == false)))
                                            {
                                                piece.Bmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
                                                if (_orangeSnailInverted == true)
                                                    _orangeSnailInverted = false;
                                                else if (_orangeSnailInverted == false)
                                                    _orangeSnailInverted = true;
                                            }
                                            break;
                                        case MonsterType.FireBoar:
                                            if (!((piece.Inverted == true) ^ (_fireboarInverted == false)))
                                            {
                                                piece.Bmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
                                                if (_fireboarInverted == true)
                                                    _fireboarInverted = false;
                                                else if (_fireboarInverted == false)
                                                    _fireboarInverted = true;
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            case 3:
                                {
                                    switch (piece.Type)
                                    {
                                        case MonsterType.SnowRed:
                                            if (!((piece.Inverted == true) ^ (_snowRedInverted == false)))
                                            {
                                                piece.Bmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
                                                _snowRedInverted = !_snowRedInverted;
                                            }
                                            break;
                                        case MonsterType.SnowMan:
                                            if (!((piece.Inverted == true) ^ (_snowManInverted == false)))
                                            {
                                                piece.Bmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
                                                _snowManInverted = !_snowManInverted;
                                            }
                                            break;
                                        case MonsterType.Yeti:
                                            if (!((piece.Inverted == true) ^ (_yetiInverted == false)))
                                            {
                                                piece.Bmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
                                                _yetiInverted = !_yetiInverted;
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            case 4:
                                {
                                    switch (piece.Type)
                                    {
                                        case MonsterType.Robiera:
                                            if (!((piece.Inverted == true) ^ (_robieraInverted == false)))
                                            {
                                                piece.Bmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
                                                if (_robieraInverted == true)
                                                    _robieraInverted = false;
                                                else if (_robieraInverted == false)
                                                    _robieraInverted = true;
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                        gr.DrawImage((Image)piece.Bmp, piece.X - _screenOffsetX, piece.Y + piece.YOffset - _screenOffsetY,
                                      new Rectangle((piece.CurrentFrame - 1) * piece.FrameWidth, 0,
                                      piece.FrameWidth, piece.Height), GraphicsUnit.Pixel);
                        piece.InvertedPrev = piece.Inverted;

                        int x = (int)(piece.X - _screenOffsetX + (piece.FrameWidth / 2)) - 35;
                        int y = (int)(piece.Y - _screenOffsetY - 15);

                        gr.FillRectangle(_blackBrush, x, y, 70, 6);
                        gr.FillRectangle(_greenBrush, x + 1, y + 1, (piece.Hp * 68) / piece.HpInit, 4);

                    }
                }
                #endregion //monsters
                #region damage
                foreach (Damage piece in _activeDamages)
                {
                    if (((piece.X + (piece.Width / piece.TotalFrames) > _screenOffsetX) &&
                         (piece.X < _screenOffsetX + Canvas.Width)) &&
                         ((piece.Y + (piece.Height) > _screenOffsetY) &&
                         (piece.Y < _screenOffsetY + Canvas.Height)))
                    {
                        gr.DrawImage((Image)piece.Bmp, piece.X - _screenOffsetX, piece.Y + piece.YOffset - _screenOffsetY,
                                      new Rectangle((piece.CurrentFrame - 1) * piece.FrameWidth, 0,
                                      piece.FrameWidth, piece.Height), GraphicsUnit.Pixel);
                    }
                }
                #endregion //damage
                #region boomerangs
                foreach (Boomerang piece in _activeBoomerangs)
                {
                    if (((piece.X + (piece.Width / piece.TotalFrames) > _screenOffsetX) &&
                         (piece.X < _screenOffsetX + Canvas.Width)) &&
                         ((piece.Y + (piece.Height) > _screenOffsetY) &&
                         (piece.Y < _screenOffsetY + Canvas.Height)))
                    {
                        gr.DrawImage((Image)piece.Bmp, piece.X - _screenOffsetX, piece.Y + piece.YOffset - _screenOffsetY,
                                      new Rectangle((piece.CurrentFrame - 1) * piece.FrameWidth, 0,
                                      piece.FrameWidth, piece.Height), GraphicsUnit.Pixel);
                        if (piece.Inverted != piece.InvertedPrev)
                            piece.Bmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        piece.InvertedPrev = piece.Inverted;
                    }
                }
                #endregion //boomerangs
                #region person
                if (((_person.X + (_person.Width / _person.TotalFrames) > _screenOffsetX) &&
                     (_person.X < _screenOffsetX + Canvas.Width)) &&
                     ((_person.Y + (_person.Height) > _screenOffsetY) &&
                     (_person.Y < _screenOffsetY + Canvas.Height)))
                {
                    float xro = _person.XRendOffset;
                    if (_person.Inverted)
                        xro *= -1;
                    gr.DrawImage(_person.Bmp, _person.X - _screenOffsetX + xro, _person.Y + _person.YOffset - _screenOffsetY,
                                new Rectangle((_person.CurrentFrame - 1) * _person.FrameWidth, 0,
                              _person.FrameWidth, _person.Height), GraphicsUnit.Pixel);
                }
                #endregion //person
                #region animations
                foreach (Animation piece in _activeAnimations)
                {
                    if (((piece.X + (piece.Width / piece.TotalFrames) > _screenOffsetX) &&
                         (piece.X < _screenOffsetX + Canvas.Width)) &&
                         ((piece.Y + (piece.Height) > _screenOffsetY) &&
                         (piece.Y < _screenOffsetY + Canvas.Height)))
                    {
                        gr.DrawImage((Image)piece.Bmp, piece.X - _screenOffsetX, piece.Y + piece.YOffset - _screenOffsetY,
                                      new Rectangle((piece.CurrentFrame - 1) * piece.FrameWidth, 0,
                                      piece.FrameWidth, piece.Height), GraphicsUnit.Pixel);
                    }
                }
                #endregion //animations

                #region MP_bar
                gr.DrawRectangle(_blackPen, Canvas.Width - 220, Canvas.Height - 25, 202, 20);
                gr.FillRectangle(_blueBrush, Canvas.Width - 219, Canvas.Height - 24, 201f * ((float)_mp / (float)_maxMp), 19);
                gr.DrawString(_mp.ToString() + "/" + _maxMp.ToString(), _labelFont1, _whiteBrush, Canvas.Width - 215, Canvas.Height - 24);
                #endregion // mp bar
                #region minimap
                gr.DrawRectangle(_bluePen, _mmX - 1, _mmY - 21, _mmWidth + 2, _mmHeight + 2);
                gr.FillRectangle(_whiteBrush, _mmX, _mmY - 20, _mmWidth, _mmHeight);
                gr.FillEllipse(_yellowBrush, _mmMarkerX, _mmY - 21, 16, 16);
                gr.DrawLine(_bluePen, _mmX - 1, _mmY - 1, _mmX + _mmWidth + 1, _mmY - 1);
                gr.DrawString("Mini Map [+] [-]", _expFont, _blackBrush, _mmX - 1, _mmY - 21);

                if (_mmExpanded)
                {
                    float mmTopY = _screenOffsetY - Canvas.Height;
                    float mmBottomY = _screenOffsetY + (2 * Canvas.Height);
                    float mmLeftX = _screenOffsetX - Canvas.Width;
                    float mmRightX = _screenOffsetX + (2 * Canvas.Width);

                    float leftX;
                    float rightX;
                    float platLeftY;
                    float platRightY;

                    float magnitude = (float)_mmWidth / (mmRightX - mmLeftX);

                    foreach (Platforms plat in _activePlatforms)
                    {
                        if (((plat.LeftY > mmTopY) || (plat.RightY > mmTopY))
                             && ((plat.LeftY < mmBottomY) || (plat.RightY < mmBottomY)))
                        {
                            if (plat.RightX > mmLeftX && plat.LeftX < mmRightX)
                            {
                                leftX = plat.LeftX;
                                if (leftX < mmLeftX)
                                    leftX = mmLeftX;
                                rightX = plat.RightX;
                                if (rightX > mmRightX)
                                    rightX = mmRightX;
                                platLeftY = plat.LeftY + plat.YOffsett;
                                if (platLeftY < mmTopY)
                                    platLeftY = mmTopY;
                                if (platLeftY > mmBottomY)
                                    platLeftY = mmBottomY;
                                platRightY = plat.RightY + plat.YOffsett;
                                if (platRightY < mmTopY)
                                    platRightY = mmTopY;
                                if (platRightY > mmBottomY)
                                    platRightY = mmBottomY;

                                leftX = (magnitude * (leftX - mmLeftX)) + _mmX;
                                rightX = (magnitude * (rightX - mmLeftX)) + _mmX;
                                platLeftY = (magnitude * (platLeftY - mmTopY)) + _mmY;
                                platRightY = (magnitude * (platRightY - mmTopY)) + _mmY;

                                gr.DrawLine(_blackPen, leftX, platLeftY, rightX, platRightY);
                            }
                        }
                    }
                    float personX;
                    float personY;

                    personX = _person.X;
                    personY = _person.Y;
                    personX = (magnitude * (personX - mmLeftX)) + _mmX;
                    personY = (magnitude * (personY - mmTopY)) + _mmY;
                    gr.FillRectangle(_blueBrush, personX, personY, 4, 6);

                    foreach (Boomerang boom in _activeBoomerangs)
                    {
                        if (boom.Y > mmTopY && boom.Y < mmBottomY)
                        {
                            if (boom.X > mmLeftX && boom.X < mmRightX)
                            {
                                personX = boom.X;
                                personY = boom.Y;
                                personX = (magnitude * (personX - mmLeftX)) + _mmX;
                                personY = (magnitude * (personY - mmTopY)) + _mmY;
                                gr.FillRectangle(_greenBrush, personX, personY, 2, 2);
                            }
                        }
                    }
                    foreach (Monster mons in _activeMonsters)
                    {
                        if (mons.Y > mmTopY && mons.Y < mmBottomY)
                        {
                            if (mons.X > mmLeftX && mons.X < mmRightX)
                            {
                                personX = mons.X;
                                personY = mons.Y + mons.Height;
                                personX = (magnitude * (personX - mmLeftX)) + _mmX;
                                personY = (magnitude * (personY - mmTopY)) + _mmY;
                                gr.FillRectangle(_redBrush, personX, personY - 4, 3, 4);
                            }
                        }
                    }
                }
                #endregion //minimap
                #region pause_menu
                if (_paused)
                {
                    gr.FillEllipse(_blackBrush, 100, 40, Canvas.Width - 200, Canvas.Height - 80);

                    gr.DrawString("PAUSED", _levelFont, _whiteBrush, 500, 60);

                    gr.DrawString("Changing maps costs 800 mana", _labelFont2, _whiteBrush, 440, 110);

                    for (int y = 1; y < 13; y++)
                    {
                        gr.DrawRectangle(_whitePen, 448, 110 + (20 * y), 110, 15);
                    }

                    if (_mapMax >= 1)
                        gr.DrawString("The Armory", _expFont, _greenBrush, 450, 130);
                    else if (_mapMax < 1)
                        gr.DrawString("LOCKED", _expFont, _redBrush, 450, 130);

                    if (_mapMax >= 2)
                        gr.DrawString("Badlands", _expFont, _greenBrush, 450, 150);
                    else if (_mapMax < 2)
                        gr.DrawString("LOCKED", _expFont, _redBrush, 450, 150);

                    if (_mapMax >= 3)
                        gr.DrawString("Tundra", _expFont, _greenBrush, 450, 170);
                    else if (_mapMax < 3)
                        gr.DrawString("LOCKED", _expFont, _redBrush, 450, 170);

                    if (_mapMax >= 4)
                        gr.DrawString("Exile", _expFont, _greenBrush, 450, 190);
                    else if (_mapMax < 4)
                        gr.DrawString("LOCKED", _expFont, _redBrush, 450, 190);

                    if (_mapMax >= 5)
                        gr.DrawString("The Ocean", _expFont, _greenBrush, 450, 210);
                    else if (_mapMax < 5)
                        gr.DrawString("LOCKED", _expFont, _redBrush, 450, 210);

                    if (_mapMax >= 6)
                        gr.DrawString("Kythira", _expFont, _greenBrush, 450, 230);
                    else if (_mapMax < 6)
                        gr.DrawString("LOCKED", _expFont, _redBrush, 450, 230);

                    if (_mapMax >= 7)
                        gr.DrawString("Zafora", _expFont, _greenBrush, 450, 250);
                    else if (_mapMax < 7)
                        gr.DrawString("LOCKED", _expFont, _redBrush, 450, 250);

                    if (_mapMax >= 8)
                        gr.DrawString("Syros", _expFont, _greenBrush, 450, 270);
                    else if (_mapMax < 8)
                        gr.DrawString("LOCKED", _expFont, _redBrush, 450, 270);

                    if (_mapMax >= 9)
                        gr.DrawString("Galatia", _expFont, _greenBrush, 450, 290);
                    else if (_mapMax < 9)
                        gr.DrawString("LOCKED", _expFont, _redBrush, 450, 290);

                    if (_mapMax >= 10)
                        gr.DrawString("Olympus", _expFont, _greenBrush, 450, 310);
                    else if (_mapMax < 10)
                        gr.DrawString("LOCKED", _expFont, _redBrush, 450, 310);

                    if (_mapMax >= 11)
                        gr.DrawString("Hades", _expFont, _greenBrush, 450, 330);
                    else if (_mapMax < 11)
                        gr.DrawString("LOCKED", _expFont, _redBrush, 450, 330);

                    if (_mapMax >= 12)
                        gr.DrawString("Eternity", _expFont, _greenBrush, 450, 350);
                    else if (_mapMax < 12)
                        gr.DrawString("LOCKED", _expFont, _redBrush, 450, 350);


                    gr.FillEllipse(_orangeBrush, 428, 110 + (20 * _map), 12, 12);


                    gr.DrawString("Resume", _labelFont1, _whiteBrush, 550, 450);
                    gr.DrawRectangle(_orangePen, 548, 450, 60, 20);
                }
                #endregion //pause menu
                #region Messages
                foreach (Message m in _activeMessages)
                {
                    gr.DrawString(m.MessageDisplayed, _expFont, _blackBrush, m.X, m.Y);
                }
                #endregion
                #region instructions
                gr.FillRectangle(_greenBrush, 1, Canvas.Height - 21, 130, 20);
                gr.DrawString("Instructions [+] [ - ]", _labelFont2, _whiteBrush, 2, Canvas.Height - 20);
                if (_instructionsShowing)
                {
                    gr.DrawImage(_instructions, (Canvas.Width / 2) - (_instructions.Width / 2), (Canvas.Height / 2) - (_instructions.Height / 2));
                }
                #endregion //instructions

            }
            Canvas.Invalidate();
        }
        private void RenderInfoBar()
        {

            using (Graphics gb = Graphics.FromImage((Image)(InfoBar.Image)))
            {
                //background
                gb.Clear(Color.Black);

                gb.DrawImage(_toolBar, 0, 0);

                //draw level
                //gb.DrawString("LEVEL: ", _levelTitleFont, _orangeBrush, 10, 18);
                gb.DrawString(_level.ToString(), _levelFont, _orangeBrush, 100, 10);



                //draw experience
                //gb.DrawRectangle(_orangePen, InfoBar.Width - 220, 25, 202, 20);
                //gb.DrawString("EXP:", _labelFont1, _whiteBrush, 940, 2);
                gb.FillRectangle(_greenBrush, 941, 26, (float)200 * ((float)_currentExp / (float)_expTNL), 18);
                gb.DrawString(_currentExp + "/" + _expTNL, _expFont, _whiteBrush, InfoBar.Width - 220, 50);
                gb.DrawString(((_currentExp * 100) / _expTNL).ToString() + "%", _levelTitleFont, _whiteBrush, 945, 24);

                //draw damage range
                gb.DrawString(((int)((_damageRate * 3) / 5)).ToString() + " ~ " + _damageRate.ToString(), _labelFont1, _whiteBrush, 100, 50);

                //draw skill points
                gb.FillRectangle(_whiteBrush, 943, 67, 80, 30);
                gb.DrawRectangle(_orangePen, 944, 68, 78, 28);
                gb.DrawString("SP: " + _skillPoints.ToString(), _expFont, _greenBrush, 948, 73);

                //draw active skill
                gb.DrawLine(_pen1, 200, 1, 200, InfoBar.Height);
                gb.DrawLine(_pen1, 197, 1, 197, InfoBar.Height);
                //gb.DrawString("Active Skill: ", _expFont, _orangeBrush, 210, 10);
                gb.DrawString(_activeSkill.ToString(), _labelFont1, _orangeBrush, 210, 30);

                //draw skills
                //gb.DrawLine(_pen1, 350, 1, 350, InfoBar.Height);
                //gb.DrawLine(_pen1, 347, 1, 347, InfoBar.Height);
                //gb.DrawLine(_whitePen, 533, 1, 533, InfoBar.Height);
                //gb.DrawLine(_whitePen, 715, 1, 715, InfoBar.Height);
                //gb.DrawLine(_whitePen, 915, 1, 915, InfoBar.Height);
                //gb.DrawLine(_whitePen, 934, 1, 934, InfoBar.Height);


                if (!_upperJobsShowing)
                {
                    if (_job >= 1)
                    {
                        //gb.DrawString("Q: ", _skillFont, _greenBrush, 352, 2);
                        //gb.DrawString("Normal", _expFont, _greenBrush, 380, 2);
                        //gb.DrawImage(_graySkillButton, 440, 2);

                        //gb.DrawString("W: ", _skillFont, _greenBrush, 352, 17);
                        //gb.DrawString("Power Throw: " + _powerThrowLevel.ToString(), _expFont, _greenBrush, 380, 17);
                        gb.DrawString(_powerThrowLevel.ToString(), _expFont, _greenBrush, 500, 16);

                        //gb.DrawString("Z: ", _skillFont, _greenBrush, 352, 32);
                        //gb.DrawString("Air Jump: " + _doubleTapLevel.ToString(), _expFont, _greenBrush, 380, 32);
                        gb.DrawString(_doubleTapLevel.ToString(), _expFont, _greenBrush, 500, 31);

                        //gb.DrawString("E: ", _skillFont, _greenBrush, 352, 47);
                        //gb.DrawString("Double Throw: " + _doubleThrowLevel.ToString(), _expFont, _greenBrush, 380, 47);
                        gb.DrawString(_doubleThrowLevel.ToString(), _expFont, _greenBrush, 500, 46);

                        //gb.DrawString("Eagle Eye: " + _eagleEyeLevel.ToString(), _expFont, _greenBrush, 380, 62);
                        gb.DrawString(_eagleEyeLevel.ToString(), _expFont, _greenBrush, 500, 61);


                        //gb.DrawImage(_graySkillButton, 520, 17);
                        //gb.DrawImage(_graySkillButton, 520, 32);
                        //gb.DrawImage(_graySkillButton, 520, 47);
                        //gb.DrawImage(_graySkillButton, 520, 62);

                        if (_skillPoints > 0)
                        {
                            if (_powerThrowLevel < _powerThrowLevelMax)
                                gb.DrawImage(_orangeSkillButton, 520, 17);
                            if (_doubleTapLevel < _doubleTapLevelMax)
                                gb.DrawImage(_orangeSkillButton, 520, 32);
                            if (_doubleThrowLevel < _doubleThrowLevelMax)
                                gb.DrawImage(_orangeSkillButton, 520, 47);
                            if (_eagleEyeLevel < _eagleEyeLevelMax)
                                gb.DrawImage(_orangeSkillButton, 520, 62);
                        }
                    }

                    //gb.DrawString("R: ", _skillFont, _skyBlueBrush, 535, 2);
                    //gb.DrawString("Warp: " + _warpLevel.ToString(), _expFont, _skyBlueBrush, 562, 2);
                    //gb.DrawString("[TAB]", _expFont, _whiteBrush, 645, 2);
                    gb.DrawString(_warpLevel.ToString(), _expFont, _skyBlueBrush, 620, 1);

                    //gb.DrawString("T: ", _skillFont, _skyBlueBrush, 535, 17);
                    //gb.DrawString("Power Slice: " + _powerSliceLevel.ToString(), _expFont, _skyBlueBrush, 562, 17);
                    gb.DrawString(_powerSliceLevel.ToString(), _expFont, _skyBlueBrush, 680, 16);

                    //gb.DrawString("Y: ", _skillFont, _skyBlueBrush, 535, 32);
                    //gb.DrawString("Assassinate: " + _assassinateLevel.ToString(), _expFont, _skyBlueBrush, 562, 32);
                    gb.DrawString(_assassinateLevel.ToString(), _expFont, _skyBlueBrush, 680, 31);

                    //gb.DrawString("MP Eater: " + _mpEaterLevel.ToString(), _expFont, _skyBlueBrush, 562, 47);
                    gb.DrawString(_mpEaterLevel.ToString(), _expFont, _skyBlueBrush, 680, 46);

                    //gb.DrawImage(_graySkillButton, 700, 2);
                    //gb.DrawImage(_graySkillButton, 700, 17);
                    //gb.DrawImage(_graySkillButton, 700, 32);
                    //gb.DrawImage(_graySkillButton, 700, 47);

                    if (_skillPoints > 0)
                    {
                        if (_warpLevel < _warpLevelMax)
                            gb.DrawImage(_orangeSkillButton, 700, 2);
                        if (_powerSliceLevel < _powerSliceLevelMax)
                            gb.DrawImage(_orangeSkillButton, 700, 17);
                        if (_assassinateLevel < _assassinateLevelMax)
                            gb.DrawImage(_orangeSkillButton, 700, 32);
                        if (_mpEaterLevel < _mpEaterLevelMax)
                            gb.DrawImage(_orangeSkillButton, 700, 47);
                    }

                    if (_job < 2)
                    {
                        gb.FillRectangle(_blackBrush, 535, 2, 176, 100);
                    }


                    //gb.DrawString("U: ", _skillFont, _redBrush, 715, 2);
                    //gb.DrawString("Skull Crusher: " + _skullCrusherLevel.ToString(), _expFont, _redBrush, 742, 2);

                    //gb.DrawString("I: ", _skillFont, _redBrush, 715, 17);
                    //gb.DrawString("Spectrum: " + _spectrumLevel.ToString(), _expFont, _redBrush, 742, 17);

                    //gb.DrawString("Fleet: " + _fleetLevel.ToString(), _expFont, _redBrush, 742, 32);

                    //gb.DrawString("O: ", _skillFont, _redBrush, 715, 47);
                    //gb.DrawString("Orbit: " + _orbitLevel.ToString(), _expFont, _redBrush, 742, 47);

                    gb.DrawImage(_graySkillButton, 890, 2);
                    gb.DrawImage(_graySkillButton, 890, 17);
                    gb.DrawImage(_graySkillButton, 890, 32);
                    gb.DrawImage(_graySkillButton, 890, 47);

                    if (_skillPoints > 0)
                    {
                        if (_skullCrusherLevel < _skullCrusherLevelMax)
                            gb.DrawImage(_orangeSkillButton, 890, 2);
                        if (_spectrumLevel < _spectrumLevelMax)
                            gb.DrawImage(_orangeSkillButton, 890, 17);
                        if (_fleetLevel < _fleetLevelMax)
                            gb.DrawImage(_orangeSkillButton, 890, 32);
                        if (_orbitLevel < _orbitLevelMax)
                            gb.DrawImage(_orangeSkillButton, 890, 47);
                    }

                    if (_job < 3)
                    {
                        gb.FillRectangle(_blackBrush, 718, 2, 190, 100);
                    }

                }
                else if (_upperJobsShowing)
                {
                    gb.FillRectangle(_blackBrush, 350, 1, 560, 103);
                    if (_job >= 4)
                    {
                        gb.DrawString("A: ", _skillFont, _greenBrush, 352, 2);
                        gb.DrawString("Fire Blade: " + _firebladeLevel.ToString(), _expFont, _greenBrush, 380, 2);

                        gb.DrawString("S: ", _skillFont, _greenBrush, 352, 17);
                        gb.DrawString("Grapple: " + _grappleLevel.ToString(), _expFont, _greenBrush, 380, 17);
                        gb.DrawString("[C]", _expFont, _whiteBrush, 493, 17);

                        gb.DrawImage(_graySkillButton, 520, 2);
                        gb.DrawImage(_graySkillButton, 520, 17);

                        if (_skillPoints > 0)
                        {
                            if (_firebladeLevel < _firebladeLevelMax)
                                gb.DrawImage(_orangeSkillButton, 520, 2);
                            if (_grappleLevel < _grappleLevelMax)
                                gb.DrawImage(_orangeSkillButton, 520, 17);
                        }
                    }
                }

                //draw up and down arrows to select showing jobs
                //gb.DrawImage(_jobArrows, 917, 2);

                //Reset button
                //gb.DrawRectangle(_whitePen, 1100, 70, 50, 25);
                //gb.DrawString("RESET", _labelFont1, _whiteBrush, 1106, 72);

                //SP Reset button
                //gb.DrawRectangle(_whitePen, 1080, 50, 70, 17);
                //gb.DrawString("SP RESET", _labelFont1, _whiteBrush, 1085, 48);

                //Save button
                //gb.DrawRectangle(_whitePen, 1055, 70, 40, 25);
                //gb.DrawString("SAVE", _labelFont1, _whiteBrush, 1057, 72);

                //boomerangs active
                int a = _maxBoomerangsActive - _boomerangsActive;
                int b = 205;
                Bitmap bmp = new Bitmap(GetType(), "Boomerang.bmp");
                bmp.MakeTransparent(Color.Magenta);

                while (a != 0)
                {
                    gb.DrawImage((Image)bmp, b, 60, new Rectangle(0, 0, 54, 54), GraphicsUnit.Pixel);
                    b += 30;
                    a--;
                }

            }
            InfoBar.Invalidate();
        }
        #endregion //Render

        #region GameState

        private void InitializeGameState()
        {
            _gameSave = new GameSave();
            List<PlayerPersist> players = new List<PlayerPersist>();
            PlayerPersist player = new PlayerPersist("player1", 1, 1, 0, 300, 24, 0, 1, 100, new int[] { 0, 0, 0, 0 },
                                    new int[] { 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0 }, new int[] { 0, 0, 0 }, /*PATCH*/ 6 /*VERSION*/ );
            players.Add(player);
            _gameSave.Players = players;
        }
        private void SaveGame()
        {
            _gameSave.Players[0].Level = _level;
            _gameSave.Players[0].Job = _job;
            _gameSave.Players[0].CurrentExp = _currentExp;
            _gameSave.Players[0].ExpTNL = _expTNL;
            _gameSave.Players[0].DamageRate = _damageRate;
            _gameSave.Players[0].SkillPoints = _skillPoints;
            _gameSave.Players[0].MaxBoomerangsActive = _maxBoomerangsActive;
            _gameSave.Players[0].FirstJSL[0] = _powerThrowLevel;
            _gameSave.Players[0].FirstJSL[1] = _doubleTapLevel;
            _gameSave.Players[0].FirstJSL[2] = _doubleThrowLevel;
            _gameSave.Players[0].FirstJSL[3] = _eagleEyeLevel;
            _gameSave.Players[0].SecondJSL[0] = _warpLevel;
            _gameSave.Players[0].SecondJSL[1] = _powerSliceLevel;
            _gameSave.Players[0].SecondJSL[2] = _assassinateLevel;
            _gameSave.Players[0].SecondJSL[3] = _mpEaterLevel;
            _gameSave.Players[0].ThirdJSL[0] = _skullCrusherLevel;
            _gameSave.Players[0].ThirdJSL[1] = _spectrumLevel;
            _gameSave.Players[0].ThirdJSL[2] = _fleetLevel;
            _gameSave.Players[0].ThirdJSL[3] = _orbitLevel;
            _gameSave.Players[0].FourthJSL[0] = _firebladeLevel;
            _gameSave.Players[0].FourthJSL[1] = _grappleLevel;
            _gameSave.Players[0].Mp = _mp;
            _gameSave.Players[0].PatchVersion = _patchVersion;



            string fileName = GetSaveName();
            try
            {
                if (File.Exists(fileName))
                    File.Delete(fileName);
                using (TextWriter writer = new StreamWriter(fileName))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(GameSave));
                    serializer.Serialize(writer, _gameSave);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fail: " + ex.Message, "Unable to save", MessageBoxButtons.OK);
            }
        }
        private string GetSaveName()
        {
            return Path.Combine(Path.GetDirectoryName(Application.ExecutablePath),
                              "E5XYLSave.xml");
        }
        private void LoadGame()
        {
            string fileName = GetSaveName();
            bool createNew = true;

            if (File.Exists(fileName))
            {
                try
                {
                    using (TextReader rdr = new StreamReader(fileName))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(GameSave));
                        _gameSave = (GameSave)serializer.Deserialize(rdr);
                        createNew = false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failboat: " + ex.Message, "Unable to load", MessageBoxButtons.OK);
                }
            }
            if (createNew)
            {
                InitializeGameState();
            }
            if (_gameSave.Players.Count > 0)
            {
                SetNewPlayerVars();
                _level *= -1;
            }

        }
        private void SetNewPlayerVars()
        {
            _currentPlayer = _gameSave.Players[0];

            if (_currentPlayer.PatchVersion > _patchVersion)
                this.Close();

            _level = -(_currentPlayer.Level);
            _job = _currentPlayer.Job;
            _currentExp = _currentPlayer.CurrentExp;
            _expTNL = _currentPlayer.ExpTNL;
            _damageRate = _currentPlayer.DamageRate;
            _skillPoints = _currentPlayer.SkillPoints;
            _maxBoomerangsActive = _currentPlayer.MaxBoomerangsActive;
            _mp = _currentPlayer.Mp;

            _powerThrowLevel = _currentPlayer.PowerThrowLevel;
            _doubleTapLevel = _currentPlayer.DoubleTapLevel;
            _doubleThrowLevel = _currentPlayer.DoubleThrowLevel;
            _eagleEyeLevel = _currentPlayer.EagleEyeLevel;
            _warpLevel = _currentPlayer.WarpLevel;
            _powerSliceLevel = _currentPlayer.PowerSliceLevel;
            _assassinateLevel = _currentPlayer.AssassinateLevel;
            _mpEaterLevel = _currentPlayer.MpEaterLevel;
            _skullCrusherLevel = _currentPlayer.SkullCrusherLevel;
            _spectrumLevel = _currentPlayer.SpectrumLevel;
            _fleetLevel = _currentPlayer.FleetLevel;
            _orbitLevel = _currentPlayer.OrbitLevel;
            _firebladeLevel = _currentPlayer.FirebladeLevel;
            _grappleLevel = _currentPlayer.GrappleLevel;

            if (_level < 0)
            {
                try
                {
                    _powerThrowLevel = _currentPlayer.FirstJSL[0];
                    _doubleTapLevel = _currentPlayer.FirstJSL[1];
                    _doubleThrowLevel = _currentPlayer.FirstJSL[2];
                    _eagleEyeLevel = _currentPlayer.FirstJSL[3];
                    _warpLevel = _currentPlayer.SecondJSL[0];
                    _powerSliceLevel = _currentPlayer.SecondJSL[1];
                    _assassinateLevel = _currentPlayer.SecondJSL[2];
                    _mpEaterLevel = _currentPlayer.SecondJSL[3];
                    _skullCrusherLevel = _currentPlayer.ThirdJSL[0];
                    _spectrumLevel = _currentPlayer.ThirdJSL[1];
                    _fleetLevel = _currentPlayer.ThirdJSL[2];
                    _orbitLevel = _currentPlayer.ThirdJSL[3];
                    _firebladeLevel = _currentPlayer.FourthJSL[0];
                    _grappleLevel = _currentPlayer.FourthJSL[1];
                }
                catch
                {
                    _level *= -1;
                }
            }
            /*if (_level >= 0)
            {
                ConvertFileType();
                SaveGame();
                LoadGame();
            }*/

        }
        private void ConvertFileType()
        {
            _currentPlayer.Level *= -1;

            int[] firstJSL = new int[] { _currentPlayer.PowerThrowLevel, _currentPlayer.DoubleTapLevel, _currentPlayer.DoubleThrowLevel, _currentPlayer.EagleEyeLevel };
            int[] secondJSL = new int[] { _currentPlayer.WarpLevel, _currentPlayer.PowerSliceLevel, _currentPlayer.AssassinateLevel, _currentPlayer.MpEaterLevel };
            int[] thirdJSL = new int[] { _currentPlayer.SkullCrusherLevel, _currentPlayer.SpectrumLevel, _currentPlayer.FleetLevel, _currentPlayer.OrbitLevel };
            int[] fourthJSL = new int[] { _currentPlayer.FirebladeLevel };

            _gameSave = new GameSave();
            List<PlayerPersist> players = new List<PlayerPersist>();
            PlayerPersist player = new PlayerPersist("player1", -(_level), _job, _currentExp, _expTNL, _damageRate, _skillPoints, _maxBoomerangsActive, _mp, firstJSL, secondJSL, thirdJSL, fourthJSL, _patchVersion);
            players.Add(player);
            _gameSave.Players = players;

            _currentPlayer.PowerThrowLevel = 0;
            _currentPlayer.DoubleTapLevel = 0;
            _currentPlayer.DoubleThrowLevel = 0;
            _currentPlayer.EagleEyeLevel = 0;
            _currentPlayer.WarpLevel = 0;
            _currentPlayer.PowerSliceLevel = 0;
            _currentPlayer.AssassinateLevel = 0;
            _currentPlayer.MpEaterLevel = 0;
            _currentPlayer.SkullCrusherLevel = 0;
            _currentPlayer.SpectrumLevel = 0;
            _currentPlayer.FleetLevel = 0;
            _currentPlayer.OrbitLevel = 0;


        }
        #endregion //GameState


        private void InfoBar_Click(object sender, EventArgs e)
        {

        }
        private void Canvas_Click(object sender, EventArgs e)
        {

        }
        void Game_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            SaveGame();
        }

        private void Game_Load(object sender, EventArgs e)
        {

        }
    }
}
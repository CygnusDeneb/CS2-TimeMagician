using Colossal.IO.AssetDatabase;
using Game.Modding;
using Game.Settings;
using Game.UI;

namespace Time_Magician.Settings
{
    [FileLocation(nameof(Time_Magician))]
    [SettingsUIGroupOrder(kMainSection)]
    public class TimeMagicianSettings : ModSetting
    {
        //Setting Layout]
        public const string kMainTab = "Main";
        public const string kMainSection = "Settings";

        public TimeMagicianSettings(IMod mod) : base(mod)
        {
            SetDefaults();
        }

        
        [SettingsUISection(kMainTab, kMainSection)]
        [SettingsUIMultilineText]
        public string descriptions => string.Empty;
        
        
        [SettingsUISection(kMainTab, kMainSection)]
        public ShiftingBehavior shiftingMod { get; set; } = ShiftingBehavior.neutral;
        private bool disableIndex => shiftingMod == ShiftingBehavior.neutral;

        
        

        /// <summary>
        /// Define the number of ticks that shifts frame or not.
        /// The actual number of ticks that shifts frame is the absolute value of these parameters plus 1.
        /// </summary>

        //NOTE : UISlider does not support uint!!!
        [SettingsUISlider(min = -10, max = 10, step = 1, scalarMultiplier = 1, unit = Unit.kInteger)]
        [SettingsUISection(kMainTab, kMainSection)]
        [SettingsUIDisableByCondition(typeof(TimeMagicianSettings), nameof(disableIndex))]
        public int accelerationIndex {get; set;}

        
        public enum ShiftingBehavior
        {
            neutral = 0,
            accelerate = 1,
            decelerate = -1
        }
        public override void SetDefaults()
        {
            this.accelerationIndex = 50;
            this.shiftingMod = ShiftingBehavior.neutral;
        }
    }
}
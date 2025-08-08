using System.Collections.Generic;
using Colossal;
using Time_Magician.Settings;

namespace Time_Magician.Locale
{
    public class LocaleEN : IDictionarySource
    {
        private readonly TimeMagicianSettings m_Settings;

        public LocaleEN(TimeMagicianSettings settings)
        {
            m_Settings = settings;
        }

        public IEnumerable<KeyValuePair<string, string>> ReadEntries(IList<IDictionaryEntryError> errors, Dictionary<string, int> indexCounts)
        {
            return new Dictionary<string, string>
            {
                { m_Settings.GetSettingsLocaleID(), "Time Magician" },
                { m_Settings.GetOptionTabLocaleID(TimeMagicianSettings.kMainTab), "Main Tab" },
                { m_Settings.GetOptionGroupLocaleID(TimeMagicianSettings.kMainSection), "Main Section" },
                {m_Settings.GetOptionLabelLocaleID(nameof(TimeMagicianSettings.descriptions)), 
                    "This is a description."},
                
                { m_Settings.GetOptionLabelLocaleID(nameof(TimeMagicianSettings.shiftingMod)), "Time Flow Speed"},
                { m_Settings.GetEnumValueLocaleID(TimeMagicianSettings.ShiftingBehavior.neutral), "Neutral"},
                { m_Settings.GetEnumValueLocaleID(TimeMagicianSettings.ShiftingBehavior.accelerate), "Acceleration"},
                { m_Settings.GetEnumValueLocaleID(TimeMagicianSettings.ShiftingBehavior.decelerate), "Deceleration"},
                { m_Settings.GetOptionDescLocaleID(nameof(TimeMagicianSettings.shiftingMod)), "Description."},
                
                { m_Settings.GetOptionLabelLocaleID(nameof(TimeMagicianSettings.accelerationIndex)), "Amount to accelerate/decelerate"},
            };
        }

        public void Unload()
        {
            
        }
    }
}
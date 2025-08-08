using Colossal.IO.AssetDatabase;
using Colossal.Logging;
using Game;
using Game.Modding;
using Game.SceneFlow;
using Time_Magician.Locale;
using Time_Magician.Settings;

namespace Time_Magician
{
    public class Mod : IMod
    {
        public static ILog log = LogManager.GetLogger($"{nameof(Time_Magician)}.{nameof(Mod)}")
            .SetShowsErrorsInUI(false);
        public static Mod instance {get; private set;}

        public static TimeMagicianSettings m_Settings;

        public void OnLoad(UpdateSystem updateSystem)
        {
            instance = this;
            log.Info(nameof(OnLoad));

            if (GameManager.instance.modManager.TryGetExecutableAsset(this, out var asset))
                log.Info($"Current mod asset at {asset.path}");

            m_Settings = new TimeMagicianSettings(this);
            log.Info($"Setting object is generated.");
            
            m_Settings.RegisterInOptionsUI();
            log.Info($"Setting for this mod is registered to game's option UI.");
            
            LoadLocale();
            log.Info($"Localization loaded.");
            
            AssetDatabase.global.LoadSettings(nameof(Time_Magician), m_Settings, new TimeMagicianSettings(this));
            
            updateSystem.UpdateAt<TimeMagician>(SystemUpdatePhase.MainLoop);
        }

        public void OnDispose()
        {
            log.Info(nameof(OnDispose));

            if (m_Settings != null)
            {
                m_Settings.UnregisterInOptionsUI();
                m_Settings = null;
            }
        }

        private void LoadLocale()
        {
            GameManager.instance.localizationManager.AddSource("en-US", new LocaleEN(m_Settings));
        }
    }
}
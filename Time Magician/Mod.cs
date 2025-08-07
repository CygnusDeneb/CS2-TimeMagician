using Colossal.Logging;
using Game;
using Game.Modding;
using Game.SceneFlow;
using Unity.Entities;

namespace Time_Magician
{
    public class Mod : IMod
    {
        public static ILog log = LogManager.GetLogger($"{nameof(Time_Magician)}.{nameof(Mod)}")
            .SetShowsErrorsInUI(false);
        //Mod setting parameter may define here.

        public void OnLoad(UpdateSystem updateSystem)
        {
            log.Info(nameof(OnLoad));

            if (GameManager.instance.modManager.TryGetExecutableAsset(this, out var asset))
                log.Info($"Current mod asset at {asset.path}");
            
            // Load System
            updateSystem.UpdateAt<TimeMagician>(SystemUpdatePhase.GameSimulation);
        }

        public void OnDispose()
        {
            log.Info(nameof(OnDispose));
        }
    }
}
using Colossal.Serialization.Entities;
using Game;
using Game.Common;
using Game.Simulation;
using Unity.Entities;

namespace Time_Magician
{
    public partial class TimeMagician : GameSystemBase
    {
        private SimulationSystem _simulation;
        private EntityQuery _timeData;

        private bool _isEditor;
        private bool _isPaused;
        private bool _isLoaded;
        
        private TimeData m_TimeData;
        private uint ticks;
        private uint elapsedTicks;
        private uint m_FirstFrame;

        private int shiftType;
        private int shiftStep;

        protected override void OnCreate()
        {
            base.OnCreate();
            _simulation = World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<SimulationSystem>();
        }

        protected override void OnGameLoaded(Context serializationContext)
        {
            base.OnGameLoaded(serializationContext);
            if (_isEditor) return;

            _isLoaded = true;

            _timeData = this.GetEntityQuery(ComponentType.ReadOnly<TimeData>());
            m_FirstFrame = _simulation.frameIndex;
            
            ticks = 0;
            elapsedTicks = 0;
        }

        protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode)
        {
            base.OnGameLoadingComplete(purpose, mode);

            _isEditor = mode.IsEditor();
            if (_isEditor)
            {
                return;
            }

            _isLoaded = true;
        }
        
        private void FastAccelerator(int jumpCount)
        {
            ticks = _simulation.frameIndex;
            elapsedTicks = ticks - m_FirstFrame;
            if (elapsedTicks > jumpCount)
            {
                m_TimeData = _timeData.GetSingleton<TimeData>();
                TimeData newTimeData = new TimeData();
                newTimeData.m_FirstFrame = (uint) (m_TimeData.m_FirstFrame - shiftType * jumpCount);
                newTimeData.TimeOffset = m_TimeData.TimeOffset;
                newTimeData.m_StartingMonth = m_TimeData.m_StartingMonth;
                newTimeData.m_StartingYear = m_TimeData.m_StartingYear;
                
                _timeData.SetSingleton(newTimeData);

                m_FirstFrame = ticks;
            }

        }
        
        private void SlowAccelerator(int jumpCount)
        {
            ticks = _simulation.frameIndex;
            elapsedTicks = ticks - m_FirstFrame;

            if (elapsedTicks > jumpCount)
            {
                m_TimeData = _timeData.GetSingleton<TimeData>();
                TimeData newTimeData = new TimeData();
                newTimeData.m_FirstFrame = (uint) (m_TimeData.m_FirstFrame - shiftType);
                newTimeData.TimeOffset = m_TimeData.TimeOffset;
                newTimeData.m_StartingMonth = m_TimeData.m_StartingMonth;
                newTimeData.m_StartingYear = m_TimeData.m_StartingYear;
                
                _timeData.SetSingleton(newTimeData);
                
                m_FirstFrame = ticks;

            }
        }
        
        protected override void OnUpdate()
        {
            if (!_isLoaded) return;
            
            _isPaused = _simulation.selectedSpeed == 0;
            if (_isPaused)
            {
                shiftType = (int) Mod.m_Settings.shiftingMod;
                shiftStep = Mod.m_Settings.accelerationIndex;
                return;
            }

            if (shiftStep > 0)
            {
                FastAccelerator(shiftStep + 1);
            }
            else
            {
                SlowAccelerator(-shiftStep + 1);
            }
        }
    }
}
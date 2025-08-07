using System;
using Colossal.Serialization.Entities;
using Game;
using Game.Common;
using Game.Prefabs;
using Game.Simulation;
using Unity.Entities;
using UnityEngine;

namespace Time_Magician
{
    public partial class TimeMagician : GameSystemBase
    {
        private SimulationSystem _simulation;
        private PlanetarySystem _planetarySystem;
        private TimeSystem _timeSystem;
        private EntityQuery _timeData;

        private bool _isEditor;
        private bool _isPaused;
        private bool _isLoaded = false;
        
        private TimeData m_TimeData;
        private float _currTime;
        private float _currDay;
        private uint ticks;
        private uint elapsedTicks;
        private uint m_FirstFrame;

        protected override void OnCreate()
        {
            base.OnCreate();
            _timeSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<TimeSystem>();
            _planetarySystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<PlanetarySystem>();
            _simulation = World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<SimulationSystem>();
        }

        protected override void OnGameLoaded(Context serializationContext)
        {
            base.OnGameLoaded(serializationContext);
            if (_isEditor) return;

            _isLoaded = true;

            _timeData = this.GetEntityQuery(ComponentType.ReadOnly<TimeData>());
            _planetarySystem.overrideTime = true;
            m_FirstFrame = _simulation.frameIndex;
            
            UpdatePlanetary();
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
        
        public void UpdatePlanetary()
        {
            if (_isEditor) return;
            
            _planetarySystem.overrideTime = true;

            _currTime = _timeSystem.normalizedTime;
            _currDay = _timeSystem.normalizedDate;
            _planetarySystem.normalizedTime = _currTime;
            _planetarySystem.normalizedDayOfYear = _currDay;
        }

        protected override void OnUpdate()
        {
            if (!_isLoaded) return;
            
            _isPaused = _simulation.selectedSpeed == 0;
            if (_isPaused) return;
            
            ticks = _simulation.frameIndex;
            elapsedTicks = ticks - m_FirstFrame;

            if (elapsedTicks < 4)
            {
                m_TimeData = _timeData.GetSingleton<TimeData>();
                TimeData newTimeData = new TimeData();
                newTimeData.m_FirstFrame = m_TimeData.m_FirstFrame + 1;
                newTimeData.TimeOffset = m_TimeData.TimeOffset;
                newTimeData.m_StartingMonth = m_TimeData.m_StartingMonth;
                newTimeData.m_StartingYear = m_TimeData.m_StartingYear;
                
                _timeData.SetSingleton<TimeData>(newTimeData);
            }
            else
            {
                m_FirstFrame = ticks;
            }
            
            UpdatePlanetary();
        }
    }
}
using UnityEngine;
using System;

namespace LTF.Timers
{
    [Serializable]
    public abstract class Timer : ITimer
    {
        [SerializeField] private bool _canTick = true;
        [SerializeField] private TimeType _timeType = TimeType.DeltaTime;
        private float _elapsedTime = 0f;
        private float _speedScale = 1f;

        protected Timer(float time,
                        bool canTick,
                        TimeType timeType,
                        float elapsedTime,
                        float speedScale)
        {
            WaitTime = time;
            _canTick = canTick;
            _timeType = timeType;
            _elapsedTime = elapsedTime;
            _speedScale = speedScale;
            TimeEvent = delegate { };
        }

        public Action TimeEvent { get; set; }

        public abstract float WaitTime { get; protected set; }
        public float ElapsedTime => _elapsedTime;
        public bool CanTick => _canTick;

        public float T => _elapsedTime / WaitTime;
        public float TimeLeft => WaitTime - _elapsedTime;

        public void Tick()
        {
            if (!_canTick)
                return;

            _elapsedTime += _speedScale * _timeType switch
            {
                TimeType.DeltaTime => Time.deltaTime,
                TimeType.UnscaledDeltaTime => Time.unscaledDeltaTime,
                _ => Time.deltaTime,
            };

            if (_elapsedTime < WaitTime)
                return;

            Reset_();
            TimeEvent?.Invoke();
        }

        public void SetSpeedScale(float scale) => _speedScale = scale;

        public void ChangeTime(float time) => WaitTime += time;

        public void SetTime(float time) => WaitTime = time;

        public void Stop() => _canTick = false;

        public void Continue() => _canTick = true;

        public void Reset_() => _elapsedTime = 0f;
    }
 }
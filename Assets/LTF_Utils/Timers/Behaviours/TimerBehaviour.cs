using UnityEngine;
using System;

namespace LTF.Timers
{
    public abstract class TimerBehaviour<Timer> : MonoBehaviour, ITimer where Timer : ITimer
    {
        [SerializeField] private Timer _timer;

        public Action TimeEvent { get => _timer.TimeEvent; set => _timer.TimeEvent = value; }

        public float WaitTime => _timer.WaitTime;
        public float ElapsedTime => _timer.ElapsedTime;
        public bool CanTick => _timer.CanTick;

        public float T => _timer.T;
        public float TimeLeft => _timer.TimeLeft;

        public void SetSpeedScale(float scale) => _timer.SetSpeedScale(scale);
        public void ChangeTime(float time) => _timer.ChangeTime(time);
        public void SetTime(float time) => _timer.SetTime(time);

        private void Update() => _timer.Tick();

        public void Tick() => _timer.Tick();

        public void Continue()
        {
            enabled = true;
            _timer.Continue();
        }

        public void Stop()
        {
            enabled = false;
            _timer.Stop();
        }

        public void Reset_() => _timer.Reset_();
    }
 }
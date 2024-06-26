﻿using UnityEngine;
using System;

namespace LTF.Timers
{
    public abstract class TimerBehaviour<Timer> : MonoBehaviour, ITimer where Timer : ITimer
    {
        [SerializeField] private Timer _timer;

        public Action Timeout { get => _timer.Timeout; set => _timer.Timeout = value; }

        public float WaitTime => _timer.WaitTime;
        public float ElapsedTime => _timer.ElapsedTime;
        public bool CanTick => _timer.CanTick;
        public bool Looping => _timer.Looping;

        public float T => _timer.T;
        public float TimeLeft => _timer.TimeLeft;

        public void SetSpeedScale(float scale) => _timer.SetSpeedScale(scale);

        public void ChangeWaitTime(float time) => _timer.ChangeWaitTime(time);

        public void SetWaitTime(float time) => _timer.SetWaitTime(time);

        public void SetLooping(bool looping) => _timer.SetLooping(looping);

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
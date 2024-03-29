using UnityEngine;

namespace LTF.Timers
{
    [System.Serializable]
    public class TimerFixed : Timer
    {
        [SerializeField] private float _time;

        public TimerFixed(float time,
                          bool canTick = true,
                          TimeType timeType = TimeType.DeltaTime,
                          float elapsedTime = 0f,
                          float speedScale = 1f)
        : base(time, canTick, timeType, elapsedTime, speedScale) { }

        public TimerFixed() : this(1f) { }

        public override float WaitTime { get => _time; protected set => _time = value; }
    }
}
using UnityEngine;

namespace LTF.Timers
{
    [System.Serializable]
    public class TimerRange : Timer
    {
        [SerializeField] private Vector2 _range;
        private float _time;

        public TimerRange(Vector2 range,
                          bool canTick = true,
                          TimeType timeType = TimeType.DeltaTime,
                          float elapsedTime = 0f,
                          float speed_scale = 1f)
            : base((range.x + range.y) * .5f, canTick, timeType, elapsedTime, speed_scale) 
            => TimeEvent += SetRandomTime;

        public TimerRange() : this(new Vector2(1f, 2f)) { }

        ~TimerRange()
        {
            TimeEvent -= SetRandomTime;
        }

        public override float WaitTime { get => _time; protected set => _time = value; }
        public Vector2 Range { get => _range; set => _range = value; }

        private void SetRandomTime() => _time = Random.Range(_range.x, _range.y);
    }
}
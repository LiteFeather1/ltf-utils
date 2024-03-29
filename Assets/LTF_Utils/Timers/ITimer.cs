using System;

namespace LTF.Timers
{
    public interface ITimer
    {
        public Action TimeEvent { get; set; }

        public float WaitTime { get; }
        public float ElapsedTime { get; }
        public bool CanTick { get; }
        public bool Looping { get; }

        public float T { get; }
        public float TimeLeft { get; }

        public void Tick();

        /// <summary>
        /// Set how fast elasedtime gets ticked 
        /// </summary>
        public void SetSpeedScale(float scale);

        /// <summary>
        /// Change Time to do Event
        /// </summary>
        public void ChangeWaitTime(float time);

        /// <summary>
        /// Set Wait Time to Time Event
        /// </summary>
        public void SetWaitTime(float time);

        /// <summary>
        /// Set looping, if true and set to false it will stop on next Time Event
        /// </summary>
        public void SetLooping(bool looping);

        /// <summary>
        /// Stops ticking
        /// </summary>
        public void Stop();

        /// <summary>
        /// Starts ticking
        /// </summary>
        public void Continue();

        //not unity reset zz
        /// <summary>
        /// Resets elapsed time to 0
        /// </summary>
        public void Reset_();

        /// <summary>
        /// Stops ticking and Resets elapsed time to 0
        /// </summary>
        public void StopAndReset()
        {
            Stop();
            Reset_();
        }

        /// <summary>
        /// Resets elapsed time to 0 and starts ticking 
        /// </summary>
        public void Restart()
        {
            Reset_();
            Continue();
        }
    }
 }
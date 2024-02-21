using UnityEngine;

namespace LTF.BiasedRandom
{
    [System.Serializable]
    public struct BiasedRandom
    {
        [SerializeField] private float _min;
        [SerializeField] private float _max;
        [Tooltip("The number which the random number will be biased")]
        [SerializeField] private float _bias;
        [Tooltip("How strong is the influence. Keep this between 0.0 and 1.0 for better results")]
        [SerializeField] private float _influence;

        public readonly float Min => _min;
        public readonly float Max => _max;

        public BiasedRandom(float min = 0f, float max = 1f, float bias = .5f, float influence = .5f)
        {
            _min = min;
            _max = max;
            _bias = bias;
            _influence = influence;
        }

        public readonly float GetNumber()
        {
            float r = Random.value * (_max - _min) + _min,
                mix = Random.value * _influence;
            return r * (1f - mix) + _bias * mix;
        }

        public readonly float GetValue(float randomV, float randomVMix) 
        {
            float r = randomV * (_max - _min) + _min,
                mix = randomVMix * _influence;
            return r * (1f - mix) + _bias * mix;
        }
    }
}

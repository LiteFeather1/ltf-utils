using UnityEngine;
using UnityEditor;
using LTF.BiasedRandom;
using LTF.Utils;

public class BiasedRandomManager : MonoBehaviour
{
    [SerializeField] private Camera _cam;

    [SerializeField] private BiasedRandom _biasedRandom = new(-100f, 100f, 0f, .5f);
    [SerializeField] private float _dotAmount = 100;

    [Header("Seed")]
    [SerializeField] private int _seed;
    [SerializeField] private bool _randomizeSeed = true;

    private const float RADIUS = .032f;

    private static readonly Color sr_unityRandom = new(.23f, .63f, .77f, .33f);
    private static readonly Color sr_biasedRandom = new(.77f, .37f, .23f, .33f);

    private void OnValidate()
    {
        if (!_randomizeSeed)
            return;

        _randomizeSeed = false;
        _seed = (int)System.DateTime.Now.Ticks;
    }

    private void OnDrawGizmos()
    {
        Random.InitState(_seed);
        var cam = SceneView.currentDrawingSceneView ? SceneView.currentDrawingSceneView.camera : _cam;

        Gizmos.color = sr_unityRandom;
        var zoom = cam.orthographicSize;
        var width = zoom * cam.aspect * .9f;
        var camPos = cam.transform.position;
        for (int i = 0; i < _dotAmount; i++)
        {
            var xR = LTFHelpersMath.Remap(Random.Range(_biasedRandom.Min, _biasedRandom.Max), 
                _biasedRandom.Min, _biasedRandom.Max, 
                -width, width);
            Gizmos.DrawSphere(new(camPos.x + xR, camPos.y, camPos.z + 10f), RADIUS * zoom);
        }
        Gizmos.color = sr_biasedRandom;
    }
}
